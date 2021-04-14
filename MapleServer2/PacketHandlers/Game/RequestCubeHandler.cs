using System;
using System.Linq;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
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
            NameHome = 0x15,
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
                case RequestCubeMode.NameHome:
                    HandleNameHome(session, packet);
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
            session.FieldManager.BroadcastPacket(ResponseCubePacket.LoadFurnishingItem(session.FieldPlayer, itemId));
        }

        private static void HandleBuyPlot(GameSession session, PacketReader packet)
        {
            int groupId = packet.ReadInt();
            int housingTemplate = packet.ReadInt();

            //TODO: If player already owns a plot, reject

            UGCMapGroup land = UGCMapMetadataStorage.GetMetadata(session.Player.MapId, (byte) groupId);

            /*
            if(!HandlePlotPayment(session, land.PriceItemCode, land.Price))
            {
                return;
            }*/

            session.Player.HomeMapId = session.Player.MapId;
            session.Player.HomeMapPlotId = land.Id;
            session.Player.HomeExpiration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount + (land.ContractDate * (24 * 60 * 60));
            session.Player.HomeName = session.Player.Name;

            session.FieldManager.BroadcastPacket(ResponseCubePacket.PurchasePlot(session.Player));
            session.FieldManager.BroadcastPacket(ResponseCubePacket.EnablePlotFurnishing(session.Player));
            session.Send(ResponseCubePacket.UpdatePlot(session.FieldPlayer));
            session.FieldManager.BroadcastPacket(ResponseCubePacket.NameHome(session.Player), session);
            session.Send(ResponseCubePacket.CompletePurchase());
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
                    if (!session.Player.Wallet.Meso.Modify(-price))
                    {
                        return false;
                    }
                    break;
                case 90000004:
                    if (!session.Player.Wallet.RemoveMerets(price))
                    {
                        return false;
                    }
                    break;
                case 90000006:
                    if (!session.Player.Wallet.ValorToken.Modify(-price))
                    {
                        return false;
                    }
                    break;
                case 90000013:
                    if (!session.Player.Wallet.Rue.Modify(-price))
                    {
                        return false;
                    }
                    break;
                case 90000014:
                    if (!session.Player.Wallet.HaviFruit.Modify(-price))
                    {
                        return false;
                    }
                    break;
                case 90000017:
                    if (!session.Player.Wallet.Treva.Modify(-price))
                    {
                        return false;
                    }
                    break;
                default:
                    session.SendNotice($"Unknown item currency: {priceItemCode}");
                    return false;
            }
            return true;
        }

        private static void HandleForfeitPlot(GameSession session)
        {
            // TODO
        }

        private static void HandleAddFurnishing(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();
            byte padding = packet.ReadByte();
            int itemId = packet.ReadInt();
            long itemUid = packet.ReadLong();
            byte padding2 = packet.ReadByte();
            CoordF rotation = packet.Read<CoordF>();

            int plotNumber = MapMetadataStorage.GetPlotNumber(session.Player.MapId, coord);
            if (plotNumber < 0)
            {
                return;
            }

            // TODO: Check if player has rights to this plot

            FurnishingShopMetadata shopMetadata = FurnishingShopMetadataStorage.GetMetadata(itemId);
            if (shopMetadata == null || !shopMetadata.Buyable)
            {
                return;
            }

            if (itemUid == 0) // player needs to purchase
            {
                if (!PurchaseFurnishingItem(session, shopMetadata))
                {
                    return;
                }


                Item item = new Item(itemId);

                //TODO: Add and remove appropriate item to warehouse inventory and furnishing inventory

                Cube cube = new Cube(item, plotNumber);

                IFieldObject<Cube> fieldCube = session.FieldManager.RequestFieldObject(cube);
                fieldCube.Coord = coord.ToFloat();
                fieldCube.Rotation = rotation;
                session.FieldManager.AddUGCCube(fieldCube);
            }

        }

        private static void HandleRemoveCube(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();

            // TODO: Find cube in coord
            // TODO: Handle Furnishing/Warehouse inventories

            session.Send(ResponseCubePacket.RemoveCube(session.FieldPlayer, coord));
        }

        private static void HandleRotateCube(GameSession session, PacketReader packet)
        {
            CoordB coord = packet.Read<CoordB>();

            // TODO: Check if there is a cube in this location and if so, rotate it -90
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

            int plotNumber = MapMetadataStorage.GetPlotNumber(session.Player.MapId, coord);
            if (plotNumber < 0)
            {
                return;
            }
            //TODO: Add and remove appropriate items to warehouse inventory and furnishing inventory

            //TODO: If itemUid is not found, send player to purchase the item
            Item item = new Item(replacementItemId);

            Cube cube = new Cube(item, plotNumber);

            IFieldObject<Cube> fieldCube = session.FieldManager.RequestFieldObject(cube);
            fieldCube.Coord = coord.ToFloat();
            fieldCube.Rotation = rotation;

            session.FieldManager.BroadcastPacket(ResponseCubePacket.ReplaceCube(session.FieldPlayer, fieldCube));

            session.FieldManager.AddUGCCube(fieldCube);
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

        private static void HandleNameHome(GameSession session, PacketReader packet)
        {
            string name = packet.ReadUnicodeString();
            session.Player.HomeName = name;
            session.FieldManager.BroadcastPacket(ResponseCubePacket.NameHome(session.Player));
            session.FieldManager.BroadcastPacket(ResponseCubePacket.UpdatePlot(session.FieldPlayer));

        }

        private static bool PurchaseFurnishingItem(GameSession session, FurnishingShopMetadata shop) // bool it
        {
            switch (shop.FurnishingTokenType)
            {
                case 1: // meso
                    if (!session.Player.Wallet.Meso.Modify(-shop.Price))
                    {
                        return false;
                    }
                    break;
                case 3: // meret
                    if (!session.Player.Wallet.RemoveMerets(shop.Price))
                    {
                        return false;
                    }
                    break;
                default:
                    session.SendNotice($"Unknown currency: {shop.FurnishingTokenType}");
                    return false;
            }
            return true;
        }

    }
}
