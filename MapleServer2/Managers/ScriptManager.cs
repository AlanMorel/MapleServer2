using Maple2Storage.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.Managers
{
    /// <summary>
    /// This class acts like an interface between the server and scripts
    /// https://www.moonsharp.org/proxy.html
    /// </summary>
    public class ScriptManager
    {
        private readonly GameSession Session;
        private Player Player => Session.Player;

        [MoonSharpHidden]
        public ScriptManager(GameSession session) => Session = session;

        public int GetItemCount(int itemId) => Player.Inventory.Items.Values.Where(x => x.Id == itemId).Sum(x => x.Amount);

        public bool CanHold(int itemId, int amount) => Player.Inventory.CanHold(itemId, amount);

        public int GetFreeSlots(byte inventoryTabId)
        {
            if (!Enum.TryParse(inventoryTabId.ToString(), out InventoryTab inventoryTab))
            {
                return -1;
            }
            return Player.Inventory.GetFreeSlots(inventoryTab);
        }

        public int GetPlayerJobId() => (int) Player.Job;

        public int GetCurrentMapId() => Player.MapId;

        public bool HasQuestStarted(int questId) => Player.QuestList.Any(x => x.Id == questId && x.Started == true && x.Completed == false);

        public bool HasQuestStarted(List<int> questIds) => Player.QuestList.Any(x => questIds.Contains(x.Id) && x.Started == true && x.Completed == false);
    }
}
