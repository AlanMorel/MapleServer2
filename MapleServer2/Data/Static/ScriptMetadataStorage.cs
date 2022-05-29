using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ScriptMetadataStorage
{
    private static readonly Dictionary<int, ScriptMetadata> QuestScripts = new();
    private static readonly Dictionary<int, ScriptMetadata> NpcScripts = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Script);
        List<ScriptMetadata> items = Serializer.Deserialize<List<ScriptMetadata>>(stream);
        foreach (ScriptMetadata item in items)
        {
            if (item.IsQuestScript)
            {
                QuestScripts[item.Id] = item;
            }
            else
            {
                NpcScripts[item.Id] = item;
            }
        }
    }

    public static ScriptMetadata GetQuestScriptMetadata(int questId)
    {
        return QuestScripts.GetValueOrDefault(questId);
    }

    public static ScriptMetadata GetNpcScriptMetadata(int npcId)
    {
        return NpcScripts.GetValueOrDefault(npcId);
    }

    public static bool NpcHasScripts(int npcId)
    {
        return NpcScripts.ContainsKey(npcId);
    }
}
