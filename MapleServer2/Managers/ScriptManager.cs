using Maple2Storage.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using MoonSharp.Interpreter;

// ReSharper disable UnusedMember.Global

namespace MapleServer2.Managers;

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

    public int GetItemCount(int itemId) => Player.Inventory.GetAllById(itemId).Sum(x => x.Amount);

    public bool CanHold(int itemId, int amount) => Player.Inventory.CanHold(itemId, amount);

    public int GetFreeSlots(byte inventoryTabId)
    {
        if (!Enum.TryParse(inventoryTabId.ToString(), out InventoryTab inventoryTab))
        {
            return -1;
        }

        return Player.Inventory.GetFreeSlots(inventoryTab);
    }

    public int GetJobId() => (int) Player.Job;

    public int GetCurrentMapId() => Player.MapId;

    public bool HasQuestStarted(int questId)
    {
        return Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) && questStatus.State == QuestState.Started;
    }

    public bool HasQuestStarted(List<int> questIds)
    {
        foreach (int questId in questIds)
        {
            return Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) && questStatus.State == QuestState.Started;
        }

        return false;
    }
}
