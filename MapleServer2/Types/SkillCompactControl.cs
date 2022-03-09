using MapleServer2.Database;

namespace MapleServer2.Types;

public class SkillCompactControl
{
    public long Id { get; set; }
    public long CharacterId { get; set; }
    public string Name;
    public long ShortcutKeyCode;
    public List<int> SkillIds;

    public SkillCompactControl(long characterId, string name, long keyCode, List<int> skillIds)
    {
        CharacterId = characterId;
        Name = name;
        ShortcutKeyCode = keyCode;
        SkillIds = skillIds;
        Id = DatabaseManager.SkillCompactControls.Insert(this);
    }

    public SkillCompactControl(long id, long characterId, string name, long keyCode, List<int> skillIds)
    {
        Id = id;
        CharacterId = characterId;
        Name = name;
        ShortcutKeyCode = keyCode;
        SkillIds = skillIds;
    }
}
