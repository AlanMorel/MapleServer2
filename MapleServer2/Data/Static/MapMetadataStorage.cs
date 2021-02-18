using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class MapMetadataStorage
    {
        private static readonly Dictionary<int, MapMetadata> map = new Dictionary<int, MapMetadata>();

        static MapMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-map-metadata");
            List<MapMetadata> items = Serializer.Deserialize<List<MapMetadata>>(stream);
            foreach (MapMetadata item in items)
            {
                map[item.Id] = item;
            }
        }

        public static MapMetadata GetMetadata(int mapId)
        {
            return map.GetValueOrDefault(mapId);
        }

        public static bool BlockExists(int mapId, CoordS coord)
        {
            return !map[mapId].Blocks.Find(x => x == coord).Equals(CoordS.From(0, 0, 0));
        }
    }
}
