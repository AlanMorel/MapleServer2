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
        pWriter.WriteInt(1);
        pWriter.WriteUnicodeString("AttendGift");
        pWriter.WriteInt(11);
        pWriter.WriteLong(1644447600);
        pWriter.WriteLong(1647471600);
        pWriter.WriteUnicodeString("Emulator Attendance");
        pWriter.WriteString("https://mapleme.me/");
        pWriter.WriteByte(); // enable bool?
        pWriter.WriteByte(1); // disable claim reward button
        pWriter.WriteInt(1800);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteByte(1); // token type
        bool t = true;
        if (t)
        {
            pWriter.WriteInt(20);
            pWriter.WriteLong(200);
            pWriter.WriteInt(5);
        }
        pWriter.WriteInt(1); // amount of attendance days
        pWriter.WriteInt(34000042); // item id
        pWriter.WriteShort(5); // rarity
        pWriter.WriteInt(4); // quantity
        pWriter.WriteByte(1);
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();

        //pWriter.WriteInt(events.Count);
        //foreach (GameEvent gameEvent in events)
        //{
        //    pWriter.WriteUnicodeString(gameEvent.Type.ToString());
        //    pWriter.WriteInt(gameEvent.Id);
        //    switch (gameEvent.Type)
        //    {
        //        case GameEventType.StringBoard:
        //            foreach (StringBoardEvent stringBoard in gameEvent.StringBoard)
        //            {
        //                pWriter.WriteInt(stringBoard.StringId);
        //                pWriter.WriteUnicodeString(stringBoard.String);
        //            }
        //            break;
        //        case GameEventType.BlueMarble:
        //            pWriter.WriteInt(gameEvent.Mapleopoly.Count);
        //            foreach (MapleopolyEvent mapleopolyEvent in gameEvent.Mapleopoly)
        //            {
        //                pWriter.WriteInt(mapleopolyEvent.TripAmount);
        //                pWriter.WriteInt(mapleopolyEvent.ItemId);
        //                pWriter.WriteByte(mapleopolyEvent.ItemRarity);
        //                pWriter.WriteInt(mapleopolyEvent.ItemAmount);
        //            }
        //            break;
        //        case GameEventType.UgcMapContractSale:
        //            pWriter.WriteInt(gameEvent.UgcMapContractSale.DiscountAmount);
        //            break;
        //        case GameEventType.UgcMapExtensionSale:
        //            pWriter.WriteInt(gameEvent.UgcMapExtensionSale.DiscountAmount);
        //            break;
        //        case GameEventType.EventFieldPopup:
        //            pWriter.WriteInt(gameEvent.FieldPopupEvent.MapId);
        //            break;
        //    }
        //}
        return pWriter;
    }
}
