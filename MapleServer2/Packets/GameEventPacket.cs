using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;

namespace MapleServer2.Packets;

public static class GameEventPacket
{
    public static PacketWriter Load(List<GameEvent> events)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_EVENT);
        pWriter.WriteByte();
        pWriter.WriteInt(events.Count);
        foreach (GameEvent gameEvent in events)
        {
            pWriter.WriteUnicodeString(gameEvent.Type.ToString());
            pWriter.WriteInt(gameEvent.Id);
            switch (gameEvent.Type)
            {
                case GameEventType.StringBoard:
                    foreach (StringBoardEvent stringBoard in gameEvent.StringBoard)
                    {
                        pWriter.WriteInt(stringBoard.StringId);
                        pWriter.WriteUnicodeString(stringBoard.String);
                    }
                    break;
                case GameEventType.BlueMarble:
                    pWriter.WriteInt(gameEvent.Mapleopoly.Count);
                    foreach (MapleopolyEvent mapleopolyEvent in gameEvent.Mapleopoly)
                    {
                        pWriter.WriteInt(mapleopolyEvent.TripAmount);
                        pWriter.WriteInt(mapleopolyEvent.ItemId);
                        pWriter.WriteByte(mapleopolyEvent.ItemRarity);
                        pWriter.WriteInt(mapleopolyEvent.ItemAmount);
                    }
                    break;
                case GameEventType.UGCMapContractSale:
                    pWriter.WriteInt(gameEvent.UGCMapContractSale.DiscountAmount);
                    break;
                case GameEventType.UGCMapExtensionSale:
                    pWriter.WriteInt(gameEvent.UGCMapExtensionSale.DiscountAmount);
                    break;
                case GameEventType.EventFieldPopup:
                    pWriter.WriteInt(gameEvent.FieldPopupEvent.MapId);
                    break;
            }
        }
        return pWriter;
    }
}
