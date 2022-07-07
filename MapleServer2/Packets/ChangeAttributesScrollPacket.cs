using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public class ChangeAttributesScrollPacket
{
    private enum Mode : byte
    {
        Open = 0,
        Preview = 2,
        Add = 3
    }

    public static PacketWriter Open(long uid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ChangeAttributesScroll);
        pWriter.Write(Mode.Open);
        pWriter.WriteLong(uid);

        return pWriter;
    }

    public static PacketWriter PreviewNewItem(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ChangeAttributesScroll);
        pWriter.Write(Mode.Preview);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteItem(item);

        return pWriter;
    }

    public static PacketWriter AddNewItem(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ChangeAttributesScroll);
        pWriter.Write(Mode.Add);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteItem(item);

        return pWriter;
    }
}
