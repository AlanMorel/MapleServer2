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
                game_options_id = gameOptionsId
            });
        }

        public List<Hotbar> FindAllByGameOptionsId(long gameOptionsId)
        {
            IEnumerable<dynamic> hotbarsResult = QueryFactory.Query(TableName).Where("game_options_id", gameOptionsId).Get();
            List<Hotbar> hotbars = new List<Hotbar>();
            foreach (dynamic data in hotbarsResult)
            {
                hotbars.Add(new Hotbar(JsonConvert.DeserializeObject<QuickSlot[]>(data.slots), data.hotbar_id));
            }
            return hotbars;
        }

        public void Update(Hotbar hotbar)
        {
            QueryFactory.Query(TableName).Where("hotbar_id", hotbar.Id).Update(new
            {
                slots = JsonConvert.SerializeObject(hotbar.Slots)
            });
        }

        public void DeleteAllByGameOptionsId(long gameOptionsId) => QueryFactory.Query(TableName).Where("gameoptions_id", gameOptionsId).Delete();
    }
}
