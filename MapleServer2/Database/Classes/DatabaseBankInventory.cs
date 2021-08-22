using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseBankInventory
    {
        private readonly string TableName = "BankInventories";

        public long Insert(BankInventory bankInventory)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                bankInventory.ExtraSize,
            });
        }

        public BankInventory FindById(long id) => ReadBankInventory(DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).FirstOrDefault());

        public void Update(BankInventory bankInventory)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Id", bankInventory.Id).Update(new
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
                DatabaseManager.Items.Update(item);
            }
        }

        public bool Delete(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;

        private static BankInventory ReadBankInventory(dynamic data) => new BankInventory(data.Id, data.ExtraSize, DatabaseManager.Items.FindAllByBankInventoryId(data.Id));
    }
}
