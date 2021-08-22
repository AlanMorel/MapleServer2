using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGameOptions : DatabaseTable
    {
        public DatabaseGameOptions() : base("GameOptions") { }

        public long Insert(GameOptions gameOptions)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                KeyBinds = JsonConvert.SerializeObject(gameOptions.KeyBinds),
                gameOptions.ActiveHotbarId
            });
        }

        public void Update(GameOptions gameOptions)
        {
            QueryFactory.Query(TableName).Where("Id", gameOptions.Id).Update(new
            {
                KeyBinds = JsonConvert.SerializeObject(gameOptions.KeyBinds),
                gameOptions.ActiveHotbarId
            });
            foreach (Hotbar hotbar in gameOptions.Hotbars)
            {
                DatabaseManager.Hotbars.Update(hotbar);
            }
        }

        public bool Delete(long id)
        {
            DatabaseManager.Hotbars.DeleteAllByGameOptionsId(id);
            return QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;
        }
    }
}
