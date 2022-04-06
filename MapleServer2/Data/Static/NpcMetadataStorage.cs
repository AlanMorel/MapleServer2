using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class NpcMetadataStorage
{
    private static readonly Dictionary<int, NpcMetadata> Npcs = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Npc}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<NpcMetadata> npcList = Serializer.Deserialize<List<NpcMetadata>>(stream);
        foreach (NpcMetadata npc in npcList)
        {
            Npcs.Add(npc.Id, npc);
        }
    }

    public static NpcMetadata GetNpcMetadata(int id) => Npcs.GetValueOrDefault(id);

    public static List<NpcMetadata> GetNpcsByMainTag(string mainTag) => Npcs.Values.Where(x => x.NpcMetadataBasic.MainTags.Contains(mainTag)).ToList();

    public static List<NpcMetadata> GetNpcsBySubTag(string subTag) => Npcs.Values.Where(x => x.NpcMetadataBasic.MainTags.Contains(subTag)).ToList();

    public static IEnumerable<NpcMetadata> GetAll() => Npcs.Values;
}
