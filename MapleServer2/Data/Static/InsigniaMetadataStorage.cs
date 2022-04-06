using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class InsigniaMetadataStorage
{
    private static readonly Dictionary<int, InsigniaMetadata> Insignias = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Insignia}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<InsigniaMetadata> items = Serializer.Deserialize<List<InsigniaMetadata>>(stream);
        foreach (InsigniaMetadata item in items)
        {
            Insignias[item.InsigniaId] = item;
        }
    }

    public static bool IsValid(int insigniaId)
    {
        return Insignias.ContainsKey(insigniaId);
    }

    public static InsigniaMetadata GetMetadata(int insigniaId)
    {
        return Insignias.GetValueOrDefault(insigniaId);
    }

    public static int GetTitleId(int insigniaId)
    {
        return Insignias.GetValueOrDefault(insigniaId).TitleId;
    }

    public static string GetConditionType(int insigniaId)
    {
        return Insignias.GetValueOrDefault(insigniaId).ConditionType;
    }
}
