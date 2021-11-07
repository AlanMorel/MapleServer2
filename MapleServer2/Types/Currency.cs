using Maple2Storage.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class Currency
{
    public GameSession Session { private get; set; }
    private readonly CurrencyType Type;
    public long Amount { get; private set; }

    public Currency() { }

    public Currency(CurrencyType type, long input)
    {
        Type = type;
        Amount = input;
    }

    public bool Modify(long input)
    {
        if (Amount + input < 0)
        {
            return false;
        }
        Amount += input;
        UpdateWallet();
        return true;
    }

    public void SetAmount(long input)
    {
        if (input < 0)
        {
            return;
        }
        Amount = input;
        UpdateWallet();
    }

    private void UpdateWallet()
    {
        switch (Type)
        {
            case CurrencyType.Meso:
                Session.Send(MesosPacket.UpdateMesos(Amount));
                break;
            case CurrencyType.Meret:
            case CurrencyType.GameMeret:
            case CurrencyType.EventMeret:
                Session.Send(MeretsPacket.UpdateMerets(Session.Player.Account));
                break;
            case CurrencyType.ValorToken:
            case CurrencyType.Treva:
            case CurrencyType.Rue:
            case CurrencyType.HaviFruit:
                Session.Send(WalletPacket.UpdateWallet(Type, Amount));
                break;
            case CurrencyType.BankMesos:
                Session.Send(StorageInventoryPacket.UpdateMesos(Amount));
                break;
            default:
                break;
        }
    }
}
