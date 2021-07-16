using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestCubeHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_CUBE;

        public RequestCubeHandler(ILogger<RequestCubeHandler> logger) : base(logger) { }

        private enum RequestCubeMode : byte
        {
            LoadFurnishingItem = 0x1,
            BuyPlot = 0x2,
            ForfeitPlot = 0x6,
            HandleAddFurnishing = 0xA,
            RemoveCube = 0xC,
            RotateCube = 0xE,
            ReplaceCube = 0xF,
            Pickup = 0x11,
            Drop = 0x12,
            HomeName = 0x15,
            HomePassword = 0x18,
            NominateHouse = 0x19,
            HomeDescription = 0x1D,
            EnablePermission = 0x2A,
            SetPermission = 0x2B,
            IncreaseSize = 0x25,
            DecreaseSize = 0x26,
            IncreaseHeight = 0x2C,
            DecreaseHeight = 0x2D,
            KickEveryone = 0x31,
            ChangeBackground = 0x33,
            ChangeLighting = 0x34,
            ChangeCamera = 0x36,
            GiveBuildingPermission = 0x39,
            RemoveBuildingPermission = 0x3A,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            RequestCubeMode mode = (RequestCubeMode) packet.ReadByte();

            switch (mode)
            {
                case RequestCubeMode.LoadFurnishingItem:
                    HandleLoadFurnishingItem(session, packet);
                    break;
                case RequestCubeMode.BuyPlot:
                    HandleBuyPlot(session, packet);
                    break;
                case RequestCubeMode.ForfeitPlot:
                    HandleForfeitPlot(session);
                    break;
                case RequestCubeMode.HandleAddFurnishing:
                    HandleAddFurnishing(session, packet);
                    break;
                case RequestCubeMode.RemoveCube:
                    HandleRemoveCube(session, packet);
                    break;
                case RequestCubeMode.RotateCube:
                    HandleRotateCube(session, packet);
                    break;
                case RequestCubeMode.ReplaceCube:
                    HandleReplaceCube(session, packet);
                    break;
                case RequestCubeMode.Pickup:
                    HandlePickup(session, packet);
                    break;
                case RequestCubeMode.Drop:
                    HandleDrop(session);
                    break;
                case RequestCubeMode.HomeName:
                    HandleHomeName(session, packet);
                    break;
                case RequestCubeMode.HomePassword:
                    HandleHomePassword(session, packet);
                    break;
                case RequestCubeMode.NominateHouse:
                    HandleNominateHouse(session);
                    break;
                case RequestCubeMode.HomeDescription:
                    HandleHomeDescription(session, packet);
                    break;
                case RequestCubeMode.IncreaseSize:
                case RequestCubeMode.DecreaseSize:
                case RequestCubeMode.IncreaseHeight:
                case RequestCubeMode.DecreaseHeight:
                    HandleModifySize(session, mode);
                    break;
                case RequestCubeMode.KickEveryone:
                    HandleKickEveryone(session);
                    break;
                case RequestCubeMode.ChangeLighting:
                case RequestCubeMode.ChangeBackground:
                case RequestCubeMode.ChangeCamera:
                    HandleModifyInteriorSettings(session, mode, packet);
                    break;
                case RequestCubeMode.EnablePermission:
                    HandleEnablePermission(session, packet);
                    break;
                case RequestCubeMode.SetPermission:
                    HandleSetPermission(session, packet);
                    break;
                case RequestCubeMode.GiveBuildingPermission:
                    HandleGiveBuildingPermission(session, packet);
                    break;
                case RequestCubeMode.RemoveBuildingPermission:
                    HandleRemoveBuildingPermission(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleLoadFurnishingItem(GameSession session, PacketReader packet)
        {
            int itemId = packet.ReadInt();
            long itemUid = packet.ReadLong();
            session.FieldManager.BroadcastPacket(ResponseCubePacket.LoadFurnishingItem(session.FieldPlayer, itemId, itemUid));
        }

        private static void HandleBuyPlot(GameSession session, PacketReader packet)
        {
            int groupId = packet.ReadInt();
            int homeTemplate = packet.ReadInt();
            Player player = session.Player;

            if (player.Account.Home != null && player.Account.Home.PlotId != 0)
            {
                return;
            }

            UGCMapGroup land = UGCMapMetadataStorage.GetGroupMetadata(player.MapId, (byte) groupId);
            if (land == null)
            {
                return;
            }

            //Check if sale event is active
            int price = land.Price;
            GameEvent gameEvent = DatabaseManager.GetSingleGameEvent(GameEventType.UGCMapContractSale);
            if (gameEvent != null)
            {
                int markdown = land.Price * (gameEvent.UGCMapContractSale.DiscountAmount / 100 / 100);
                price = land.Price - markdown;
            }

            if (!HandlePlotPayment(session, land.PriceItemCode, price))
            {
                session.SendNotice("You don't enough mesos!");
                return;
            }

            if (player.Account.Home == null)
            {
                player.Account.Home = new Home(player.Account, player.Name, homeTemplate)
                {
                    PlotId = player.MapId,
                    PlotNumber = land.Id,
                    Expiration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount + (land.ContractDate * (24 * 60 * 60)),
                    Name = player.Name,
                };
                GameServer.HomeManager.AddHome(player.Account.Home);
            }
            else
            {
                player.Account.Home.PlotId = player.MapId;
                player.Account.Home.PlotNumber = land.Id;
                player.Account.Home.Expiration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount + (land.ContractDate * (24 * 60 * 60));

                GameServer.HomeManager.GetHome(player.Account.Home.Id).PlotId = player.Account.Home.PlotId;
                GameServer.HomeManager.GetHome(player.Account.Home.Id).PlotNumber = player.Account.Home.PlotNumber;
                GameServer.HomeManager.GetHome(player.Account.Home.Id).Expiration = player.Account.Home.Expiration;
            }

            session.FieldManager.BroadcastPacket(ResponseCubePacket.PurchasePlot(player.Account.Home.PlotNumber, 0, player.Account.Home.Expiration));
            session.FieldManager.BroadcastPacket(ResponseCubePacket.EnablePlotFurnishing(player));
            session.Send(ResponseCubePacket.LoadHome(session.FieldPlayer));
            session.FieldManager.BroadcastPacket(ResponseCubePacket.HomeName(player), session);
            session.Send(ResponseCubePacket.CompletePurchase());
        }

        private static void HandleForfeitPlot(GameSession session)
        {
            Player player = session.Player;
            if (player.Account.Home == null || player.Account.Home.PlotId == 0)
            {
                return;
            }
            int plotId = player.Account.Home.PlotId;
            int plotNumber = player.Account.Home.PlotNumber;
            int apartmentNumber = player.Account.Home.ApartmentNumber;
            long expiration = player.Account.Home.Expiration;

            player.Account.Home.PlotId = 0;
            player.Account.Home.PlotNumber = 0;
            player.Account.Home.ApartmentNumber = 0;
            player.Account.Home.Expiration = 32503561200; // year 2999

            GameServer.HomeManager.GetHome(player.Account.Home.Id).PlotId = 0;
            GameServer.HomeManager.GetHome(player.Account.Home.Id).PlotNumber = 0;
            GameServer.HomeManager.GetHome(player.Account.Home.Id).ApartmentNumber = 0;
            GameServer.HomeManager.GetHome(player.Account.Home.Id).Expiration = 32503561200; // year 2999

            session.Send(ResponseCubePacket.ForfeitPlot(plotId, plotNumber, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
            session.Send(ResponseCubePacket.RemovePlot(plotNumber, apartmentNumber));
            session.Send(ResponseCubePacket.LoadHome(session.FieldPlayer));
            session.Send(ResponseCubePacket.RemovePlot2(plotId, plotNumber));
            // 54 00 0E 01 00 00 00 01 01 00 00 00, send mail
        }

        private static void HandleAddFurnishing(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();
            byte padding = packet.ReadByte();
            int itemId = packet.ReadInt();
            long itemUid = packet.ReadLong();
            byte padding2 = packet.ReadByte();
            CoordF rotation = packet.Read<CoordF>();

            CoordF coordF = coord.ToFloat();
            Player player = session.Player;

            bool isCubeProp = ItemMetadataStorage.GetCubeProp(itemId);
            int plotNumber = MapMetadataStorage.GetPlotNumber(player.MapId, coord);
            if (plotNumber <= 0)
            {
                session.Send(ResponseCubePacket.CantPlaceHere(session.FieldPlayer));
                return;
            }

            bool playerInPrivateResidence = player.MapId == (int) Map.PrivateResidence;

            Home home = playerInPrivateResidence ? GameServer.HomeManager.GetHome(player.VisitingHomeId) : GameServer.HomeManager.GetHome(player.Account.Home.Id);
            UGCMapGroup plot = UGCMapMetadataStorage.GetGroupMetadata(player.MapId, (byte) plotNumber);

            byte height = playerInPrivateResidence ? home.Height : plot.HeightLimit;
            int size = playerInPrivateResidence ? home.Size : plot.Area / 2;
            if (IsCoordOutsideHeightLimit(coord.ToShort(), player.MapId, height) || (playerInPrivateResidence && IsCoordOutsideSizeLimit(coord, size)))
            {
                session.Send(ResponseCubePacket.CantPlaceHere(session.FieldPlayer));
                return;
            }

            FurnishingShopMetadata shopMetadata = FurnishingShopMetadataStorage.GetMetadata(itemId);
            if (shopMetadata == null || !shopMetadata.Buyable)
            {
                return;
            }

            if (session.Player.Account.Id != home.AccountId && !home.BuildingPermissions.Contains(session.Player.Account.Id))
            {
                return;
            }

            IFieldObject<Cube> fieldCube;
            Dictionary<long, Item> warehouseItems = home.WarehouseInventory;
            Dictionary<long, Cube> furnishingInventory = home.FurnishingInventory;
            if (itemUid == 0 || !warehouseItems.ContainsKey(itemUid))
            {
                if (!PurchaseFurnishingItem(session, shopMetadata))
                {
                    return;
                }
                Item item = new Item(itemId);
                Cube cube = new Cube(item, plotNumber, coordF, rotation);

                fieldCube = session.FieldManager.RequestFieldObject(cube);
                fieldCube.Coord = coordF;
                fieldCube.Rotation = rotation;

                furnishingInventory.Add(cube.Uid, cube);

                session.Send(WarehouseInventoryPacket.Load(item, warehouseItems.Values.Count));
                session.Send(WarehouseInventoryPacket.GainItemMessage(item, 1));
                session.Send(WarehouseInventoryPacket.Count(warehouseItems.Values.Count + 1));
                session.Send(ResponseCubePacket.PlaceFurnishing(fieldCube, session.FieldPlayer, sendOnlyObjectId: true));
                session.Send(WarehouseInventoryPacket.Remove(item.Uid));
            }
            else
            {
                Item item = warehouseItems[itemUid];
                Cube cube = new Cube(item, plotNumber, coordF, rotation);

                fieldCube = session.FieldManager.RequestFieldObject(cube);
                fieldCube.Coord = coordF;
                fieldCube.Rotation = rotation;

                furnishingInventory.Add(cube.Uid, cube);

                if (item.Amount - 1 > 0)
                {
                    item.Amount--;
                    session.Send(WarehouseInventoryPacket.UpdateAmount(item.Uid, item.Amount));
                }
                else
                {
                    warehouseItems.Remove(itemUid);
                    session.Send(WarehouseInventoryPacket.Remove(item.Uid));
                }
            }

            session.Send(FurnishingInventoryPacket.Load(fieldCube.Value));
            session.FieldManager.AddCube(fieldCube, session.FieldPlayer);
        }

        private static void HandleRemoveCube(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();
            Player player = session.Player;

            bool playerInPrivateResidence = player.MapId == (int) Map.PrivateResidence;
            Home home = playerInPrivateResidence ? GameServer.HomeManager.GetHome(player.VisitingHomeId) : GameServer.HomeManager.GetHome(player.Account.Home.Id);
            if (session.Player.Account.Id != home.AccountId && !home.BuildingPermissions.Contains(session.Player.Account.Id))
            {
                return;
            }

            IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord.ToFloat());
            if (cube == default || cube.Value.Item == null)
            {
                return;
            }

            RemoveCube(session, cube, home);
        }

        private static void HandleRotateCube(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();
            Player player = session.Player;

            bool playerInPrivateResidence = player.MapId == (int) Map.PrivateResidence;
            Home home = playerInPrivateResidence ? GameServer.HomeManager.GetHome(player.VisitingHomeId) : GameServer.HomeManager.GetHome(player.Account.Home.Id);
            if (session.Player.Account.Id != home.AccountId && !home.BuildingPermissions.Contains(session.Player.Account.Id))
            {
                return;
            }

            IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord.ToFloat());
            if (cube == default)
            {
                return;
            }

            cube.Rotation -= CoordF.From(0, 0, 90);
            home.FurnishingInventory[cube.Value.Uid].Rotation = cube.Rotation;

            session.Send(ResponseCubePacket.RotateCube(session.FieldPlayer, cube));
        }

        private static void HandleReplaceCube(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();
            packet.Skip(1);
            int replacementItemId = packet.ReadInt();
            long replacementItemUid = packet.ReadLong();
            byte unk = packet.ReadByte();
            long unk2 = packet.ReadLong(); // maybe part of rotation?
            float zRotation = packet.ReadFloat();
            CoordF rotation = new CoordF();
            rotation.Z = zRotation;

            Player player = session.Player;

            int plotNumber = MapMetadataStorage.GetPlotNumber(player.MapId, coord);
            if (plotNumber <= 0)
            {
                session.Send(ResponseCubePacket.CantPlaceHere(session.FieldPlayer));
                return;
            }

            bool playerInPrivateResidence = player.MapId == (int) Map.PrivateResidence;
            Home home = playerInPrivateResidence ? GameServer.HomeManager.GetHome(player.VisitingHomeId) : GameServer.HomeManager.GetHome(player.Account.Home.Id);
            if (session.Player.Account.Id != home.AccountId && !home.BuildingPermissions.Contains(session.Player.Account.Id))
            {
                return;
            }

            bool isCubeProp = ItemMetadataStorage.GetCubeProp(replacementItemId);
            UGCMapGroup plot = UGCMapMetadataStorage.GetGroupMetadata(player.MapId, (byte) plotNumber);

            byte height = playerInPrivateResidence ? home.Height : plot.HeightLimit;
            int size = playerInPrivateResidence ? home.Size : plot.Area / 2;
            CoordB groundHeight = GetGroundCoord(coord, player.MapId, height);
            if (groundHeight == default)
            {
                return;
            }
            if (!isCubeProp && coord.Z == groundHeight.Z)
            {
                return;
            }

            if (IsCoordOutsideHeightLimit(coord.ToShort(), player.MapId, height) || (playerInPrivateResidence && IsCoordOutsideSizeLimit(coord, size)))
            {
                session.Send(ResponseCubePacket.CantPlaceHere(session.FieldPlayer));
                return;
            }

            FurnishingShopMetadata shopMetadata = FurnishingShopMetadataStorage.GetMetadata(replacementItemId);
            if (shopMetadata == null || !shopMetadata.Buyable)
            {
                return;
            }

            // Not checking if oldFieldCube is null on ground height because of default blocks.
            IFieldObject<Cube> oldFieldCube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord.ToFloat());
            if (oldFieldCube == default && coord.Z != groundHeight.Z)
            {
                return;
            }

            IFieldObject<Cube> newFieldCube;
            Item item;

            Dictionary<long, Item> warehouseItems = home.WarehouseInventory;
            Dictionary<long, Cube> furnishingInventory = home.FurnishingInventory;
            if (!warehouseItems.ContainsKey(replacementItemUid) || warehouseItems[replacementItemUid].Amount <= 0)
            {
                if (!PurchaseFurnishingItem(session, shopMetadata))
                {
                    return;
                }
                item = new Item(replacementItemId);
                Cube cube = new Cube(item, plotNumber, coord.ToFloat(), rotation);

                newFieldCube = session.FieldManager.RequestFieldObject(cube);
                newFieldCube.Coord = coord.ToFloat();
                newFieldCube.Rotation = rotation;

                session.Send(WarehouseInventoryPacket.Load(item, warehouseItems.Values.Count));
                session.Send(WarehouseInventoryPacket.GainItemMessage(item, 1));
                session.Send(WarehouseInventoryPacket.Count(warehouseItems.Values.Count + 1));
                session.FieldManager.BroadcastPacket(ResponseCubePacket.ReplaceCube(session.FieldPlayer, newFieldCube, sendOnlyObjectId: true));
                session.Send(WarehouseInventoryPacket.Remove(item.Uid));
            }
            else
            {
                item = warehouseItems[replacementItemUid];
                Cube cube = new Cube(item, plotNumber, coord.ToFloat(), rotation);

                newFieldCube = session.FieldManager.RequestFieldObject(cube);
                newFieldCube.Coord = coord.ToFloat();
                newFieldCube.Rotation = rotation;

                warehouseItems[replacementItemUid].Amount--;
                if (item.Amount > 0)
                {
                    session.Send(WarehouseInventoryPacket.UpdateAmount(item.Uid, item.Amount));
                }
                else
                {
                    session.Send(WarehouseInventoryPacket.Remove(item.Uid));
                }
            }

            furnishingInventory.Add(newFieldCube.Value.Uid, newFieldCube.Value);
            if (oldFieldCube != null)
            {
                furnishingInventory.Remove(oldFieldCube.Value.Uid);
                DatabaseManager.Delete(oldFieldCube.Value);
                session.Send(FurnishingInventoryPacket.Remove(oldFieldCube.Value));
                session.FieldManager.State.RemoveCube(oldFieldCube.ObjectId);
            }

            session.Send(FurnishingInventoryPacket.Load(newFieldCube.Value));
            if (oldFieldCube?.Value.Item != null)
            {
                Item oldItem = warehouseItems.Values.FirstOrDefault(x => x.Id == oldFieldCube.Value.Item.Id);
                if (oldItem == default)
                {
                    oldItem = oldFieldCube.Value.Item;
                    warehouseItems[oldItem.Uid] = oldItem;
                    session.Send(WarehouseInventoryPacket.Load(oldItem, warehouseItems.Values.Count));
                }
                else
                {
                    oldItem.Amount++;
                    session.Send(WarehouseInventoryPacket.UpdateAmount(oldItem.Uid, oldItem.Amount));
                }
            }
            session.FieldManager.BroadcastPacket(ResponseCubePacket.ReplaceCube(session.FieldPlayer, newFieldCube, sendOnlyObjectId: false));
            session.FieldManager.AddCube(newFieldCube, session.FieldPlayer);
        }

        private static void HandlePickup(GameSession session, PacketReader packet)
        {
            byte[] coords = packet.Read(3);

            // Convert to signed byte array
            sbyte[] sCoords = Array.ConvertAll(coords, b => unchecked((sbyte) b));
            // Default to rainbow tree
            int weaponId = 18000004;

            // Find matching mapObject
            foreach (MapObject mapObject in MapEntityStorage.GetObjects(session.Player.MapId))
            {
                if (mapObject.Coord.Equals(CoordB.From(sCoords[0], sCoords[1], sCoords[2])))
                {
                    weaponId = mapObject.WeaponId;
                    break;
                }
            }

            // Pickup item then set battle state to true
            session.Send(ResponseCubePacket.Pickup(session, weaponId, coords));
            session.Send(UserBattlePacket.UserBattle(session.FieldPlayer, true));
        }

        private static void HandleDrop(GameSession session)
        {
            // Drop item then set battle state to false
            session.Send(ResponseCubePacket.Drop(session.FieldPlayer));
            session.Send(UserBattlePacket.UserBattle(session.FieldPlayer, false));
        }

        private static void HandleHomeName(GameSession session, PacketReader packet)
        {
            string name = packet.ReadUnicodeString();

            Home home = session.Player.Account.Home;

            home.Name = name;
            GameServer.HomeManager.GetHome(home.Id).Name = name;
            session.FieldManager.BroadcastPacket(ResponseCubePacket.HomeName(session.Player));
            session.FieldManager.BroadcastPacket(ResponseCubePacket.LoadHome(session.FieldPlayer));
        }

        private static void HandleHomePassword(GameSession session, PacketReader packet)
        {
            packet.ReadByte();
            string password = packet.ReadUnicodeString();

            Home home = session.Player.Account.Home;

            home.Password = password;
            GameServer.HomeManager.GetHome(home.Id).Password = password;
            session.FieldManager.BroadcastPacket(ResponseCubePacket.ChangePassword());
            session.FieldManager.BroadcastPacket(ResponseCubePacket.LoadHome(session.FieldPlayer));
        }

        private static void HandleNominateHouse(GameSession session)
        {
            long homeId = session.Player.VisitingHomeId;
            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);

            home.ArchitectScoreCurrent++;
            home.ArchitectScoreTotal++;

            session.FieldManager.BroadcastPacket(ResponseCubePacket.UpdateArchitectScore(home.ArchitectScoreCurrent, home.ArchitectScoreTotal));
            IFieldObject<Player> owner = session.FieldManager.State.Players.Values.FirstOrDefault(x => x.Value.Account.Home.Id == homeId);
            if (owner != default)
            {
                owner.Value.Session.Send(HomeCommandPacket.UpdateArchitectScore(owner.ObjectId, home.ArchitectScoreCurrent, home.ArchitectScoreTotal));
            }
            // TODO: add expiration to db
            session.Send(ResponseCubePacket.ArchitectScoreExpiration(session.Player.AccountId, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        }

        private static void HandleHomeDescription(GameSession session, PacketReader packet)
        {
            string description = packet.ReadUnicodeString();
            Home home = session.Player.Account.Home;

            home.Description = description;
            GameServer.HomeManager.GetHome(home.Id).Description = description;
            session.FieldManager.BroadcastPacket(ResponseCubePacket.HomeDescription(description));
        }

        private static void HandleModifySize(GameSession session, RequestCubeMode mode)
        {
            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            if ((mode == RequestCubeMode.IncreaseSize && home.Size + 1 > 25) || mode == RequestCubeMode.IncreaseHeight && home.Height + 1 > 15)
            {
                return;
            }
            if ((mode == RequestCubeMode.DecreaseSize && home.Size - 1 < 4) || mode == RequestCubeMode.DecreaseHeight && home.Height - 1 < 3)
            {
                return;
            }

            RemoveBlocks(session, mode, home);

            switch (mode)
            {
                case RequestCubeMode.IncreaseSize:
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.IncreaseSize(++home.Size));
                    break;
                case RequestCubeMode.DecreaseSize:
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.DecreaseSize(--home.Size));
                    break;
                case RequestCubeMode.IncreaseHeight:
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.IncreaseHeight(++home.Height));
                    break;
                case RequestCubeMode.DecreaseHeight:
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.DecreaseHeight(--home.Height));
                    break;
            }

            if (mode == RequestCubeMode.DecreaseHeight || mode == RequestCubeMode.DecreaseSize)
            {
                int x = -1 * Block.BLOCK_SIZE * (home.Size - 1);
                session.Send(UserMoveByPortalPacket.Move(session, CoordF.From(x, x, Block.BLOCK_SIZE * 3), CoordF.From(0, 0, 0)));
            }
        }

        private static void HandleKickEveryone(GameSession session)
        {
            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            if (home.AccountId != session.Player.AccountId)
            {
                return;
            }

            List<IFieldObject<Player>> players = session.FieldManager.State.Players.Values.Where(p => p.Value.CharacterId != session.Player.CharacterId).ToList();
            foreach (IFieldObject<Player> fieldPlayer in players)
            {
                fieldPlayer.Value.Session.Send(ResponseCubePacket.KickEveryone());
            }
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                players = session.FieldManager.State.Players.Values.Where(p => p.Value.CharacterId != session.Player.CharacterId).ToList();

                foreach (IFieldObject<Player> fieldPlayer in players)
                {
                    Player player = fieldPlayer.Value;
                    player.Warp(player.ReturnMapId, player.ReturnCoord);
                }
            });
        }

        private static void HandleEnablePermission(GameSession session, PacketReader packet)
        {
            HomePermission permission = (HomePermission) packet.ReadByte();
            bool enabled = packet.ReadBool();

            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            if (enabled)
            {
                home.Permissions[permission] = 0;
            }
            else
            {
                home.Permissions.Remove(permission);
            }

            session.FieldManager.BroadcastPacket(ResponseCubePacket.EnablePermission(permission, enabled));
        }

        private static void HandleSetPermission(GameSession session, PacketReader packet)
        {
            HomePermission permission = (HomePermission) packet.ReadByte();
            byte setting = packet.ReadByte();

            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);

            home.Permissions[permission] = home.Permissions.ContainsKey(permission) ? setting : (byte) 0;

            session.FieldManager.BroadcastPacket(ResponseCubePacket.SetPermission(permission, setting));
        }

        private static void HandleModifyInteriorSettings(GameSession session, RequestCubeMode mode, PacketReader packet)
        {
            byte value = packet.ReadByte();

            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            switch (mode)
            {
                case RequestCubeMode.ChangeBackground:
                    home.Background = value;
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.ChangeLighting(value));
                    break;
                case RequestCubeMode.ChangeLighting:
                    home.Lighting = value;
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.ChangeBackground(value));
                    break;
                case RequestCubeMode.ChangeCamera:
                    home.Camera = value;
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.ChangeCamera(value));
                    break;
            }
        }

        private static void HandleGiveBuildingPermission(GameSession session, PacketReader packet)
        {
            string characterName = packet.ReadUnicodeString();

            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            Player target = GameServer.Storage.GetPlayerByName(characterName);
            if (home.BuildingPermissions.Contains(target.AccountId))
            {
                return;
            }

            home.BuildingPermissions.Add(target.AccountId);

            session.Send(ResponseCubePacket.AddBuildingPermission(target.AccountId));
            target.Session.Send(ResponseCubePacket.UpdateBuildingPermissions(target.AccountId, session.Player.AccountId));
            target.Session.SendNotice("You have been granted furnishing rights."); // TODO: use the notice packet
        }

        private static void HandleRemoveBuildingPermission(GameSession session, PacketReader packet)
        {
            string characterName = packet.ReadUnicodeString();

            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            Player target = GameServer.Storage.GetPlayerByName(characterName);
            if (!home.BuildingPermissions.Contains(target.AccountId))
            {
                return;
            }

            home.BuildingPermissions.Remove(target.AccountId);

            session.Send(ResponseCubePacket.RemoveBuildingPermission(target.AccountId, target.Name));
            target.Session.Send(ResponseCubePacket.UpdateBuildingPermissions(0, session.Player.AccountId));
            target.Session.SendNotice("Your furnishing right has been removed."); // TODO: use the notice packet
        }

        private static bool PurchaseFurnishingItem(GameSession session, FurnishingShopMetadata shop)
        {
            switch (shop.FurnishingTokenType)
            {
                case 1: // meso
                    return session.Player.Wallet.Meso.Modify(-shop.Price);
                case 3: // meret
                    return session.Player.Wallet.RemoveMerets(shop.Price);
                default:
                    session.SendNotice($"Unknown currency: {shop.FurnishingTokenType}");
                    return false;
            }
        }

        private static bool HandlePlotPayment(GameSession session, int priceItemCode, int price)
        {
            switch (priceItemCode)
            {
                case 0:
                    return true;
                case 90000001:
                case 90000002:
                case 90000003:
                    return session.Player.Wallet.Meso.Modify(-price);
                case 90000004:
                    return session.Player.Wallet.RemoveMerets(price);
                case 90000006:
                    return session.Player.Wallet.ValorToken.Modify(-price);
                case 90000013:
                    return session.Player.Wallet.Rue.Modify(-price);
                case 90000014:
                    return session.Player.Wallet.HaviFruit.Modify(-price);
                case 90000017:
                    return session.Player.Wallet.Treva.Modify(-price);
                default:
                    session.SendNotice($"Unknown item currency: {priceItemCode}");
                    return false;
            }
        }

        private static bool IsCoordOutsideHeightLimit(CoordS coordS, int mapId, byte height)
        {
            MapMetadata metadata = MapMetadataStorage.GetMetadata(mapId);
            for (int i = 0; i <= height; i++) // checking blocks in the same Z axis
            {
                MapBlock block = metadata.Blocks.FirstOrDefault(x => x.Coord == coordS);
                if (block == null)
                {
                    coordS.Z -= Block.BLOCK_SIZE;
                    continue;
                }
                return false;
            }
            return true;
        }

        private static bool IsCoordOutsideSizeLimit(CoordB cubeCoord, int homeSize)
        {
            int size = (homeSize - 1) * -1;

            return cubeCoord.Y < size || cubeCoord.Y > 0 || cubeCoord.X < size || cubeCoord.X > 0;
        }

        private static CoordB GetGroundCoord(CoordB coord, int mapId, byte height)
        {
            List<MapBlock> blocks = MapMetadataStorage.GetMetadata(mapId).Blocks;
            for (int i = 0; i <= height; i++)
            {
                MapBlock blockExists = blocks.FirstOrDefault(x => x.Coord == coord.ToShort());
                if (blockExists != default)
                {
                    return blockExists.Coord.ToByte();
                }
                coord -= CoordB.From(0, 0, 1);
            }
            return default;
        }

        private static void RemoveBlocks(GameSession session, RequestCubeMode mode, Home home)
        {
            if (mode == RequestCubeMode.DecreaseSize)
            {
                int maxSize = (home.Size - 1) * Block.BLOCK_SIZE * -1;
                for (int i = 0; i < home.Size; i++)
                {
                    for (int j = 0; j <= home.Height; j++)
                    {
                        CoordF coord = CoordF.From(maxSize, i * Block.BLOCK_SIZE * -1, j * Block.BLOCK_SIZE);
                        IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord);
                        if (cube != default)
                        {
                            RemoveCube(session, cube, home);
                        }
                    }
                }

                for (int i = 0; i < home.Size; i++)
                {
                    for (int j = 0; j <= home.Height; j++)
                    {
                        CoordF coord = CoordF.From(i * Block.BLOCK_SIZE * -1, maxSize, j * Block.BLOCK_SIZE);
                        IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord);
                        if (cube != default)
                        {
                            RemoveCube(session, cube, home);
                        }
                    }
                }
            }

            if (mode == RequestCubeMode.DecreaseHeight)
            {
                for (int i = 0; i < home.Size; i++)
                {
                    for (int j = 0; j < home.Size; j++)
                    {
                        CoordF coord = CoordF.From(i * Block.BLOCK_SIZE * -1, j * Block.BLOCK_SIZE * -1, home.Height * Block.BLOCK_SIZE);
                        IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord);
                        if (cube != default)
                        {
                            RemoveCube(session, cube, home);
                        }
                    }
                }
            }
        }

        private static void RemoveCube(GameSession session, IFieldObject<Cube> cube, Home home)
        {
            Dictionary<long, Item> warehouseItems = home.WarehouseInventory;
            Dictionary<long, Cube> furnishingInventory = home.FurnishingInventory;

            furnishingInventory.Remove(cube.Value.Uid);
            session.Send(FurnishingInventoryPacket.Remove(cube.Value));

            DatabaseManager.Delete(cube.Value);

            Item item = warehouseItems.Values.FirstOrDefault(x => x.Id == cube.Value.Item.Id);
            if (item == default)
            {
                warehouseItems[cube.Value.Item.Uid] = cube.Value.Item;
                session.Send(WarehouseInventoryPacket.Load(cube.Value.Item, warehouseItems.Values.Count));
            }
            else
            {
                item.Amount++;
                session.Send(WarehouseInventoryPacket.UpdateAmount(item.Uid, item.Amount));
            }
            session.FieldManager.RemoveCube(cube, session.FieldPlayer);
        }
    }
}
