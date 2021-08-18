using Maple2.Trigger.Enum;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

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
            return !Field.State.Players.IsEmpty;
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

        public bool MonsterDead(int[] spawnPointIds, bool arg2)
        {
            // TODO: Needs a better check for multiple mob spawns
            foreach (int spawnPointId in spawnPointIds)
            {
                MapEventNpcSpawnPoint spawnPoint = MapEntityStorage.GetMapEventNpcSpawnPoint(Field.MapId, spawnPointId);
                if (spawnPoint == null)
                {
                    continue;
                }
                foreach (string npcId in spawnPoint.NpcIds)
                {
                    if (int.TryParse(npcId, out int id))
                    {
                        if (Field.State.Mobs.Values.Where(x => x.Value.Id == id).Any())
                        {
                            return false;
                        }
                    }
                }

            }
            return true;
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

        public bool RandomCondition(float proc, string desc)
        {
            Random random = new Random();
            float result = (float) random.NextDouble();
            result *= 100;
            return result <= proc;
        }

        public bool TimeExpired(string id)
        {
            MapTimer timer = Field.GetMapTimer(id);
            if (timer == null || timer.EndTick >= Environment.TickCount)
            {
                return false;
            }

            return true;
        }

        public bool UserDetected(int[] boxIds, byte jobId)
        {
            Job job = (Job) jobId;
            List<IFieldObject<Player>> players = Field.State.Players.Values.ToList();
            if (job != Job.None)
            {
                players = players.Where(x => x.Value.Job == job).ToList();
            }
            foreach (int boxId in boxIds)
            {
                MapTriggerBox box = MapEntityStorage.GetTriggerBox(Field.MapId, boxId);

                foreach (IFieldObject<Player> player in players)
                {
                    if (FieldManager.IsPlayerInBox(box, player))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool WaitAndResetTick(int waitTick)
        {
            return false;
        }

        public bool WaitTick(int waitTick)
        {
            if (NextTick == 0)
            {
                NextTick = Environment.TickCount + waitTick;
                return false;
            }

            if (NextTick > Environment.TickCount)
            {
                return false;
            }

            NextTick = 0;
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

        public bool WidgetCondition(WidgetType type, string name, string arg3)
        {
            List<IFieldObject<Player>> players = Field.State.Players.Values.ToList();
            foreach (IFieldObject<Player> player in players)
            {
                Widget widget = Field.GetWidget(type);
                if (widget == null)
                {
                    continue;
                }
                return widget.State == name;
            }
            return false;
        }
    }
}
