using Maple2.Trigger.Enum;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        public bool BonusGameRewardDetected(byte arg1)
        {
            return false;
        }

        public bool CheckAnyUserAdditionalEffect(int triggerBoxId, int additionalEffectId, byte level)
        {
            return false;
        }

        public bool CheckDungeonLobbyUserCount()
        {
            //TODO: Implement checking dungeon lobby user count. The below is temporary.
            return Field.State.Players.IsEmpty;
        }

        public bool CheckNpcAdditionalEffect(int spawnPointId, int additionalEffectId, byte level)
        {
            return false;
        }

        public bool CheckSameUserTag(int triggerBoxId)
        {
            return false;
        }

        public bool DayOfWeek(byte[] dayOfWeeks, string desc)
        {
            return false;
        }

        public bool DetectLiftableObject(int[] triggerBoxIds, int itemId)
        {
            return false;
        }

        public bool DungeonTimeOut()
        {
            return false;
        }

        public bool GuildVsGameScoredTeam(int teamId)
        {
            return false;
        }

        public bool GuildVsGameWinnerTeam(int teamId)
        {
            return false;
        }

        public bool IsDungeonRoom()
        {
            return false;
        }

        public bool IsPlayingMapleSurvival()
        {
            return false;
        }

        public bool MonsterDead(int[] arg1, bool arg2)
        {
            return false;
        }

        public bool MonsterInCombat(int[] arg1)
        {
            return false;
        }

        public bool NpcDetected(int arg1, int[] arg2)
        {
            return false;
        }

        public bool NpcIsDeadByStringId(string stringId)
        {
            return false;
        }

        public bool ObjectInteracted(int[] arg1, byte arg2)
        {
            return false;
        }

        public bool PvpZoneEnded(byte arg1)
        {
            return false;
        }

        public bool QuestUserDetected(int[] arg1, int[] arg2, byte[] arg3, byte arg4)
        {
            return false;
        }

        public bool RandomCondition(float arg1, string desc)
        {
            return false;
        }

        public bool TimeExpired(string arg1)
        {
            return false;
        }

        public bool UserDetected(int[] arg1, byte arg2)
        {
            return false;
        }

        public bool WaitAndResetTick(int waitTick)
        {
            return false;
        }

        public bool WaitTick(int waitTick)
        {
            NextTick += waitTick;
            return true;
        }

        public bool WeddingEntryInField(WeddingEntryType type)
        {
            return false;
        }

        public bool WeddingHallState(string hallState)
        {
            return false;
        }

        public bool? WeddingMutualAgreeResult(WeddingAgreeType type)
        {
            return false;
        }

        public bool WidgetCondition(WidgetType type, string arg2, string arg3)
        {
            return false;
        }
    }
}
