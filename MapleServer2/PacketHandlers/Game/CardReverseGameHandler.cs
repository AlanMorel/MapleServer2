using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class CardReverseGameHandler : GamePacketHandler<CardReverseGameHandler>
{
    public override RecvOp OpCode => RecvOp.CardReverseGame;

    private enum Mode : byte
    {
        Open = 0x0,
        Mix = 0x1,
        Select = 0x2
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.Open:
                HandleOpen(session);
                break;
            case Mode.Mix:
                HandleMix(session);
                break;
            case Mode.Select:
                HandleSelect(session);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpen(GameSession session)
    {
        List<CardReverseGame> cards = DatabaseManager.CardReverseGame.FindAll();
        session.Send(CardReverseGamePacket.Open(cards));
    }

    private static void HandleMix(GameSession session)
    {
        Item token = session.Player.Inventory.GetById(CardReverseGame.TOKEN_ITEM_ID);
        if (token == null || token.Amount < CardReverseGame.TOKEN_COST)
        {
            session.Send(CardReverseGamePacket.Notice());
            return;
        }
        session.Player.Inventory.ConsumeItem(session, token.Uid, CardReverseGame.TOKEN_COST);

        session.Send(CardReverseGamePacket.Mix());
    }

    private static void HandleSelect(GameSession session)
    {
        // Unknown how this game works as to whether it's weighted or not
        // Currently being handled by each item having an equal chance

        List<CardReverseGame> cards = DatabaseManager.CardReverseGame.FindAll();

        int index = Random.Shared.Next(cards.Count);

        CardReverseGame card = cards[index];
        Item item = new(card.ItemId, card.ItemAmount, card.ItemRarity);

        session.Send(CardReverseGamePacket.Select(index));
        session.Player.Inventory.AddItem(session, item, true);
    }
}
