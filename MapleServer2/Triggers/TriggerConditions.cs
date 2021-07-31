using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maple2.Trigger.Enum;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        private static int WaitTickTimeFirst;
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
                        if (Field.State.Mobs.Values.Where(x => x.Value.Id == id).Count() != 0)
                        {
                            Console.WriteLine("Monster is NOT dead");
                            return false;
                        }
                    }
                }

            }
            Console.WriteLine("Monster is dead");
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

        public bool RandomCondition(float arg1, string desc)
        {
            Console.WriteLine("Checking Random Condition");
            return true;
        }

        public bool TimeExpired(string id)
        {
            Console.WriteLine("Checking TimeExpired");
            MapTimer timer = Field.GetMapTimer(id);
            if (timer == null)
            {
                Console.WriteLine("No timer found");
                return false;
            }

            System.Console.WriteLine($"TimerID: {timer.Id}, CurrentTick: {Environment.TickCount}, EndTick of Timer: {timer.EndTick}");
            if (timer.EndTick < Environment.TickCount)
            {
                Console.WriteLine($"{timer.Id}: EndTick is less than TickCount");
                return true;
            }
            return false;
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
                if (box == null)
                {
                    return false;
                }

                CoordF minCoord = CoordF.From(
                    box.Position.X - box.Dimension.X,
                    box.Position.Y - box.Dimension.Y,
                    box.Position.Z - box.Dimension.Z);
                CoordF maxCoord = CoordF.From(
                    box.Position.X + box.Dimension.X,
                    box.Position.Y + box.Dimension.Y,
                    box.Position.Z + box.Dimension.Z);
                foreach (IFieldObject<Player> player in players)
                {
                    bool min = player.Coord.X >= minCoord.X && player.Coord.Y >= minCoord.Y && player.Coord.Z >= minCoord.Z;
                    bool max = player.Coord.X <= maxCoord.X && player.Coord.Y <= maxCoord.Y && player.Coord.Z <= maxCoord.Z;
                    if (min && max)
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
            string debug = $"Widget Type: {type}, State Name: {name}, Arg3: {arg3}";
            Field.BroadcastPacket(NoticePacket.Notice(debug, NoticeType.Chat));
            List<IFieldObject<Player>> players = Field.State.Players.Values.ToList();
            foreach (IFieldObject<Player> player in players)
            {
                Widget widget = player.Value.Widgets.FirstOrDefault(x => x.Type == type);
                Console.WriteLine($"Widget: {widget.Type} State: {widget.State}");
                if (widget == null)
                {
                    continue;
                }
                widget.State = name;
                Console.WriteLine("Widget condition true");
                return widget.State == name;

            }

            Console.WriteLine("Widget condition false");
            return false;
        }
    }
}
