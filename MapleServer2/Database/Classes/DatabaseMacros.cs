using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseMacros : DatabaseTable
{
    public DatabaseMacros() : base("macros") { }

    public long Insert(Macro macro)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            character_id = macro.CharacterId,
            name = macro.Name,
            shortcut_keycode = macro.ShortcutKeyCode,
            skill_ids = JsonConvert.SerializeObject(macro.SkillIds),
        });
    }

    public void Update(Macro macro)
    {
        QueryFactory.Query(TableName).Where("id", macro.Id).Update(new
        {
            name = macro.Name,
            shortcut_keycode = macro.ShortcutKeyCode,
            skill_ids = JsonConvert.SerializeObject(macro.SkillIds)
        });
    }

    public List<Macro> FindAllByCharacterId(long characterId)
    {
        IEnumerable<dynamic> macrosResults = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        List<Macro> macros = new();
        foreach (dynamic data in macrosResults)
        {
            macros.Add(ReadMacro(data));
        }

        return macros;
    }

    public bool Delete(long id)
    {
        DatabaseManager.Hotbars.DeleteAllByGameOptionsId(id);
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static Macro ReadMacro(dynamic data)
    {
        return new(data.id, data.character_id, data.name, data.shortcut_keycode, JsonConvert.DeserializeObject<List<int>>(data.skill_ids));
    }
}
