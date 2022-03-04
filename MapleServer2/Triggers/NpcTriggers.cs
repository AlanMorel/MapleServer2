using System.Numerics;
using Maple2.Trigger.Enum;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Triggers;

public partial class TriggerContext
{
    public void AddBalloonTalk(int spawnPointId, string msg, int duration, int delayTick, bool isNpcId)
    {
        if (!isNpcId)
        {
            if (spawnPointId == 0)
            {
                IFieldObject<Player> player = Field.State.Players.Values.First();
                if (player == null)
                {
                    return;
                }

                player.Value.Session.Send(CinematicPacket.BalloonTalk(player.ObjectId, isNpcId, msg, duration, delayTick));
            }
        }
    }

    public void RemoveBalloonTalk(int spawnPointId)
    {
    }

    public void AddCinematicTalk(int npcId, string illustId, string script, int duration, Align align, int delayTick)
    {
        Field.BroadcastPacket(CinematicPacket.Conversation(npcId, script, duration, align));
    }

    public void CreateMonster(int[] spawnPointIds, bool arg2, int arg3)
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
                    if (int.TryParse(npcId, out int id))
                    {
                        Field.RequestNpc(id, spawnPoint.Position, spawnPoint.Rotation);
                    }
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
            MapEventNpcSpawnPoint spawnPoint = MapEntityMetadataStorage.GetMapEventNpcSpawnPoint(Field.MapId, spawnPointId);
            if (spawnPoint is null)
            {
                continue;
            }

            foreach (string npcId in spawnPoint.NpcIds)
            {
                if (!int.TryParse(npcId, out int id))
                {
                    continue;
                }

                IFieldActor<NpcMetadata> fieldNpc = Field.State.Npcs.Values.FirstOrDefault(x => x.Value.Id == id);
                if (fieldNpc is not null)
                {
                    Field.RemoveNpc(fieldNpc);
                    continue;
                }

                IFieldActor<NpcMetadata> fieldMob = Field.State.Mobs.Values.FirstOrDefault(x => x.Value.Id == id);
                if (fieldMob is not null)
                {
                    Field.RemoveMob(fieldMob);
                }
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
        (PatrolData, List<WayPoint>) patrolData = MapEntityMetadataStorage.GetPatrolData(Field.MapId, patrolDataName);

        MapEventNpcSpawnPoint spawnPoint = MapEntityMetadataStorage.GetMapEventNpcSpawnPoint(Field.MapId, spawnTriggerId);
        if (spawnPoint is null)
        {
            return;
        }

        foreach (string npcId in spawnPoint.NpcIds)
        {
            if (!int.TryParse(npcId, out int id))
            {
                continue;
            }

            IFieldActor<NpcMetadata> fieldNpc = Field.State.Npcs.Values.FirstOrDefault(x => x.Value.Id == id);
            if (fieldNpc is null)
            {
                continue;
            }

            // Just setting the coord as the last waypoint for now, replace with moveTo later
            // fieldNpc.MoveTo(patrolData.Item2.Last().Position);
            fieldNpc.Coord = patrolData.Item2.Last().Position.ToFloat();
        }
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
        MapEventNpcSpawnPoint spawnPoint = MapEntityMetadataStorage.GetMapEventNpcSpawnPoint(Field.MapId, spawnTriggerId);
        if (spawnPoint is null)
        {
            return;
        }

        foreach (string npcId in spawnPoint.NpcIds)
        {
            if (!int.TryParse(npcId, out int id))
            {
                continue;
            }

            IFieldActor<NpcMetadata> fieldNpc = Field.State.Npcs.Values.FirstOrDefault(x => x.Value.Id == id);

            fieldNpc?.Animate(sequenceName);
        }
    }

    public void SetNpcEmotionSequence(int spawnTriggerId, string sequenceName, int arg3)
    {
        MapEventNpcSpawnPoint spawnPoint = MapEntityMetadataStorage.GetMapEventNpcSpawnPoint(Field.MapId, spawnTriggerId);
        if (spawnPoint is null)
        {
            return;
        }

        foreach (string npcId in spawnPoint.NpcIds)
        {
            if (!int.TryParse(npcId, out int id))
            {
                continue;
            }

            IFieldActor<NpcMetadata> fieldNpc = Field.State.Npcs.Values.FirstOrDefault(x => x.Value.Id == id);

            fieldNpc?.Animate(sequenceName);
        }
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

                Field.RequestNpc(id, spawnPoint.Position, spawnPoint.Rotation);
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
