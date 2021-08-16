using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGameOptions
    {
        public static long CreateGameOptions(GameOptions gameOptions)
        {
            return DatabaseManager.QueryFactory.Query("GameOptions").InsertGetId<long>(new
            {
                KeyBinds = JsonConvert.SerializeObject(gameOptions.KeyBinds),
                gameOptions.ActiveHotbarId
            });
        }

        public static void Update(GameOptions gameOptions)
        {
            DatabaseManager.QueryFactory.Query("GameOptions").Where("Id", gameOptions.Id).Update(new
            {
                KeyBinds = JsonConvert.SerializeObject(gameOptions.KeyBinds),
                gameOptions.ActiveHotbarId
            });
            foreach (Hotbar hotbar in gameOptions.Hotbars)
            {
                DatabaseHotbar.Update(hotbar);
            }
        }

        public static bool Delete(long id)
        {
            DatabaseManager.QueryFactory.Query("Hotbars").Where("GameOptionsId", id).Delete();
            return DatabaseManager.QueryFactory.Query("GameOptions").Where("Id", id).Delete() == 1;
        }
    }
}
