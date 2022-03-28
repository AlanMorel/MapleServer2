using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ItemExtractionPacket
{
    private enum ItemExtractionPacketMode : byte
    {
        Extract = 0x0,
        InsufficientAnvils = 0x2
    }

    public static PacketWriter Extract(Item resultItem)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemExtraction);
        pWriter.Write(ItemExtractionPacketMode.Extract);
        pWriter.WriteLong(resultItem.Uid);
        pWriter.WriteLong(resultItem.Uid);
        pWriter.WriteShort();
        pWriter.Write(resultItem.Color);
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter InsufficientAnvils()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemExtraction);
        pWriter.Write(ItemExtractionPacketMode.InsufficientAnvils);
        return pWriter;
    }
}
