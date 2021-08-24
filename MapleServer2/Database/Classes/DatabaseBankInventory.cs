using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseBankInventory : DatabaseTable
    {
        public DatabaseBankInventory() : base("BankInventories") { }

        public long Insert(BankInventory bankInventory)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                bankInventory.ExtraSize,
            });
        }

        public BankInventory FindById(long id) => ReadBankInventory(QueryFactory.Query(TableName).Where("Id", id).FirstOrDefault());

        public void Update(BankInventory bankInventory)
        {
            QueryFactory.Query(TableName).Where("Id", bankInventory.Id).Update(new
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

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;

        private static BankInventory ReadBankInventory(dynamic data) => new BankInventory(data.Id, data.ExtraSize, DatabaseManager.Items.FindAllByBankInventoryId(data.Id));
    }
}
