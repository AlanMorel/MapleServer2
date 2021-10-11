using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers
{
    public class MailHelper
    {
        public static void SendMail(MailType type, long recipientCharacterId, long senderCharacterId, string senderName, string title, string body, string addParameter1, string addParameter2, List<Item> items, long mesos, out Mail mail)
        {
            mail = new Mail(type, recipientCharacterId, senderCharacterId, senderName, title, body, addParameter1, addParameter2, items, mesos);
            GameServer.MailManager.AddMail(mail);

            // TODO: Handle Black Market mails

            SendNotification(mail);
        }

        private static void SendNotification(Mail mail)
        {
            Player recipient = GameServer.Storage.GetPlayerById(mail.RecipientCharacterId);
            if (recipient == null)
            {
                return;
            }

            recipient.Mailbox.Add(mail);
            recipient.GetUnreadMailCount();
        }
    }
}
