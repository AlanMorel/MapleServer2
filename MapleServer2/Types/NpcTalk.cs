namespace MapleServer2.Types
{
    public class NpcTalk
    {
        public int ScriptId;
        public int QuestId;
        public int ContentIndex;
        public Npc Npc;
        public List<QuestStatus> Quests;
        public bool IsQuest;

        public NpcTalk(Npc npc, List<QuestStatus> quests)
        {
            Npc = npc;
            Quests = quests;
        }
    }
}
