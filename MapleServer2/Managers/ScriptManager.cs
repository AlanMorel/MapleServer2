using MapleServer2.Servers.Game;
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

        [MoonSharpHidden]
        public ScriptManager(GameSession session) => Session = session;

        public int GetItemCount(int itemId) => Session.Player.Inventory.Items.Values.Where(x => x.Id == itemId).Sum(x => x.Amount);

        public int GetPlayerJobId() => (int) Session.Player.Job;
        
        public int GetCurrentMapId() => Session.Player.MapId;

        public bool HasQuestStarted(List<int> questIds) => Session.Player.QuestList.Any(x => questIds.Contains(x.Id) && x.Started == true && x.Completed == false);
    }
}