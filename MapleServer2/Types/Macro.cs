using MapleServer2.Database;

namespace MapleServer2.Types;

public class Macro
{
    public long Id { get; set; }
    public long CharacterId { get; set; }
    public string Name;
    public long ShortcutKeyCode;
    public List<int> SkillIds;

    public Macro(long characterId, string name, long keyCode, List<int> skillIds)
    {
        CharacterId = characterId;
        Name = name;
        ShortcutKeyCode = keyCode;
        SkillIds = skillIds;
        Id = DatabaseManager.Macros.Insert(this);
    }

    public Macro(long id, long characterId, string name, long keyCode, List<int> skillIds)
    {
        Id = id;
        CharacterId = characterId;
        Name = name;
        ShortcutKeyCode = keyCode;
        SkillIds = skillIds;
    }
}
