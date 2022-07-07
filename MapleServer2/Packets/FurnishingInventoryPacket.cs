using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FurnishingInventoryPacket
{
    private enum Mode : byte
    {
        StartList = 0x0,
        Load = 0x1,
        Remove = 0x2,
        EndList = 0x4
    }

    public static PacketWriter StartList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FurnishingInventory);
        pWriter.Write(Mode.StartList);

        return pWriter;
    }

    public static PacketWriter Load(Cube cube)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FurnishingInventory);
        pWriter.Write(Mode.Load);
        pWriter.WriteInt(cube.Item.Id);
        pWriter.WriteLong(cube.Uid);
        pWriter.WriteLong(); // expire timestamp for ugc items
        pWriter.WriteBool(cube.Item.Ugc is not null);
        if (cube.Item.Ugc is not null)
        {
            pWriter.WriteClass(cube.Item.Ugc);
        }

        return pWriter;
    }

    public static PacketWriter Remove(Cube cube)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FurnishingInventory);
        pWriter.Write(Mode.Remove);
        pWriter.WriteLong(cube.Uid);

        return pWriter;
    }

    public static PacketWriter EndList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FurnishingInventory);
        pWriter.Write(Mode.EndList);

        return pWriter;
    }
}
