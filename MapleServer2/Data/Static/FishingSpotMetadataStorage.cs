using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class FishingSpotMetadataStorage
    {
        private static readonly Dictionary<int, FishingSpotMetadata> map = new Dictionary<int, FishingSpotMetadata>();

        static FishingSpotMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-fishing-spot-metadata");
            List<FishingSpotMetadata> items = Serializer.Deserialize<List<FishingSpotMetadata>>(stream);
            foreach (FishingSpotMetadata item in items)
            {
                map[item.Id] = item;
            }
        }

        public static bool IsValid(int mapId)
        {
            return map.ContainsKey(mapId);
        }

        public static FishingSpotMetadata GetMetadata(int mapId)
        {
            return map.GetValueOrDefault(mapId);
        }

        public static bool CanFish(int mapId, long playerExp)
        {
            int minExpRequired = map.Values.FirstOrDefault(x => x.Id == mapId).MinMastery;
            if (playerExp < minExpRequired)
            {
                return false;
            }
            return true;
        }
    }
}
