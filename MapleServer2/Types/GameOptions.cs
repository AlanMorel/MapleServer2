using System.Collections.Generic;

namespace MapleServer2.Types
{
    public class GameOptions
    {
        public readonly long Id;
        public Dictionary<int, KeyBind> KeyBinds { get; set; }
        public List<Hotbar> Hotbars { get; private set; }
        public short ActiveHotbarId { get; private set; }

        public GameOptions() { }

        public void Initialize()
        {
            KeyBinds = new Dictionary<int, KeyBind>();
            Hotbars = new List<Hotbar>();

            // Have 3 hotbars available
            for (int i = 0; i < 3; i++)
            {
                Hotbar hotbar = new Hotbar();
                hotbar.Initialize();
                Hotbars.Add(hotbar);
            }
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
}
