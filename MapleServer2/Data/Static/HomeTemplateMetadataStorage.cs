using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class HomeTemplateMetadataStorage
{
    private static readonly Dictionary<string, HomeTemplateMetadata> Templates = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.HomeTemplate);
        List<HomeTemplateMetadata> homeTemplates = Serializer.Deserialize<List<HomeTemplateMetadata>>(stream);
        foreach (HomeTemplateMetadata template in homeTemplates)
        {
            Templates[template.Id] = template;
        }
    }

    public static HomeTemplateMetadata? GetTemplate(string id)
    {
        Templates.TryGetValue(id, out HomeTemplateMetadata? metadata);
        return metadata;
    }
}
