using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            MapBlock block = mapD.Blocks.FirstOrDefault(x => x.Coord == coord);
            if (block == null)
            {
                return false;
            }
            return true;
        }

        public static bool BlockAboveExists(int mapId, CoordS coord)
        {
            MapMetadata mapD = GetMetadata(mapId);
            coord.Z += Block.BLOCK_SIZE;
            MapBlock block = mapD.Blocks.FirstOrDefault(x => x.Coord == coord);
            return block != null;
        }

        public static MapBlock GetMapBlock(int mapId, CoordS coord)
        {
            MapMetadata mapD = GetMetadata(mapId);
            return mapD.Blocks.FirstOrDefault(x => x.Coord == coord);
        }

        public static int GetPlotNumber(int mapId, CoordB coord)
        {
            CoordS coordS = coord.ToShort();
            List<MapBlock> blocks = new List<MapBlock>();
            MapMetadata mapD = GetMetadata(mapId);
            for (int i = 0; i < 20; i++) // checking 20 blocks in the same Z axis
            {
                MapBlock block = mapD.Blocks.FirstOrDefault(x => x.Coord == coordS);
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

        public static List<int> GetPlots(int mapId)
        {
            MapMetadata map = GetMetadata(mapId);
            return map.Blocks.Where(x => x.SaleableGroup != 0).GroupBy(x => x.SaleableGroup).Select(x => x.Key).ToList();
        }
    }
}
