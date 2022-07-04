using System.Numerics;
using Maple2.Trigger.Enum;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Managers;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Triggers;

public partial class TriggerContext
{
    public void AddEffectNif(int spawnPointId, string nifPath, bool isOutline, float scale, int rotateZ)
    {
    }

    public void RemoveEffectNif(int spawnPointId)
    {
    }

    public void EnableSpawnPointPc(int spawnPointId, bool isEnable)
    {
    }

    public void FaceEmotion(int spawnPointId, string emotionName)
    {
        if (spawnPointId == 0)
        {
            IFieldActor<Player> firstPlayer = Field.State.Players.FirstOrDefault().Value;
            Field.BroadcastPacket(TriggerPacket.SetFaceEmotion(firstPlayer.ObjectId, emotionName));
            return;
        }

        MapEventNpcSpawnPoint spawnPoint = MapEntityMetadataStorage.GetMapEventNpcSpawnPoint(Field.MapId, spawnPointId);
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

            if (Field.State.Npcs.TryGetValue(id, out Npc npc))
            {
                Field.BroadcastPacket(TriggerPacket.SetFaceEmotion(npc.ObjectId, emotionName));
                return;
            }

            if (Field.State.Mobs.TryGetValue(id, out Npc mob))
            {
                Field.BroadcastPacket(TriggerPacket.SetFaceEmotion(mob.ObjectId, emotionName));
                return;
            }
        }
    }

    public void GiveExp(byte arg1, byte arg2)
    {
    }

    public void GiveGuildExp(bool boxId, byte type)
    {
    }

    public void GiveRewardContent(int rewardId)
    {
    }

    public void KickMusicAudience(int targetBoxId, int targetPortalId)
    {
    }

    public void MoveRandomUser(int arg1, byte arg2, int arg3, byte arg4)
    {
    }

    public void MoveToPortal(int userTagId, int portalId, int boxId)
    {
    }

    public void MoveUser(int mapId, int triggerId, int boxId)
    {
        List<Character> players = Field.State.Players.Values.ToList();
        if (boxId != 0)
        {
            MapTriggerBox box = MapEntityMetadataStorage.GetTriggerBox(Field.MapId, boxId);
            if (box is null)
            {
                return;
            }

            players = players.Where(player => FieldManager.IsActorInBox(box, player)).ToList();
        }

        // move player back to return map
        if (mapId == 0 && triggerId == 0)
        {
            foreach (IFieldObject<Player> player in players)
            {
                player.Value.Warp(player.Value.ReturnMapId, player.Value.ReturnCoord);
            }

            return;
        }

        if (mapId == Field.MapId)
        {
            IFieldObject<Portal> portal = Field.State.Portals.Values.First(p => p.Value.Id == triggerId);
            foreach (IFieldObject<Player> player in players)
            {
                player.Value.Move(portal.Coord, portal.Rotation, isTrigger: true);
            }

            return;
        }

        CoordF moveCoord;
        CoordF moveRotation;
        MapPortal dstPortal = MapEntityMetadataStorage.GetPortals(mapId).FirstOrDefault(portal => portal.Id == triggerId);
        if (dstPortal == null)
        {
            MapPlayerSpawn spawn = MapEntityMetadataStorage.GetRandomPlayerSpawn(mapId);
            moveCoord = spawn.Coord.ToFloat();
            moveRotation = spawn.Rotation.ToFloat();
        }
        else
        {
            moveCoord = dstPortal.Coord.ToFloat();
            moveRotation = dstPortal.Rotation.ToFloat();
        }

        foreach (IFieldObject<Player> player in players)
        {
            player.Value.Warp(mapId, moveCoord, moveRotation, instanceId: 1);
        }
    }

    public void MoveUserPath(string movePath)
    {
        PatrolData patrolData = MapEntityMetadataStorage.GetPatrolData(Field.MapId, movePath);
        Field.MovePlayerAlongPath(Field.State.Players.First().Value, patrolData);
    }

    public void MoveUserToBox(int boxId, bool portalId)
    {
    }

    public void MoveUserToPos(Vector3 pos, Vector3 rot)
    {
    }

    public void PatrolConditionUser(string patrolName, byte patrolIndex, int additionalEffectId)
    {
    }

    public void RandomAdditionalEffect(string target, int triggerBoxId, int spawnPointId, byte targetCount, int tick, int waitTick, string targetEffect,
        int additionalEffectId)
    {
    }

    public void SetPcEmotionLoop(string animationState, float duration, bool isLoop)
    {
        Field.BroadcastPacket(TriggerPacket.SetAnimationLoop(animationState, (int) duration, isLoop));
    }

    public void SetPcEmotionSequence(string animation)
    {
        Field.BroadcastPacket(TriggerPacket.SetAnimationSequence(animation));
    }

    public void SetPcRotation(Vector3 rotation)
    {
    }

    public void SetAchievement(int boxId, string type, string trophySet)
    {
        List<Character> players = Field.State.Players.Values.ToList();
        if (boxId != 0)
        {
            MapTriggerBox box = MapEntityMetadataStorage.GetTriggerBox(Field.MapId, boxId);
            if (box is null)
            {
                return;
            }

            players = players.Where(player => FieldManager.IsActorInBox(box, player)).ToList();
        }

        foreach (IFieldObject<Player> player in players)
        {
            if (type == "trigger")
            {
                TrophyManager.OnTrigger(player.Value, trophySet);
            }
        }
    }

    public void SetConversation(byte arg1, int npcId, string script, int delay, byte arg5, Align align)
    {
        if (npcId == 0)
        {
            IFieldActor<Player> player = Field.State.Players.Values.FirstOrDefault();
            if (player is null)
            {
                return;
            }

            Field.BroadcastPacket(CinematicPacket.BalloonTalk(player.ObjectId, false, script, delay * 1000, 0));
            return;
        }

        if (arg1 == 1) // Use npc object id?
        {
            Npc npc = Field.State.Npcs.Values.FirstOrDefault(x => x.SpawnPointId == npcId);
            if (npc is null)
            {
                return;
            }

            Field.BroadcastPacket(CinematicPacket.BalloonTalk(npc.ObjectId, false, script, delay * 1000, 0));
            return;
        }

        Field.BroadcastPacket(CinematicPacket.Conversation(npcId, npcId.ToString(), script, delay * 1000, align));
    }

    public void SetOnetimeEffect(int id, bool enable, string path)
    {
        Field.BroadcastPacket(OneTimeEffectPacket.View(id, enable, path));
    }

    public void SetTimeScale(bool enable, float startScale, float endScale, float duration, byte interpolator)
    {
        Field.BroadcastPacket(TimeScalePacket.SetTimeScale(enable, startScale, endScale, duration, interpolator));
    }

    public void AddBuff(int[] boxIds, int skillId, byte skillLevel, bool arg4, bool arg5, string feature)
    {
        foreach (int boxId in boxIds)
        {
            MapTriggerBox box = MapEntityMetadataStorage.GetTriggerBox(Field.MapId, boxId);
            if (box is null)
            {
                return;
            }

            foreach (Character player in Field.State.Players.Values.Where(player => FieldManager.IsActorInBox(box, player)))
            {
                // TODO: Rework when buff system is implemented
                Status status = new(new(skillId, skillLevel), player.ObjectId, player.ObjectId, 1);
                StatusHandler.Handle(player.Value.Session, status);
            }
        }
    }

    public void RemoveBuff(int arg1, int arg2, bool arg3)
    {
    }

    public void AddUserValue(string key, int value)
    {
    }

    public void SetUserValue(int triggerId, string key, int value)
    {
        PlayerTrigger playerTrigger = new(key)
        {
            TriggerId = triggerId,
            Value = value
        };
        foreach (IFieldObject<Player> player in Field.State.Players.Values)
        {
            player.Value.Triggers.Add(playerTrigger);
        }
    }

    public void SetUserValueFromDungeonRewardCount(string key, int dungeonRewardId)
    {
    }

    public void SetUserValueFromUserCount(int triggerBoxId, string key, int userTagId)
    {
    }

    public void UserValueToNumberMesh(string key, int startMeshId, byte digitCount)
    {
    }
}
