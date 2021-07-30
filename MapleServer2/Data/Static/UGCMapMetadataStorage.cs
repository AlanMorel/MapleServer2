using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class UGCMapMetadataStorage
    {
        private static readonly Dictionary<int, UGCMapMetadata> map = new Dictionary<int, UGCMapMetadata>();

        static UGCMapMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-ugc-map-metadata");
            List<UGCMapMetadata> items = Serializer.Deserialize<List<UGCMapMetadata>>(stream);
            foreach (UGCMapMetadata item in items)
            {
                map[item.MapId] = item;
            }
        }

        public static bool IsValid(int mapId)
        {
            return map.ContainsKey(mapId);
        }

        public static UGCMapGroup GetGroupMetadata(int mapId, byte groupId) => GetMetadata(mapId).Groups.FirstOrDefault(x => x.Id == groupId);

        public static UGCMapMetadata GetMetadata(int mapId) => map.GetValueOrDefault(mapId);

        public static int GetId(int exchangeId)
        {
            return map.GetValueOrDefault(exchangeId).MapId;
        }
    }
}
