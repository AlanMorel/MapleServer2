using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class FishingHandler : GamePacketHandler<FishingHandler>
{
    public override RecvOp OpCode => RecvOp.Fishing;

    private enum Mode : byte
    {
        PrepareFishing = 0x0,
        Stop = 0x1,
        Catch = 0x8,
        Start = 0x9,
        FailMinigame = 0xA
    }

    private enum FishingNotice : short
    {
        CanOnlyFishNearWater = 0x1,
        InvalidFishingRod = 0x2,
        MasteryTooLowForMap = 0x3,
        CannotFishHere = 0x4,
        MasteryTooLowForRod = 0x6,
        GearOrMiscInventoryFull = 0x7
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.PrepareFishing:
                HandlePrepareFishing(session, packet);
                break;
            case Mode.Stop:
                HandleStop(session);
                break;
            case Mode.Catch:
                HandleCatch(session, packet);
                break;
            case Mode.Start:
                HandleStart(session, packet);
                break;
            case Mode.FailMinigame:
                HandleFailMinigame();
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandlePrepareFishing(GameSession session, PacketReader packet)
    {
        long fishingRodUid = packet.ReadLong();
        MasteryExp masteryExp = session.Player.Levels.MasteryExp.FirstOrDefault(x => x.Type == MasteryType.Fishing);

        if (!FishingSpotMetadataStorage.CanFish(session.Player.MapId, masteryExp.CurrentExp))
        {
            session.Send(FishingPacket.Notice((short) FishingNotice.MasteryTooLowForMap));
            return;
        }

        if (!session.Player.Inventory.HasItem(fishingRodUid))
        {
            session.Send(FishingPacket.Notice((short) FishingNotice.InvalidFishingRod));
            return;
        }

        Item fishingRod = session.Player.Inventory.GetByUid(fishingRodUid);
        FishingRodMetadata rodMetadata = FishingRodMetadataStorage.GetMetadata(fishingRod.Function.Id);

        if (rodMetadata.MasteryLimit < masteryExp.CurrentExp)
        {
            session.Send(FishingPacket.Notice((short) FishingNotice.MasteryTooLowForRod));
        }

        int direction = Direction.GetClosestDirection(session.Player.FieldPlayer.Rotation);
        CoordF startCoord = Block.ClosestBlock(session.Player.FieldPlayer.Coord);

        List<MapBlock> fishingBlocks = CollectFishingBlocks(startCoord, direction, session.Player.MapId);
        if (fishingBlocks.Count == 0)
        {
            session.Send(FishingPacket.Notice((short) FishingNotice.CanOnlyFishNearWater));
            return;
        }
        session.Player.FishingRod = fishingRod;

        // Adding GuideObject
        CoordF guideBlock = GetObjectBlock(fishingBlocks, session.Player.FieldPlayer.Coord);
        guideBlock.Z += Block.BLOCK_SIZE; // sits on top of the block
        GuideObject guide = new(1, session.Player.CharacterId);
        IFieldObject<GuideObject> fieldGuide = session.FieldManager.RequestFieldObject(guide);
        fieldGuide.Coord = guideBlock;
        session.Player.Guide = fieldGuide;
        session.FieldManager.AddGuide(fieldGuide);

        session.Send(FishingPacket.LoadFishTiles(fishingBlocks, rodMetadata.ReduceTime));
        session.FieldManager.BroadcastPacket(GuideObjectPacket.Add(fieldGuide));
        session.Send(FishingPacket.PrepareFishing(fishingRodUid));
    }

    private static CoordF GetObjectBlock(IEnumerable<MapBlock> blocks, CoordF playerCoord)
    {
        MapBlock guideBlock = blocks.OrderBy(o => Math.Sqrt(Math.Pow(playerCoord.X - o.Coord.X, 2) + Math.Pow(playerCoord.Y - o.Coord.Y, 2))).First();
        return guideBlock.Coord.ToFloat();
    }

    private static List<MapBlock> CollectFishingBlocks(CoordF startCoord, int direction, int mapId)
    {
        List<MapBlock> blocks = new();

        startCoord.Z -= Block.BLOCK_SIZE;
        CoordF checkBlock = startCoord;
        if (direction == Direction.NORTH_EAST)
        {
            checkBlock.Y += 2 * Block.BLOCK_SIZE; // start at the corner

            for (int yAxis = 0; yAxis < 5; yAxis++)
            {
                for (int xAxis = 0; xAxis < 3; xAxis++)
                {
                    checkBlock.X += Block.BLOCK_SIZE;

                    MapBlock block = ScanZAxisForLiquidBlock(checkBlock, mapId);
                    if (block != null)
                    {
                        blocks.Add(block);
                    }
                }
                checkBlock.Y -= Block.BLOCK_SIZE;
                checkBlock.X = startCoord.X; // reset X
            }
        }
        else if (direction == Direction.NORTH_WEST)
        {
            checkBlock.X += 2 * Block.BLOCK_SIZE; // start at the corner

            for (int xAxis = 0; xAxis < 5; xAxis++)
            {
                for (int yAxis = 0; yAxis < 3; yAxis++)
                {
                    checkBlock.Y += Block.BLOCK_SIZE;
                    MapBlock block = ScanZAxisForLiquidBlock(checkBlock, mapId);
                    if (block != null)
                    {
                        blocks.Add(block);
                    }
                }
                checkBlock.X -= Block.BLOCK_SIZE;
                checkBlock.Y = startCoord.Y; // reset Y
            }
        }
        else if (direction == Direction.SOUTH_WEST)
        {
            checkBlock.Y -= 2 * Block.BLOCK_SIZE; // start at the corner

            for (int yAxis = 0; yAxis < 5; yAxis++)
            {
                for (int xAxis = 0; xAxis < 3; xAxis++)
                {
                    checkBlock.X -= Block.BLOCK_SIZE;

                    MapBlock block = ScanZAxisForLiquidBlock(checkBlock, mapId);
                    if (block != null)
                    {
                        blocks.Add(block);
                    }
                }
                checkBlock.Y += Block.BLOCK_SIZE;
                checkBlock.X = startCoord.X; // reset X
            }
        }
        else if (direction == Direction.SOUTH_EAST)
        {
            checkBlock.X -= 2 * Block.BLOCK_SIZE; // start at the corner

            for (int xAxis = 0; xAxis < 5; xAxis++)
            {
                for (int yAxis = 0; yAxis < 3; yAxis++)
                {
                    checkBlock.Y -= Block.BLOCK_SIZE;

                    MapBlock block = ScanZAxisForLiquidBlock(checkBlock, mapId);
                    if (block != null)
                    {
                        blocks.Add(block);
                    }
                }
                checkBlock.X += Block.BLOCK_SIZE;
                checkBlock.Y = startCoord.Y; // reset Y
            }
        }
        return blocks;
    }

    private static MapBlock ScanZAxisForLiquidBlock(CoordF checkBlock, int mapId)
    {
        for (int zAxis = 0; zAxis < 3; zAxis++)
        {
            if (MapMetadataStorage.BlockAboveExists(mapId, checkBlock.ToShort()))
            {
                return null;
            }

            MapBlock block = MapMetadataStorage.GetMapBlock(mapId, checkBlock.ToShort());
            if (block == null || !MapMetadataStorage.IsLiquidBlock(block))
            {
                checkBlock.Z -= Block.BLOCK_SIZE;
                continue;
            }

            return block;
        }
        return null;
    }

    private static void HandleStop(GameSession session)
    {
        session.Send(FishingPacket.Stop());
        session.FieldManager.BroadcastPacket(GuideObjectPacket.Remove(session.Player.Guide));
        session.FieldManager.RemoveGuide(session.Player.Guide);
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

        Random rnd = Random.Shared;
        int randomFishIndex = rnd.Next(selectedFishRarities.Count);
        FishMetadata fish = selectedFishRarities[randomFishIndex];

        //determine fish size
        int fishSize = rnd.NextDouble() switch
        {
            >= 0.0 and < 0.03 => rnd.Next(fish.SmallSize[0], fish.SmallSize[1]),
            >= 0.03 and < 0.15 => rnd.Next(fish.SmallSize[1], fish.BigSize[0]),
            >= 0.15 => rnd.Next(fish.SmallSize[0], fish.SmallSize[1]),
            _ => rnd.Next(fish.SmallSize[0], fish.SmallSize[1])
        };

        if (success)
        {
            if (session.Player.FishAlbum.ContainsKey(fish.Id))
            {
                Fishing.AddExistingFish(session, fish, fishSize);
            }
            else
            {
                Fishing.AddNewFish(session, fish, fishSize);
            }

            session.Send(FishingPacket.CatchFish(session.Player, fish, fishSize, true));
            HandleCatchItem(session);
            session.Player.Levels.GainMasteryExp(MasteryType.Fishing, fish.Rarity);
            return;
        }

        session.Send(FishingPacket.CatchFish(session.Player, fish, fishSize, false));
    }

    private static List<FishMetadata> FilterFishesByRarity(List<FishMetadata> fishes)
    {
        List<FishMetadata> selectedFishRarities = new();
        int fishRarity;
        do // re-rolls until there is an acceptable rarity
        {
            fishRarity = Random.Shared.NextDouble() switch
            {
                >= 0 and < 0.60 => 1,
                >= 0.60 and < 0.85 => 2,
                >= 0.85 and < 0.95 => 3,
                >= 0.95 => 4,
                _ => 1
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
        if (block == null || !MapMetadataStorage.IsLiquidBlock(block))
        {
            return;
        }

        List<FishMetadata> fishes = FishMetadataStorage.GetValidFishes(session.Player.MapId, block.Attribute);

        bool minigame = false;
        int fishingTick = 15000; // base fishing tick

        Random rnd = Random.Shared;

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
            fishingTick = rnd.Next(rodFishingTick - rodFishingTick / 3, rodFishingTick); // chance for early catch
        }
        else
        {
            fishingTick = 20000; // if tick is over the base fishing tick, it will fail
        }
        session.Send(FishingPacket.Start(session.ServerTick + fishingTick, minigame));
    }

    private static void HandleFailMinigame()
    {
        // nothing happens?
    }

    private static void HandleCatchItem(GameSession session)
    {
        Random rnd = Random.Shared;

        int itemChance = rnd.Next(0, 100);
        if (itemChance > 10)
        {
            return;
        }

        FishingItemType type = rnd.NextDouble() switch
        {
            >= 0 and < 0.825 => FishingItemType.Trash,
            >= 0.825 and < 0.975 => FishingItemType.LightBox,
            >= 0.975 and < 0.995 => FishingItemType.HeavyBox,
            >= 0.95 => FishingItemType.Skin,
            _ => FishingItemType.Trash
        };

        FishingRewardItem fishingItem = FishingRewardsMetadataStorage.GetFishingRewardItem(type);

        Item item = new(fishingItem.Id, fishingItem.Amount, fishingItem.Rarity);
        List<Item> items = new()
        {
            item
        };
        session.Send(FishingPacket.CatchItem(items));
        session.Player.Inventory.AddItem(session, item, true);
    }
}
