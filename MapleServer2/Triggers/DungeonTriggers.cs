using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        public void DungeonClear(string uiType)
        {
        }

        public void DungeonClearRound(byte round)
        {
        }

        public void DungeonCloseTimer()
        {
        }

        public void DungeonDisableRanking()
        {
        }

        public void DungeonEnableGiveUp(bool isEnable)
        {
        }

        public void DungeonFail()
        {
        }

        public void DungeonMissionComplete(int missionId, string feature)
        {
        }

        public void DungeonMoveLapTimeToNow(bool id)
        {
        }

        public void DungeonResetTime(int seconds)
        {
        }

        public void DungeonSetEndTime()
        {
        }

        public void DungeonSetLapTime(byte id, int lapTime)
        {
        }

        public void DungeonStopTimer()
        {
        }

        public void SetDungeonVariable(int varId, bool value)
        {
        }

        public void SetTimer(string id, int time, bool clearAtZero, bool display, int arg5, string arg6)
        {
            int msTime = time * 1000;
            int endTick = Environment.TickCount + msTime;
            MapTimer timer = new MapTimer(id, endTick);
            Field.AddMapTimer(timer);
            Field.BroadcastPacket(TriggerPacket.Timer(msTime, clearAtZero, display));
        }

        public void ResetTimer(string id)
        {
        }

        public void RoomExpire()
        {
        }

        public void FieldWarEnd(bool isClear)
        {
        }

        public void StartTutorial()
        {
        }

        #region DarkStream
        public void DarkStreamSpawnMonster(int[] spawnId, int score)
        {
        }

        public void DarkStreamStartGame(byte round)
        {
        }

        public void DarkStreamStartRound(byte round, int uiDuration, int damagePenalty)
        {
        }

        public void DarkStreamClearRound(byte round)
        {
        }
        #endregion

        #region ShadowExpedition
        public void ShadowExpeditionOpenBossGauge(int maxGaugePoint, string title)
        {
        }

        public void ShadowExpeditionCloseBossGauge()
        {
        }
        #endregion
    }
}
