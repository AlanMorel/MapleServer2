using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseMapleopoly : DatabaseTable
    {
        public DatabaseMapleopoly() : base("mapleopoly_tiles") { }

        public List<MapleopolyTile> FindAllTiles()
        {
            List<MapleopolyTile> tiles = new List<MapleopolyTile>();

            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Get();
            foreach (dynamic result in results)
            {
                tiles.Add(ReadMapleopolyTile(result));
            }
            return tiles.OrderBy(x => x.TilePosition).ToList();
        }

        public MapleopolyTile FindTileByPosition(int tilePosition) => ReadMapleopolyTile(QueryFactory.Query(TableName).Where("tile_position", tilePosition).Get().FirstOrDefault());

        private static MapleopolyTile ReadMapleopolyTile(dynamic data) => new MapleopolyTile(data.tile_position, data.item_amount, data.item_id, data.item_rarity, data.tile_parameter, data.type);
    }
}
