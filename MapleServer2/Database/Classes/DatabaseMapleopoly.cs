using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseMapleopoly
    {
        public static List<MapleopolyTile> GetMapleopolyTiles() => DatabaseManager.QueryFactory.Query("mapleopolytiles").Get<MapleopolyTile>().OrderBy(x => x.TilePosition).ToList();

        public static MapleopolyTile GetSingleMapleopolyTile(int tilePosition) => DatabaseManager.QueryFactory.Query("mapleopolytiles").Where("TilePosition", tilePosition).Get<MapleopolyTile>().FirstOrDefault();
    }
}
