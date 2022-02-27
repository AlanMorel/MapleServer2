using Maple2.Trigger.Enum;
using Maple2Storage.Enums;
using Maple2Storage.Tools;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Types;

namespace MapleServer2.Triggers;

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
                if (!int.TryParse(npcId, out int id))
                {
                    continue;
                }

                if (Field.State.Mobs.Values.Any(x => x.Value.Id == id && !x.IsDead))
                {
                    return false;
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

    public bool ObjectInteracted(int[] interactIds, byte state)
    {
        InteractObjectState objectState = (InteractObjectState) state;
        foreach (int interactId in interactIds)
        {
            InteractObject interactObject = Field.State.InteractObjects.Values.FirstOrDefault(x => x.InteractId == interactId);
            if (interactObject == null)
            {
                continue;
            }

            if (interactObject.State != objectState)
            {
                return false;
            }
        }

        return true;
    }

    public bool PvpZoneEnded(byte arg1)
    {
        return false;
    }

    public bool QuestUserDetected(int[] boxes, int[] questIds, byte[] modes, byte arg4)
    {
        QuestState mode = (QuestState) modes[0];
        foreach (int boxId in boxes)
        {
            MapTriggerBox box = MapEntityStorage.GetTriggerBox(Field.MapId, boxId);
            List<IFieldActor<Player>> players = Field.State.Players.Values.ToList();

            foreach (IFieldObject<Player> player in players)
            {
                if (!FieldManager.IsPlayerInBox(box, player))
                {
                    continue;
                }

                foreach (int questId in questIds)
                {
                    if (!player.Value.QuestData.TryGetValue(questId, out QuestStatus quest))
                    {
                        return false;
                    }

                    switch (mode)
                    {
                        case QuestState.Started:
                            return quest.State is QuestState.Started;
                        case QuestState.ConditionCompleted:
                            return quest.State is not QuestState.None && quest.Condition.All(condition => condition.Completed);
                        case QuestState.Finished:
                            return quest.State is QuestState.Finished;
                    }
                }
            }
        }

        return false;
    }

    public bool RandomCondition(float proc, string desc) => RandomProvider.Get().Next(100) <= proc;

    public bool TimeExpired(string id)
    {
        MapTimer timer = Field.GetMapTimer(id);
        return timer != null && timer.EndTick < Environment.TickCount;
    }

    public bool UserDetected(int[] boxIds, byte jobId)
    {
        Job job = (Job) jobId;
        List<IFieldActor<Player>> players = Field.State.Players.Values.ToList();
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
        Widget widget = Field.GetWidget(type);
        if (widget == null)
        {
            return false;
        }

        if (widget.Arg == "")
        {
            return widget.State == name;
        }

        return widget.State == name && widget.Arg == arg3;
    }
}
