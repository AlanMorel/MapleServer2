using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class DungeonHelperPacket
{
    private enum DungeonHelperPacketMode : byte
    {
        BroadcastAssist = 0x0,
        DisplayVetAndRookie = 0x1
    }

    public static PacketWriter BroadcastAssist(Party party, int dungeonId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.DungeonHelper);
        pWriter.Write(DungeonHelperPacketMode.BroadcastAssist);
        pWriter.WriteInt(party.Id);
        pWriter.WriteUnicodeString();
        pWriter.WriteLong(); // unk
        pWriter.WriteLong(); // unk
        pWriter.WriteInt(dungeonId);
        pWriter.WriteByte((byte) party.Members.Count);
        return pWriter;
    }

    public static PacketWriter DisplayVetAndRookie(Party party)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.DungeonHelper);
        pWriter.Write(DungeonHelperPacketMode.DisplayVetAndRookie);
        pWriter.WriteByte(); // rookie count
        pWriter.WriteByte(); // veteran count
        pWriter.WriteInt(party.Id);
        pWriter.WriteUnicodeString(party.Leader.Name);
        pWriter.WriteLong(); // unk
        pWriter.WriteLong(); // unk
        pWriter.WriteInt(); // dungeonId
        pWriter.WriteByte((byte) party.Members.Count);
        return pWriter;
    }
}
