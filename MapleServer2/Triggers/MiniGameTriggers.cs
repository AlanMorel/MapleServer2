using System.Collections.Generic;
using System.Linq;
using Maple2.Trigger.Enum;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Triggers
{
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
            List<IFieldObject<Player>> players = new List<IFieldObject<Player>>();
            foreach (IFieldObject<Player> player in Field.State.Players.Values)
            {
                if (FieldManager.IsPlayerInBox(box, player))
                {
                    if (type == MiniGame.LudibriumEscape)
                    {
                        PlayerTrigger trigger = player.Value.Triggers.FirstOrDefault(x => x.Key == "gameStart");
                        player.Value.Triggers.Remove(trigger);
                    }
                }
            }
        }

        public void EndMiniGameRound(int winnerBoxId, float expRate, bool isOnlyWinner, bool isGainLoserBonus, bool meso, MiniGame type)
        {
            // TODO: Properly implement results packet. 
            MapTriggerBox box = MapEntityStorage.GetTriggerBox(Field.MapId, winnerBoxId);
            List<IFieldObject<Player>> players = new List<IFieldObject<Player>>();
            foreach (IFieldObject<Player> player in Field.State.Players.Values)
            {
                if (FieldManager.IsPlayerInBox(box, player))
                {
                    player.Value.Session.Send(ResultsPacket.Rounds(1, 1));
                }
            }
        }

        public void MiniGameCameraDirection(int boxId, int cameraId)
        {
        }

        public void MiniGameGiveExp(int boxId, float expRate, bool isOutSide)
        {
        }

        public void MiniGameGiveReward(int winnerBoxId, string contentType, MiniGame type)
        {
            MapTriggerBox box = MapEntityStorage.GetTriggerBox(Field.MapId, winnerBoxId);
            List<IFieldObject<Player>> players = new List<IFieldObject<Player>>();
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
                        InventoryController.Add(player.Value.Session, item, true);
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
}
