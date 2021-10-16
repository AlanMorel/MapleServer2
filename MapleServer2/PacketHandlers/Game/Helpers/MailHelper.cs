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

        public static void SendBlackMarketMail(MailType type, BlackMarketListing listing, long recipientCharacterId)
        {
            string senderName = "<ms2><v key=\"s_blackmarket_mail_to_sender\" /></ms2>";
            string title = "<ms2><v key=\"s_blackmarket_mail_to_cancel_title\" /></ms2>";
            string body = "<ms2><v key=\"s_blackmarket_mail_to_cancel_content\" /></ms2>";
            string addParameter1 = $"<ms2><v item=\"{listing.Item.Id}\" ></v></ms2>";
            string addParameter2 = $"<ms2>" +
                $"<v key=\"s_blackmarket_mail_to_cancel_direct\" ></v>" +
                $"<v item=\"{listing.Item.Id}\" ></v>" +
                $"<v str=\"{listing.ListedQuantity}\" ></v>" +
                $"<v money=\"{listing.Price * listing.ListedQuantity}\" ></v>" +
                $"<v money=\"{listing.Price}\" ></v>" +
                $"<v money=\"0\" ></v>" + // deposit refund. always 0 if cancelled
                $"</ms2>";

            Mail mail = new Mail(type, listing.OwnerCharacterId, 0, senderName, title, body, addParameter1, addParameter2, new List<Item>() { listing.Item }, 0);
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
