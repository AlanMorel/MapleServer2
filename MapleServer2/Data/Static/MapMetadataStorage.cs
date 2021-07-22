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
            MapMetadata mapD = GetMetadata(mapId);
            if (mapD == null)
            {
                return false;
            }
            mapD.Blocks.TryGetValue(coord, out MapBlock block);
            return block != null;
        }

        public static bool BlockAboveExists(int mapId, CoordS coord)
        {
            MapMetadata mapD = GetMetadata(mapId);
            if (mapD == null)
            {
                return false;
            }
            coord.Z += Block.BLOCK_SIZE;
            mapD.Blocks.TryGetValue(coord, out MapBlock block);
            return block != null;
        }

        public static MapBlock GetMapBlock(int mapId, CoordS coord)
        {
            MapMetadata mapD = GetMetadata(mapId);
            if (mapD == null)
            {
                return null;
            }
            mapD.Blocks.TryGetValue(coord, out MapBlock block);
            return block;
        }

        public static int GetPlotNumber(int mapId, CoordB coord)
        {
            CoordS coordS = coord.ToShort();
            List<MapBlock> blocks = new List<MapBlock>();
            MapMetadata mapD = GetMetadata(mapId);
            for (int i = 0; i < 20; i++) // checking 20 blocks in the same Z axis
            {
                mapD.Blocks.TryGetValue(coordS, out MapBlock block);
                if (block == null)
                {
                    coordS.Z -= Block.BLOCK_SIZE;
                    continue;
                }

                if (block.SaleableGroup > 0)
                {
                    return block.SaleableGroup;
                }
                coordS.Z -= Block.BLOCK_SIZE;
            }
            return 0;
        }
    }
}
