using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
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
            ReplaceCube = 0xF,
            Pickup = 0x11,
            Drop = 0x12,
            UpdatePlot = 0x14,
            NameHome = 0x15,
            PurchasePlot = 0x16
        }

        public static Packet LoadFurnishingItem(IFieldObject<Player> player, int itemId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.LoadFurnishingItem);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(itemId);
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet EnablePlotFurnishing(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.EnablePlotFurnishing);
            pWriter.WriteByte(); // disable bool
            pWriter.WriteInt(player.HomeMapPlotId);
            pWriter.WriteInt(player.ApartmentNumber);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteLong(player.HomeExpiration);
            pWriter.WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet CompletePurchase()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.CompletePurchase);
            return pWriter;
        }

        public static Packet PlaceFurnishing(IFieldObject<Cube> cube)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.PlaceFurnishing);
            pWriter.WriteBool(false); // unk bool
            pWriter.WriteInt(cube.ObjectId); // objectId PALYER!
            pWriter.WriteInt(cube.ObjectId); // objectId PALYER!
            pWriter.WriteInt(cube.Value.PlotNumber);
            pWriter.WriteInt();
            pWriter.Write(cube.Coord.ToShort().ToByte()); // coords of block
            pWriter.WriteByte();
            pWriter.WriteLong(cube.Value.Item.Uid); // item uid
            pWriter.WriteInt(cube.Value.Item.Id); // itemid
            pWriter.WriteLong(cube.Value.Item.Uid); // item uid
            pWriter.WriteLong();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.Write(cube.Rotation.Z); // CoordF.Z rotation
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

        public static Packet ReplaceCube(IFieldObject<Player> player, IFieldObject<Cube> newCube)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ReplaceCube);
            pWriter.WriteByte();
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.Write(newCube.Coord.ToShort().ToByte());
            pWriter.WriteByte();
            pWriter.WriteInt(newCube.Value.Item.Id);
            pWriter.WriteLong(newCube.Value.Item.Uid);
            pWriter.WriteLong(); // previous item Uid
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

        public static Packet UpdatePlot(IFieldObject<Player> fieldPlayer)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.UpdatePlot);
            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.WriteInt(62000000);// interior map ID?
            pWriter.WriteInt(fieldPlayer.Value.HomeMapId);
            pWriter.WriteInt(fieldPlayer.Value.HomeMapPlotId);
            pWriter.WriteInt(fieldPlayer.Value.ApartmentNumber);
            pWriter.WriteUnicodeString(fieldPlayer.Value.HomeName);
            pWriter.WriteLong(fieldPlayer.Value.HomeExpiration);
            pWriter.WriteLong(); // some timestamp
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet NameHome(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.NameHome);
            pWriter.WriteByte();
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteInt(player.HomeMapPlotId);
            pWriter.WriteInt(player.ApartmentNumber);
            pWriter.WriteUnicodeString(player.HomeName);
            return pWriter;
        }
        public static Packet PurchasePlot(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.PurchasePlot);
            pWriter.WriteInt(player.HomeMapPlotId);
            pWriter.WriteInt(player.ApartmentNumber);
            pWriter.WriteByte(1);
            pWriter.WriteLong(player.HomeExpiration);
            return pWriter;
        }
    }
}
