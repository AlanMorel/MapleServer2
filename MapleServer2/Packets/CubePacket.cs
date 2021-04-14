using System.Collections.Concurrent;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{

    public static class CubePacket
    {
        private enum CubePacketMode : byte
        {
            LoadCubes = 0x0,
            LoadPurchasedLand = 0x1,
        }

        public static Packet LoadCubes(ICollection<IFieldObject<Cube>> fieldCube)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CUBES);
            pWriter.WriteEnum(CubePacketMode.LoadCubes);
            pWriter.WriteByte();
            pWriter.WriteInt(fieldCube.Count);
            foreach(IFieldObject<Cube> cube in fieldCube)
            {
                pWriter.Write(cube.Coord.ToShort().ToByte());
                pWriter.WriteByte();
                pWriter.WriteLong(cube.Value.Id);
                pWriter.WriteInt(cube.Value.Item.Id);
                pWriter.WriteLong(cube.Value.Item.Uid);
                pWriter.WriteLong();
                pWriter.WriteByte(); // UGC bool ?
                pWriter.WriteInt(cube.Value.PlotNumber);
                pWriter.WriteInt();
                pWriter.WriteByte();
                pWriter.WriteFloat(cube.Rotation.Z);
                pWriter.WriteInt();
                pWriter.WriteByte();
            }
            return pWriter;
        }

        public static Packet LoadPurchasedLand()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(CubePacketMode.LoadPurchasedLand);
            pWriter.WriteInt(); // count
            //loop
            pWriter.WriteInt(); //  plot number
            pWriter.WriteInt(); // apartment number
            pWriter.WriteByte(1); // ??
            pWriter.WriteLong(); // expiration
            return pWriter;
        }
    }
}
