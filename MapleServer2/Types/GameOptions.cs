using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class GameOptions
{
    public readonly long Id;
    public Dictionary<int, KeyBind> KeyBinds { get; set; }
    public List<Hotbar> Hotbars { get; private set; }
    public short ActiveHotbarId { get; private set; }

    public GameOptions(JobCode jobCode)
    {
        KeyBinds = new();
        Id = DatabaseManager.GameOptions.Insert(this);

        Hotbars = new();

        // Have 3 hotbars available
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                Hotbars.Add(new(Id, jobCode));
                continue;
            }

            Hotbars.Add(new(Id));
        }
    }

    public GameOptions(Dictionary<int, KeyBind> keyBinds, List<Hotbar> hotbars, short activeHotbarId, long id)
    {
        KeyBinds = keyBinds;
        Hotbars = hotbars;
        ActiveHotbarId = activeHotbarId;
        Id = id;
    }

    public void SetKeyBind(ref KeyBind keyBind)
    {
        KeyBinds[keyBind.KeyCode] = keyBind;
    }

    // Hotbar related
    public void SetActiveHotbar(short hotbarId)
    {
        ActiveHotbarId = hotbarId;
    }

    public bool TryGetHotbar(short hotbarId, out Hotbar hotbar)
    {
        if (hotbarId < Hotbars.Count)
        {
            hotbar = Hotbars[hotbarId];
            return true;
        }

        hotbar = null;
        return false;
    }
}
