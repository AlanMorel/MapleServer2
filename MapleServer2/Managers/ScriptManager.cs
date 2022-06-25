using Maple2Storage.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using MoonSharp.Interpreter;

// ReSharper disable NotAccessedField.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace MapleServer2.Managers;

/// <summary>
/// This class acts like an interface between the server and scripts
/// https://www.moonsharp.org/proxy.html
/// </summary>
public class ScriptManager
{
    private readonly GameSession Session;
    private ScriptPlayer Player => new(Session.Player);

    [MoonSharpHidden]
    public ScriptManager(GameSession session) => Session = session;

    public ScriptPlayer GetPlayer() => Player;

    public class ScriptPlayer
    {
        private readonly Player Player;
        private Wallet Wallet => Player.Wallet;
        private Account Account => Player.Account;

        // Attributes need to be public so LUA scripts can access it.
        public int JobId => (int) Player.Job;
        public int MapId => Player.MapId;
        public int Level => Player.Levels.Level;
        public long MesoAmount => Wallet.Meso.Amount;
        public long RueAmount => Wallet.Rue.Amount;
        public long TrevaAmount => Wallet.Treva.Amount;
        public long HaviFruitAmount => Wallet.HaviFruit.Amount;
        public long ValorTokenAmount => Wallet.ValorToken.Amount;
        public long MeretAmount => Account.Meret.Amount;
        public long GameMeretAmount => Account.GameMeret.Amount;
        public long EventMeretAmount => Account.EventMeret.Amount;
        public long MesoTokenAmount => Account.MesoToken.Amount;

        public ScriptPlayer(Player player) => Player = player;

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

        public bool HasQuestStarted(int questId)
        {
            return Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) && questStatus.State is QuestState.Started;
        }

        public bool HasQuestCompleted(int questId)
        {
            return Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) && questStatus.State is QuestState.Completed;
        }

        public bool HasAnyQuestStarted(List<int> questIds)
        {
            foreach (int questId in questIds)
            {
                if (Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) && questStatus.State is QuestState.Started)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasAnyQuestCompleted(List<int> questIds)
        {
            foreach (int questId in questIds)
            {
                if (Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) && questStatus.State is QuestState.Completed)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasSufficientMesos(long amount) => Wallet.Meso.Amount >= amount;
        public bool ModifyMeso(long amount) => Wallet.Meso.Modify(amount);
        public bool ModifyRue(long amount) => Wallet.Rue.Modify(amount);
        public bool ModifyTreva(long amount) => Wallet.Treva.Modify(amount);
        public bool ModifyHaviFruit(long amount) => Wallet.HaviFruit.Modify(amount);
        public bool ModifyValorToken(long amount) => Wallet.ValorToken.Modify(amount);
        public bool ModifyMeret(long amount) => Account.Meret.Modify(amount);
        public bool ModifyGameMeret(long amount) => Account.GameMeret.Modify(amount);
        public bool ModifyEventMeret(long amount) => Account.EventMeret.Modify(amount);
        public bool ModifyMesoToken(long amount) => Account.MesoToken.Modify(amount);
    }
}
