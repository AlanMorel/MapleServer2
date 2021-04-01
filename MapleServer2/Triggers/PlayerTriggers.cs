using System.Numerics;
using Maple2.Trigger.Enum;

namespace MapleServer2.Triggers
{
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

        public void MoveUser(int arg1, int arg2, int arg3)
        {
        }

        public void MoveUserPath(string arg1)
        {
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

        public void RandomAdditionalEffect(string target, int triggerBoxId, int spawnPointId, byte targetCount, int tick, int waitTick, string targetEffect, int additionalEffectId)
        {
        }
        
        public void SetPcEmotionLoop(string arg1, float arg2, bool arg3)
        {
        }

        public void SetPcEmotionSequence(string arg1)
        {
        }

        public void SetPcRotation(Vector3 rotation)
        {
        }
        
        public void SetAchievement(int arg1, string arg2, string arg3)
        {
        }

        public void SetConversation(byte arg1, int arg2, string script, int arg4, byte arg5, Align align)
        {
        }

        public void SetOnetimeEffect(int id, bool enable, string path)
        {
        }
        
        public void SetTimeScale(bool enable, float startScale, float endScale, float duration, byte interpolator)
        {
        }
        
        public void AddBuff(int[] arg1, int arg2, byte arg3, bool arg4, bool arg5, string feature)
        {
        }
        
        public void RemoveBuff(int arg1, int arg2, bool arg3)
        {
        }

        public void AddUserValue(string key, int value)
        {
        }
        
        public void SetUserValue(int triggerId, string key, int value)
        {
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
}
