using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class MailPacket
    {
        public static Packet Notify(GameSession session)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(14); // Mode for mail notification
            pWriter.WriteInt(session.Player.Mailbox.GetUnreadCount()); // Count of unread mail
            pWriter.WriteByte(); // Unknown
            pWriter.WriteInt(); // Unknown maybe repeat of count?

            return pWriter;
        }

        public static Packet Open(List<Mail> box)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(0); // Mode
            pWriter.WriteInt(box.Count); // Amount of mail encoded in this packet

            for (int i = 0; i < box.Count; i++)
            {
                if (box[i].Type == 1) // Regular mail
                {
                    pWriter = MailPacketHelper.WriteRegular(pWriter, box[i]);
                }
                else if (box[i].Type == 101) // System mail
                {
                    pWriter = MailPacketHelper.WriteSystem(pWriter, box[i]);
                }
            }

            return pWriter;
        }

        public static Packet StartOpen()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(16);

            return pWriter;
        }

        public static Packet EndOpen()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(17);

            return pWriter;
        }

        public static Packet Send(Mail mail)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(1); // Mode for send
            pWriter.WriteInt(mail.Uid); // Mail uid
            pWriter.WriteInt(0);

            return pWriter;
        }

        public static Packet Read(int id, long timestamp)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(2); // Mode for read
            pWriter.WriteInt(id); // Mail uid
            pWriter.WriteInt(0);
            pWriter.WriteLong(timestamp + AccountStorage.TickCount); // Read timestamp

            return pWriter;
        }

        public static Packet CollectedAmount(int id, long timestamp)
        {
            // Not sure what the purpose of this packet is, perhaps if collect fails?
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(10); // Mode for collect success/failure? collect mode = 11
            pWriter.WriteInt(id); // Mail uid
            pWriter.WriteInt(0);
            pWriter.WriteShort(1); // Successfully collected? 01 00
            pWriter.WriteLong(timestamp + AccountStorage.TickCount); // Collect timestamp

            return pWriter;
        }

        public static Packet CollectResponse(int id, long timestamp)
        {
            // Collect response packet
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(11); // Mode for collect
            pWriter.WriteInt(id); // Mail uid
            pWriter.WriteInt(0);
            pWriter.WriteLong(timestamp + AccountStorage.TickCount); // Collect timestamp

            return pWriter;
        }
    }
}
