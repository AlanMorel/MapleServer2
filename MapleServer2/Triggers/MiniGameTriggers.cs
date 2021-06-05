using Maple2.Trigger.Enum;

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
        }

        public void EndMiniGameRound(int winnerBoxId, float expRate, bool isOnlyWinner, bool isGainLoserBonus, bool meso, MiniGame type)
        {
        }

        public void MiniGameCameraDirection(int boxId, int cameraId)
        {
        }

        public void MiniGameGiveExp(int boxId, float expRate, bool isOutSide)
        {
        }

        public void MiniGameGiveReward(int winnerBoxId, string contentType, MiniGame type)
        {
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
