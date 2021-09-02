using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class TrophyMetadataStorage
    {
        private static readonly Dictionary<int, TrophyMetadata> map = new Dictionary<int, TrophyMetadata>();

        static TrophyMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-trophy-metadata");
            List<TrophyMetadata> trophies = Serializer.Deserialize<List<TrophyMetadata>>(stream);
            foreach (TrophyMetadata trophy in trophies)
            {
                map[trophy.Id] = trophy;
            }
        }

        public static List<int> GetTrophyIds() => map.Keys.ToList();

        public static TrophyMetadata GetMetadata(int id) => map.GetValueOrDefault(id);
    }
}
