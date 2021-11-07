using Maple2.Trigger.Enum;
using Maple2Storage.Tools;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Triggers;

public partial class TriggerContext
{
    public void StartMiniGame(int boxId, byte round, MiniGame type, bool isShowResultUI)
    {
    }

    public void StartMiniGameRound(int boxId, byte round)
    {

    }

    public void EndMiniGame(int winnerBoxId, MiniGame type, bool isOnlyWinner)
    {
        MapTriggerBox box = MapEntityStorage.GetTriggerBox(Field.MapId, winnerBoxId);
        List<IFieldObject<Player>> players = new();
        foreach (IFieldObject<Player> player in Field.State.Players.Values)
        {
            if (FieldManager.IsPlayerInBox(box, player))
            {
                if (type == MiniGame.LudibriumEscape)
                {
                    PlayerTrigger trigger = player.Value.Triggers.FirstOrDefault(x => x.Key == "gameStart");
                    player.Value.Triggers.Remove(trigger);
                    player.Value.Session.Send(ResultsPacket.Rounds(1, 1));
                }
                else if (type == MiniGame.OXQuiz)
                {
                    player.Value.Session.Send(ResultsPacket.Rounds(10, 10));
                }
            }
        }
    }

    public void EndMiniGameRound(int winnerBoxId, float expRate, bool isOnlyWinner, bool isGainLoserBonus, bool meso, MiniGame type)
    {
        MapTriggerBox box = MapEntityStorage.GetTriggerBox(Field.MapId, winnerBoxId);
        foreach (IFieldObject<Player> player in Field.State.Players.Values)
        {
            if (FieldManager.IsPlayerInBox(box, player))
            {
                // TODO: calculate correct amount of exp;
                player.Value.Levels.GainExp(10000);
            }
        }
    }

    public void MiniGameCameraDirection(int boxId, int cameraId)
    {
        MapTriggerBox box = MapEntityStorage.GetTriggerBox(Field.MapId, boxId);
        List<IFieldObject<Player>> boxPlayers = new();
        foreach (IFieldObject<Player> player in Field.State.Players.Values)
        {
            if (FieldManager.IsPlayerInBox(box, player))
            {
                boxPlayers.Add(player);
            }
        }

        Random random = RandomProvider.Get();
        int index = random.Next(boxPlayers.Count);
        IFieldObject<Player> randomPlayer = boxPlayers[index];
        Field.BroadcastPacket(LocalCameraPacket.Camera(cameraId, 1, randomPlayer.ObjectId));
    }

    public void MiniGameGiveExp(int boxId, float expRate, bool isOutSide)
    {
    }

    public void MiniGameGiveReward(int winnerBoxId, string contentType, MiniGame type)
    {
        MapTriggerBox box = MapEntityStorage.GetTriggerBox(Field.MapId, winnerBoxId);
        List<IFieldObject<Player>> players = new();
        foreach (IFieldObject<Player> player in Field.State.Players.Values)
        {
            if (FieldManager.IsPlayerInBox(box, player))
            {
                players.Add(player);
            }
        }
        foreach (IFieldObject<Player> player in players)
        {
            if (contentType == "miniGame")
            {
                List<Item> items = RewardContentMetadataStorage.GetRewardItems(3, player.Value.Levels.Level);
                foreach (Item item in items)
                {
                    player.Value.Inventory.AddItem(player.Value.Session, item, true);
                }
            }
            else if (contentType == "UserOpenMiniGameExtraReward")
            {

            }
        }
    }

    public void SetMiniGameAreaForHack(int boxId)
    {
    }

    public void UnSetMiniGameAreaForHack()
    {
    }

    public void SetState(byte arg1, string arg2, bool arg3)
    {
    }

    public void UseState(byte arg1, bool arg2)
    {
    }

    #region FieldGame
    public void CreateFieldGame(FieldGame type, bool reset)
    {
    }

    public void FieldGameConstant(string key, string value, string feature, Locale locale)
    {
    }

    public void FieldGameMessage(byte custom, string type, byte arg1, string arg2, int arg3)
    {
    }
    #endregion

    #region GuildVsGame
    public void GuildVsGameEndGame()
    {
    }

    public void GuildVsGameGiveContribution(int teamId, bool isWin, string desc)
    {
    }

    public void GuildVsGameGiveReward(GuildReward type, int teamId, bool isWin, string desc)
    {
    }

    public void GuildVsGameLogResult(string desc)
    {
    }

    public void GuildVsGameLogWonByDefault(int teamId, string desc)
    {
    }

    public void GuildVsGameResult(string desc)
    {
    }

    public void GuildVsGameScoreByUser(int triggerBoxId, bool score, string desc)
    {
    }

    public void SetUserValueFromGuildVsGameScore(int teamId, string key)
    {
    }
    #endregion
}
