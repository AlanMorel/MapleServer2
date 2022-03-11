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
            if (e is not MeratMarketNotice)
            {
                pWriter.WriteInt(e.Id);
            }

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
                case StringBoardLink stringBoardLink:
                    pWriter.WriteUnicodeString(stringBoardLink.Link);
                    break;
                case MeratMarketNotice notice:
                    pWriter.WriteUnicodeString(notice.Message);
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
                case RPS rps:
                    pWriter.WriteUnicodeString("<ms2>" +
                                               "<rps_game>" +
                                               "<play><actions rock=\"rock_A\" paper=\"paper_A\" scissors=\"scissors_A\" />" +
                                               "<messages>" +
                                               "<message value=\"s_microgame_rps_game_message_0\" />" +
                                               "<message value=\"s_microgame_rps_game_message_1\" />" +
                                               "<message value=\"s_microgame_rps_game_message_2\" />" +
                                               "</messages>" +
                                               "</play>" +
                                               "<result>" +
                                               "<draw>" +
                                               "<actions>" +
                                               "<action value=\"troubled\" />" +
                                               "</actions>" +
                                               "<messages>" +
                                               "<message value=\"s_microgame_rps_game_message_draw_0\" />" +
                                               "<message value=\"s_microgame_rps_game_message_draw_1\" />" +
                                               "<message value=\"s_microgame_rps_game_message_draw_2\" />" +
                                               "</messages>" +
                                               "</draw>" +
                                               "<win>" +
                                               "<actions>" +
                                               "<action value=\"happy\" />" +
                                               "<action value=\"dance_L\" />" +
                                               "</actions>" +
                                               "<messages>" +
                                               "<message value=\"s_microgame_rps_game_message_win_0\" />" +
                                               "<message value=\"s_microgame_rps_game_message_win_1\" />" +
                                               "<message value=\"s_microgame_rps_game_message_win_2\" />" +
                                               "</messages>" +
                                               "</win>" +
                                               "<lose>" +
                                               "<actions>" +
                                               "<action value=\"fuss\" />" +
                                               "<action value=\"Point_A\" />" +
                                               "</actions>" +
                                               "<messages>" +
                                               "<message value=\"s_microgame_rps_game_message_lose_0\" />" +
                                               "<message value=\"s_microgame_rps_game_message_lose_1\" />" +
                                               "<message value=\"s_microgame_rps_game_message_lose_2\" />" +
                                               "</messages>" +
                                               "</lose>" +
                                               "</result>" +
                                               "</rps_game>" +
                                               "</ms2>");
                    pWriter.WriteInt(rps.Tiers.Count);
                    foreach (RPSTier tier in rps.Tiers)
                    {
                        pWriter.WriteInt(tier.PlayAmount);
                        pWriter.WriteInt(tier.Rewards.Count);
                        foreach (RPSReward reward in tier.Rewards)
                        {
                            pWriter.WriteInt(reward.ItemId);
                            pWriter.WriteShort(reward.ItemRarity);
                            pWriter.WriteInt(reward.ItemAmount);
                            pWriter.WriteByte();
                            pWriter.WriteByte();
                            pWriter.WriteByte();
                            pWriter.WriteByte();
                        }
                    }

                    pWriter.WriteInt(rps.VoucherId);
                    pWriter.WriteInt(rps.Id);
                    pWriter.WriteLong(rps.EndTimestamp);
                    break;
                case SaleChat saleChat:
                    pWriter.WriteInt(saleChat.WorldChatDiscountAmount);
                    pWriter.WriteInt(saleChat.ChannelChatDiscountAmount);
                    break;
            }
        }

        return pWriter;
    }
}
