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
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class RequestCubeHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_CUBE;

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
        ClearInterior = 0x1F,
        RequestLayout = 0x23,
        IncreaseSize = 0x25,
        DecreaseSize = 0x26,
        DecorationReward = 0x28,
        InteriorDesignReward = 0x29,
        EnablePermission = 0x2A,
        SetPermission = 0x2B,
        IncreaseHeight = 0x2C,
        DecreaseHeight = 0x2D,
        SaveLayout = 0x2E,
        DecorPlannerLoadLayout = 0x2F,
        LoadLayout = 0x30,
        KickEveryone = 0x31,
        ChangeBackground = 0x33,
        ChangeLighting = 0x34,
        ChangeCamera = 0x36,
        UpdateBudget = 0x38,
        GiveBuildingPermission = 0x39,
        RemoveBuildingPermission = 0x3A
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
            case RequestCubeMode.ClearInterior:
                HandleClearInterior(session);
                break;
            case RequestCubeMode.RequestLayout:
                HandleRequestLayout(session, packet);
                break;
            case RequestCubeMode.IncreaseSize:
            case RequestCubeMode.DecreaseSize:
            case RequestCubeMode.IncreaseHeight:
            case RequestCubeMode.DecreaseHeight:
                HandleModifySize(session, mode);
                break;
            case RequestCubeMode.DecorationReward:
                HandleDecorationReward(session);
                break;
            case RequestCubeMode.InteriorDesignReward:
                HandleInteriorDesignReward(session, packet);
                break;
            case RequestCubeMode.SaveLayout:
                HandleSaveLayout(session, packet);
                break;
            case RequestCubeMode.DecorPlannerLoadLayout:
                HandleDecorPlannerLoadLayout(session, packet);
                break;
            case RequestCubeMode.LoadLayout:
                HandleLoadLayout(session, packet);
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
            case RequestCubeMode.UpdateBudget:
                HandleUpdateBudget(session, packet);
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

        Home home = session.Player.Account.Home;
        if (home is null || !home.WarehouseInventory.TryGetValue(itemUid, out Item item))
        {
            session.FieldManager.BroadcastPacket(ResponseCubePacket.LoadFurnishingItem(session.Player.FieldPlayer, itemId, itemUid));
            return;
        }

        session.FieldManager.BroadcastPacket(ResponseCubePacket.LoadFurnishingItem(session.Player.FieldPlayer, itemId, itemUid, item));
    }

    private static void HandleBuyPlot(GameSession session, PacketReader packet)
    {
        int groupId = packet.ReadInt();
        int homeTemplate = packet.ReadInt();
        Player player = session.Player;

        if (player.Account.Home is not null && player.Account.Home.PlotMapId != 0)
        {
            return;
        }

        UgcMapGroup land = UgcMapMetadataStorage.GetGroupMetadata(player.MapId, (byte) groupId);
        if (land is null)
        {
            return;
        }

        //Check if sale event is active
        int price = land.Price;
        UgcMapContractSale ugcMapContractSale = DatabaseManager.Events.FindUgcMapContractSaleEvent();
        if (ugcMapContractSale is not null)
        {
            int markdown = land.Price * (ugcMapContractSale.DiscountAmount / 100 / 100);
            price = land.Price - markdown;
        }

        if (!HandlePlotPayment(session, land.PriceItemCode, price))
        {
            session.Send(ChatPacket.Error(session.Player, SystemNotice.ErrorInsufficientMeso, ChatType.NoticeAlert));
            return;
        }

        if (player.Account.Home is null)
        {
            player.Account.Home = new(player.Account.Id, player.Name, homeTemplate)
            {
                PlotMapId = player.MapId,
                PlotNumber = land.Id,
                Expiration = TimeInfo.Now() + Environment.TickCount + land.ContractDate * 24 * 60 * 60,
                Name = player.Name
            };
            GameServer.HomeManager.AddHome(player.Account.Home);
        }
        else
        {
            player.Account.Home.PlotMapId = player.MapId;
            player.Account.Home.PlotNumber = land.Id;
            player.Account.Home.Expiration = TimeInfo.Now() + Environment.TickCount + land.ContractDate * 24 * 60 * 60;

            Home home = GameServer.HomeManager.GetHomeById(player.Account.Home.Id);
            home.PlotMapId = player.Account.Home.PlotMapId;
            home.PlotNumber = player.Account.Home.PlotNumber;
            home.Expiration = player.Account.Home.Expiration;
        }

        session.FieldManager.BroadcastPacket(ResponseCubePacket.PurchasePlot(player.Account.Home.PlotNumber, 0, player.Account.Home.Expiration));
        session.FieldManager.BroadcastPacket(ResponseCubePacket.EnablePlotFurnishing(player));
        session.Send(ResponseCubePacket.LoadHome(session.Player.FieldPlayer.ObjectId, session.Player.Account.Home));
        session.FieldManager.BroadcastPacket(ResponseCubePacket.HomeName(player), session);
        session.Send(ResponseCubePacket.CompletePurchase());
    }

    private static void HandleForfeitPlot(GameSession session)
    {
        Player player = session.Player;
        if (player.Account.Home is null || player.Account.Home.PlotMapId == 0)
        {
            return;
        }

        int plotMapId = player.Account.Home.PlotMapId;
        int plotNumber = player.Account.Home.PlotNumber;
        int apartmentNumber = player.Account.Home.ApartmentNumber;

        player.Account.Home.PlotMapId = 0;
        player.Account.Home.PlotNumber = 0;
        player.Account.Home.ApartmentNumber = 0;
        player.Account.Home.Expiration = 32503561200; // year 2999

        Home home = GameServer.HomeManager.GetHomeById(player.Account.Home.Id);
        home.PlotMapId = 0;
        home.PlotNumber = 0;
        home.ApartmentNumber = 0;
        home.Expiration = 32503561200; // year 2999

        IEnumerable<IFieldObject<Cube>> cubes = session.FieldManager.State.Cubes.Values.Where(x => x.Value.PlotNumber == plotNumber);
        foreach (IFieldObject<Cube> cube in cubes)
        {
            RemoveCube(session, session.Player.FieldPlayer, cube, home);
        }

        session.Send(ResponseCubePacket.ForfeitPlot(plotNumber, apartmentNumber, TimeInfo.Now()));
        session.Send(ResponseCubePacket.RemovePlot(plotNumber, apartmentNumber));
        session.Send(ResponseCubePacket.LoadHome(session.Player.FieldPlayer.ObjectId, session.Player.Account.Home));
        session.Send(ResponseCubePacket.RemovePlot2(plotMapId, plotNumber));
        // 54 00 0E 01 00 00 00 01 01 00 00 00, send mail
    }

    private static void HandleAddFurnishing(GameSession session, PacketReader packet)
    {
        CoordB coord = packet.Read<CoordB>();
        byte padding = packet.ReadByte();
        int itemId = packet.ReadInt();
        long itemUid = packet.ReadLong();
        byte ugcBranch = packet.ReadByte();
        CoordF rotation = packet.Read<CoordF>();

        CoordF coordF = coord.ToFloat();
        Player player = session.Player;

        LiftableObject liftable = session.FieldManager.State.LiftableObjects.Values.FirstOrDefault(x => x.Metadata.ItemId == itemId);
        if (liftable is not null)
        {
            HandleAddLiftable(player, session.FieldManager, liftable, coord, rotation);
            return;
        }

        bool mapIsHome = player.MapId == (int) Map.PrivateResidence;
        Home home = mapIsHome ? GameServer.HomeManager.GetHomeById(player.VisitingHomeId) : GameServer.HomeManager.GetHomeById(player.Account.Home.Id);

        int plotNumber = MapMetadataStorage.GetPlotNumber(player.MapId, coord);
        if (plotNumber <= 0)
        {
            session.Send(ResponseCubePacket.CantPlaceHere(session.Player.FieldPlayer.ObjectId));
            return;
        }

        UgcMapGroup plot = UgcMapMetadataStorage.GetGroupMetadata(player.MapId, (byte) plotNumber);
        byte height = mapIsHome ? home.Height : plot.HeightLimit;
        int size = mapIsHome ? home.Size : plot.Area / 2;
        if (IsCoordOutsideHeightLimit(coord.ToShort(), player.MapId, height) || mapIsHome && IsCoordOutsideSizeLimit(coord, size))
        {
            session.Send(ResponseCubePacket.CantPlaceHere(session.Player.FieldPlayer.ObjectId));
            return;
        }

        FurnishingShopMetadata shopMetadata = FurnishingShopMetadataStorage.GetMetadata(itemId);
        if (shopMetadata is null || !shopMetadata.Buyable)
        {
            return;
        }

        // Don't allow to place furniture in the same place
        if (session.FieldManager.State.Cubes.Values.Any(x => x.Coord == coord.ToFloat()))
        {
            return;
        }

        IFieldObject<Cube> fieldCube;
        if (player.IsInDecorPlanner)
        {
            Cube cube = new(new(itemId), plotNumber, coordF, rotation);

            fieldCube = session.FieldManager.RequestFieldObject(cube);
            fieldCube.Coord = coordF;
            fieldCube.Rotation = rotation;
            home.DecorPlannerInventory.Add(cube.Uid, cube);

            session.FieldManager.AddCube(fieldCube, session.Player.FieldPlayer.ObjectId, session.Player.FieldPlayer.ObjectId);
            return;
        }

        IFieldObject<Player> homeOwner = player.FieldPlayer;
        if (player.Account.Id != home.AccountId)
        {
            homeOwner = session.FieldManager.State.Players.Values.FirstOrDefault(x => x.Value.AccountId == home.AccountId);
            if (homeOwner is null)
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.ErrUgcMapCantFindDelegateOwner, ChatType.NoticeAlert));
                return;
            }

            if (!home.BuildingPermissions.Contains(player.Account.Id))
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.SocketError10013, ChatType.NoticeAlert));
                return;
            }
        }

        Dictionary<long, Item> warehouseItems = home.WarehouseInventory;
        Item item;
        if (player.Account.Id != home.AccountId)
        {
            if ((!warehouseItems.TryGetValue(itemUid, out item) || warehouseItems[itemUid].Amount <= 0) &&
                !PurchaseFurnishingItemFromBudget(session.FieldManager, homeOwner.Value, home, shopMetadata))
            {
                NotEnoughMoneyInBudget(session, shopMetadata);
                return;
            }
        }
        else
        {
            if ((!warehouseItems.TryGetValue(itemUid, out item) || warehouseItems[itemUid].Amount <= 0) && !PurchaseFurnishingItem(session, shopMetadata))
            {
                NotEnoughMoney(session, shopMetadata);
                return;
            }
        }

        fieldCube = AddCube(session, item, itemId, rotation, coordF, plotNumber, homeOwner, home);

        homeOwner.Value.Session.Send(FurnishingInventoryPacket.Load(fieldCube.Value));
        session.FieldManager.AddCube(fieldCube, homeOwner.ObjectId, session.Player.FieldPlayer.ObjectId);
    }

    private static void HandleAddLiftable(Player player, FieldManager fieldManager, LiftableObject liftable, CoordB coord, CoordF rotation)
    {
        liftable.Position = coord.ToFloat();
        liftable.Rotation = rotation;

        liftable.State = LiftableState.Active;
        liftable.Enabled = true;

        MapLiftableTarget target = MapEntityMetadataStorage.GetLiftablesTargets(player.MapId)?.FirstOrDefault(x => x.Position == liftable.Position);
        if (target is not null)
        {
            liftable.State = LiftableState.Disabled;
            QuestManager.OnItemMove(player, liftable.Metadata.ItemId, target.Target);
        }

        fieldManager.BroadcastPacket(LiftablePacket.Drop(liftable));
        fieldManager.BroadcastPacket(ResponseCubePacket.PlaceLiftable(liftable, player.FieldPlayer.ObjectId));
        fieldManager.BroadcastPacket(BuildModePacket.Use(player.FieldPlayer, BuildModeHandler.BuildModeType.Stop));

        Task.Run(async () =>
        {
            await Task.Delay(liftable.Metadata.ItemLifeTime);
            fieldManager.BroadcastPacket(LiftablePacket.UpdateEntityByCoord(liftable));
            fieldManager.BroadcastPacket(ResponseCubePacket.RemoveCube(0, 0, liftable.Position.ToByte()));
            fieldManager.BroadcastPacket(LiftablePacket.RemoveCube(liftable));
            liftable.State = LiftableState.Removed;
            liftable.Enabled = false;
            // TODO: regen task
        });
    }

    private static void HandleRemoveCube(GameSession session, PacketReader packet)
    {
        CoordB coord = packet.Read<CoordB>();
        Player player = session.Player;

        bool mapIsHome = player.MapId == (int) Map.PrivateResidence;
        Home home = mapIsHome ? GameServer.HomeManager.GetHomeById(player.VisitingHomeId) : GameServer.HomeManager.GetHomeById(player.Account.Home.Id);

        IFieldObject<Player> homeOwner = player.FieldPlayer;
        if (player.Account.Id != home.AccountId)
        {
            homeOwner = session.FieldManager.State.Players.Values.FirstOrDefault(x => x.Value.AccountId == home.AccountId);
            if (homeOwner is null)
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.ErrUgcMapCantFindDelegateOwner, ChatType.NoticeAlert));
                return;
            }

            if (!home.BuildingPermissions.Contains(player.Account.Id))
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.SocketError10013, ChatType.NoticeAlert));
                return;
            }
        }

        IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord.ToFloat());
        if (cube?.Value.Item is null)
        {
            return;
        }

        RemoveCube(session, homeOwner, cube, home);
    }

    private static void HandleRotateCube(GameSession session, PacketReader packet)
    {
        CoordB coord = packet.Read<CoordB>();
        Player player = session.Player;

        bool mapIsHome = player.MapId == (int) Map.PrivateResidence;
        Home home = mapIsHome ? GameServer.HomeManager.GetHomeById(player.VisitingHomeId) : GameServer.HomeManager.GetHomeById(player.Account.Home.Id);
        if (player.Account.Id != home.AccountId && !home.BuildingPermissions.Contains(player.Account.Id))
        {
            return;
        }

        IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord.ToFloat());
        if (cube is null)
        {
            return;
        }

        cube.Rotation -= CoordF.From(0, 0, 90);
        Dictionary<long, Cube> inventory = player.IsInDecorPlanner ? home.DecorPlannerInventory : home.FurnishingInventory;
        inventory[cube.Value.Uid].Rotation = cube.Rotation;

        session.Send(ResponseCubePacket.RotateCube(session.Player.FieldPlayer, cube));
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
        int fieldPlayerObjectId = session.Player.FieldPlayer.ObjectId;

        bool mapIsHome = player.MapId == (int) Map.PrivateResidence;
        Home home = mapIsHome ? GameServer.HomeManager.GetHomeById(player.VisitingHomeId) : GameServer.HomeManager.GetHomeById(player.Account.Home.Id);

        int plotNumber = MapMetadataStorage.GetPlotNumber(player.MapId, coord);
        if (plotNumber <= 0)
        {
            session.Send(ResponseCubePacket.CantPlaceHere(fieldPlayerObjectId));
            return;
        }

        UgcMapGroup plot = UgcMapMetadataStorage.GetGroupMetadata(player.MapId, (byte) plotNumber);
        byte height = mapIsHome ? home.Height : plot.HeightLimit;
        int size = mapIsHome ? home.Size : plot.Area / 2;
        CoordB? groundHeight = GetGroundCoord(coord, player.MapId, height);
        if (groundHeight is null)
        {
            return;
        }

        bool isCubeSolid = ItemMetadataStorage.GetIsCubeSolid(replacementItemId);
        if (!isCubeSolid && coord.Z == groundHeight?.Z)
        {
            session.Send(ResponseCubePacket.CantPlaceHere(fieldPlayerObjectId));
            return;
        }

        if (IsCoordOutsideHeightLimit(coord.ToShort(), player.MapId, height) || mapIsHome && IsCoordOutsideSizeLimit(coord, size))
        {
            session.Send(ResponseCubePacket.CantPlaceHere(fieldPlayerObjectId));
            return;
        }

        FurnishingShopMetadata shopMetadata = FurnishingShopMetadataStorage.GetMetadata(replacementItemId);
        if (shopMetadata is null || !shopMetadata.Buyable)
        {
            return;
        }

        // Not checking if oldFieldCube is null on ground height because default blocks don't have IFieldObjects.
        IFieldObject<Cube> oldFieldCube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord.ToFloat());
        if (oldFieldCube is null && coord.Z != groundHeight?.Z)
        {
            return;
        }

        IFieldObject<Cube> newFieldCube;
        if (player.IsInDecorPlanner)
        {
            Cube cube = new(new(replacementItemId), plotNumber, coord.ToFloat(), rotation);

            newFieldCube = session.FieldManager.RequestFieldObject(cube);
            newFieldCube.Coord = coord.ToFloat();
            newFieldCube.Rotation = rotation;

            if (oldFieldCube is not null)
            {
                home.DecorPlannerInventory.Remove(oldFieldCube.Value.Uid);
                session.FieldManager.State.RemoveCube(oldFieldCube.ObjectId);
            }

            home.DecorPlannerInventory.Add(cube.Uid, cube);
            session.FieldManager.BroadcastPacket(ResponseCubePacket.ReplaceCube(fieldPlayerObjectId, fieldPlayerObjectId, newFieldCube, false));
            session.FieldManager.State.AddCube(newFieldCube);
            return;
        }

        IFieldObject<Player> homeOwner = player.FieldPlayer;
        if (player.Account.Id != home.AccountId)
        {
            homeOwner = session.FieldManager.State.Players.Values.FirstOrDefault(x => x.Value.AccountId == home.AccountId);
            if (homeOwner is null)
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.ErrUgcMapCantFindDelegateOwner, ChatType.NoticeAlert));
                return;
            }

            if (!home.BuildingPermissions.Contains(player.Account.Id))
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.SocketError10013, ChatType.NoticeAlert));
                return;
            }
        }

        Dictionary<long, Item> warehouseItems = home.WarehouseInventory;
        Dictionary<long, Cube> furnishingInventory = home.FurnishingInventory;
        Item item;
        if (player.Account.Id != home.AccountId)
        {
            if ((!warehouseItems.TryGetValue(replacementItemUid, out item) || warehouseItems[replacementItemUid].Amount <= 0) &&
                !PurchaseFurnishingItemFromBudget(session.FieldManager, homeOwner.Value, home, shopMetadata))
            {
                NotEnoughMoneyInBudget(session, shopMetadata);
                return;
            }
        }
        else
        {
            if ((!warehouseItems.TryGetValue(replacementItemUid, out item) || warehouseItems[replacementItemUid].Amount <= 0) &&
                !PurchaseFurnishingItem(session, shopMetadata))
            {
                NotEnoughMoney(session, shopMetadata);
                return;
            }
        }

        newFieldCube = AddCube(session, item, replacementItemId, rotation, coord.ToFloat(), plotNumber, homeOwner, home);

        if (oldFieldCube is not null)
        {
            furnishingInventory.Remove(oldFieldCube.Value.Uid);
            DatabaseManager.Cubes.Delete(oldFieldCube.Value.Uid);

            homeOwner.Value.Session.Send(FurnishingInventoryPacket.Remove(oldFieldCube.Value));
            session.FieldManager.State.RemoveCube(oldFieldCube.ObjectId);
        }

        homeOwner.Value.Session.Send(FurnishingInventoryPacket.Load(newFieldCube.Value));
        if (oldFieldCube?.Value.Item is not null)
        {
            homeOwner.Value.Inventory.AddItem(homeOwner.Value.Session, oldFieldCube.Value.Item, true);
        }

        session.FieldManager.BroadcastPacket(ResponseCubePacket.ReplaceCube(homeOwner.ObjectId, fieldPlayerObjectId, newFieldCube, false));
        session.FieldManager.AddCube(newFieldCube, homeOwner.ObjectId, fieldPlayerObjectId);
    }

    private static void HandlePickup(GameSession session, PacketReader packet)
    {
        CoordB coords = packet.Read<CoordB>();

        int weaponId = MapEntityMetadataStorage.GetWeaponObjectItemId(session.Player.MapId, coords);
        if (weaponId == 0)
        {
            return;
        }

        session.Send(ResponseCubePacket.Pickup(session.Player.FieldPlayer, weaponId, coords));
        session.FieldManager.BroadcastPacket(UserBattlePacket.UserBattle(session.Player.FieldPlayer, true));
    }

    private static void HandleDrop(GameSession session)
    {
        // Drop item then set battle state to false
        session.Send(ResponseCubePacket.Drop(session.Player.FieldPlayer));
        session.FieldManager.BroadcastPacket(UserBattlePacket.UserBattle(session.Player.FieldPlayer, false));
    }

    private static void HandleHomeName(GameSession session, PacketReader packet)
    {
        string name = packet.ReadUnicodeString();

        Player player = session.Player;
        Home home = player.Account.Home;
        if (player.AccountId != home.AccountId)
        {
            return;
        }

        home.Name = name;
        GameServer.HomeManager.GetHomeById(home.Id).Name = name;
        session.FieldManager.BroadcastPacket(ResponseCubePacket.HomeName(player));
        session.FieldManager.BroadcastPacket(ResponseCubePacket.LoadHome(session.Player.FieldPlayer.ObjectId, session.Player.Account.Home));
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
        GameServer.HomeManager.GetHomeById(home.Id).Password = password;
        session.FieldManager.BroadcastPacket(ResponseCubePacket.ChangePassword());
        session.FieldManager.BroadcastPacket(ResponseCubePacket.LoadHome(session.Player.FieldPlayer.ObjectId, session.Player.Account.Home));
    }

    private static void HandleNominateHouse(GameSession session)
    {
        Player player = session.Player;
        Home home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);

        home.ArchitectScoreCurrent++;
        home.ArchitectScoreTotal++;

        session.FieldManager.BroadcastPacket(ResponseCubePacket.UpdateArchitectScore(home.ArchitectScoreCurrent, home.ArchitectScoreTotal));

        IFieldObject<Player> owner = session.FieldManager.State.Players.Values.FirstOrDefault(x => x.Value.Account.Home.Id == player.VisitingHomeId);
        owner?.Value.Session.Send(HomeCommandPacket.UpdateArchitectScore(owner.ObjectId, home.ArchitectScoreCurrent, home.ArchitectScoreTotal));

        session.Send(ResponseCubePacket.ArchitectScoreExpiration(player.AccountId, TimeInfo.Now()));
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
        GameServer.HomeManager.GetHomeById(home.Id).Description = description;
        session.FieldManager.BroadcastPacket(ResponseCubePacket.HomeDescription(description));
    }

    private static void HandleClearInterior(GameSession session)
    {
        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        if (home is null)
        {
            return;
        }

        foreach (IFieldObject<Cube> cube in session.FieldManager.State.Cubes.Values)
        {
            RemoveCube(session, session.Player.FieldPlayer, cube, home);
        }

        session.Send(ChatPacket.Error(session.Player, SystemNotice.UgcMapPackageAutomaticRemovalCompleted, ChatType.NoticeAlert));
    }

    private static void HandleRequestLayout(GameSession session, PacketReader packet)
    {
        int layoutId = packet.ReadInt();

        if (!session.FieldManager.State.Cubes.IsEmpty)
        {
            session.Send(ChatPacket.Error(session.Player, SystemNotice.ErrUgcMapPackageClearIndoorFirst, ChatType.NoticeAlert));
            return;
        }

        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);

        HomeLayout layout = home?.Layouts.FirstOrDefault(x => x.Id == layoutId);
        if (layout is null)
        {
            return;
        }

        Dictionary<int, int> groupedCubes = layout.Cubes.GroupBy(x => x.Item.Id).ToDictionary(x => x.Key, x => x.Count()); // Dictionary<item id, count> 

        int cubeCount = 0;
        Dictionary<byte, long> cubeCosts = new();
        foreach ((int id, int amount) in groupedCubes)
        {
            FurnishingShopMetadata shopMetadata;
            Item item = home.WarehouseInventory.Values.FirstOrDefault(x => x.Id == id);
            if (item is null)
            {
                shopMetadata = FurnishingShopMetadataStorage.GetMetadata(id);
                if (cubeCosts.ContainsKey(shopMetadata.FurnishingTokenType))
                {
                    cubeCosts[shopMetadata.FurnishingTokenType] += shopMetadata.Price * amount;
                }
                else
                {
                    cubeCosts.Add(shopMetadata.FurnishingTokenType, shopMetadata.Price * amount);
                }

                cubeCount += amount;
                continue;
            }

            if (item.Amount >= amount)
            {
                continue;
            }

            shopMetadata = FurnishingShopMetadataStorage.GetMetadata(id);
            int missingCubes = amount - item.Amount;
            if (cubeCosts.ContainsKey(shopMetadata.FurnishingTokenType))
            {
                cubeCosts[shopMetadata.FurnishingTokenType] += shopMetadata.Price * missingCubes;
            }
            else
            {
                cubeCosts.Add(shopMetadata.FurnishingTokenType, shopMetadata.Price * missingCubes);
            }

            cubeCount += missingCubes;
        }

        session.Send(ResponseCubePacket.BillPopup(cubeCosts, cubeCount));
    }

    private static void HandleDecorPlannerLoadLayout(GameSession session, PacketReader packet)
    {
        int layoutId = packet.ReadInt();

        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        HomeLayout layout = home?.Layouts.FirstOrDefault(x => x.Id == layoutId);

        if (layout is null)
        {
            return;
        }

        home.Size = layout.Size;
        home.Height = layout.Height;
        session.Send(ResponseCubePacket.UpdateHomeSizeAndHeight(layout.Size, layout.Height));

        int x = -1 * Block.BLOCK_SIZE * (home.Size - 1);
        foreach (IFieldObject<Player> fieldPlayer in session.FieldManager.State.Players.Values)
        {
            fieldPlayer.Value.Session.Send(UserMoveByPortalPacket.Move(fieldPlayer, CoordF.From(x, x, Block.BLOCK_SIZE * 3), CoordF.From(0, 0, 0)));
        }

        foreach (Cube layoutCube in layout.Cubes)
        {
            Cube _ = new(new(layoutCube.Item.Id), layoutCube.PlotNumber, layoutCube.CoordF, layoutCube.Rotation);
            IFieldObject<Cube> fieldCube = session.FieldManager.RequestFieldObject(layoutCube);
            fieldCube.Coord = layoutCube.CoordF;
            fieldCube.Rotation = layoutCube.Rotation;
            home.DecorPlannerInventory.Add(layoutCube.Uid, layoutCube);

            session.FieldManager.AddCube(fieldCube, session.Player.FieldPlayer.ObjectId, session.Player.FieldPlayer.ObjectId);
        }

        session.Send(ChatPacket.Error(session.Player, SystemNotice.UgcMapPackageAutomaticCreationCompleted, ChatType.NoticeAlert));
    }

    private static void HandleLoadLayout(GameSession session, PacketReader packet)
    {
        int fieldPlayerObjectId = session.Player.FieldPlayer.ObjectId;
        int layoutId = packet.ReadInt();

        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        HomeLayout layout = home.Layouts.FirstOrDefault(x => x.Id == layoutId);

        if (layout == default)
        {
            return;
        }

        home.Size = layout.Size;
        home.Height = layout.Height;
        session.Send(ResponseCubePacket.UpdateHomeSizeAndHeight(layout.Size, layout.Height));

        int x = -1 * Block.BLOCK_SIZE * (home.Size - 1);
        foreach (IFieldObject<Player> fieldPlayer in session.FieldManager.State.Players.Values)
        {
            fieldPlayer.Value.Session.Send(UserMoveByPortalPacket.Move(fieldPlayer, CoordF.From(x, x, Block.BLOCK_SIZE * 3), CoordF.From(0, 0, 0)));
        }

        foreach (Cube cube in layout.Cubes)
        {
            Item item = home.WarehouseInventory.Values.FirstOrDefault(y => y.Id == cube.Item.Id);
            IFieldObject<Cube> fieldCube = AddCube(session, item, cube.Item.Id, cube.Rotation, cube.CoordF, cube.PlotNumber, session.Player.FieldPlayer, home);
            session.Send(FurnishingInventoryPacket.Load(fieldCube.Value));
            if (fieldCube.Coord.Z == 0)
            {
                session.FieldManager.BroadcastPacket(ResponseCubePacket.ReplaceCube(fieldPlayerObjectId, fieldPlayerObjectId, fieldCube, false));
            }

            session.FieldManager.AddCube(fieldCube, fieldPlayerObjectId, fieldPlayerObjectId);
        }

        session.Send(WarehouseInventoryPacket.Count(home.WarehouseInventory.Count));
        session.Send(ChatPacket.Error(session.Player, SystemNotice.UgcMapPackageAutomaticCreationCompleted, ChatType.NoticeAlert));
    }

    private static void HandleModifySize(GameSession session, RequestCubeMode mode)
    {
        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        if (session.Player.AccountId != home.AccountId)
        {
            return;
        }

        switch (mode)
        {
            case RequestCubeMode.IncreaseSize when home.Size + 1 > 25:
            case RequestCubeMode.IncreaseHeight when home.Height + 1 > 15:
            case RequestCubeMode.DecreaseSize when home.Size - 1 < 4:
            case RequestCubeMode.DecreaseHeight when home.Height - 1 < 3:
                return;
        }

        RemoveBlocks(session, mode, home);

        if (session.Player.IsInDecorPlanner)
        {
            switch (mode)
            {
                case RequestCubeMode.IncreaseSize:
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.IncreaseSize(++home.DecorPlannerSize));
                    break;
                case RequestCubeMode.DecreaseSize:
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.DecreaseSize(--home.DecorPlannerSize));
                    break;
                case RequestCubeMode.IncreaseHeight:
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.IncreaseHeight(++home.DecorPlannerHeight));
                    break;
                case RequestCubeMode.DecreaseHeight:
                    session.FieldManager.BroadcastPacket(ResponseCubePacket.DecreaseHeight(--home.DecorPlannerHeight));
                    break;
            }
        }
        else
        {
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
        }

        if (mode is not (RequestCubeMode.DecreaseHeight or RequestCubeMode.DecreaseSize))
        {
            return;
        }

        // move players to safe coord
        int x;
        if (session.Player.IsInDecorPlanner)
        {
            x = -1 * Block.BLOCK_SIZE * (home.DecorPlannerSize - 1);
        }
        else
        {
            x = -1 * Block.BLOCK_SIZE * (home.Size - 1);
        }

        foreach (IFieldObject<Player> fieldPlayer in session.FieldManager.State.Players.Values)
        {
            fieldPlayer.Value.Session.Send(UserMoveByPortalPacket.Move(fieldPlayer, CoordF.From(x, x, Block.BLOCK_SIZE * 3), CoordF.From(0, 0, 0)));
        }
    }

    private static void HandleSaveLayout(GameSession session, PacketReader packet)
    {
        int layoutId = packet.ReadInt();
        string layoutName = packet.ReadUnicodeString();

        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        HomeLayout layout = home?.Layouts.FirstOrDefault(x => x.Id == layoutId);
        if (layout is null)
        {
            return;
        }

        DatabaseManager.HomeLayouts.Delete(layout.Uid);
        home.Layouts.Remove(layout);

        if (session.Player.IsInDecorPlanner)
        {
            home.Layouts.Add(new(home.Id, layoutId, layoutName, home.DecorPlannerSize, home.DecorPlannerHeight, TimeInfo.Now(),
                home.DecorPlannerInventory.Values.ToList()));
        }
        else
        {
            home.Layouts.Add(new(home.Id, layoutId, layoutName, home.Size, home.Height, TimeInfo.Now(), home.FurnishingInventory.Values.ToList()));
        }

        session.Send(ResponseCubePacket.SaveLayout(home.AccountId, layoutId, layoutName, TimeInfo.Now()));
    }

    private static void HandleDecorationReward(GameSession session)
    {
        // Decoration score goals
        Dictionary<ItemHousingCategory, int> goals = new()
        {
            {
                ItemHousingCategory.Bed,
                1
            },
            {
                ItemHousingCategory.Table,
                1
            },
            {
                ItemHousingCategory.SofasChairs,
                2
            },
            {
                ItemHousingCategory.Storage,
                1
            },
            {
                ItemHousingCategory.WallDecoration,
                1
            },
            {
                ItemHousingCategory.WallTiles,
                3
            },
            {
                ItemHousingCategory.Bathroom,
                1
            },
            {
                ItemHousingCategory.Lighting,
                1
            },
            {
                ItemHousingCategory.Electronics,
                1
            },
            {
                ItemHousingCategory.Fences,
                2
            },
            {
                ItemHousingCategory.NaturalTerrain,
                4
            }
        };

        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        if (home is null || session.Player.AccountId != home.AccountId)
        {
            return;
        }

        List<Item> items = home.FurnishingInventory.Values.Select(x => x.Item).ToList();
        items.ForEach(x => x.HousingCategory = ItemMetadataStorage.GetHousingCategory(x.Id));

        Dictionary<ItemHousingCategory, int> current = items.GroupBy(x => x.HousingCategory).ToDictionary(x => x.Key, x => x.Count());

        int decorationScore = 0;
        foreach (ItemHousingCategory category in goals.Keys)
        {
            if (!current.TryGetValue(category, out int currentCount))
            {
                continue;
            }

            if (currentCount >= goals[category])
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

        List<int> rewardsIds = new()
        {
            20300039,
            20000071,
            20301018
        }; // Default rewards

        switch (decorationScore)
        {
            case < 300:
                rewardsIds.Add(20000028);
                break;
            case >= 300 and < 500:
                rewardsIds.Add(20000029);
                break;
            case >= 500 and < 1100:
                rewardsIds.Add(20000030);
                rewardsIds.Add(20300078);
                break;
            default:
                rewardsIds.Add(20000031);
                rewardsIds.Add(20300078);
                rewardsIds.Add(20300040);
                session.Player.Inventory.AddItem(session, new(20300559), true);
                break;
        }

        home.GainExp(decorationScore);
        session.Player.Inventory.AddItem(session, new(rewardsIds.OrderBy(_ => RandomProvider.Get().Next()).First()), true);
        home.DecorationRewardTimestamp = TimeInfo.Now();
        session.Send(ResponseCubePacket.DecorationScore(home));
    }

    private static void HandleInteriorDesignReward(GameSession session, PacketReader packet)
    {
        byte rewardId = packet.ReadByte();
        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        if (home is null || session.Player.AccountId != home.AccountId)
        {
            return;
        }

        if (rewardId is <= 1 or >= 11 || home.InteriorRewardsClaimed.Contains(rewardId))
        {
            return;
        }

        MasteryUgcHousingMetadata metadata = MasteryUgcHousingMetadataStorage.GetMetadata(rewardId);
        if (metadata is null)
        {
            return;
        }

        session.Player.Inventory.AddItem(session, new(metadata.ItemId), true);
        home.InteriorRewardsClaimed.Add(rewardId);
        session.Send(ResponseCubePacket.DecorationScore(home));
    }

    private static void HandleKickEveryone(GameSession session)
    {
        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        if (session.Player.AccountId != home.AccountId)
        {
            return;
        }

        long playerCharacterId = session.Player.CharacterId;

        IEnumerable<IFieldActor<Player>> players = session.FieldManager.State.Players.Values.Where(p => p.Value.CharacterId != playerCharacterId);
        foreach (IFieldObject<Player> fieldPlayer in players)
        {
            fieldPlayer.Value.Session.Send(ResponseCubePacket.KickEveryone());
        }

        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            players = session.FieldManager.State.Players.Values.Where(p => p.Value.CharacterId != playerCharacterId);

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

        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
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

        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        if (session.Player.AccountId != home.AccountId)
        {
            return;
        }

        if (home.Permissions.ContainsKey(permission))
        {
            home.Permissions[permission] = setting;
        }

        session.FieldManager.BroadcastPacket(ResponseCubePacket.SetPermission(permission, setting));
    }

    private static void HandleUpdateBudget(GameSession session, PacketReader packet)
    {
        long mesos = packet.ReadLong();
        long merets = packet.ReadLong();

        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        if (home is null || session.Player.AccountId != home.AccountId)
        {
            return;
        }

        home.Mesos = mesos;
        home.Merets = merets;

        session.FieldManager.BroadcastPacket(ResponseCubePacket.UpdateBudget(home));
    }

    private static void HandleModifyInteriorSettings(GameSession session, RequestCubeMode mode, PacketReader packet)
    {
        byte value = packet.ReadByte();

        Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
        if (home is null || session.Player.AccountId != home.AccountId)
        {
            return;
        }

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

        Player player = session.Player;
        Home home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);
        if (home is null || player.AccountId != home.AccountId)
        {
            return;
        }

        Player target = GameServer.PlayerManager.GetPlayerByName(characterName);
        if (home.BuildingPermissions.Contains(target.AccountId))
        {
            return;
        }

        home.BuildingPermissions.Add(target.AccountId);

        session.Send(ResponseCubePacket.AddBuildingPermission(target.AccountId));
        session.Send(ChatPacket.Error(session.Player, SystemNotice.UgcMapPackageAutomaticCreationCompleted, ChatType.NoticeAlert));
        session.Send(NoticePacket.Notice(SystemNotice.UgcMapGiveDelegatorUser, NoticeType.Chat | NoticeType.FastText, new(){target.Name}));

        target.Session.Send(ResponseCubePacket.UpdateBuildingPermissions(target.AccountId, player.AccountId));
        target.Session.Send(ChatPacket.Error(session.Player, SystemNotice.UgcMapAddDelegatorUser, ChatType.NoticeAlert));
    }

    private static void HandleRemoveBuildingPermission(GameSession session, PacketReader packet)
    {
        string characterName = packet.ReadUnicodeString();

        Player player = session.Player;
        Home home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);
        if (home is null || player.AccountId != home.AccountId)
        {
            return;
        }

        Player target = GameServer.PlayerManager.GetPlayerByName(characterName);
        if (!home.BuildingPermissions.Contains(target.AccountId))
        {
            return;
        }

        home.BuildingPermissions.Remove(target.AccountId);

        session.Send(ResponseCubePacket.RemoveBuildingPermission(target.AccountId, target.Name));
        target.Session.Send(ResponseCubePacket.UpdateBuildingPermissions(0, player.AccountId));
        target.Session.Send(ChatPacket.Error(session.Player, SystemNotice.UgcMapReleaseDelegatorUser, ChatType.NoticeAlert));

    }

    private static bool PurchaseFurnishingItem(GameSession session, FurnishingShopMetadata shop)
    {
        switch (shop.FurnishingTokenType)
        {
            case 1: // meso
                return session.Player.Wallet.Meso.Modify(-shop.Price);
            case 3: // meret
                return session.Player.Account.RemoveMerets(shop.Price);
            default:
                session.SendNotice($"Unknown currency: {shop.FurnishingTokenType}");
                return false;
        }
    }

    private static bool PurchaseFurnishingItemFromBudget(FieldManager fieldManager, Player owner, Home home, FurnishingShopMetadata shop)
    {
        switch (shop.FurnishingTokenType)
        {
            case 1: // meso
                if (home.Mesos - shop.Price < 0)
                {
                    return false;
                }

                home.Mesos -= shop.Price;
                owner.Wallet.Meso.Modify(-shop.Price);
                fieldManager.BroadcastPacket(ResponseCubePacket.UpdateBudget(home));
                return true;
            case 3: // meret
                if (home.Merets - shop.Price < 0)
                {
                    return false;
                }

                home.Merets -= shop.Price;
                owner.Account.RemoveMerets(shop.Price);
                fieldManager.BroadcastPacket(ResponseCubePacket.UpdateBudget(home));
                return true;
            default:
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
                return session.Player.Account.RemoveMerets(price);
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
        Dictionary<CoordS, MapBlock> mapBlocks = MapMetadataStorage.GetMetadata(mapId).Blocks;
        for (int i = 0; i <= height; i++) // checking blocks in the same Z axis
        {
            mapBlocks.TryGetValue(coordS, out MapBlock block);
            if (block is not null)
            {
                return false;
            }

            coordS.Z -= Block.BLOCK_SIZE;
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
        Dictionary<CoordS, MapBlock> mapBlocks = MapMetadataStorage.GetMetadata(mapId).Blocks;
        for (int i = 0; i <= height; i++)
        {
            mapBlocks.TryGetValue(coord.ToShort(), out MapBlock block);
            if (block is not null)
            {
                return block.Coord.ToByte();
            }

            coord -= CoordB.From(0, 0, 1);
        }

        return null;
    }

    private static void RemoveBlocks(GameSession session, RequestCubeMode mode, Home home)
    {
        if (mode is RequestCubeMode.DecreaseSize)
        {
            int maxSize = (home.Size - 1) * Block.BLOCK_SIZE * -1;
            for (int i = 0; i < home.Size; i++)
            {
                for (int j = 0; j <= home.Height; j++)
                {
                    CoordF coord = CoordF.From(maxSize, i * Block.BLOCK_SIZE * -1, j * Block.BLOCK_SIZE);
                    IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord);
                    if (cube is not null)
                    {
                        RemoveCube(session, session.Player.FieldPlayer, cube, home);
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
                        RemoveCube(session, session.Player.FieldPlayer, cube, home);
                    }
                }
            }
        }

        if (mode is RequestCubeMode.DecreaseHeight)
        {
            for (int i = 0; i < home.Size; i++)
            {
                for (int j = 0; j < home.Size; j++)
                {
                    CoordF coord = CoordF.From(i * Block.BLOCK_SIZE * -1, j * Block.BLOCK_SIZE * -1, home.Height * Block.BLOCK_SIZE);
                    IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coord);
                    if (cube is not null)
                    {
                        RemoveCube(session, session.Player.FieldPlayer, cube, home);
                    }
                }
            }
        }
    }

    private static IFieldObject<Cube> AddCube(GameSession session, Item item, int itemId, CoordF rotation, CoordF coordF, int plotNumber,
        IFieldObject<Player> homeOwner, Home home)
    {
        IFieldObject<Cube> fieldCube;
        Dictionary<long, Item> warehouseItems = home.WarehouseInventory;
        Dictionary<long, Cube> furnishingInventory = home.FurnishingInventory;
        if (item is null || item.Amount <= 0)
        {
            Cube cube = new(new(itemId), plotNumber, coordF, rotation, homeId: home.Id);

            fieldCube = session.FieldManager.RequestFieldObject(cube);
            fieldCube.Coord = coordF;
            fieldCube.Rotation = rotation;

            homeOwner.Value.Session.Send(WarehouseInventoryPacket.Load(cube.Item, warehouseItems.Values.Count));
            homeOwner.Value.Session.Send(WarehouseInventoryPacket.GainItemMessage(cube.Item, 1));
            homeOwner.Value.Session.Send(WarehouseInventoryPacket.Count(warehouseItems.Values.Count + 1));
            session.FieldManager.BroadcastPacket(ResponseCubePacket.PlaceFurnishing(fieldCube, homeOwner.ObjectId, session.Player.FieldPlayer.ObjectId, true));
            homeOwner.Value.Session.Send(WarehouseInventoryPacket.Remove(cube.Item.Uid));
        }
        else
        {
            Cube cube = new(item, plotNumber, coordF, rotation, homeId: home.Id);

            fieldCube = session.FieldManager.RequestFieldObject(cube);
            fieldCube.Coord = coordF;
            fieldCube.Rotation = rotation;

            if (item.Amount - 1 > 0)
            {
                item.Amount--;
                session.Send(WarehouseInventoryPacket.UpdateAmount(item.Uid, item.Amount));
            }
            else
            {
                warehouseItems.Remove(item.Uid);
                session.Send(WarehouseInventoryPacket.Remove(item.Uid));
            }
        }

        furnishingInventory.Add(fieldCube.Value.Uid, fieldCube.Value);
        return fieldCube;
    }

    private static void RemoveCube(GameSession session, IFieldObject<Player> homeOwner, IFieldObject<Cube> cube, Home home)
    {
        Dictionary<long, Cube> furnishingInventory = home.FurnishingInventory;

        if (session.Player.IsInDecorPlanner)
        {
            home.DecorPlannerInventory.Remove(cube.Value.Uid);
            session.FieldManager.RemoveCube(cube, homeOwner.ObjectId, session.Player.FieldPlayer.ObjectId);
            return;
        }

        furnishingInventory.Remove(cube.Value.Uid);
        homeOwner.Value.Session.Send(FurnishingInventoryPacket.Remove(cube.Value));

        DatabaseManager.Cubes.Delete(cube.Value.Uid);
        homeOwner.Value.Inventory.AddItem(session, cube.Value.Item, true);
        session.FieldManager.RemoveCube(cube, homeOwner.ObjectId, session.Player.FieldPlayer.ObjectId);
    }

    private static string GetCurrencyText(FurnishingShopMetadata shopMetadata)
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

        return currency;
    }

    private static void NotEnoughMoney(GameSession session, FurnishingShopMetadata shopMetadata)
    {
        session.SendNotice($"You don't have enough {GetCurrencyText(shopMetadata)}!");
    }

    private static void NotEnoughMoneyInBudget(GameSession session, FurnishingShopMetadata shopMetadata)
    {
        session.SendNotice($"Budget doesn't have enough {GetCurrencyText(shopMetadata)}!");
    }
}
