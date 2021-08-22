using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHotbar
    {
        private readonly string TableName = "hotbars";

        public long Insert(Hotbar hotbar, long gameOptionsId)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                Slots = JsonConvert.SerializeObject(hotbar.Slots),
                gameOptionsId
            });
        }

        public List<Hotbar> FindAllByGameOptionsId(long gameOptionsId)
        {
            IEnumerable<dynamic> hotbarsResult = DatabaseManager.QueryFactory.Query(TableName).Where("GameOptionsId", gameOptionsId).Get();
            List<Hotbar> hotbars = new List<Hotbar>();
            foreach (dynamic item in hotbarsResult)
            {
                hotbars.Add(new Hotbar(JsonConvert.DeserializeObject<QuickSlot[]>(item.Slots), item.HotbarId));
            }
            return hotbars;
        }

        public void Update(Hotbar hotbar)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("HotbarId", hotbar.Id).Update(new
            {
                Slots = JsonConvert.SerializeObject(hotbar.Slots)
            });
        }

        public void DeleteAllByGameOptionsId(long gameOptionsId) => DatabaseManager.QueryFactory.Query(TableName).Where("GameOptionsId", gameOptionsId).Delete();
    }
}
