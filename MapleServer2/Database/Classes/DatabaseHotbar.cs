using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHotbar : DatabaseTable
    {
        public DatabaseHotbar() : base("hotbars") { }

        public long Insert(Hotbar hotbar, long gameOptionsId)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                slots = JsonConvert.SerializeObject(hotbar.Slots),
                gameOptionsId
            });
        }

        public List<Hotbar> FindAllByGameOptionsId(long gameOptionsId)
        {
            IEnumerable<dynamic> hotbarsResult = QueryFactory.Query(TableName).Where("gameoptionsid", gameOptionsId).Get();
            List<Hotbar> hotbars = new List<Hotbar>();
            foreach (dynamic data in hotbarsResult)
            {
                hotbars.Add(new Hotbar(JsonConvert.DeserializeObject<QuickSlot[]>(data.slots), data.hotbarid));
            }
            return hotbars;
        }

        public void Update(Hotbar hotbar)
        {
            QueryFactory.Query(TableName).Where("hotbarid", hotbar.Id).Update(new
            {
                slots = JsonConvert.SerializeObject(hotbar.Slots)
            });
        }

        public void DeleteAllByGameOptionsId(long gameOptionsId) => QueryFactory.Query(TableName).Where("gameoptionsid", gameOptionsId).Delete();
    }
}
