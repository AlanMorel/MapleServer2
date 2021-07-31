using MapleServer2.Commands.Core;

namespace MapleServer2.Commands.Game
{
    public class CurrencyCommands : InGameCommand
    {
        public CurrencyCommands()
        {
            Aliases = new[]
            {
                "currency"
            };
            Description = "Set the amount of the currency type specified.";
            AddParameter<string>("name", "Type of the currency you want to add.");
            AddParameter<long>("amount", "Amount of the currency type.");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            string currencyName = trigger.Get<string>("name");
            long amount = trigger.Get<long>("amount");

            switch (currencyName)
            {
                case "valor":
                    trigger.Session.Player.Wallet.ValorToken.SetAmount(amount);
                    break;
                case "treva":
                    trigger.Session.Player.Wallet.Treva.SetAmount(amount);
                    break;
                case "rue":
                    trigger.Session.Player.Wallet.Rue.SetAmount(amount);
                    break;
                case "havi":
                    trigger.Session.Player.Wallet.HaviFruit.SetAmount(amount);
                    break;
                case "meso":
                    trigger.Session.Player.Wallet.Meso.SetAmount(amount);
                    break;
                case "meret":
                    trigger.Session.Player.Wallet.Meret.SetAmount(amount);
                    break;
                default:
                    trigger.Session.SendNotice("No currency type were found. Try again.");
                    break;
            }
        }
    }
}
