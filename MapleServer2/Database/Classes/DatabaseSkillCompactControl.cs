using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseSkillCompactControl : DatabaseTable
{
    public DatabaseSkillCompactControl() : base("skill_compact_controls") { }

    public long Insert(SkillCompactControl compactControl)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            character_id = compactControl.CharacterId,
            name = compactControl.Name,
            shortcut_keycode = compactControl.ShortcutKeyCode,
            skill_ids = JsonConvert.SerializeObject(compactControl.SkillIds),
        });
    }

    public void Update(SkillCompactControl compactControl)
    {
        QueryFactory.Query(TableName).Where("id", compactControl.Id).Update(new
        {
            name = compactControl.Name,
            shortcut_keycode = compactControl.ShortcutKeyCode,
            skill_ids = JsonConvert.SerializeObject(compactControl.SkillIds)
        });
    }

    public List<SkillCompactControl> FindAllByCharacterId(long characterId)
    {
        IEnumerable<dynamic> compactResults = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        List<SkillCompactControl> compactControls = new();
        foreach (dynamic data in compactResults)
        {
            compactControls.Add(ReadCompactControl(data));
        }

        return compactControls;
    }

    public bool Delete(long id)
    {
        DatabaseManager.Hotbars.DeleteAllByGameOptionsId(id);
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static SkillCompactControl ReadCompactControl(dynamic data)
    {
        return new(data.id, data.character_id, data.name, data.shortcut_keycode, JsonConvert.DeserializeObject<List<int>>(data.skill_ids));
    }
}
