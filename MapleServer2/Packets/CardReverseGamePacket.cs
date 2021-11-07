using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;

namespace MapleServer2.Packets;

public static class CardReverseGamePacket
{
    private enum CardReverseGamePacketMode : byte
    {
        Open = 0x0,
        Mix = 0x1,
        Select = 0x2,
        Notice = 0x3
    }

    public static PacketWriter Open(List<CardReverseGame> cards)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CARD_REVERSE_GAME);
        pWriter.Write(CardReverseGamePacketMode.Open);
        pWriter.WriteInt(CardReverseGame.TOKEN_ITEM_ID);
        pWriter.WriteInt(CardReverseGame.TOKEN_COST);
        pWriter.WriteInt(cards.Count);
        foreach (CardReverseGame card in cards)
        {
            pWriter.WriteInt(card.ItemId);
            pWriter.WriteByte(card.ItemRarity);
            pWriter.WriteInt(card.ItemAmount);
        }
        return pWriter;
    }

    public static PacketWriter Mix()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CARD_REVERSE_GAME);
        pWriter.Write(CardReverseGamePacketMode.Mix);
        return pWriter;
    }

    public static PacketWriter Select(int index)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CARD_REVERSE_GAME);
        pWriter.Write(CardReverseGamePacketMode.Select);
        pWriter.WriteInt(index);
        return pWriter;
    }

    public static PacketWriter Notice()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CARD_REVERSE_GAME);
        pWriter.Write(CardReverseGamePacketMode.Notice);
        pWriter.WriteByte(0x0);
        return pWriter;
    }
}
