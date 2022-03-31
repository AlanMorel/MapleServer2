using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public class MailHelper
{
    public static void SendMail(MailType type, long recipientCharacterId, long senderCharacterId, string senderName, string title, string body, string addParameter1, string addParameter2, List<Item> items, long mesos, long merets, out Mail mail)
    {
        mail = new(type, recipientCharacterId, senderCharacterId, senderName, title, body, addParameter1, addParameter2, items, mesos, merets);
        GameServer.MailManager.AddMail(mail);

        SendNotification(mail);
    }

    public static void BlackMarketCancellation(BlackMarketListing listing)
    {
        long deposit = 0;
        if (TimeInfo.Now() > listing.ExpiryTimestamp)
        {
            deposit = listing.Deposit;
        }

        string senderName = "<ms2><v key=\"s_blackmarket_mail_to_sender\" /></ms2>";
        string title = "<ms2><v key=\"s_blackmarket_mail_to_cancel_title\" /></ms2>";
        string body = "<ms2><v key=\"s_blackmarket_mail_to_cancel_content\" /></ms2>";
        string addParameter1 = $"<ms2><v item=\"{listing.Item.Id}\" ></v></ms2>";
        string addParameter2 = "<ms2>" +
                               "<v key=\"s_blackmarket_mail_to_cancel_direct\" ></v>" +
                               $"<v item=\"{listing.Item.Id}\" ></v>" +
                               $"<v str=\"{listing.ListedQuantity}\" ></v>" +
                               $"<v money=\"{listing.Price * listing.ListedQuantity}\" ></v>" +
                               $"<v money=\"{listing.Price}\" ></v>" +
                               $"<v money=\"{deposit}\" ></v>" +
                               "</ms2>";

        Mail mail = new(MailType.BlackMarketListingCancel, listing.OwnerCharacterId, 0, senderName, title, body, addParameter1, addParameter2, new()
        {
            listing.Item
        }, deposit, 0);
        GameServer.MailManager.AddMail(mail);
        SendNotification(mail);
    }

    public static void BlackMarketTransaction(Item item, BlackMarketListing listing, long recipientCharacterId, long price, bool removeListing)
    {
        // Create mail for purchaser
        SendBlackMarketPurchaseMail(item, recipientCharacterId, price);

        // Create mail for seller
        SendBlackMarketSoldMail(listing, item, price, removeListing);
    }

    private static void SendBlackMarketPurchaseMail(Item item, long recipientCharacterId, long price)
    {
        string senderName = "<ms2><v key=\"s_blackmarket_mail_to_sender\" /></ms2>";
        string title = "<ms2><v key=\"s_blackmarket_mail_to_buyer_title\" /></ms2>";
        string body = "<ms2><v key=\"s_blackmarket_mail_to_buyer_content\" /></ms2>";
        string addParameter1 = $"<ms2><v item=\"{item.Id}\"></v></ms2>";
        string addParameter2 = $"<ms2><v item=\"{item.Id}\" ></v><v str=\"{item.Amount}\" ></v><v money=\"{price * item.Amount}\" ></v><v money=\"{price}\" ></v></ms2>";

        Mail mail = new(MailType.BlackMarketSale, recipientCharacterId, 0, senderName, title, body, addParameter1, addParameter2, new()
        {
            item
        }, 0, 0);
        GameServer.MailManager.AddMail(mail);
        SendNotification(mail);
    }

    private static void SendBlackMarketSoldMail(BlackMarketListing listing, Item item, long price, bool removeListing)
    {
        ScriptLoader scriptLoader = new("Functions/calcBlackMarketCostRate");
        DynValue scriptResults = scriptLoader.Call("calcBlackMarketCostRate");
        float salesFeeRate = (float) scriptResults.Number;
        long tax = (long) (salesFeeRate * (item.Amount * price));
        long revenue = item.Amount * price - tax;

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

        Mail mail = new(MailType.BlackMarketSale, listing.OwnerCharacterId, 0, senderName, title, body, addParameter1, addParameter2, new(), revenue, 0);
        GameServer.MailManager.AddMail(mail);
        SendNotification(mail);
    }

    public static void MesoMarketTransaction(MesoMarketListing listing, long recipientCharacterId)
    {
        // Create mail for purchaser
        SendMesoMarketPurchaseMail(listing, recipientCharacterId);

        // Create mail for seller
        SendMesoMarketSoldMail(listing);
    }

    private static void SendMesoMarketPurchaseMail(MesoMarketListing listing, long recipientCharacterId)
    {
        string senderName = "<ms2><v key=\"s_mesoMarket_mail_to_sender\" /></ms2>";
        string title = "<ms2><v key=\"s_mesoMarket_mail_to_buyer_title\" /></ms2>";
        string body = "<ms2><v key=\"s_mesoMarket_mail_to_buyer_content\" /></ms2>";
        string addParameter2 = $"<ms2><v money=\"{listing.Mesos}\" ></v><v money=\"{listing.Price}\" ></v></ms2>";

        Mail mail = new(MailType.MesoMarket, recipientCharacterId, 0, senderName, title, body, "", addParameter2, new(), listing.Mesos, 0);
        GameServer.MailManager.AddMail(mail);
        SendNotification(mail);
    }

    private static void SendMesoMarketSoldMail(MesoMarketListing listing)
    {
        string senderName = "<ms2><v key=\"s_mesoMarket_mail_to_sender\" /></ms2>";
        string title = "<ms2><v key=\"s_mesoMarket_mail_to_seller_title\" /></ms2>";
        string body = "<ms2><v key=\"s_mesoMarket_mail_to_seller_content\" /></ms2>";
        string addParameter2 = $"<ms2><v money=\"{listing.Mesos}\" ></v><v money=\"{listing.Price}\" ></v><v money=\"0\" ></v><v money=\"0\" ></v><v money=\"{listing.Price}\" ></v></ms2>";

        Mail mail = new(MailType.MesoMarket, listing.OwnerCharacterId, 0, senderName, title, body, "", addParameter2, new(), 0, listing.Price);
        GameServer.MailManager.AddMail(mail);
        SendNotification(mail);
    }

    public static void SendMesoMarketCancellation(MesoMarketListing listing, long recipientCharacterId)
    {
        string senderName = "<ms2><v key=\"s_mesoMarket_mail_to_sender\" /></ms2>";
        string title = "<ms2><v key=\"s_mesoMarket_mail_to_cancel_title\" /></ms2>";
        string body = "<ms2><v key=\"s_mesoMarket_mail_to_cancel_content\" /></ms2>";
        string addParameter2 = $"<ms2><v money=\"{listing.Mesos}\" ></v></ms2>";

        Mail mail = new(MailType.MesoMarket, recipientCharacterId, 0, senderName, title, body, "", addParameter2, new(), listing.Mesos, 0);
        GameServer.MailManager.AddMail(mail);
        SendNotification(mail);
    }

    public static void SendAttendanceMail(Item item, long recipientCharacterId)
    {
        // TODO: Change where this is to a more dynamic location
        string senderName = "MapleStory2";
        string title = "[Emulator Attendance] Attendance Reward";
        string body = "Thanks for testing out the emulator. Here is a token of appreciation. " +
                      "P.S. did you know you can use /commands to spawn in items?";

        Mail mail = new(MailType.System, recipientCharacterId, 0, senderName, title, body, "", "", new()
        {
            item
        }, 0, 0);
        GameServer.MailManager.AddMail(mail);
        SendNotification(mail, true);
    }

    private static void SendNotification(Mail mail, bool sendExpiryNotification = false)
    {
        Player recipient = GameServer.PlayerManager.GetPlayerById(mail.RecipientCharacterId);
        if (recipient == null)
        {
            return;
        }

        recipient.Mailbox.Add(mail);
        recipient.GetUnreadMailCount(sendExpiryNotification);
    }
}
