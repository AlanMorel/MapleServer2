using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ScriptMetadataStorage
{
    private static readonly Dictionary<int, ScriptMetadata> QuestScripts = new();
    private static readonly Dictionary<int, ScriptMetadata> NpcScripts = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Script}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
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

    public static ScriptMetadata GetQuestScriptMetadata(int value)
    {
        return QuestScripts.GetValueOrDefault(value);
    }

    public static ScriptMetadata GetNpcScriptMetadata(int value)
    {
        return NpcScripts.GetValueOrDefault(value);
    }

    public static bool NpcHasScripts(int npcId)
    {
        return NpcScripts.ContainsKey(npcId);
    }
}
