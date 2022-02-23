using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseMapleopoly : DatabaseTable
{
    public DatabaseMapleopoly() : base("mapleopoly_tiles") { }

    public List<MapleopolyTile> FindAllTiles()
    {
        List<MapleopolyTile> tiles = new();

        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Get();
        foreach (dynamic result in results)
        {
            tiles.Add(ReadMapleopolyTile(result));
        }
        return tiles.OrderBy(x => x.TilePosition).ToList();
    }

    public MapleopolyTile FindTileByPosition(int tilePosition)
    {
        return ReadMapleopolyTile(QueryFactory.Query(TableName).Where("tile_position", tilePosition).Get().FirstOrDefault());
    }

    private static MapleopolyTile ReadMapleopolyTile(dynamic data)
    {
        return new MapleopolyTile()
        {
            TilePosition = data.tile_position,
            ItemAmount = data.item_amount,
            ItemId = data.item_id,
            ItemRarity = data.item_rarity,
            TileParameter = data.tile_parameter,
            Type = (MapleopolyTileType) data.type
        };
    }
}
