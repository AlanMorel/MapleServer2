using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maple2Storage.Enums;
using Maple2Storage.Tools;
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
using MapleServer2.Tools;
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
            LoadLayout = 0x23,
            IncreaseSize = 0x25,
            DecreaseSize = 0x26,
            DecorationReward = 0x28,
            InteriorDesingReward = 0x29,
            EnablePermission = 0x2A,
            SetPermission = 0x2B,
            IncreaseHeight = 0x2C,
            DecreaseHeight = 0x2D,
            SaveLayout = 0x2E,
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
                case RequestCubeMode.LoadLayout:
                    HandleLoadLayout(session, packet);
                    break;
                case RequestCubeMode.IncreaseSize:
                case RequestCubeMode.DecreaseSize:
                case RequestCubeMode.IncreaseHeight:
                case RequestCubeMode.DecreaseHeight:
                    HandleModifySize(session, mode);
                    break;
                case RequestCubeMode.SaveLayout:
                    HandleSaveLayout(session, packet);
                    break;
                case RequestCubeMode.DecorationReward:
                    HandleDecorationReward(session);
                    break;
                case RequestCubeMode.InteriorDesingReward:
                    HandleInteriorDesingReward(session, packet);
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
                session.SendNotice("You don't have enough mesos!");
                return;
            }

            if (player.Account.Home == null)
            {
                player.Account.Home = new Home(player.Account.Id, player.Name, homeTemplate)
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

                Home home = GameServer.HomeManager.GetHome(player.Account.Home.Id);
                home.PlotId = player.Account.Home.PlotId;
                home.PlotNumber = player.Account.Home.PlotNumber;
                home.Expiration = player.Account.Home.Expiration;
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

            Home home = GameServer.HomeManager.GetHome(player.Account.Home.Id);
            home.PlotId = 0;
            home.PlotNumber = 0;
            home.ApartmentNumber = 0;
            home.Expiration = 32503561200; // year 2999

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

            bool mapIsHome = player.MapId == (int) Map.PrivateResidence;
            Home home = mapIsHome ? GameServer.HomeManager.GetHome(player.VisitingHomeId) : GameServer.HomeManager.GetHome(player.Account.Home.Id);

            int plotNumber = MapMetadataStorage.GetPlotNumber(player.MapId, coord);
            if (plotNumber <= 0)
            {
                session.Send(ResponseCubePacket.CantPlaceHere(session.FieldPlayer.ObjectId));
                return;
            }

            UGCMapGroup plot = UGCMapMetadataStorage.GetGroupMetadata(player.MapId, (byte) plotNumber);
            byte height = mapIsHome ? home.Height : plot.HeightLimit;
            int size = mapIsHome ? home.Size : plot.Area / 2;
            if (IsCoordOutsideHeightLimit(coord.ToShort(), player.MapId, height) || (mapIsHome && IsCoordOutsideSizeLimit(coord, size)))
            {
                session.Send(ResponseCubePacket.CantPlaceHere(session.FieldPlayer.ObjectId));
                return;
            }

            FurnishingShopMetadata shopMetadata = FurnishingShopMetadataStorage.GetMetadata(itemId);
            if (shopMetadata == null || !shopMetadata.Buyable)
            {
                return;
            }

            IFieldObject<Cube> fieldCube;
            if (session.Player.IsInDecorPlanner)
            {
                Item item = new Item(itemId);
                Cube cube = new Cube(item, plotNumber, coordF, rotation);

                fieldCube = session.FieldManager.RequestFieldObject(cube);
                fieldCube.Coord = coordF;
                fieldCube.Rotation = rotation;
                home.DecorPlannerInventory.Add(cube.Uid, cube);

                session.FieldManager.AddCube(fieldCube, session.FieldPlayer.ObjectId, session.FieldPlayer.ObjectId);
                return;
            }

            IFieldObject<Player> homeOwner = session.FieldManager.State.Players.Values.FirstOrDefault(x => x.Value.AccountId == home.AccountId);
            if (session.Player.Account.Id != home.AccountId)
            {
                if (homeOwner == default)
                {
                    session.SendNotice("You cannot do that unless the home owner is present."); // TODO: use notice packet
                    return;
                }
                if (!home.BuildingPermissions.Contains(session.Player.Account.Id))
                {
                    session.SendNotice("You don't have building rights.");
                    return;
                }
            }

            Dictionary<long, Item> warehouseItems = home.WarehouseInventory;
            Dictionary<long, Cube> furnishingInventory = home.FurnishingInventory;
            if ((!warehouseItems.ContainsKey(itemUid) || warehouseItems[itemUid].Amount <= 0) && !PurchaseFurnishingItem(session, shopMetadata))
            {
                NotEnoughMoney(session, shopMetadata);
                return;
            }

            fieldCube = AddCube(session, itemId, itemUid, rotation, coordF, plotNumber, homeOwner, warehouseItems, furnishingInventory);

            homeOwner.Value.Session.Send(FurnishingInventoryPacket.Load(fieldCube.Value));
            session.FieldManager.AddCube(fieldCube, homeOwner.ObjectId, session.FieldPlayer.ObjectId);
        }

        private static void HandleRemoveCube(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();
            Player player = session.Player;

            bool mapIsHome = player.MapId == (int) Map.PrivateResidence;
            Home home = mapIsHome ? GameServer.HomeManager.GetHome(player.VisitingHomeId) : GameServer.HomeManager.GetHome(player.Account.Home.Id);

            IFieldObject<Player> homeOwner = session.FieldManager.State.Players.Values.FirstOrDefault(x => x.Value.AccountId == home.AccountId);
            if (session.Player.Account.Id != home.AccountId)
            {
                if (homeOwner == default)
                {
                    session.SendNotice("You cannot do that unless the home owner is present."); // TODO: use notice packet
                    return;
                }
                if (!home.BuildingPermissions.Contains(session.Player.Account.Id))
                {
                    session.SendNotice("You don't have building rights.");
                    return;
                }
            }

            IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord.ToFloat());
            if (cube == default || cube.Value.Item == null)
            {
                return;
            }

            if (player.IsInDecorPlanner)
            {
                home.DecorPlannerInventory.Remove(cube.Value.Uid);
                session.FieldManager.RemoveCube(cube, homeOwner.ObjectId, session.FieldPlayer.ObjectId);
                return;
            }

            RemoveCube(session, homeOwner, cube, home);
        }

        private static void HandleRotateCube(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();
            Player player = session.Player;

            bool mapIsHome = player.MapId == (int) Map.PrivateResidence;
            Home home = mapIsHome ? GameServer.HomeManager.GetHome(player.VisitingHomeId) : GameServer.HomeManager.GetHome(player.Account.Home.Id);
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
            Dictionary<long, Cube> inventory = player.IsInDecorPlanner ? home.DecorPlannerInventory : home.FurnishingInventory;
            inventory[cube.Value.Uid].Rotation = cube.Rotation;

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
            CoordF rotation = CoordF.From(0, 0, zRotation);

            Player player = session.Player;

            bool mapIsHome = player.MapId == (int) Map.PrivateResidence;
            Home home = mapIsHome ? GameServer.HomeManager.GetHome(player.VisitingHomeId) : GameServer.HomeManager.GetHome(player.Account.Home.Id);

            int plotNumber = MapMetadataStorage.GetPlotNumber(player.MapId, coord);
            if (plotNumber <= 0)
            {
                session.Send(ResponseCubePacket.CantPlaceHere(session.FieldPlayer.ObjectId));
                return;
            }

            UGCMapGroup plot = UGCMapMetadataStorage.GetGroupMetadata(player.MapId, (byte) plotNumber);
            byte height = mapIsHome ? home.Height : plot.HeightLimit;
            int size = mapIsHome ? home.Size : plot.Area / 2;
            CoordB? groundHeight = GetGroundCoord(coord, player.MapId, height);
            if (groundHeight == null)
            {
                return;
            }

            bool isCubeSolid = ItemMetadataStorage.GetIsCubeSolid(replacementItemId);
            if (!isCubeSolid && coord.Z == groundHeight?.Z)
            {
                session.Send(ResponseCubePacket.CantPlaceHere(session.FieldPlayer.ObjectId));
                return;
            }

            if (IsCoordOutsideHeightLimit(coord.ToShort(), player.MapId, height) || (mapIsHome && IsCoordOutsideSizeLimit(coord, size)))
            {
                session.Send(ResponseCubePacket.CantPlaceHere(session.FieldPlayer.ObjectId));
                return;
            }

            FurnishingShopMetadata shopMetadata = FurnishingShopMetadataStorage.GetMetadata(replacementItemId);
            if (shopMetadata == null || !shopMetadata.Buyable)
            {
                return;
            }

            // Not checking if oldFieldCube is null on ground height because default blocks don't have IFieldObjects.
            IFieldObject<Cube> oldFieldCube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord.ToFloat());
            if (oldFieldCube == default && coord.Z != groundHeight?.Z)
            {
                return;
            }

            IFieldObject<Cube> newFieldCube;
            Item item;
            if (player.IsInDecorPlanner)
            {
                item = new Item(replacementItemId);
                Cube cube = new Cube(item, plotNumber, coord.ToFloat(), rotation);

                newFieldCube = session.FieldManager.RequestFieldObject(cube);
                newFieldCube.Coord = coord.ToFloat();
                newFieldCube.Rotation = rotation;

                home.DecorPlannerInventory.Remove(oldFieldCube.Value.Uid);
                session.FieldManager.State.RemoveCube(oldFieldCube.ObjectId);

                home.DecorPlannerInventory.Add(cube.Uid, cube);
                session.FieldManager.BroadcastPacket(ResponseCubePacket.ReplaceCube(session.FieldPlayer.ObjectId, session.FieldPlayer.ObjectId, newFieldCube, sendOnlyObjectId: false));
                session.FieldManager.State.AddCube(newFieldCube);
                return;
            }

            IFieldObject<Player> homeOwner = session.FieldManager.State.Players.Values.FirstOrDefault(x => x.Value.AccountId == home.AccountId);
            if (session.Player.Account.Id != home.AccountId)
            {
                if (homeOwner == default)
                {
                    session.SendNotice("You cannot do that unless the home owner is present."); // TODO: use notice packet
                    return;
                }
                if (!home.BuildingPermissions.Contains(session.Player.Account.Id))
                {
                    session.SendNotice("You don't have building rights.");
                    return;
                }
            }

            Dictionary<long, Item> warehouseItems = home.WarehouseInventory;
            Dictionary<long, Cube> furnishingInventory = home.FurnishingInventory;
            if ((!warehouseItems.ContainsKey(replacementItemUid) || warehouseItems[replacementItemUid].Amount <= 0) && !PurchaseFurnishingItem(session, shopMetadata))
            {
                NotEnoughMoney(session, shopMetadata);
                return;
            }

            newFieldCube = AddCube(session, replacementItemId, replacementItemUid, rotation, coord.ToFloat(), plotNumber, homeOwner, warehouseItems, furnishingInventory);

            if (oldFieldCube != null)
            {
                furnishingInventory.Remove(oldFieldCube.Value.Uid);
                DatabaseManager.Delete(oldFieldCube.Value);
                homeOwner.Value.Session.Send(FurnishingInventoryPacket.Remove(oldFieldCube.Value));
                session.FieldManager.State.RemoveCube(oldFieldCube.ObjectId);
            }

            homeOwner.Value.Session.Send(FurnishingInventoryPacket.Load(newFieldCube.Value));
            if (oldFieldCube?.Value.Item != null)
            {
                Item oldItem = warehouseItems.Values.FirstOrDefault(x => x.Id == oldFieldCube.Value.Item.Id);
                if (oldItem == default)
                {
                    warehouseItems[oldFieldCube.Value.Item.Uid] = oldFieldCube.Value.Item;
                    homeOwner.Value.Session.Send(WarehouseInventoryPacket.Load(oldFieldCube.Value.Item, warehouseItems.Values.Count));
                }
                else
                {
                    oldItem.Amount++;
                    homeOwner.Value.Session.Send(WarehouseInventoryPacket.UpdateAmount(oldItem.Uid, oldItem.Amount));
                }
            }
            session.FieldManager.BroadcastPacket(ResponseCubePacket.ReplaceCube(homeOwner.ObjectId, session.FieldPlayer.ObjectId, newFieldCube, sendOnlyObjectId: false));
            session.FieldManager.State.AddCube(newFieldCube);
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
            if (session.Player.AccountId != home.AccountId)
            {
                return;
            }

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
            if (session.Player.AccountId != home.AccountId)
            {
                return;
            }

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
            session.Send(ResponseCubePacket.ArchitectScoreExpiration(session.Player.AccountId, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        }

        private static void HandleHomeDescription(GameSession session, PacketReader packet)
        {
            string description = packet.ReadUnicodeString();

            Home home = session.Player.Account.Home;
            if (session.Player.AccountId != home.AccountId)
            {
                return;
            }

            home.Description = description;
            GameServer.HomeManager.GetHome(home.Id).Description = description;
            session.FieldManager.BroadcastPacket(ResponseCubePacket.HomeDescription(description));
        }

        private static void HandleLoadLayout(GameSession session, PacketReader packet)
        {
            int layoutId = packet.ReadInt();

            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
        }

        private static void HandleModifySize(GameSession session, RequestCubeMode mode)
        {
            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            if (session.Player.AccountId != home.AccountId)
            {
                return;
            }
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

            // move player to safe coord
            if (mode == RequestCubeMode.DecreaseHeight || mode == RequestCubeMode.DecreaseSize)
            {
                int x = -1 * Block.BLOCK_SIZE * (home.Size - 1);
                session.Send(UserMoveByPortalPacket.Move(session, CoordF.From(x, x, Block.BLOCK_SIZE * 3), CoordF.From(0, 0, 0)));
            }
        }

        private static void HandleSaveLayout(GameSession session, PacketReader packet)
        {
            int layoutId = packet.ReadInt();
            string layoutName = packet.ReadUnicodeString();

            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
        }

        private static void HandleDecorationReward(GameSession session)
        {
            // Decoration score goals
            Dictionary<ItemHousingCategory, int> goals = new Dictionary<ItemHousingCategory, int>() {
                {ItemHousingCategory.Bed, 1}, {ItemHousingCategory.Table, 1}, {ItemHousingCategory.SofasChairs, 2},
                {ItemHousingCategory.Storage, 1}, {ItemHousingCategory.WallDecoration, 1}, {ItemHousingCategory.WallTiles, 3},
                {ItemHousingCategory.Bathroom, 1}, {ItemHousingCategory.Lighting, 1}, {ItemHousingCategory.Electronics, 1},
                {ItemHousingCategory.Fences, 2}, {ItemHousingCategory.NaturalTerrain, 4},
            };

            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            if (home == null || session.Player.AccountId != home.AccountId)
            {
                return;
            }

            List<Item> items = home.FurnishingInventory.Values.Select(x => x.Item).ToList();
            items.ForEach(x => x.HousingCategory = ItemMetadataStorage.GetHousingCategory(x.Id));
            Dictionary<ItemHousingCategory, int> current = items.GroupBy(x => x.HousingCategory).ToDictionary(x => x.Key, x => x.Count());

            int decorationScore = 0;
            foreach (ItemHousingCategory category in goals.Keys)
            {
                if (!current.ContainsKey(category))
                {
                    continue;
                }
                if (current[category] >= goals[category])
                {
                    switch (category)
                    {
                        case ItemHousingCategory.Bed:
                        case ItemHousingCategory.SofasChairs:
                        case ItemHousingCategory.WallDecoration:
                        case ItemHousingCategory.WallTiles:
                        case ItemHousingCategory.Bathroom:
                        case ItemHousingCategory.Lighting:
                        case ItemHousingCategory.Fences:
                        case ItemHousingCategory.NaturalTerrain:
                            decorationScore += 100;
                            break;
                        case ItemHousingCategory.Table:
                        case ItemHousingCategory.Storage:
                            decorationScore += 50;
                            break;
                        case ItemHousingCategory.Electronics:
                            decorationScore += 200;
                            break;
                    }
                }
            }

            List<int> rewardsIds = new List<int>() { 20300039, 20000071, 20301018 }; // Default rewards
            switch (decorationScore)
            {
                case < 300:
                    rewardsIds.Add(20000028);
                    break;
                case >= 300 and < 500:
                    rewardsIds.Add(20000029);
                    break;
                case >= 500 and < 1100:
                    rewardsIds.Add(20300078);
                    rewardsIds.Add(20000030);
                    break;
                default:
                    rewardsIds.Add(20300078);
                    rewardsIds.Add(20000031);
                    rewardsIds.Add(20300040);
                    InventoryController.Add(session, new Item(20300559), true);
                    break;
            }

            home.GainExp(decorationScore);
            InventoryController.Add(session, new Item(rewardsIds.OrderBy(x => RandomProvider.Get().Next()).First()), true);
            home.DecorationRewardTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            session.Send(ResponseCubePacket.DecorationScore(home));
        }

        private static void HandleInteriorDesingReward(GameSession session, PacketReader packet)
        {
            byte rewardId = packet.ReadByte();
            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            if (home == null || session.Player.AccountId != home.AccountId)
            {
                return;
            }

            if (rewardId <= 1 || rewardId >= 11 || home == null || home.InteriorRewardsClaimed.Contains(rewardId))
            {
                return;
            }

            switch (rewardId)
            {
                case 2:
                case 5:
                    InventoryController.Add(session, new Item(20301019), true);
                    break;
                case 3:
                    InventoryController.Add(session, new Item(20301020), true);
                    break;
                case 4:
                case 8:
                    InventoryController.Add(session, new Item(20301021), true);
                    break;
                case 6:
                    InventoryController.Add(session, new Item(20301022), true);
                    break;
                case 7:
                case 9:
                    InventoryController.Add(session, new Item(20300552), true);
                    break;
                default:
                    break;
            }
            home.InteriorRewardsClaimed.Add(rewardId);
            session.Send(ResponseCubePacket.DecorationScore(home));
        }

        private static void HandleKickEveryone(GameSession session)
        {
            Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
            if (session.Player.AccountId != home.AccountId)
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
            if (session.Player.AccountId != home.AccountId)
            {
                return;
            }

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
            if (session.Player.AccountId != home.AccountId)
            {
                return;
            }

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
            if (session.Player.AccountId != home.AccountId)
            {
                return;
            }

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
            if (session.Player.AccountId != home.AccountId)
            {
                return;
            }

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

        private static CoordB? GetGroundCoord(CoordB coord, int mapId, byte height)
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
            return null;
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
                            RemoveCube(session, session.FieldPlayer, cube, home);
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
                            RemoveCube(session, session.FieldPlayer, cube, home);
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
                            RemoveCube(session, session.FieldPlayer, cube, home);
                        }
                    }
                }
            }
        }

        private static IFieldObject<Cube> AddCube(GameSession session, int itemId, long itemUid, CoordF rotation, CoordF coordF, int plotNumber, IFieldObject<Player> homeOwner, Dictionary<long, Item> warehouseItems, Dictionary<long, Cube> furnishingInventory)
        {
            IFieldObject<Cube> fieldCube;
            if (!warehouseItems.ContainsKey(itemUid) || warehouseItems[itemUid].Amount <= 0)
            {
                Item item = new Item(itemId);
                Cube cube = new Cube(item, plotNumber, coordF, rotation);

                fieldCube = session.FieldManager.RequestFieldObject(cube);
                fieldCube.Coord = coordF;
                fieldCube.Rotation = rotation;

                homeOwner.Value.Session.Send(WarehouseInventoryPacket.Load(item, warehouseItems.Values.Count));
                homeOwner.Value.Session.Send(WarehouseInventoryPacket.GainItemMessage(item, 1));
                homeOwner.Value.Session.Send(WarehouseInventoryPacket.Count(warehouseItems.Values.Count + 1));
                session.FieldManager.BroadcastPacket(ResponseCubePacket.PlaceFurnishing(fieldCube, homeOwner.ObjectId, session.FieldPlayer.ObjectId, sendOnlyObjectId: true));
                homeOwner.Value.Session.Send(WarehouseInventoryPacket.Remove(item.Uid));
            }
            else
            {
                Item item = warehouseItems[itemUid];
                Cube cube = new Cube(item, plotNumber, coordF, rotation);

                fieldCube = session.FieldManager.RequestFieldObject(cube);
                fieldCube.Coord = coordF;
                fieldCube.Rotation = rotation;

                warehouseItems[itemUid].Amount--;
                if (item.Amount > 0)
                {
                    homeOwner.Value.Session.Send(WarehouseInventoryPacket.UpdateAmount(item.Uid, item.Amount));
                }
                else
                {
                    homeOwner.Value.Session.Send(WarehouseInventoryPacket.Remove(item.Uid));
                }
            }
            furnishingInventory.Add(fieldCube.Value.Uid, fieldCube.Value);
            return fieldCube;
        }

        private static void RemoveCube(GameSession session, IFieldObject<Player> homeOwner, IFieldObject<Cube> cube, Home home)
        {
            Dictionary<long, Item> warehouseItems = home.WarehouseInventory;
            Dictionary<long, Cube> furnishingInventory = home.FurnishingInventory;

            furnishingInventory.Remove(cube.Value.Uid);
            homeOwner.Value.Session.Send(FurnishingInventoryPacket.Remove(cube.Value));

            DatabaseManager.Delete(cube.Value);

            Item item = warehouseItems.Values.FirstOrDefault(x => x.Id == cube.Value.Item.Id);
            if (item == default)
            {
                warehouseItems[cube.Value.Item.Uid] = cube.Value.Item;
                homeOwner.Value.Session.Send(WarehouseInventoryPacket.Load(cube.Value.Item, warehouseItems.Values.Count));
            }
            else
            {
                item.Amount++;
                homeOwner.Value.Session.Send(WarehouseInventoryPacket.UpdateAmount(item.Uid, item.Amount));
            }
            session.FieldManager.RemoveCube(cube, homeOwner.ObjectId, session.FieldPlayer.ObjectId);
        }

        private static void NotEnoughMoney(GameSession session, FurnishingShopMetadata shopMetadata)
        {
            string currency = "";
            switch (shopMetadata.FurnishingTokenType)
            {
                case 1:
                    currency = "mesos";
                    break;
                case 3:
                    currency = "merets";
                    break;
            }
            session.SendNotice($"You don't have enough {currency}!");
        }
    }
}
