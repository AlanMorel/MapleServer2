using System.Drawing;
using MapleServer2.Commands.Core;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Commands.Game;

public class CurrencyCommands : InGameCommand
{
    public CurrencyCommands()
    {
        Aliases = new()
        {
            "currency"
        };
        Description = "Set the amount of the currency type specified.";
        Parameters = new()
        {
            new Parameter<string>("name", "Type of the currency you want to add."),
            new Parameter<long>("amount", "Amount of the currency type.")
        };
        Usage = "/currency [name] [amount]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string currencyName = trigger.Get<string>("name");
        long amount = trigger.Get<long>("amount");
        GameSession session = trigger.Session;

        if (string.IsNullOrEmpty(currencyName))
        {
            session.SendNotice("No currency type were found. Try one of the list.");
            session.Send(NoticePacket.Notice("valor, treva, rue, havi, meso, meret".Color(Color.DarkOrange), NoticeType.Chat));
            return;
        }
        if (amount <= 0)
        {
            session.SendNotice("Amount must be more than 0 to add.");
            return;
        }

        switch (currencyName)
        {
            case "valor":
                session.Player.Wallet.ValorToken.SetAmount(amount);
                break;
            case "treva":
                session.Player.Wallet.Treva.SetAmount(amount);
                break;
            case "rue":
                session.Player.Wallet.Rue.SetAmount(amount);
                break;
            case "havi":
                session.Player.Wallet.HaviFruit.SetAmount(amount);
                break;
            case "meso":
                session.Player.Wallet.Meso.SetAmount(amount);
                break;
            case "meret":
                session.Player.Account.Meret.SetAmount(amount);
                break;
        }
    }
}
