using Maple2Storage.Enums;
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
        foreach (GameEvent e in events)
        {
            pWriter.WriteUnicodeString(e.GetType().Name);
            pWriter.WriteInt(e.Id);
            switch (e)
            {
                case AttendGift attend:
                    pWriter.WriteLong(attend.BeginTimestamp);
                    pWriter.WriteLong(attend.EndTimestamp);
                    pWriter.WriteUnicodeString(attend.Name);
                    pWriter.WriteString(attend.Url);
                    pWriter.WriteByte();
                    pWriter.WriteBool(attend.DisableClaimButton);
                    pWriter.WriteInt(attend.TimeRequired);
                    pWriter.WriteByte();
                    pWriter.WriteInt();
                    pWriter.Write(attend.SkipDayCurrencyType);
                    if (attend.SkipDayCurrencyType != GameEventCurrencyType.None)
                    {
                        pWriter.WriteInt(attend.SkipDaysAllowed);
                        pWriter.WriteLong(attend.SkipDayCost);
                        pWriter.WriteInt();
                    }
                    pWriter.WriteInt(attend.Days.Count);
                    foreach (AttendGiftDay day in attend.Days.OrderBy(x => x.Day))
                    {
                        pWriter.WriteInt(day.ItemId);
                        pWriter.WriteShort(day.ItemRarity);
                        pWriter.WriteInt(day.ItemAmount);
                        pWriter.WriteByte();
                        pWriter.WriteByte();
                        pWriter.WriteByte();
                        pWriter.WriteByte();
                    }
                    break;
                case StringBoard stringBoard:
                    pWriter.WriteInt(stringBoard.StringId);
                    pWriter.WriteUnicodeString(stringBoard.String);
                    break;
                case EventFieldPopup field:
                    pWriter.WriteInt(field.MapId);
                    break;
                case BlueMarble mapleopoly:
                    pWriter.WriteInt(mapleopoly.Rewards.Count);
                    foreach (BlueMarbleReward reward in mapleopoly.Rewards)
                    {
                        pWriter.WriteInt(reward.TripAmount);
                        pWriter.WriteInt(reward.ItemId);
                        pWriter.WriteByte(reward.ItemRarity);
                        pWriter.WriteInt(reward.ItemAmount);
                    }
                    break;
                case UgcMapContractSale contractSale:
                    pWriter.WriteInt(contractSale.DiscountAmount);
                    break;
                case UgcMapExtensionSale extensionSale:
                    pWriter.WriteInt(extensionSale.DiscountAmount);
                    break;
            }
        }
        return pWriter;
    }
}
