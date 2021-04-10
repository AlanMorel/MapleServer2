using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class FishingHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.FISHING;

        public FishingHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        private enum FishingMode : byte
        {
            PrepareFishing = 0x0,
            Stop = 0x1,
            Catch = 0x8,
            Start = 0x9,
            FailMinigame = 0xA,
        }

        private enum FishingNotice : short
        {
            CanOnlyFishNearWater = 0x1,
            InvalidFishingRod = 0x2,
            MasteryTooLowForMap = 0x3,
            CannotFishHere = 0x4,
            MasteryTooLowForRod = 0x6,
            GearOrMiscInventoryFull = 0x7,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            FishingMode mode = (FishingMode) packet.ReadByte();

            switch (mode)
            {
                case FishingMode.PrepareFishing:
                    HandlePrepareFishing(session, packet);
                    break;
                case FishingMode.Stop:
                    HandleStop(session);
                    break;
                case FishingMode.Catch:
                    HandleCatch(session, packet);
                    break;
                case FishingMode.Start:
                    HandleStart(session, packet);
                    break;
                case FishingMode.FailMinigame:
                    HandleFailMinigame(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandlePrepareFishing(GameSession session, PacketReader packet)
        {
            long fishingRodUid = packet.ReadLong();
            MasteryExp masteryExp = session.Player.Levels.MasteryExp.FirstOrDefault(x => x.Type == MasteryType.Fishing);
            if (masteryExp == null) // add mastery to list
            {
                masteryExp = new MasteryExp(MasteryType.Fishing);
                session.Player.Levels.MasteryExp.Add(masteryExp);
            }

            if (!FishingSpotMetadataStorage.CanFish(session.Player.MapId, masteryExp.CurrentExp))
            {
                session.Send(FishingPacket.Notice((short) FishingNotice.MasteryTooLowForMap));
                return;
            }

            if (!session.Player.Inventory.Items.ContainsKey(fishingRodUid))
            {
                session.Send(FishingPacket.Notice((short) FishingNotice.InvalidFishingRod));
                return;
            }

            Item fishingRod = session.Player.Inventory.Items[fishingRodUid];
            FishingRodMetadata rodMetadata = FishingRodMetadataStorage.GetMetadata(fishingRod.Function.Id);

            if (rodMetadata.MasteryLimit < masteryExp.CurrentExp)
            {
                session.Send(FishingPacket.Notice((short) FishingNotice.MasteryTooLowForRod));
            }

            int direction = Direction.GetClosestDirection(session.FieldPlayer.Rotation);
            CoordF startCoord = Block.ClosestBlock(session.FieldPlayer.Coord);

            List<MapBlock> fishingBlocks = CollectFishingBlocks(startCoord, direction, session.Player.MapId);
            if (fishingBlocks.Count == 0)
            {
                session.Send(FishingPacket.Notice((short) FishingNotice.CanOnlyFishNearWater));
                return;
            }
            session.Player.FishingRod = fishingRod;

            // Adding GuideObject
            CoordF guideBlock = GetObjectBlock(fishingBlocks, session.FieldPlayer.Coord);
            guideBlock.Z += Block.BLOCK_SIZE; // sits on top of the block
            GuideObject guide = new GuideObject();
            IFieldObject<GuideObject> fieldGuide = session.FieldManager.RequestFieldObject(guide);
            fieldGuide.Coord = guideBlock;
            session.Player.Guide = fieldGuide;
            session.FieldManager.AddGuide(fieldGuide);

            session.Send(FishingPacket.LoadFishTiles(fishingBlocks, rodMetadata.ReduceTime));
            session.FieldManager.BroadcastPacket(GuideObjectPacket.Add(session.FieldPlayer, 1));
            session.Send(FishingPacket.PrepareFishing(fishingRodUid));
        }

        private static CoordF GetObjectBlock(List<MapBlock> blocks, CoordF playerCoord)
        {
            MapBlock guideBlock = blocks.OrderBy(o => Math.Sqrt(Math.Pow((playerCoord.X - o.Coord.X), 2) + Math.Pow((playerCoord.Y - o.Coord.Y), 2))).First();
            return guideBlock.Coord.ToFloat();
        }

        private static List<MapBlock> CollectFishingBlocks(CoordF startCoord, int direction, int mapId)
        {
            List<MapBlock> blocks = new List<MapBlock>();

            startCoord.Z -= Block.BLOCK_SIZE;
            CoordF checkBlock = startCoord;
            if (direction == Direction.NORTH_EAST)
            {
                checkBlock.Y += (2 * Block.BLOCK_SIZE); // start at the corner

                for (int yAxis = 0; yAxis < 5; yAxis++)
                {
                    for (int xAxis = 0; xAxis < 3; xAxis++)
                    {
                        checkBlock.X += Block.BLOCK_SIZE;

                        for (int zAxis = 0; zAxis < 3; zAxis++)
                        {

                            if (MapMetadataStorage.BlockAboveExists(mapId, checkBlock.ToShort()))
                            {
                                break;
                            }

                            MapBlock block = MapMetadataStorage.GetMapBlock(mapId, checkBlock.ToShort());
                            if (block == null || !IsLiquidBlock(block))
                            {
                                checkBlock.Z -= Block.BLOCK_SIZE;
                                continue;
                            }

                            blocks.Add(block);
                            checkBlock.Z -= Block.BLOCK_SIZE;
                        }
                        checkBlock.Z = startCoord.Z; // reset Z
                    }
                    checkBlock.Y -= Block.BLOCK_SIZE;
                    checkBlock.X = startCoord.X; // reset X
                }
            }
            else if (direction == Direction.NORTH_WEST)
            {
                checkBlock.X += (2 * Block.BLOCK_SIZE); // start at the corner

                for (int xAxis = 0; xAxis < 5; xAxis++)
                {
                    for (int yAxis = 0; yAxis < 3; yAxis++)
                    {
                        checkBlock.Y += Block.BLOCK_SIZE;
                        for (int zAxis = 0; zAxis < 3; zAxis++)
                        {

                            if (MapMetadataStorage.BlockAboveExists(mapId, checkBlock.ToShort()))
                            {
                                break;
                            }

                            MapBlock block = MapMetadataStorage.GetMapBlock(mapId, checkBlock.ToShort());
                            if (block == null || !IsLiquidBlock(block))
                            {
                                checkBlock.Z -= Block.BLOCK_SIZE;
                                continue;
                            }

                            blocks.Add(block);
                            checkBlock.Z -= Block.BLOCK_SIZE;
                        }
                        checkBlock.Z = startCoord.Z; // reset Z
                    }
                    checkBlock.X -= Block.BLOCK_SIZE;
                    checkBlock.Y = startCoord.Y; // reset Y
                }
            }
            else if (direction == Direction.SOUTH_WEST)
            {
                checkBlock.Y -= (2 * Block.BLOCK_SIZE); // start at the corner

                for (int yAxis = 0; yAxis < 5; yAxis++)
                {
                    for (int xAxis = 0; xAxis < 3; xAxis++)
                    {
                        checkBlock.X -= Block.BLOCK_SIZE;
                        for (int zAxis = 0; zAxis < 3; zAxis++)
                        {

                            if (MapMetadataStorage.BlockAboveExists(mapId, checkBlock.ToShort()))
                            {
                                break;
                            }

                            MapBlock block = MapMetadataStorage.GetMapBlock(mapId, checkBlock.ToShort());
                            if (block == null || !IsLiquidBlock(block))
                            {
                                checkBlock.Z -= Block.BLOCK_SIZE;
                                continue;
                            }

                            blocks.Add(block);
                            checkBlock.Z -= Block.BLOCK_SIZE;
                        }
                        checkBlock.Z = startCoord.Z; // reset Z
                    }
                    checkBlock.Y += Block.BLOCK_SIZE;
                    checkBlock.X = startCoord.X; // reset X
                }
            }
            else if (direction == Direction.SOUTH_EAST)
            {
                checkBlock.X -= (2 * Block.BLOCK_SIZE); // start at the corner

                for (int xAxis = 0; xAxis < 5; xAxis++)
                {
                    for (int yAxis = 0; yAxis < 3; yAxis++)
                    {
                        checkBlock.Y -= Block.BLOCK_SIZE;
                        for (int zAxis = 0; zAxis < 3; zAxis++)
                        {

                            if (MapMetadataStorage.BlockAboveExists(mapId, checkBlock.ToShort()))
                            {
                                break;
                            }

                            MapBlock block = MapMetadataStorage.GetMapBlock(mapId, checkBlock.ToShort());
                            if (block == null || !IsLiquidBlock(block))
                            {
                                checkBlock.Z -= Block.BLOCK_SIZE;
                                continue;
                            }

                            blocks.Add(block);
                            checkBlock.Z -= Block.BLOCK_SIZE;
                        }
                        checkBlock.Z = startCoord.Z; // reset Z
                    }
                    checkBlock.X += Block.BLOCK_SIZE;
                    checkBlock.Y = startCoord.Y; // reset Y
                }
            }
            return blocks;
        }

        private static bool IsLiquidBlock(MapBlock block)
        {
            if (block.Type == "Ground")
            {
                return false;
            }

            if (block.Attribute == "water" ||
                block.Attribute == "seawater" ||
                block.Attribute == "devilwater" ||
                block.Attribute == "lava" ||
                block.Attribute == "poison" ||
                block.Attribute == "oil" ||
                block.Attribute == "emeraldwater")
            {
                return true;
            }
            return false;
        }

        private static void HandleStop(GameSession session)
        {
            session.Send(FishingPacket.Stop());
            session.FieldManager.BroadcastPacket(GuideObjectPacket.Remove(session.FieldPlayer));
            session.FieldManager.RemoveGuide(session.FieldPlayer.Value.Guide);
            session.Player.Guide = null; // remove guide from player
        }

        private static void HandleCatch(GameSession session, PacketReader packet)
        {
            bool success = packet.ReadBool();

            CoordF guideBlock = Block.ClosestBlock(session.Player.Guide.Coord);
            guideBlock.Z -= Block.BLOCK_SIZE; // get liquid block coord
            MapBlock block = MapMetadataStorage.GetMapBlock(session.Player.MapId, guideBlock.ToShort());
            List<FishMetadata> fishes = FishMetadataStorage.GetValidFishes(session.Player.MapId, block.Attribute);

            //determine fish rarity
            List<FishMetadata> selectedFishRarities = FilterFishesByRarity(fishes);

            Random rnd = new Random();
            int randomFishIndex = rnd.Next(selectedFishRarities.Count);
            FishMetadata fish = selectedFishRarities[randomFishIndex];

            //determine fish size
            int fishSize = rnd.NextDouble() switch
            {
                >= 0.0 and < 0.03 => rnd.Next(fish.SmallSize[0], fish.SmallSize[1]),
                >= 0.03 and < 0.15 => rnd.Next((fish.SmallSize[1]), (fish.BigSize[0])),
                >= 0.15 => rnd.Next(fish.SmallSize[0], fish.SmallSize[1]),
                _ => rnd.Next(fish.SmallSize[0], fish.SmallSize[1]),
            };

            if (success)
            {
                if (session.Player.FishAlbum.ContainsKey(fish.Id))
                {
                    session.Player.FishAlbum[fish.Id].AddExistingFish(session, fish, fishSize);
                }
                else
                {
                    session.Player.FishAlbum[fish.Id] = new Fishing();
                    session.Player.FishAlbum[fish.Id].AddNewFish(session, fish, fishSize);
                }

                session.Send(FishingPacket.CatchFish(session.Player, fish, fishSize, true));
                HandleCatchItem(session);
                session.Player.Levels.GainMasteryExp(MasteryType.Fishing, fish.Rarity);
            }
            else
            {
                session.Send(FishingPacket.CatchFish(session.Player, fish, fishSize, false));
            }
        }

        private static List<FishMetadata> FilterFishesByRarity(List<FishMetadata> fishes)
        {
            Random rnd = new Random();
            List<FishMetadata> selectedFishRarities = new List<FishMetadata>();
            int fishRarity;
            do // re-rolls until there is an acceptable rarity
            {
                fishRarity = rnd.NextDouble() switch
                {
                    >= 0 and < 0.60 => 1,
                    >= 0.60 and < 0.85 => 2,
                    >= 0.85 and < 0.95 => 3,
                    >= 0.95 => 4,
                    _ => 1,
                };

                selectedFishRarities = fishes.Where(x => x.Rarity == fishRarity).ToList();
            } while (selectedFishRarities.Count == 0);

            return selectedFishRarities;
        }

        private static void HandleStart(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();
            CoordS fishingBlock = coord.ToShort();
            MapBlock block = MapMetadataStorage.GetMapBlock(session.Player.MapId, fishingBlock);
            if (block == null)
            {
                return;
            }

            if (!IsLiquidBlock(block))
            {
                return;
            }

            List<FishMetadata> fishes = FishMetadataStorage.GetValidFishes(session.Player.MapId, block.Attribute);

            bool minigame = false;
            int fishingTick = 15000; // base fishing tick

            Random rnd = new Random();

            int successChance = rnd.Next(0, 100);
            if (successChance < 90)
            {
                int minigameChance = rnd.Next(0, 100);
                if (minigameChance < 20)
                {
                    minigame = true;
                }
                FishingRodMetadata rodMetadata = FishingRodMetadataStorage.GetMetadata(session.Player.FishingRod.Function.Id);
                int rodFishingTick = fishingTick - rodMetadata.ReduceTime; // 10000 is the base tick for fishing
                fishingTick = rnd.Next(rodFishingTick - (rodFishingTick / 3), rodFishingTick); // chance for early catch
            }
            else
            {
                session.Send(NoticePacket.Notice("This will fail"));
                fishingTick = 20000; // if tick is over the base fishing tick, it will fail
            }

            session.Send(FishingPacket.Start((session.ClientTick + fishingTick), minigame));
        }

        private static void HandleFailMinigame(GameSession session, PacketReader packet)
        {
            // nothing happens?
        }

        private static void HandleCatchItem(GameSession session)
        {
            // hardcoding the catch items for now
            Random rnd = new Random();

            int itemChance = rnd.Next(0, 100);
            if (itemChance > 10)
            {
                return;
            }

            List<int> trash = new List<int>()
            {
                30000487,
                30000488,
                30000489,
                30000490,
                30000491,
            };

            List<int> lightBoxes = new List<int>()
            {
                59200199,
                59200201
            };

            List<int> heavyBoxes = new List<int>()
            {
                59200200,
                59200202
            };

            List<int> skins = new List<int>()
            {
                15100141,
                13400141,
                15000148,
                15300141,
                13200144,
                14100132,
                13300143,
                15400109,
                15600504,
                13100148,
                15200147,
                11300424
            };

            string itemGroup = rnd.NextDouble() switch
            {
                >= 0 and < 0.825 => "trash",
                >= 0.825 and < 0.975 => "lightBox",
                >= 0.975 and < 0.995 => "heavyBox",
                >= 0.95 => "skins",
                _ => "trash",
            };

            List<int> selectedGroup = new List<int>();
            switch (itemGroup)
            {
                case "trash":
                    selectedGroup.AddRange(trash);
                    break;
                case "lightBox":
                    selectedGroup.AddRange(lightBoxes);
                    break;
                case "heavyBox":
                    selectedGroup.AddRange(heavyBoxes);
                    break;
                case "skins":
                    selectedGroup.AddRange(skins);
                    break;
            }

            int selectItemIndex = rnd.Next(selectedGroup.Count);

            Item item = new Item(selectedGroup[selectItemIndex]) { };
            List<Item> items = new List<Item>() { item };
            session.Send(FishingPacket.CatchItem(items));
            InventoryController.Add(session, item, true);
        }
    }
}
