using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ResponseCubePacket
    {
        private enum ResponseCubePacketMode : byte
        {
            LoadFurnishingItem = 0x1,
            EnablePlotFurnishing = 0x2,
            LoadPurchasedLand = 0x3,
            CompletePurchase = 0x4,
            PlaceFurnishing = 0xA,
            RemoveCube = 0xC,
            RotateCube = 0xE,
            ReplaceCube = 0xF,
            Pickup = 0x11,
            Drop = 0x12,
            LoadHome = 0x14,
            HomeName = 0x15,
            PurchasePlot = 0x16,
            ChangePassword = 0x18,
            HomeDescription = 0x1D,
            ReturnMap = 0x22,
            IncreaseSize = 0x25,
            DecreaseSize = 0x26,
            Rewards = 0x27, // decoration score
            EnablePermission = 0x2A,
            SetPermission = 0x2B,
            IncreaseHeight = 0x2C,
            DecreaseHeight = 0x2D,
            ChangeBackground = 0x33,
            ChangeLighting = 0x34,
            ChangeCamera = 0x36,
        }

        public static Packet LoadFurnishingItem(IFieldObject<Player> player, int itemId, long itemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.LoadFurnishingItem);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(itemId);
            pWriter.WriteLong(itemUid);
            pWriter.WriteLong();
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet EnablePlotFurnishing(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.EnablePlotFurnishing);
            pWriter.WriteByte(); // disable bool
            pWriter.WriteInt(player.Account.Home.PlotNumber);
            pWriter.WriteInt(player.Account.Home.ApartmentNumber);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteLong(player.Account.Home.Expiration);
            pWriter.WriteLong(player.CharacterId); // check this
            return pWriter;
        }

        public static Packet CompletePurchase()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.CompletePurchase);
            return pWriter;
        }

        public static Packet PlaceFurnishing(IFieldObject<Cube> cube, IFieldObject<Player> player, bool sendOnlyObjectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.PlaceFurnishing);
            pWriter.WriteBool(sendOnlyObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);

            if (sendOnlyObjectId)
            {
                return pWriter;
            }
            pWriter.WriteInt(cube.Value.PlotNumber);
            pWriter.WriteInt();
            pWriter.Write(cube.Coord.ToByte());
            pWriter.WriteByte();
            pWriter.WriteLong(cube.Value.Uid);
            pWriter.WriteInt(cube.Value.Item.Id);
            pWriter.WriteLong(cube.Value.Item.Uid);
            pWriter.WriteLong();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.Write(cube.Rotation.Z);
            pWriter.WriteInt();
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet RemoveCube(IFieldObject<Player> player, CoordB coord)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.RemoveCube);
            pWriter.WriteByte();
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.Write(coord);
            pWriter.WriteByte();
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet RotateCube(IFieldObject<Player> player, IFieldObject<Cube> cube)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.RotateCube);
            pWriter.WriteByte();
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.Write(cube.Coord.ToByte());
            pWriter.WriteByte();
            pWriter.WriteFloat(cube.Rotation.Z);

            return pWriter;
        }

        public static Packet ReplaceCube(IFieldObject<Player> player, Item item, IFieldObject<Cube> newCube, bool sendOnlyObjectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ReplaceCube);
            pWriter.WriteBool(sendOnlyObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);

            if (sendOnlyObjectId)
            {
                return pWriter;
            }
            pWriter.Write(newCube.Coord.ToByte());
            pWriter.WriteByte();
            pWriter.WriteInt(newCube.Value.Item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteLong(newCube.Value.Uid);
            pWriter.WriteLong();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteFloat(newCube.Rotation.Z);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet Pickup(GameSession session, int weaponId, byte[] coords)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.Pickup);
            pWriter.WriteZero(1);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.Write(coords);
            pWriter.WriteZero(1);
            pWriter.WriteInt(weaponId);
            pWriter.WriteInt(GuidGenerator.Int()); // Item uid

            return pWriter;
        }

        public static Packet Drop(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.Drop);
            pWriter.WriteZero(1);
            pWriter.WriteInt(player.ObjectId);

            return pWriter;
        }

        public static Packet LoadHome(IFieldObject<Player> fieldPlayer)
        {
            Player player = fieldPlayer.Value;

            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.LoadHome);
            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.WriteInt(player.Account.Home?.MapId ?? 0);
            pWriter.WriteInt(player.Account.Home?.PlotId ?? 0);
            pWriter.WriteInt(player.Account.Home?.PlotNumber ?? 0);
            pWriter.WriteInt(player.Account.Home?.ApartmentNumber ?? 0);
            pWriter.WriteUnicodeString(player.Account.Home?.Name ?? "");
            pWriter.WriteLong(player.Account.Home?.Expiration ?? 0);
            pWriter.WriteLong();
            pWriter.WriteByte(1);

            return pWriter;
        }

        public static Packet HomeName(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.HomeName);
            pWriter.WriteByte();
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteInt(player.Account.Home.PlotNumber);
            pWriter.WriteInt(player.Account.Home.ApartmentNumber);
            pWriter.WriteUnicodeString(player.Account.Home.Name);
            return pWriter;
        }

        public static Packet PurchasePlot(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.PurchasePlot);
            pWriter.WriteInt(player.Account.Home.PlotNumber);
            pWriter.WriteInt(player.Account.Home.ApartmentNumber);
            pWriter.WriteByte(1);
            pWriter.WriteLong(player.Account.Home.Expiration);
            return pWriter;
        }

        public static Packet ChangePassword()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ChangePassword);
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet HomeDescription(string description)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.HomeDescription);
            pWriter.WriteByte();
            pWriter.WriteUnicodeString(description);

            return pWriter;
        }

        public static Packet ReturnMap(int mapId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ReturnMap);
            pWriter.WriteInt(mapId);

            return pWriter;
        }

        public static Packet IncreaseSize(byte size)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.IncreaseSize);
            pWriter.WriteByte();
            pWriter.WriteByte(size);

            return pWriter;
        }

        public static Packet DecreaseSize(byte size)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.DecreaseSize);
            pWriter.WriteByte();
            pWriter.WriteByte(size);

            return pWriter;
        }

        public static Packet EnablePermission(HomePermission permission, bool enabled)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.EnablePermission);
            pWriter.WriteEnum(permission);
            pWriter.WriteBool(enabled);

            return pWriter;
        }

        public static Packet SetPermission(HomePermission permission, byte setting)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.SetPermission);
            pWriter.WriteEnum(permission);
            pWriter.WriteByte(setting);

            return pWriter;
        }

        public static Packet IncreaseHeight(byte height)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.IncreaseHeight);
            pWriter.WriteByte();
            pWriter.WriteByte(height);

            return pWriter;
        }

        public static Packet DecreaseHeight(byte size)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.DecreaseHeight);
            pWriter.WriteByte();
            pWriter.WriteByte(size);

            return pWriter;
        }

        public static Packet ChangeBackground(byte lighthing)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ChangeLighting);
            pWriter.WriteByte();
            pWriter.WriteByte(lighthing);

            return pWriter;
        }

        public static Packet ChangeLighting(byte background)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ChangeBackground);
            pWriter.WriteByte();
            pWriter.WriteByte(background);

            return pWriter;
        }

        public static Packet ChangeCamera(byte camera)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ChangeCamera);
            pWriter.WriteByte();
            pWriter.WriteByte(camera);

            return pWriter;
        }

        public static Packet Mode27(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.Rewards);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(); // some date
            pWriter.WriteLong(10);
            pWriter.WriteLong();
            pWriter.WriteInt(9);
            pWriter.WriteInt(3);
            pWriter.WriteInt(5);
            pWriter.WriteInt(4);
            pWriter.WriteInt(2);
            pWriter.WriteInt(6);
            pWriter.WriteInt(7);
            pWriter.WriteInt(8);
            pWriter.WriteInt(9);
            pWriter.WriteInt(10);


            return pWriter;
        }
    }
}
