using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class AttendanceHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.ATTENDANCE;

    private enum AttendanceMode : byte
    {
        Claim = 0x0,
        BeginTimer = 0x1,
        EarlyParticipate = 0x4,
    }

    private enum AttendanceNotice : byte
    {
        NotEnoughMesos = 1,
        NotEnoughMerets = 2,
        NoVouchers = 3,
        EventHasAlreadyBeenCompleted = 5,
        EventNotFound = 6,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        AttendanceMode mode = (AttendanceMode) packet.ReadByte();

        switch (mode)
        {
            case AttendanceMode.Claim:
                HandleClaim(session);
                break;
            case AttendanceMode.BeginTimer:
                HandleBeginTimer(session);
                break;
            case AttendanceMode.EarlyParticipate:
                HandleEarlyParticipation(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleClaim(GameSession session)
    {
        AttendGift attendanceEvent = DatabaseManager.Events.FindAttendGiftEvent();
        if (attendanceEvent is null || attendanceEvent.EndTimestamp < TimeInfo.Now())
        {
            session.Send(AttendancePacket.Notice((int) AttendanceNotice.EventNotFound));
            return;
        }

        GameEventUserValue timeValue = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id,
            TimeInfo.Tomorrow(), GameEventUserValueType.AttendanceAccumulatedTime);
        long.TryParse(timeValue.EventValue, out long accumulatedTime);
        if (TimeInfo.Now() - session.Player.LastLogTime + accumulatedTime <
            attendanceEvent.TimeRequired)
        {
            return;
        }

        GameEventUserValue completeTimestampValue = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id,
            attendanceEvent.EndTimestamp, GameEventUserValueType.AttendanceCompletedTimestamp);

        long.TryParse(completeTimestampValue.EventValue, out long completedTimestamp);

        DateTimeOffset savedTime = DateTimeOffset.FromUnixTimeSeconds(completedTimestamp);
        if (DateTimeOffset.Now.UtcDateTime < savedTime.UtcDateTime && DateTimeOffset.Now.Date != savedTime.Date)
        {
            session.Send(AttendancePacket.Notice((int) AttendanceNotice.EventHasAlreadyBeenCompleted));
            return;
        }

        // get current day value
        UpdateRewardsClaimed(session, attendanceEvent);

        // update completed timestamp
        long convertedValue2 = TimeInfo.Now();
        completeTimestampValue.UpdateValue(session, convertedValue2);
    }

    private static void HandleBeginTimer(GameSession session)
    {
        AttendGift attendanceEvent = DatabaseManager.Events.FindAttendGiftEvent();
        if (attendanceEvent is null)
        {
            return;
        }

        GameEventUserValue accumulatedTime = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id,
            TimeInfo.Tomorrow(), GameEventUserValueType.AttendanceAccumulatedTime);
        if (accumulatedTime.ExpirationTimestamp < TimeInfo.Now())
        {
            accumulatedTime.ExpirationTimestamp = TimeInfo.Tomorrow();
            accumulatedTime.EventValue = "0";
            DatabaseManager.GameEventUserValue.Update(accumulatedTime);
        }
    }

    private static void HandleEarlyParticipation(GameSession session, PacketReader packet)
    {
        int eventId = packet.ReadInt();
        AttendGift attendanceEvent = DatabaseManager.Events.FindAttendGiftEvent();
        if (attendanceEvent is null || attendanceEvent.SkipDayCurrencyType == GameEventCurrencyType.None)
        {
            return;
        }

        GameEventUserValue skipDay = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id,
            attendanceEvent.EndTimestamp, GameEventUserValueType.AttendanceEarlyParticipationRemaining);

        int.TryParse(skipDay.EventValue, out int skipsTotal);

        if (skipsTotal >= attendanceEvent.SkipDaysAllowed)
        {
            return;
        }

        // Charge player
        CurrencyType currencyType = attendanceEvent.SkipDayCurrencyType switch
        {
            GameEventCurrencyType.Meso => CurrencyType.Meso,
            GameEventCurrencyType.Meret => CurrencyType.Meret,
            _ => CurrencyType.Meret
        };

        switch (currencyType)
        {
            case CurrencyType.Meso:
                if (!session.Player.Wallet.Meso.Modify(-attendanceEvent.SkipDayCost))
                {
                    return;
                }

                break;
            case CurrencyType.Meret:
                if (!session.Player.Account.Meret.Modify(-attendanceEvent.SkipDayCost))
                {
                    return;
                }

                break;
            default:
                return;
        }

        skipsTotal++;
        skipDay.UpdateValue(session, skipsTotal);

        UpdateRewardsClaimed(session, attendanceEvent);
    }

    private static void GiveAttendanceReward(GameSession session, AttendGift attendanceEvent, int day)
    {
        AttendGiftDay attendGift = attendanceEvent.Days.FirstOrDefault(x => x.Day == day);
        if (attendGift is null)
        {
            return;
        }

        Item item = new(attendGift.ItemId)
        {
            Rarity = attendGift.ItemRarity,
            Amount = attendGift.ItemAmount
        };

        MailHelper.SendAttendanceMail(item, session.Player.CharacterId);
    }

    private static void UpdateRewardsClaimed(GameSession session, AttendGift attendanceEvent)
    {
        GameEventUserValue rewardsClaimValue = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id,
            attendanceEvent.EndTimestamp, GameEventUserValueType.AttendanceRewardsClaimed);

        int.TryParse(rewardsClaimValue.EventValue, out int convertedValue);

        convertedValue++;
        GiveAttendanceReward(session, attendanceEvent, convertedValue);
        rewardsClaimValue.UpdateValue(session, convertedValue);
    }
}
