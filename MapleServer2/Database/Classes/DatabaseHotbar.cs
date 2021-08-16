using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHotbar
    {
        public static long CreateHotbar(Hotbar hotbar, long gameOptionsId)
        {
            return DatabaseManager.QueryFactory.Query("hotbars").InsertGetId<long>(new
            {
                Slots = JsonConvert.SerializeObject(hotbar.Slots),
                gameOptionsId
            });
        }

        public static List<Hotbar> FindAllByGameOptionsId(long gameOptionsId)
        {
            IEnumerable<dynamic> hotbarsResult = DatabaseManager.QueryFactory.Query("Hotbars").Where("GameOptionsId", gameOptionsId).Get();
            List<Hotbar> hotbars = new List<Hotbar>();
            foreach (dynamic item in hotbarsResult)
            {
                hotbars.Add(new Hotbar(JsonConvert.DeserializeObject<QuickSlot[]>(item.Slots), item.HotbarId));
            }
            return hotbars;
        }

        public static void Update(Hotbar hotbar)
        {
            DatabaseManager.QueryFactory.Query("Hotbars").Where("HotbarId", hotbar.Id).Update(new
            {
                Slots = JsonConvert.SerializeObject(hotbar.Slots)
            });
        }
    }
}
