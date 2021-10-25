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

        public static void BlackMarketCancellation(BlackMarketListing listing)
        {
            long deposit = 0;
            if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > listing.ExpiryTimestamp)
            {
                deposit = listing.Deposit;
            }

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
                $"<v money=\"{deposit}\" ></v>" +
                $"</ms2>";

            Mail mail = new Mail(MailType.BlackMarketListingCancel, listing.OwnerCharacterId, 0, senderName, title, body, addParameter1, addParameter2, new List<Item>() { listing.Item }, deposit);
            GameServer.MailManager.AddMail(mail);
            SendNotification(mail);
        }

        public static void BlackMarketTransaction(Item item, BlackMarketListing listing, long recipientCharacterId, long price, bool removeListing)
        {
            // Create mail for purchaser
            SendBlackMarketPurchaseMail(item, recipientCharacterId, price);

            //Create mail for seller
            SendBlackMarketSoldMail(listing, item, price, removeListing);
        }

        private static void SendBlackMarketPurchaseMail(Item item, long recipientCharacterId, long price)
        {
            string senderName = "<ms2><v key=\"s_blackmarket_mail_to_sender\" /></ms2>";
            string title = "<ms2><v key=\"s_blackmarket_mail_to_buyer_title\" /></ms2>";
            string body = "<ms2><v key=\"s_blackmarket_mail_to_buyer_content\" /></ms2>";
            string addParameter1 = $"<ms2><v item=\"{item.Id}\"></v></ms2>";
            string addParameter2 = $"<ms2><v item=\"{item.Id}\" ></v><v str=\"{item.Amount}\" ></v><v money=\"{price * item.Amount}\" ></v><v money=\"{price}\" ></v></ms2>";

            Mail mail = new Mail(MailType.BlackMarketSale, recipientCharacterId, 0, senderName, title, body, addParameter1, addParameter2, new List<Item>() { item }, 0);
            GameServer.MailManager.AddMail(mail);
            SendNotification(mail);
        }

        private static void SendBlackMarketSoldMail(BlackMarketListing listing, Item item, long price, bool removeListing)
        {
            double salesFeeRate = 0.1; // TODO: Use from constant.xml (if it exists?)
            long tax = (long) (salesFeeRate * (item.Amount * price));
            long revenue = (item.Amount * price) - tax;

            string senderName = "<ms2><v key=\"s_blackmarket_mail_to_sender\" /></ms2>";
            string title = "<ms2><v key=\"s_blackmarket_mail_to_seller_title\" /></ms2>";
            string body = "<ms2><v key=\"s_blackmarket_mail_to_seller_content\" /></ms2>";
            string addParameter1 = $"<ms2><v item=\"{item.Id}\" ></v></ms2>";
            string addParameter2 = $"<ms2><v item=\"{item.Id}\" ></v><v str=\"{item.Amount}\" ></v><v money=\"{price * item.Amount}\" ></v><v money=\"{price}\" ></v><v money=\"{tax}\" ></v><v str=\"{salesFeeRate * 100}%\" ></v><v money=\"{revenue}\" ></v></ms2>";

            if (removeListing)
            {
                revenue += listing.Deposit;
                body = "<ms2><v key=\"s_blackmarket_mail_to_seller_content_soldout\" /></ms2>";
                addParameter2 = $"<ms2><v item=\"{item.Id}\" ></v><v str=\"{item.Amount}\" ></v><v money=\"{price * item.Amount}\" ></v><v money=\"{price}\" ></v><v money=\"{tax}\" ></v><v str=\"{salesFeeRate * 100}%\" ></v><v money=\"{listing.Deposit}\" ></v><v money=\"{revenue}\" ></v></ms2>";
            }

            Mail mail = new Mail(MailType.BlackMarketSale, listing.OwnerCharacterId, 0, senderName, title, body, addParameter1, addParameter2, new List<Item>() { }, revenue);
            GameServer.MailManager.AddMail(mail);
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
