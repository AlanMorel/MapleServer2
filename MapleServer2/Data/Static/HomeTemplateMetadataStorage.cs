using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class HomeTemplateMetadataStorage
{
    private static readonly Dictionary<string, HomeTemplateMetadata> Templates = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.HomeTemplate}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<HomeTemplateMetadata> homeTemplates = Serializer.Deserialize<List<HomeTemplateMetadata>>(stream);
        foreach (HomeTemplateMetadata template in homeTemplates)
        {
            Templates[template.Id] = template;
        }
    }

    public static HomeTemplateMetadata GetTemplate(string id)
    {
        Templates.TryGetValue(id, out HomeTemplateMetadata metadata);
        return metadata;
    }
}
