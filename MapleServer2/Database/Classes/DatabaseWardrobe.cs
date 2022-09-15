using Maple2Storage.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseWardrobe : DatabaseTable
{
    public DatabaseWardrobe() : base("wardrobes") { }

    public void Insert(Wardrobe wardrobe, long characterId)
    {
        List<long> equipUids = wardrobe.Equips.Values.Select(item => item.Uid).ToList();

        QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            character_id = characterId,
            type = wardrobe.Type,
            name = wardrobe.Name,
            index = wardrobe.Index,
            shortcut_keycode = wardrobe.Key,
            equip_uids = JsonConvert.SerializeObject(equipUids)
        });
    }

    public void Update(Wardrobe wardrobe, long characterId)
    {
        List<long> equipUids = wardrobe.Equips.Values.Select(item => item.Uid).ToList();

        QueryFactory.Query(TableName).Where(new
        {
            character_id = characterId,
            index = wardrobe.Index
        }).Update(new
        {
            type = wardrobe.Type,
            name = wardrobe.Name,
            shortcut_keycode = wardrobe.Key,
            equip_uids = JsonConvert.SerializeObject(equipUids)
        });
    }

    public List<Wardrobe> FindAllByCharacterId(long characterId)
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        List<Wardrobe> wardrobes = new();
        foreach (dynamic entry in result)
        {
            wardrobes.Add(ReadWardrobe(entry));
        }

        return wardrobes;
    }

    private static Wardrobe ReadWardrobe(dynamic entry)
    {
        Dictionary<ItemSlot, Item> equips = new();
        List<long> equipUids = JsonConvert.DeserializeObject<List<long>>(entry.equip_uids);
        foreach (long equipUid in equipUids)
        {
            Item? equip = DatabaseManager.Items.FindByUid(equipUid);
            if (equip is null)
            {
                continue;
            }

            equips[equip.ItemSlot] = new(equip);
        }

        return new()
        {
            Type = entry.type,
            Key = entry.shortcut_keycode,
            Index = entry.index,
            Name = entry.name,
            Equips = equips
        };
    }
}
