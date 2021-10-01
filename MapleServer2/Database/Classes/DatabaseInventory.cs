using Maple2Storage.Types;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseInventory : DatabaseTable
    {
        public DatabaseInventory() : base("inventories") { }

        public long Insert(Inventory inventory)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                extra_size = JsonConvert.SerializeObject(inventory.ExtraSize),
            });
        }

        public Inventory FindById(long id) => ReadInventory(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());

        public void Update(Inventory inventory)
        {
            QueryFactory.Query(TableName).Where("id", inventory.Id).Update(new
            {
                extra_size = JsonConvert.SerializeObject(inventory.ExtraSize),
            });

            List<Item> items = new List<Item>();
            items.AddRange(inventory.Items.Values.Where(x => x != null).ToList());
            items.AddRange(inventory.Equips.Values.Where(x => x != null).ToList());
            items.AddRange(inventory.Badges.Where(x => x != null).ToList());
            items.AddRange(inventory.Cosmetics.Values.Where(x => x != null).ToList());
            foreach (Item item in items)
            {
                item.InventoryId = inventory.Id;
                item.BankInventoryId = 0;
                item.HomeId = 0;
                DatabaseManager.Items.Update(item);
            }
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;

        private static Inventory ReadInventory(dynamic data) => new Inventory(data.id, JsonConvert.DeserializeObject<Dictionary<InventoryTab, short>>(data.extra_size), DatabaseManager.Items.FindAllByInventoryId(data.id));
    }
}
