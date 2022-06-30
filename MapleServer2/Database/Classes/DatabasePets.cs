using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabasePets : DatabaseTable
{
    public DatabasePets() : base("pets") { }

    public long Insert(PetInfo petInfo)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            name = petInfo.Name,
            exp = petInfo.Exp,
            level = petInfo.Level,
            potion_settings = JsonConvert.SerializeObject(petInfo.PotionSettings),
            loot_settings = JsonConvert.SerializeObject(petInfo.LootSettings)
        });
    }

    public void Update(PetInfo petInfo)
    {
        QueryFactory.Query(TableName).Where("uid", petInfo.Uid).Update(new
        {
            name = petInfo.Name,
            exp = petInfo.Exp,
            level = petInfo.Level,
            potion_settings = JsonConvert.SerializeObject(petInfo.PotionSettings),
            loot_settings = JsonConvert.SerializeObject(petInfo.LootSettings)
        });
    }

    public PetInfo Get(long petUid)
    {
        dynamic result = QueryFactory.Query(TableName).Where("uid", petUid).FirstOrDefault();
        if (result == null)
        {
            return null;
        }

        return new(result.uid)
        {
            Name = result.name,
            Exp = result.exp,
            Level = result.level,
            PotionSettings = JsonConvert.DeserializeObject<PetPotionSettings>(result.potion_settings),
            LootSettings = JsonConvert.DeserializeObject<PetLootSettings>(result.loot_settings)
        };
    }
}
