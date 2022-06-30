using System.Numerics;
using Maple2.Trigger.Enum;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Triggers;

public partial class TriggerContext
{
    public void AddBalloonTalk(int spawnPointId, string msg, int duration, int delayTick, bool isNpcId)
    {
        if (spawnPointId == 0)
        {
            IFieldObject<Player> player = Field.State.Players.Values.First();

            Field.BroadcastPacket(CinematicPacket.BalloonTalk(player.ObjectId, isNpcId, msg, duration, delayTick));
            return;
        }

        Npc npc = Field.State.Npcs.Values.FirstOrDefault(x => x.SpawnPointId == spawnPointId);
        if (npc is null)
        {
            return;
        }

        Field.BroadcastPacket(CinematicPacket.BalloonTalk(npc.ObjectId, isNpcId, msg, duration, delayTick));
    }

    public void RemoveBalloonTalk(int spawnPointId)
    {
    }

    public void AddCinematicTalk(int npcId, string illustId, string script, int duration, Align align, int delayTick)
    {
        Field.BroadcastPacket(CinematicPacket.Conversation(npcId, illustId, script, duration, align));
    }

    public void CreateMonster(int[] spawnPointIds, bool spawnAnimation, int arg3)
    {
        foreach (int spawnPointId in spawnPointIds)
        {
            MapEventNpcSpawnPoint spawnPoint = MapEntityMetadataStorage.GetMapEventNpcSpawnPoint(Field.MapId, spawnPointId);
            if (spawnPoint is null)
            {
                continue;
            }

            for (int i = 0; i < spawnPoint.Count; i++)
            {
                foreach (string npcId in spawnPoint.NpcIds)
                {
                    if (!int.TryParse(npcId, out int id))
                    {
                        continue;
                    }

                    short animation = -1;
                    if (spawnAnimation)
                    {
                        NpcMetadata npcMetadata = NpcMetadataStorage.GetNpcMetadata(id);
                        if (npcMetadata is null || !npcMetadata.StateActions.TryGetValue(NpcState.Normal, out (string, NpcAction, short)[] stateAction))
                        {
                            continue;
                        }

                        if (stateAction.Length == 0)
                        {
                            continue;
                        }

                        animation = AnimationStorage.GetSequenceIdBySequenceName(npcMetadata.NpcMetadataModel.Model, stateAction[0].Item1);
                    }

                    Npc npc = Field.RequestNpc(id, spawnPoint.Position, spawnPoint.Rotation, animation);
                    npc.SpawnPointId = spawnPointId;
                }
            }
        }
    }

    public void ChangeMonster(int arg1, int arg2)
    {
    }

    public void DestroyMonster(int[] rangeId, bool arg2)
    {
        foreach (int spawnPointId in rangeId)
        {
            if (spawnPointId == -1)
            {
                foreach (Npc npc in Field.State.Npcs.Values)
                {
                    Field.RemoveNpc(npc);
                }
                foreach (Npc mob in Field.State.Mobs.Values)
                {
                    Field.RemoveMob(mob);
                }
                continue;
            }

            Npc fieldNpc = Field.State.Npcs.Values.FirstOrDefault(x => x.SpawnPointId == spawnPointId);
            if (fieldNpc is not null)
            {
                Field.RemoveNpc(fieldNpc);
                continue;
            }

            Npc fieldMob = Field.State.Mobs.Values.FirstOrDefault(x => x.SpawnPointId == spawnPointId);
            if (fieldMob is not null)
            {
                Field.RemoveMob(fieldMob);
            }
        }
    }

    public void StartCombineSpawn(int[] groupId, bool isStart)
    {
    }

    public void InitNpcRotation(int[] arg1)
    {
    }

    public void LimitSpawnNpcCount(byte limitCount)
    {
    }

    public void MoveNpc(int spawnTriggerId, string patrolDataName)
    {
        PatrolData patrolData = MapEntityMetadataStorage.GetPatrolData(Field.MapId, patrolDataName);

        Field.State.Npcs.Values.FirstOrDefault(x => x.SpawnPointId == spawnTriggerId)?.SetPatrolData(patrolData);
    }

    public void MoveNpcToPos(int spawnPointId, Vector3 pos, Vector3 rot)
    {
    }

    public void NpcRemoveAdditionalEffect(int spawnPointId, int additionalEffectId)
    {
    }

    public void NpcToPatrolInBox(int boxId, int npcId, string spawnId, string patrolName)
    {
    }

    public void SetNpcDuelHpBar(bool isOpen, int spawnPointId, int durationTick, byte npcHpStep)
    {
    }

    public void SetNpcEmotionLoop(int spawnTriggerId, string sequenceName, float duration)
    {
        IFieldActor<NpcMetadata> fieldNpc = Field.State.Npcs.Values.FirstOrDefault(x => x.SpawnPointId == spawnTriggerId);

        fieldNpc?.Animate(sequenceName, duration);
    }

    public void SetNpcEmotionSequence(int spawnTriggerId, string sequenceName, int arg3)
    {
        IFieldActor<NpcMetadata> fieldNpc = Field.State.Npcs.Values.FirstOrDefault(x => x.SpawnPointId == spawnTriggerId);

        fieldNpc?.Animate(sequenceName);
    }

    public void SetNpcRotation(int arg1, int arg2)
    {
    }

    public void SpawnNpcRange(int[] rangeId, bool isAutoTargeting, byte randomPickCount, int score)
    {
        foreach (int spawnPointId in rangeId)
        {
            MapEventNpcSpawnPoint spawnPoint = MapEntityMetadataStorage.GetMapEventNpcSpawnPoint(Field.MapId, spawnPointId);
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

                Npc npc = Field.RequestNpc(id, spawnPoint.Position, spawnPoint.Rotation);
                npc.SpawnPointId = spawnPointId;
            }
        }
    }

    public void TalkNpc(int spawnPointId)
    {
    }

    public void SetAiExtraData(string key, int value, bool isModify, int boxId)
    {
    }

    public void SetQuestAccept(int questId, int arg1)
    {
    }

    public void SetQuestComplete(int questId)
    {
    }
}
