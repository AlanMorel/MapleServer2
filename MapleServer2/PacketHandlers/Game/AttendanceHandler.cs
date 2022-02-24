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

        GameEventUserValue timeValue = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id, TimeInfo.Tomorrow(), GameEventUserValueType.AccumulatedTime);

        if (TimeInfo.Now() - session.Player.LastLogTime + int.Parse(timeValue.EventValue) < attendanceEvent.TimeRequired)
        {
            return;
        }

        GameEventUserValue completeTimestampValue = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id, attendanceEvent.EndTimestamp, GameEventUserValueType.CompletedTimestamp);
        DateTimeOffset savedTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(completeTimestampValue.EventValue));
        if (DateTimeOffset.Now.Day < savedTime.Day)
        {
            session.Send(AttendancePacket.Notice((int) AttendanceNotice.EventHasAlreadyBeenCompleted));
            return;
        }

        // get current day value
        UpdateRewardsClaimed(session, attendanceEvent);

        // update completed timestamp
        long convertedValue2 = TimeInfo.Now();
        completeTimestampValue.EventValue = convertedValue2.ToString();
        DatabaseManager.GameEventUserValue.Update(completeTimestampValue);

        session.Send(GameEventUserValuePacket.UpdateValue(completeTimestampValue));
    }

    private static void HandleBeginTimer(GameSession session)
    {
        AttendGift attendanceEvent = DatabaseManager.Events.FindAttendGiftEvent();
        if (attendanceEvent is null)
        {
            return;
        }

        GameEventUserValue accumulatedTime = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id, TimeInfo.Tomorrow(), GameEventUserValueType.AccumulatedTime);
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

        GameEventUserValue skipDay = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id, attendanceEvent.EndTimestamp, GameEventUserValueType.EarlyParticipationRemaining);
        int skipsTotal = int.Parse(skipDay.EventValue);
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
                    session.Send(AttendancePacket.Notice((int) AttendanceNotice.NotEnoughMesos));
                    return;
                }
                break;
            case CurrencyType.Meret:
                if (!session.Player.Account.Meret.Modify(-attendanceEvent.SkipDayCost))
                {
                    session.Send(AttendancePacket.Notice((int) AttendanceNotice.NotEnoughMerets));
                    return;
                }
                break;
            default:
                return;
        }

        skipsTotal++;
        skipDay.EventValue = skipsTotal.ToString();
        DatabaseManager.GameEventUserValue.Update(skipDay);
        session.Send(GameEventUserValuePacket.UpdateValue(skipDay));

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
        GameEventUserValue rewardsClaimValue = GameEventHelper.GetUserValue(session.Player, attendanceEvent.Id, attendanceEvent.EndTimestamp, GameEventUserValueType.RewardsClaimed);
        int convertedValue = int.Parse(rewardsClaimValue.EventValue);
        convertedValue++;
        GiveAttendanceReward(session, attendanceEvent, convertedValue);
        rewardsClaimValue.EventValue = convertedValue.ToString();
        DatabaseManager.GameEventUserValue.Update(rewardsClaimValue);
        session.Send(GameEventUserValuePacket.UpdateValue(rewardsClaimValue));
    }
}
