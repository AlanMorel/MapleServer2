using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class MasteryUGCHousingMetadataStorage
    {
        private static readonly Dictionary<byte, MasteryUGCHousingMetadata> masteryMetadata = new Dictionary<byte, MasteryUGCHousingMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-mastery-ugc-housing-metadata");
            List<MasteryUGCHousingMetadata> masteryMetadatas = Serializer.Deserialize<List<MasteryUGCHousingMetadata>>(stream);
            foreach (MasteryUGCHousingMetadata metadata in masteryMetadatas)
            {
                masteryMetadata[metadata.Grade] = metadata;
            }
        }

        public static MasteryUGCHousingMetadata GetMetadata(byte grade)
        {
            masteryMetadata.TryGetValue(grade, out MasteryUGCHousingMetadata metadata);
            return metadata;
        }
    }
}
