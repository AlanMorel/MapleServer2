using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using MapleServer2.Servers.Game;
using MapleServer2.Data;
using MapleServer2.Packets.Helpers;
using MapleServer2.Tools;

namespace MapleServer2.Packets
{
    public static class MailPacket
    {
        public static Packet Notify(GameSession session)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(14); // Mode for mail notification
            pWriter.WriteInt(session.Mailbox.GetUnread()); // Count of unread mail
            pWriter.WriteByte(); // Unknown
            pWriter.WriteInt(); // Unknown maybe repeat of count?

            return pWriter;
        }

        public static void Open(GameSession session)
        {
            // Start of mail packets
            session.Send(PacketWriter.Of(SendOp.MAIL).WriteByte(16));

            // Mail packets
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(0); // Mode
            pWriter.WriteInt(session.Mailbox.Box.Count); // Amount of mail encoded in this packet

            for (int i = 0; i < session.Mailbox.Box.Count; i++)
            {
                if (session.Mailbox.Box[i].Type == 1) // Regular mail
                {
                    pWriter = MailPacketHelper.WriteRegular(pWriter, session.Mailbox.Box[i]);
                }
                else if (session.Mailbox.Box[i].Type == 101) // System mail
                {
                    pWriter = MailPacketHelper.WriteSystem(pWriter, session.Mailbox.Box[i]);
                }
            }

            session.Send(pWriter);

            // End of mail packets
            session.Send(PacketWriter.Of(SendOp.MAIL).WriteByte(17));
        }

        public static Packet Send(GameSession session, string recipient, string title, string body)
        {
            // Would make database call to look for recipient and add mail to their mailbox, instead add mail to session
            Mail mail = new Mail
            (
                1,
                GuidGenerator.Int(),
                session.Player.CharacterId,
                session.Player.Name,
                title,
                body,
                0,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                null
            );
            session.Mailbox.AddOrUpdate(mail);

            // Send packet
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(1); // Mode for send
            pWriter.WriteInt(mail.Uid); // Mail uid
            pWriter.WriteInt(0);

            return pWriter;
        }

        public static Packet Read(GameSession session, int id)
        {
            long timestamp = session.Mailbox.Read(id);

            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(2); // Mode for read
            pWriter.WriteInt(id); // Mail uid
            pWriter.WriteInt(0);
            pWriter.WriteLong(timestamp + AccountStorage.TickCount); // Read timestamp

            return pWriter;
        }

        public static void Collect(GameSession session, int id)
        {
            // Get items and add to inventory
            List<Item> items = session.Mailbox.Collect(id);

            if (items == null)
            {
                return;
            }

            foreach (Item item in items)
            {
                session.Inventory.Remove(item.Uid, out Item removed);
                session.Inventory.Add(item);

                // Item packet, not sure if this is only used for mail, it also doesn't seem to do anything
                session.Send(ItemPacket.ItemData(item));
                // Inventory packets
                session.Send(ItemInventoryPacket.Add(item));
                session.Send(ItemInventoryPacket.MarkItemNew(item));
            }

            // Not sure what the purpose of this packet is, perhaps if collect fails?
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(10); // Mode for collect success/failure? collect mode = 11
            pWriter.WriteInt(id); // Mail uid
            pWriter.WriteInt(0);
            pWriter.WriteShort(1); // Successfully collected? 01 00
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + AccountStorage.TickCount); // Collect timestamp

            session.Send(pWriter);

            // Collect response packet
            pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(11); // Mode for collect
            pWriter.WriteInt(id); // Mail uid
            pWriter.WriteInt(0);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + AccountStorage.TickCount); // Collect timestamp

            session.Send(pWriter);
        }
    }
}