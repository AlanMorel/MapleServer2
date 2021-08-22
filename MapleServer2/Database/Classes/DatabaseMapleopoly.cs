using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseMapleopoly
    {
        private readonly string TableName = "mapleopolytiles";

        public List<MapleopolyTile> FindAllTiles() => DatabaseManager.QueryFactory.Query(TableName).Get<MapleopolyTile>().OrderBy(x => x.TilePosition).ToList();

        public MapleopolyTile FindTileByPosition(int tilePosition) => DatabaseManager.QueryFactory.Query(TableName).Where("TilePosition", tilePosition).Get<MapleopolyTile>().FirstOrDefault();
    }
}
