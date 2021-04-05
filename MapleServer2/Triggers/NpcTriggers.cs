using System.Numerics;
using Maple2.Trigger.Enum;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        public void AddBalloonTalk(int spawnPointId, string msg, int duration, int delayTick, bool npcId)
        {
        }

        public void RemoveBalloonTalk(int spawnPointId)
        {
        }

        public void AddCinematicTalk(int npcId, string illustId, string script, int duration, Align align, int delayTick)
        {
        }

        public void CreateMonster(int[] arg1, bool arg2, int arg3)
        {
        }

        public void ChangeMonster(int arg1, int arg2)
        {
        }

        public void DestroyMonster(int[] arg1, bool arg2)
        {
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

        public void MoveNpc(int arg1, string arg2)
        {
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

        public void SetNpcEmotionLoop(int arg1, string arg2, float arg3)
        {
        }

        public void SetNpcEmotionSequence(int arg1, string arg2, int arg3)
        {
        }

        public void SetNpcRotation(int arg1, int arg2)
        {
        }

        public void SpawnNpcRange(int[] rangeId, bool isAutoTargeting, byte randomPickCount, int score)
        {
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
}
