using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class MailHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.MAIL;

        public MailHandler(ILogger<MailHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();

            switch (mode)
            {
                case 0: // Open
                    HandleOpen(session);
                    break;
                case 1: // Send
                    HandleSend(session, packet);
                    break;
                case 2: // Read
                    HandleRead(session, packet);
                    break;
                case 11: // Collect
                    HandleCollect(session, packet);
                    break;
                case 18: // Read batch
                    HandleReadBatch(session, packet);
                    break;
                case 19: // Collect batch
                    HandleCollectBatch(session, packet);
                    break;
            }
        }

        private void HandleOpen(GameSession session)
        {
            session.Player.Mailbox.ClearExpired();

            session.Send(MailPacket.StartOpen());
            session.Send(MailPacket.Open(session.Player.Mailbox.Box));
            session.Send(MailPacket.EndOpen());
        }

        private void HandleSend(GameSession session, PacketReader packet)
        {
            string recipient = packet.ReadUnicodeString();
            string title = packet.ReadUnicodeString();
            string body = packet.ReadUnicodeString();

            // Would make database call to look for recipient and add mail to their mailbox, instead add mail to session
            Mail mail = new Mail
            (
                1,
                session.Player.CharacterId,
                session.Player.Name,
                title,
                body,
                0,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                null
            );
            session.Player.Mailbox.AddOrUpdate(mail);

            session.Send(MailPacket.Send(mail));
        }

        private void HandleRead(GameSession session, PacketReader packet)
        {
            int id = packet.ReadInt();
            packet.ReadInt();

            long timestamp = session.Player.Mailbox.Read(id);

            session.Send(MailPacket.Read(id, timestamp));
        }

        private void HandleCollect(GameSession session, PacketReader packet)
        {
            int id = packet.ReadInt();
            packet.ReadInt();

            // Get items and add to inventory
            List<Item> items = session.Player.Mailbox.Collect(id);

            if (items == null)
            {
                return;
            }

            foreach (Item item in items)
            {
                session.Player.Inventory.Remove(item.Uid, out Item removed);
                session.Player.Inventory.Add(item);

                // Item packet, not sure if this is only used for mail, it also doesn't seem to do anything
                session.Send(ItemPacket.ItemData(item));
                // Inventory packets
                session.Send(ItemInventoryPacket.Add(item));
                session.Send(ItemInventoryPacket.MarkItemNew(item, item.Amount));
                // TODO remove these inventory packets
            }

            session.Send(MailPacket.CollectedAmount(id, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
            session.Send(MailPacket.CollectResponse(id, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        }

        private void HandleReadBatch(GameSession session, PacketReader packet)
        {
            int count = packet.ReadInt();

            for (int i = 0; i < count; i++)
            {
                HandleRead(session, packet);
            }
        }

        private void HandleCollectBatch(GameSession session, PacketReader packet)
        {
            int count = packet.ReadInt();

            for (int i = 0; i < count; i++)
            {
                HandleRead(session, packet);

                packet.Skip(-8); // Back track to reread id

                HandleCollect(session, packet);
            }
        }
    }
}
