using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseRouletteGameItem : DatabaseTable
{
    public DatabaseRouletteGameItem() : base("roulette_game_items") { }

    public List<RouletteGameItem> FindAllByRouletteId(long rouletteId)
    {
        List<RouletteGameItem> items = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("roulette_id", rouletteId).Get();
        foreach (dynamic data in results)
        {
            items.Add(ReadRouletteItem(data));
        }
        return items;
    }

    private static RouletteGameItem ReadRouletteItem(dynamic data)
    {
        return new RouletteGameItem(data)
        {
        };
    }
}
