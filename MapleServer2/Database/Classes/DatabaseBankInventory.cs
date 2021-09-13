using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseBankInventory : DatabaseTable
    {
        public DatabaseBankInventory() : base("bankinventories") { }

        public long Insert(BankInventory bankInventory)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                mesos = bankInventory.Mesos.Amount,
                bankInventory.ExtraSize,
            });
        }

        public BankInventory FindById(long id) => ReadBankInventory(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());

        public void Update(BankInventory bankInventory)
        {
            QueryFactory.Query(TableName).Where("id", bankInventory.Id).Update(new
            {
                mesos = bankInventory.Mesos.Amount,
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

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;

        private static BankInventory ReadBankInventory(dynamic data) => new BankInventory(data.id, data.extrasize, DatabaseManager.Items.FindAllByBankInventoryId(data.id), data.mesos);
    }
}
