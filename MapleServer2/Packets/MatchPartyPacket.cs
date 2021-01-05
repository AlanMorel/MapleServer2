using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class MatchPartyPacket
    {
        private enum MatchPartyPacketMode : byte
        {
            Create = 0x0,
            Remove = 0x1,
            Refresh = 0x2
        }

        public static Packet CreateListing(Party party)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MATCH_PARTY)
                .WriteByte((byte) MatchPartyPacketMode.Create);
            WritePartyInformation(pWriter, party, false);
            return pWriter;
        }

        public static Packet RemoveListing(Party party)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MATCH_PARTY)
                .WriteByte((byte) MatchPartyPacketMode.Remove)
                .WriteLong(party.PartyFinderId);
            return pWriter;
        }

        public static Packet SendListings(List<Party> parties)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MATCH_PARTY)
                .WriteByte((byte) MatchPartyPacketMode.Refresh)
                .WriteInt(parties.Count);
            foreach (Party party in parties)
            {
                WritePartyInformation(pWriter, party);
            }

            return pWriter;
        }

        public static void WritePartyInformation(PacketWriter pWriter, Party party, bool header = true)
        {
            if (header)
            {
                pWriter.WriteByte(1);
            }

            pWriter.WriteLong(party.PartyFinderId)
                .WriteInt(party.Id)
                .WriteLong()
                .WriteUnicodeString(party.Name)
                .WriteBool(party.Approval)
                .WriteInt(party.Members.Count)
                .WriteInt(party.MaxMembers)
                .WriteLong(party.Leader.AccountId)
                .WriteLong(party.Leader.CharacterId)
                .WriteUnicodeString(party.Leader.Name)
                .WriteLong(party.CreationTimestamp);
        }
    }
}
