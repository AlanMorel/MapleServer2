using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MapMetadataStorage
{
    private static readonly Dictionary<int, MapMetadata> map = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-map-metadata");
        List<MapMetadata> items = Serializer.Deserialize<List<MapMetadata>>(stream);
        foreach (MapMetadata item in items)
        {
            map[item.Id] = item;
        }
    }

    public static MapMetadata GetMetadata(int mapId) => map.GetValueOrDefault(mapId);

    public static List<MapMetadata> GetAll() => map.Values.ToList();

    public static bool BlockExists(int mapId, CoordS coord)
    {
        MapMetadata mapMetadata = GetMetadata(mapId);
        if (mapMetadata == null)
        {
            return false;
        }
        mapMetadata.Blocks.TryGetValue(coord, out MapBlock block);
        return block != null;
    }

    public static bool BlockAboveExists(int mapId, CoordS coord)
    {
        MapMetadata mapMetadata = GetMetadata(mapId);
        if (mapMetadata == null)
        {
            return false;
        }
        coord.Z += Block.BLOCK_SIZE;
        mapMetadata.Blocks.TryGetValue(coord, out MapBlock block);
        return block != null;
    }

    public static MapBlock GetMapBlock(int mapId, CoordS coord)
    {
        MapMetadata mapMetadata = GetMetadata(mapId);
        if (mapMetadata == null)
        {
            return null;
        }
        mapMetadata.Blocks.TryGetValue(coord, out MapBlock block);
        return block;
    }

    public static int GetPlotNumber(int mapId, CoordB coord)
    {
        CoordS coordS = coord.ToShort();
        MapMetadata mapMetadata = GetMetadata(mapId);
        for (int i = 0; i < 20; i++) // checking 20 blocks in the same Z axis
        {
            mapMetadata.Blocks.TryGetValue(coordS, out MapBlock block);
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
