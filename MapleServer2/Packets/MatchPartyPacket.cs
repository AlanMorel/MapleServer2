using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MatchPartyPacket
{
    private enum Mode : byte
    {
        Create = 0x0,
        Remove = 0x1,
        Refresh = 0x2
    }

    public static PacketWriter CreateListing(Party party)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MatchParty);
        pWriter.Write(Mode.Create);
        WritePartyInformation(pWriter, party, false);

        return pWriter;
    }

    public static PacketWriter RemoveListing(Party party)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MatchParty);
        pWriter.Write(Mode.Remove);
        pWriter.WriteLong(party.PartyFinderId);

        return pWriter;
    }

    public static PacketWriter SendListings(List<Party> parties)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MatchParty);
        pWriter.Write(Mode.Refresh);
        pWriter.WriteInt(parties.Count);
        foreach (Party party in parties)
        {
            WritePartyInformation(pWriter, party);
        }

        return pWriter;
    }

    private static void WritePartyInformation(PacketWriter pWriter, Party party, bool header = true)
    {
        if (header)
        {
            pWriter.WriteByte(1);
        }

        pWriter.WriteLong(party.PartyFinderId);
        pWriter.WriteInt(party.Id);
        pWriter.WriteLong();
        pWriter.WriteUnicodeString(party.Name);
        pWriter.WriteBool(party.Approval);
        pWriter.WriteInt(party.Members.Count);
        pWriter.WriteInt(party.RecruitMemberCount);
        pWriter.WriteLong(party.Leader.AccountId);
        pWriter.WriteLong(party.Leader.CharacterId);
        pWriter.WriteUnicodeString(party.Leader.Name);
        pWriter.WriteLong(party.CreationTimestamp);
    }
}
