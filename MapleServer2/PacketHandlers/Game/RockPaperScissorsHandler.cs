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

public class RockPaperScissorsHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.ROCK_PAPER_SCISSORS;

    private enum RpsMode : short
    {
        OpenMatch = 0x00,
        RequestMatch = 0x01,
        ConfirmMatch = 0x02,
        DenyMatch = 0x03,
        CancelRequestMatch = 0x05,
        SelectRpsChoice = 0x07,
        ClaimReward = 0x0A,
        ConfirmMatch2 = 0x0C,
    }

    private enum RpsError : byte
    {
        OtherPlayerCannotPlayRightNow = 0x0,
        CannotPlayInThisMap = 0x1,
        FailedToStart = 0x4,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        RpsMode mode = (RpsMode) packet.ReadShort();

        switch (mode)
        {
            case RpsMode.OpenMatch:
                HandleOpenMatch(session);
                break;
            case RpsMode.RequestMatch:
                HandleRequestMatch(session, packet);
                break;
            case RpsMode.ConfirmMatch:
                HandleConfirmMatch(session, packet);
                break;
            case RpsMode.DenyMatch:
                HandleDenyMatch(session, packet);
                break;
            case RpsMode.CancelRequestMatch:
                HandleCancelRequestMatch(session, packet);
                break;
            case RpsMode.SelectRpsChoice:
                HandleSelectRpsChoice(session, packet);
                break;
            case RpsMode.ClaimReward:
                HandleClaimReward(session, packet);
                break;
            case RpsMode.ConfirmMatch2:
                HandleConfirmMatch2(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpenMatch(GameSession session)
    {
        RPS rpsEvent = DatabaseManager.Events.FindRockPaperScissorsEvent();
        if (rpsEvent is null)
        {
            return;
        }

        if (!session.Player.Inventory.Items.Values.Any(x => x.Id == rpsEvent.VoucherId))
        {
            // TODO: Find correct packet to let player know they don't have a voucher
            session.Send(NoticePacket.Notice("You must have a Rock Papers Scissors Play Ticket",
                NoticeType.ChatAndFastText));
            return;
        }

        session.Send(RockPaperScissorsPacket.Open());
    }

    private static void HandleRequestMatch(GameSession session, PacketReader packet)
    {
        long characterId = packet.ReadLong();

        Player otherPlayer = session.FieldManager.State.Players.Values
            .FirstOrDefault(x => x.Value.CharacterId == characterId)?.Value;
        if (otherPlayer is null)
        {
            return;
        }

        RPS rpsEvent = DatabaseManager.Events.FindRockPaperScissorsEvent();
        if (rpsEvent is null)
        {
            return;
        }

        if (!session.Player.Inventory.Items.Values.Any(x => x.Id == rpsEvent.VoucherId))
        {
            // TODO: Find correct packet to let player know they don't have a voucher
            session.Send(NoticePacket.Notice("You must have a Rock Papers Scissors Play Ticket",
                NoticeType.ChatAndFastText));
            return;
        }

        session.Player.RPSOpponentId = otherPlayer.CharacterId;

        otherPlayer.Session.Send(RockPaperScissorsPacket.RequestMatch(session.Player.CharacterId));
    }

    private static void HandleConfirmMatch(GameSession session, PacketReader packet)
    {
        long characterId = packet.ReadLong();

        Player otherPlayer = session.FieldManager.State.Players
            .FirstOrDefault(x => x.Value.Value.CharacterId == characterId).Value?.Value;
        if (otherPlayer == null)
        {
            return;
        }

        session.Player.RPSOpponentId = otherPlayer.CharacterId;

        otherPlayer.Session.Send(RockPaperScissorsPacket.ConfirmMatch(session.Player.CharacterId));
        otherPlayer.Session.Send(RockPaperScissorsPacket.BeginMatch());
        session.Send(RockPaperScissorsPacket.BeginMatch());
    }

    private static void HandleDenyMatch(GameSession session, PacketReader packet)
    {
        long characterId = packet.ReadLong();
        Player otherPlayer = session.FieldManager.State.Players
            .FirstOrDefault(x => x.Value.Value.CharacterId == characterId).Value?.Value;
        if (otherPlayer == null)
        {
            return;
        }

        otherPlayer.Session.Send(RockPaperScissorsPacket.DenyMatch(session.Player.CharacterId));
    }

    private static void HandleCancelRequestMatch(GameSession session, PacketReader packet)
    {
        long characterId = packet.ReadLong();
        Player otherPlayer = session.FieldManager.State.Players
            .FirstOrDefault(x => x.Value.Value.CharacterId == characterId).Value?.Value;
        if (otherPlayer is null)
        {
            return;
        }

        otherPlayer.Session.Send(RockPaperScissorsPacket.CancelRequestMatch(characterId));
    }

    private static void HandleSelectRpsChoice(GameSession session, PacketReader packet)
    {
        session.Player.RPSSelection = (RpsChoice) packet.ReadInt();

        // delay for 1 sec for opponent to update their selection
        Task.Run(async () =>
        {
            await Task.Delay(1000);
        });

        // confirm if opponent is still in the map
        Player opponent = session.FieldManager.State.Players
            .FirstOrDefault(x => x.Value.Value.CharacterId == session.Player.RPSOpponentId).Value?.Value;
        if (opponent == null)
        {
            return;
        }

        // handle choices
        RpsResult[,] resultMatrix =
        {
            {
                RpsResult.Draw, RpsResult.Lose, RpsResult.Win
            },
            {
                RpsResult.Win, RpsResult.Draw, RpsResult.Lose
            },
            {
                RpsResult.Lose, RpsResult.Win, RpsResult.Draw
            }
        };

        RpsResult result = resultMatrix[(int) session.Player.RPSSelection, (int) opponent.RPSSelection];

        RPS rpsEvent = DatabaseManager.Events.FindRockPaperScissorsEvent();
        if (rpsEvent is null)
        {
            return;
        }

        Item voucher = session.Player.Inventory.Items.Values.FirstOrDefault(x => x.Id == rpsEvent.VoucherId);
        if (voucher is null)
        {
            return;
        }

        session.Player.Inventory.ConsumeItem(session, voucher.Uid, 1);

        GameEventUserValue dailyMatches = GameEventHelper.GetUserValue(session.Player, rpsEvent.Id,
            TimeInfo.Tomorrow(), GameEventUserValueType.RPSDailyMatches);
        int.TryParse(dailyMatches.EventValue, out int dailyMatchCount);

        dailyMatchCount++;

        dailyMatches.EventValue = dailyMatchCount.ToString();

        DatabaseManager.GameEventUserValue.Update(dailyMatches);
        session.Send(GameEventUserValuePacket.UpdateValue(dailyMatches));
        session.Send(
            RockPaperScissorsPacket.MatchResults(result, session.Player.RPSSelection, opponent.RPSSelection));
    }

    private static void HandleClaimReward(GameSession session, PacketReader packet)
    {
        int rewardTier = packet.ReadInt();

        RPS rpsEvent = DatabaseManager.Events.FindRockPaperScissorsEvent();
        if (rpsEvent is null)
        {
            return;
        }

        GameEventUserValue rewardsAccumulatedValue = GameEventHelper.GetUserValue(session.Player, rpsEvent.Id,
            TimeInfo.Tomorrow(), GameEventUserValueType.RPSRewardsClaimed);
        List<string> rewardsClaimedStrings = rewardsAccumulatedValue.EventValue.Split(",").ToList();
        foreach (string rewardString in rewardsClaimedStrings)
        {
            // User has not claimed any rewards
            if (rewardString == "")
            {
                break;
            }

            if (int.TryParse(rewardString, out int rewardInt) && rewardInt == rewardTier)
            {
                return;
            }
        }

        RPSTier tier = rpsEvent.Tiers.ElementAtOrDefault(rewardTier);
        if (tier is null)
        {
            return;
        }

        foreach (RPSReward reward in tier.Rewards)
        {
            Item item = new(reward.ItemId)
            {
                Rarity = reward.ItemRarity,
                Amount = reward.ItemAmount
            };

            session.Player.Inventory.AddItem(session, item, true);
        }

        // update event value
        rewardsClaimedStrings.Add(rewardTier.ToString());
        rewardsAccumulatedValue.EventValue = string.Join(",", rewardsClaimedStrings);
        DatabaseManager.GameEventUserValue.Update(rewardsAccumulatedValue);

        session.Send(GameEventUserValuePacket.UpdateValue(rewardsAccumulatedValue));
    }

    private static void HandleConfirmMatch2(GameSession session, PacketReader packet)
    {
        long characterId = packet.ReadLong();

        Player otherPlayer = session.FieldManager.State.Players
            .FirstOrDefault(x => x.Value.Value.CharacterId == characterId).Value?.Value;
        if (otherPlayer is null)
        {
            return;
        }

        otherPlayer.Session.Send(RockPaperScissorsPacket.ConfirmMatch2(session.Player.CharacterId));
    }
}
