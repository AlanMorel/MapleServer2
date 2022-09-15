using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseBankInventory : DatabaseTable
{
    public DatabaseBankInventory() : base("bank_inventories") { }

    public long Insert(BankInventory bankInventory)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            mesos = bankInventory.Mesos.Amount,
            extra_size = bankInventory.ExtraSize
        });
    }

    public BankInventory FindById(long id)
    {
        return ReadBankInventory(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());
    }

    public void Update(BankInventory bankInventory)
    {
        QueryFactory.Query(TableName).Where("id", bankInventory.Id).Update(new
        {
            mesos = bankInventory.Mesos.Amount,
            extra_size = bankInventory.ExtraSize
        });

        List<Item> items = new();
        items.AddRange(bankInventory.Items.Where(item => item is not null).ToList()!);
        foreach (Item item in items)
        {
            item.BankInventoryId = bankInventory.Id;
            item.InventoryId = 0;
            item.HomeId = 0;
            DatabaseManager.Items.Update(item);
        }
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static BankInventory ReadBankInventory(dynamic data)
    {
        return new BankInventory(data.id, data.extra_size, DatabaseManager.Items.FindAllByBankInventoryId(data.id), data.mesos);
    }
}
