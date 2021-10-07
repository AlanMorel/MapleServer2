using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Classes;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using static MapleServer2.Types.Mail;

namespace MapleServer2.PacketHandlers.Game
{
    public class MailHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.MAIL;

        public MailHandler() : base() { }

        private enum MailMode : byte
        {
            Open = 0x0,
            Send = 0x1,
            Read = 0x2,
            Collect = 0xB,
            Delete = 0xD,
            ReadBatch = 0x12, //batch?
            CollectBatch = 0x13,
        }

        private enum MailErrorCode : byte
        {
            CharacterNotFound = 0x01,
            ItemAmountMismatch = 0x02,
            ItemCannotBeSent = 0x03,
            MailNotSent = 0x0C,
            MailAlreadyRead = 0x10,
            ItemAlreadyRetrieved = 0x11,
            FullInventory = 0x14,
            MailItemExpired = 0x15,
            SaleEnded = 0x17,
            MailCreationFailed = 0x18,
            CannotMailYourself = 0x19,
            NotEnouhgMeso = 0x1A,
            PlayerIsBlocked = 0x1B,
            PlayerBlockedYou = 0x1C,
            GMCannotSendMail = 0x1D,
            ContainsForbiddenWord = 0x1F,
            MailPrivilegeSuspended = 0x20
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            MailMode mode = (MailMode) packet.ReadByte();

            switch (mode)
            {
                case MailMode.Open:
                    HandleOpen(session);
                    break;
                case MailMode.Send:
                    HandleSend(session, packet);
                    break;
                case MailMode.Read:
                    HandleRead(session, packet);
                    break;
                case MailMode.Collect:
                    HandleCollect(session, packet);
                    break;
                case MailMode.Delete:
                    HandleDelete(session, packet);
                    break;
                case MailMode.ReadBatch:
                    HandleReadBatch(session, packet);
                    break;
                case MailMode.CollectBatch:
                    HandleCollectBatch(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleOpen(GameSession session)
        {
            //session.Player.Mailbox.ClearExpired();

            session.Send(MailPacket.StartOpen());
            //session.Send(MailPacket.Test());
            session.Send(MailPacket.Open(session.Player.Mails));
            session.Send(MailPacket.EndOpen());
        }

        private static void HandleSend(GameSession session, PacketReader packet)
        {
            string recipientName = packet.ReadUnicodeString();
            string title = packet.ReadUnicodeString();
            string body = packet.ReadUnicodeString();

            long recipientCharacterId = DatabaseManager.Characters.FindPartialPlayerByName(recipientName).CharacterId;
            if (recipientCharacterId == -1)
            {
                session.Send(MailPacket.Error((byte) MailErrorCode.CharacterNotFound));
                return;
            }

            Mail mail = new Mail
            (
                MailType.Player,
                recipientCharacterId,
                session.Player.CharacterId,
                session.Player.Name,
                title,
                body,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            );

            session.Send(MailPacket.Send(mail));

            // Check if player is online
            Player recipient = GameServer.Storage.GetPlayerByName(recipientName);
            if (recipient == null)
            {
                return;
            }

            recipient.Mails.Add(mail);
            GameServer.MailManager.AddMail(mail);
            recipient.GetUnreadMailCount();
        }

        private static void HandleRead(GameSession session, PacketReader packet)
        {
            long id = packet.ReadLong();

            Mail mail = session.Player.Mails.FirstOrDefault(x => x.Id == id);
            if (mail == null)
            {
                return;
            }

            if (mail.ReadTimestamp == 0)
            {
                mail.Read(session);
            }
        }

        private static void HandleCollect(GameSession session, PacketReader packet)
        {
            long id = packet.ReadLong();

            // Get items and add to inventory
            //List<Item> items = session.Player.Mailbox.Collect(id);

            //if (items == null)
            //{
            //    return;
            //}

            //foreach (Item item in items)
            //{
            //    session.Player.Inventory.Remove(item.Uid, out Item removed);
            //    InventoryController.Add(session, item, true);

            //    // Item packet, not sure if this is only used for mail, it also doesn't seem to do anything
            //    session.Send(ItemPacket.ItemData(item));
            //}

            //session.Send(MailPacket.CollectedAmount(id, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
            //session.Send(MailPacket.CollectResponse(id, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        }

        private static void HandleDelete(GameSession session, PacketReader packet)
        {
            int count = packet.ReadInt();
            for (int i = 0; i < count; i++)
            {
                long mailId = packet.ReadLong();
                Mail mail = session.Player.Mails.FirstOrDefault(x => x.Id == mailId);
                if (mail == null)
                {
                    continue;
                }

                mail.Delete(session);
            }
        }

        private static void HandleReadBatch(GameSession session, PacketReader packet)
        {
            int count = packet.ReadInt();

            for (int i = 0; i < count; i++)
            {
                HandleRead(session, packet);
            }
        }

        private static void HandleCollectBatch(GameSession session, PacketReader packet)
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
