using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

public class NpcTalk
{
    public int ScriptId;
    public int QuestId;
    public int ContentIndex;
    public NpcMetadata Npc;
    public List<QuestStatus> Quests;
    public bool IsQuest;

    public NpcTalk(NpcMetadata npc, List<QuestStatus> quests)
    {
        Npc = npc;
        Quests = quests;
    }
}
