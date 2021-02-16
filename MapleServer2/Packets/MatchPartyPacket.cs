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
            PacketWriter pWriter = PacketWriter.Of(SendOp.MATCH_PARTY);
            pWriter.WriteEnum(MatchPartyPacketMode.Create);
            WritePartyInformation(pWriter, party, false);

            return pWriter;
        }

        public static Packet RemoveListing(Party party)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MATCH_PARTY);
            pWriter.WriteEnum(MatchPartyPacketMode.Remove);
            pWriter.WriteLong(party.PartyFinderId);

            return pWriter;
        }

        public static Packet SendListings(List<Party> parties)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MATCH_PARTY);
            pWriter.WriteEnum(MatchPartyPacketMode.Refresh);
            pWriter.WriteInt(parties.Count);
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

            pWriter.WriteLong(party.PartyFinderId);
            pWriter.WriteInt(party.Id);
            pWriter.WriteLong();
            pWriter.WriteUnicodeString(party.Name);
            pWriter.WriteBool(party.Approval);
            pWriter.WriteInt(party.Members.Count);
            pWriter.WriteInt(party.MaxMembers);
            pWriter.WriteLong(party.Leader.AccountId);
            pWriter.WriteLong(party.Leader.CharacterId);
            pWriter.WriteUnicodeString(party.Leader.Name);
            pWriter.WriteLong(party.CreationTimestamp);
        }
    }
}
