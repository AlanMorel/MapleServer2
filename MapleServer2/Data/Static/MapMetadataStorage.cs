using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MapMetadataStorage
{
    private static readonly Dictionary<int, MapMetadata> Maps = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Map);
        List<MapMetadata> items = Serializer.Deserialize<List<MapMetadata>>(stream);
        foreach (MapMetadata item in items)
        {
            Maps[item.Id] = item;
            MapEntityMetadataStorage.AddToStorage(item.Id, item.Entities);
        }
    }

    public static MapMetadata? GetMetadata(int mapId) => Maps.GetValueOrDefault(mapId);

    public static IEnumerable<MapMetadata> GetAll() => Maps.Values.ToList();

    public static bool BlockExists(int mapId, CoordS coord)
    {
        MapMetadata? mapMetadata = GetMetadata(mapId);
        if (mapMetadata is null)
        {
            return false;
        }

        mapMetadata.Blocks.TryGetValue(coord, out MapBlock? block);
        return block is not null;
    }

    public static bool BlockAboveExists(int mapId, CoordS coord)
    {
        MapMetadata? mapMetadata = GetMetadata(mapId);
        if (mapMetadata is null)
        {
            return false;
        }

        coord.Z += Block.BLOCK_SIZE;
        mapMetadata.Blocks.TryGetValue(coord, out MapBlock? block);
        return block is not null;
    }

    public static bool BlockBelowExists(int mapId, CoordS coord)
    {
        MapMetadata? mapMetadata = GetMetadata(mapId);
        if (mapMetadata is null)
        {
            return false;
        }

        coord.Z -= Block.BLOCK_SIZE;
        mapMetadata.Blocks.TryGetValue(coord, out MapBlock? block);
        return block is not null;
    }

    public static MapBlock? GetMapBlock(int mapId, CoordS coord)
    {
        MapMetadata? mapMetadata = GetMetadata(mapId);
        if (mapMetadata is null)
        {
            return null;
        }

        mapMetadata.Blocks.TryGetValue(coord, out MapBlock? block);
        return block;
    }

    public static int GetPlotNumber(int mapId, CoordB coord)
    {
        CoordS coordS = coord.ToShort();
        MapMetadata? mapMetadata = GetMetadata(mapId);
        if (mapMetadata is null)
        {
            return 0;
        }

        for (int i = 0; i < 20; i++) // checking 20 blocks in the same Z axis
        {
            mapMetadata.Blocks.TryGetValue(coordS, out MapBlock? block);

            if (block is null)
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

    public static bool IsLiquidBlock(int mapId, CoordS coord)
    {
        MapBlock? block = GetMapBlock(mapId, coord);
        return block is not null && IsLiquidBlock(block);
    }

    public static bool IsLiquidBlock(MapBlock block)
    {
        if (block.Type == "Ground")
        {
            return false;
        }

        return block.Attribute is "water" or "seawater" or "devilwater" or "lava" or "poison" or "oil" or "emeraldwater";
    }

    public static int GetDistanceToNextBlockBelow(int mapId, CoordS coord, out MapBlock? block)
    {
        CoordS tempCoord = coord;
        MapMetadata? mapMetadata = GetMetadata(mapId);
        if (mapMetadata is null)
        {
            block = null;
            return 99;
        }

        // checking 10 blocks in the same Z axis
        for (int i = 0; i < 10; i++)
        {
            mapMetadata.Blocks.TryGetValue(tempCoord, out block);
            if (block is not null)
            {
                return coord.Z - block.Coord.Z;
            }

            tempCoord.Z -= Block.BLOCK_SIZE;
        }

        block = null;
        return 99;
    }

    public static bool IsInstancedOnly(int mapId)
    {
        if (DungeonStorage.IsDungeonMap(mapId)) // lobbyIds are capacity > 0, yet are instanced only
        {
            return true;
        }

        switch (mapId)
        {
            case (int) Map.RosettaBeautySalon:
            case (int) Map.TriaPlasticSurgery:
            case (int) Map.DouglasDyeWorkshop:
                return true;
        }

        MapMetadata? mapMetadata = GetMetadata(mapId);
        if (mapMetadata is null)
        {
            return false;
        }

        return mapMetadata.Property.Capacity == 0;
    }

    public static bool IsTutorialMap(int mapId)
    {
        MapMetadata? mapMetadata = GetMetadata(mapId);
        return mapMetadata is not null && mapMetadata.Property.IsTutorialMap;
    }

    public static MapProperty? GetMapProperty(int mapId)
    {
        return Maps.GetValueOrDefault(mapId)?.Property;
    }

    public static MapUi? GetMapUi(int mapId)
    {
        return Maps.GetValueOrDefault(mapId)?.Ui;
    }

    public static MapCashCall? GetMapCashCall(int mapId)
    {
        return Maps.GetValueOrDefault(mapId)?.CashCall;
    }
}
