using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseGameOptions : DatabaseTable
{
    public DatabaseGameOptions() : base("game_options") { }

    public long Insert(GameOptions gameOptions)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            keybinds = JsonConvert.SerializeObject(gameOptions.KeyBinds),
            active_hotbar_id = gameOptions.ActiveHotbarId
        });
    }

    public void Update(GameOptions gameOptions)
    {
        QueryFactory.Query(TableName).Where("id", gameOptions.Id).Update(new
        {
            keybinds = JsonConvert.SerializeObject(gameOptions.KeyBinds),
            active_hotbar_id = gameOptions.ActiveHotbarId
        });
        foreach (Hotbar hotbar in gameOptions.Hotbars)
        {
            DatabaseManager.Hotbars.Update(hotbar);
        }
    }

    public bool Delete(long id)
    {
        DatabaseManager.Hotbars.DeleteAllByGameOptionsId(id);
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }
}
