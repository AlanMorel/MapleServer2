using Maple2Storage.Types;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseInventory
    {
        public static long CreateInventory(Inventory inventory)
        {
            return DatabaseManager.QueryFactory.Query("inventories").InsertGetId<long>(new
            {
                ExtraSize = JsonConvert.SerializeObject(inventory.ExtraSize),
            });
        }

        public static Inventory FindById(long id) => ReadInventory(DatabaseManager.QueryFactory.Query("inventories").Where("Id", id).FirstOrDefault());

        public static void Update(Inventory inventory)
        {
            DatabaseManager.QueryFactory.Query("inventories").Where("Id", inventory.Id).Update(new
            {
                ExtraSize = JsonConvert.SerializeObject(inventory.ExtraSize),
            });

            List<Item> items = new List<Item>();
            items.AddRange(inventory.Items.Values.Where(x => x != null).ToList());
            items.AddRange(inventory.Equips.Values.Where(x => x != null).ToList());
            items.AddRange(inventory.Cosmetics.Values.Where(x => x != null).ToList());
            foreach (Item item in items)
            {
                item.InventoryId = inventory.Id;
                item.BankInventoryId = 0;
                item.HomeId = 0;
                DatabaseItem.Update(item);
            }
        }

        public static bool Delete(long id) => DatabaseManager.QueryFactory.Query("inventories").Where("Id", id).Delete() == 1;

        private static Inventory ReadInventory(dynamic data) => new Inventory(data.Id, JsonConvert.DeserializeObject<Dictionary<InventoryTab, short>>(data.ExtraSize), DatabaseItem.FindAllByInventoryId(data.Id));
    }
}
