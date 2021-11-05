using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FurnishingInventoryPacket
{
    private enum FurnishingInventoryPacketMode : byte
    {
        StartList = 0x0,
        Load = 0x1,
        Remove = 0x2,
        EndList = 0x4
    }

    public static PacketWriter StartList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FURNISHING_INVENTORY);
        pWriter.Write(FurnishingInventoryPacketMode.StartList);

        return pWriter;
    }

    public static PacketWriter Load(Cube cube)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FURNISHING_INVENTORY);
        pWriter.Write(FurnishingInventoryPacketMode.Load);
        pWriter.WriteInt(cube.Item.Id);
        pWriter.WriteLong(cube.Uid);
        pWriter.WriteLong(); // expire timestamp for ugc items
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter Remove(Cube cube)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FURNISHING_INVENTORY);
        pWriter.Write(FurnishingInventoryPacketMode.Remove);
        pWriter.WriteLong(cube.Uid);

        return pWriter;
    }

    public static PacketWriter EndList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FURNISHING_INVENTORY);
        pWriter.Write(FurnishingInventoryPacketMode.EndList);

        return pWriter;
    }
}
