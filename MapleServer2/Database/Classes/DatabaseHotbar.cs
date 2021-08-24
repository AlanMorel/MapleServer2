using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHotbar : DatabaseTable
    {
        public DatabaseHotbar() : base("Hotbars") { }

        public long Insert(Hotbar hotbar, long gameOptionsId)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                Slots = JsonConvert.SerializeObject(hotbar.Slots),
                gameOptionsId
            });
        }

        public List<Hotbar> FindAllByGameOptionsId(long gameOptionsId)
        {
            IEnumerable<dynamic> hotbarsResult = QueryFactory.Query(TableName).Where("GameOptionsId", gameOptionsId).Get();
            List<Hotbar> hotbars = new List<Hotbar>();
            foreach (dynamic item in hotbarsResult)
            {
                hotbars.Add(new Hotbar(JsonConvert.DeserializeObject<QuickSlot[]>(item.Slots), item.HotbarId));
            }
            return hotbars;
        }

        public void Update(Hotbar hotbar)
        {
            QueryFactory.Query(TableName).Where("HotbarId", hotbar.Id).Update(new
            {
                Slots = JsonConvert.SerializeObject(hotbar.Slots)
            });
        }

        public void DeleteAllByGameOptionsId(long gameOptionsId) => QueryFactory.Query(TableName).Where("GameOptionsId", gameOptionsId).Delete();
    }
}
