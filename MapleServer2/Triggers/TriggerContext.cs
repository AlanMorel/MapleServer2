using Maple2.Trigger;
using MapleServer2.Servers.Game;
using NLog;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext : ITriggerContext
    {
        public int NextTick;

        private readonly FieldManager Field;
        private readonly ILogger Logger;

        public TriggerContext(FieldManager field, ILogger logger)
        {
            Field = field;
            Logger = logger;
        }

        public void WriteLog(string arg1, int arg2, string arg3, byte arg4, string arg5)
        {
        }

        public void DebugString(string message, string feature)
        {
            Logger.Debug(message);
        }

        public int GetDungeonFirstUserMissionScore()
        {
            return 0;
        }

        public int GetDungeonId()
        {
            return 0;
        }

        public int GetDungeonLevel()
        {
            return 3;
        }

        public int GetDungeonMaxUserCount()
        {
            return 1;
        }

        public int GetDungeonPlayTime()
        {
            return 0;
        }

        public int GetDungeonRoundsRequired()
        {
            return int.MaxValue;
        }

        public string GetDungeonState()
        {
            return string.Empty;
        }

        public bool GetDungeonVariable(int id)
        {
            return false;
        }

        public float GetNpcDamageRate(int spawnPointId)
        {
            return 1.0f;
        }

        public int GetNpcExtraData(int spawnPointId, string extraDataKey)
        {
            return 0;
        }

        public float GetNpcHpRate(int spawnPointId)
        {
            return 1.0f;
        }

        public int GetScoreBoardScore()
        {
            return 0;
        }

        public int GetShadowExpeditionPoints()
        {
            return 0;
        }

        public int GetUserCount(int boxId, int userTagId)
        {
            return 20;
        }

        public int GetUserValue(string key)
        {
            return 0;
        }
    }
}
