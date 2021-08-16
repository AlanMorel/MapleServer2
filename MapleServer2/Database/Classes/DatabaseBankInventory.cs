using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseBankInventory
    {
        public static long CreateBankInventory(BankInventory bankInventory)
        {
            return DatabaseManager.QueryFactory.Query("BankInventories").InsertGetId<long>(new
            {
                bankInventory.ExtraSize,
            });
        }

        public static BankInventory FindById(long id) => ReadBankInventory(DatabaseManager.QueryFactory.Query("BankInventories").Where("Id", id).FirstOrDefault());

        public static void Update(BankInventory bankInventory)
        {
            DatabaseManager.QueryFactory.Query("BankInventories").Where("Id", bankInventory.Id).Update(new
            {
                bankInventory.ExtraSize,
            });

            List<Item> items = new List<Item>();
            items.AddRange(bankInventory.Items.Where(item => item != null).ToList());
            foreach (Item item in items)
            {
                item.BankInventoryId = bankInventory.Id;
                item.InventoryId = 0;
                item.HomeId = 0;
                DatabaseItem.Update(item);
            }
        }

        public static bool Delete(long id) => DatabaseManager.QueryFactory.Query("BankInventories").Where("Id", id).Delete() == 1;

        private static BankInventory ReadBankInventory(dynamic data) => new BankInventory(data.Id, data.ExtraSize, DatabaseItem.FindAllByBankInventoryId(data.Id));
    }
}
