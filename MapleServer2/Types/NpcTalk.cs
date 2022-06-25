using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MoonSharp.Interpreter;
using Serilog.Core;

namespace MapleServer2.Types;

public class NpcTalk
{
    public int ScriptId;
    public int QuestId;
    public int ContentIndex;
    public readonly NpcMetadata Npc;
    public List<QuestStatus> Quests;
    public DialogType DialogType;

    public NpcTalk(NpcMetadata npc, List<QuestStatus> quests)
    {
        Npc = npc;
        Quests = quests;
    }

    public NpcScript GetCurrentScript()
    {
        if (DialogType == DialogType.Quest)
        {
            return ScriptMetadataStorage.GetQuestScriptMetadata(QuestId)?.NpcScripts.First(x => x.Id == ScriptId);
        }

        return ScriptMetadataStorage.GetNpcScriptMetadata(Npc.Id).NpcScripts.First(x => x.Id == ScriptId);
    }

    public void TalkFunction(GameSession session, int functionId, string function)
    {
        if (functionId == 0)
        {
            return;
        }

        List<ActionType> actions = new();
        Script npcScript = ScriptLoader.GetScript($"Npcs/{Npc.Id}");
        DynValue actionResults = npcScript?.RunFunction(function);
        if (actionResults == null)
        {
            return;
        }

        switch (actionResults.Type)
        {
            case DataType.Number:
                actions.Add((ActionType) actionResults.Number);
                break;
            case DataType.Tuple:
                foreach (DynValue value in actionResults.Tuple)
                {
                    actions.Add((ActionType) value.Number);
                }

                break;
            default:
                return;
        }

        MapPortal portal = new();
        foreach (ActionType action in actions)
        {
            switch (action)
            {
                case ActionType.OpenWindow:
                    DynValue windowResults = npcScript.RunFunction("actionWindow");
                    session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, windowResults.Tuple[0].String, windowResults.Tuple[1].String));
                    break;
                case ActionType.Portal:
                    DynValue portalResults = npcScript.RunFunction("actionPortal");
                    portal = MapEntityMetadataStorage.GetPortals(session.Player.MapId).FirstOrDefault(portal => portal.Id == portalResults.Number);
                    if (portal is null)
                    {
                        return;
                    }

                    session.Send(NpcTalkPacket.Action(ActionType.Portal, "", "", portal.Id));
                    break;
                case ActionType.ItemReward:
                    DynValue itemResults = npcScript.RunFunction("actionItemReward"); // TODO: Support > 1 item
                    Item item = new(id: (int) itemResults.Tuple[0].Number,
                        amount: (int) itemResults.Tuple[2].Number,
                        rarity: (int) itemResults.Tuple[1].Number);
                    session.Player.Inventory.AddItem(session, item, true);
                    session.Send(NpcTalkPacket.Action(action, "", "", 0, item));
                    break;
            }
        }

        // this needs to be sent after the UI window action
        if (actions.Contains(ActionType.Portal))
        {
            session.Player.Move(portal.Coord.ToFloat(), portal.Rotation.ToFloat());
        }
    }
}
