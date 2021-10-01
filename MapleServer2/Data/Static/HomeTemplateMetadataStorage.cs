using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class HomeTemplateMetadataStorage
    {
        private static readonly Dictionary<string, HomeTemplateMetadata> templates = new Dictionary<string, HomeTemplateMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-home-template-metadata");
            List<HomeTemplateMetadata> homeTemplates = Serializer.Deserialize<List<HomeTemplateMetadata>>(stream);
            foreach (HomeTemplateMetadata template in homeTemplates)
            {
                templates[template.Id] = template;
            }
        }

        public static HomeTemplateMetadata GetTemplate(string id)
        {
            templates.TryGetValue(id, out HomeTemplateMetadata metadata);
            return metadata;
        }
    }
}
