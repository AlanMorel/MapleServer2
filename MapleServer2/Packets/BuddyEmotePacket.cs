using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class BuddyEmotePacket
    {
        private enum BuddyEmotePacketMode : byte
        {
            SendRequest = 0x0,
            ConfirmSendRequest = 0x1,
            LearnEmote = 0x2,
            AcceptEmote = 0x3,
            DeclineEmote = 0x4,
            StartEmote = 0x5,
            StopEmote = 0x6,
        }

        public static Packet SendRequest(int buddyEmoteId, Player sender)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY_EMOTE);
            pWriter.WriteEnum(BuddyEmotePacketMode.SendRequest);
            pWriter.WriteInt(buddyEmoteId);
            pWriter.WriteLong(sender.CharacterId);
            pWriter.WriteUnicodeString(sender.Name);
            return pWriter;
        }

        public static Packet ConfirmSendRequest(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY_EMOTE);
            pWriter.WriteEnum(BuddyEmotePacketMode.ConfirmSendRequest);
            pWriter.WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet LearnEmote()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY_EMOTE);
            pWriter.WriteEnum(BuddyEmotePacketMode.LearnEmote);
            pWriter.WriteInt(); // emoteID
            pWriter.WriteInt(1); // quantity
            pWriter.WriteLong();
            return pWriter;
        }

        public static Packet SendAccept(int buddyEmoteId, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY_EMOTE);
            pWriter.WriteEnum(BuddyEmotePacketMode.AcceptEmote);
            pWriter.WriteInt(buddyEmoteId);
            pWriter.WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet DeclineEmote(int buddyEmoteId, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY_EMOTE);
            pWriter.WriteEnum(BuddyEmotePacketMode.DeclineEmote);
            pWriter.WriteInt(buddyEmoteId);
            pWriter.WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet StartEmote(int buddyEmoteId, Player player1, Player player2, CoordF coords, int rotation)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY_EMOTE);
            pWriter.WriteEnum(BuddyEmotePacketMode.StartEmote);
            pWriter.WriteInt(buddyEmoteId);
            pWriter.WriteLong(player1.CharacterId);
            pWriter.WriteLong(player2.CharacterId);
            pWriter.Write(coords);
            pWriter.WriteLong();
            pWriter.Write(rotation);

            pWriter.WriteInt(0);
            return pWriter;
        }

        public static Packet StopEmote(int buddyEmoteId, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY_EMOTE);
            pWriter.WriteEnum(BuddyEmotePacketMode.StopEmote);
            pWriter.WriteInt(buddyEmoteId);
            pWriter.WriteLong(player.CharacterId);
            return pWriter;
        }
    }
}
