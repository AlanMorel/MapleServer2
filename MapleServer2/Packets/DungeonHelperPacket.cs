using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class DungeonHelperPacket
{
    private enum Mode : byte
    {
        BroadcastAssist = 0x0,
        DisplayVetAndRookie = 0x1
    }

    public static PacketWriter BroadcastAssist(Party party, int dungeonId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.DungeonHelper);
        pWriter.Write(Mode.BroadcastAssist);
        WriteDungeonHelperParty(pWriter, party, dungeonId);
        return pWriter;
    }

    public static PacketWriter DisplayVetAndRookie(Party party, int dungeonId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.DungeonHelper);
        pWriter.Write(Mode.DisplayVetAndRookie);
        pWriter.WriteByte(); // rookie count
        pWriter.WriteByte(); // veteran count
        WriteDungeonHelperParty(pWriter, party, dungeonId);
        return pWriter;
    }

    private static void WriteDungeonHelperParty(PacketWriter pWriter, Party party, int dungeonId)
    {
        pWriter.WriteInt(party.Id);
        pWriter.WriteUnicodeString(party.Leader.Name);
        pWriter.WriteLong(party.Leader.AccountId);
        pWriter.WriteLong(party.Leader.CharacterId);
        pWriter.WriteInt(dungeonId);
        pWriter.WriteByte((byte) party.Members.Count);
    }
}
