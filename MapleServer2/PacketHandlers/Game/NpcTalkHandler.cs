using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

public class NpcTalkHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.NpcTalk;

    private enum NpcTalkMode : byte
    {
        Close = 0,
        Respond = 1,
        Continue = 2,
        NextQuest = 7
    }

    private enum ScriptIdType : byte
    {
        Start = 1,
        Continue = 2,
        End = 3
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        NpcTalkMode function = (NpcTalkMode) packet.ReadByte();
        switch (function)
        {
            case NpcTalkMode.Close:
                return;
            case NpcTalkMode.Respond:
                HandleRespond(session, packet);
                break;
            case NpcTalkMode.Continue:
                HandleContinue(session, packet.ReadInt());
                break;
            case NpcTalkMode.NextQuest:
                HandleNextQuest(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(function);
                break;
        }
    }

    private static void HandleRespond(GameSession session, PacketReader packet)
    {
        List<QuestStatus> npcQuests = new();
        int objectId = packet.ReadInt();

        // Find if npc object id exists in field manager
        if (!session.FieldManager.State.Npcs.TryGetValue(objectId, out IFieldActor<NpcMetadata> npc))
        {
            return;
        }

        // Get all quests for this npc
        foreach (QuestStatus item in session.Player.QuestData.Values.Where(x => x.State is not QuestState.Finished))
        {
            if (npc.Value.Id == item.StartNpcId)
            {
                npcQuests.Add(item);
            }

            if (item.State is QuestState.Started && npc.Value.Id == item.CompleteNpcId && !npcQuests.Contains(item))
            {
                npcQuests.Add(item);
            }
        }

        session.Player.NpcTalk = new(npc.Value, npcQuests);
        NpcTalk npcTalk = session.Player.NpcTalk;

        Script script = ScriptLoader.GetScript($"Npcs/{npc.Value.Id}", session);

        // If NPC is a shop, load and open the shop
        if (npc.Value.IsShop())
        {
            ShopHandler.HandleOpen(session, npc);
            return;
        }

        if (npc.Value.IsBank())
        {
            session.Send(HomeBank.OpenBank());
            return;
        }

        if (npc.Value.IsBeauty())
        {
            NpcMetadata npcTarget = NpcMetadataStorage.GetNpcMetadata(npcTalk.Npc.Id);
            if (npcTarget.ShopId == 507) // mirror
            {
                session.Send(NpcTalkPacket.Respond(npc, NpcType.Default, DialogType.Beauty, 0));
                HandleBeauty(session);
                return;
            }

            session.Send(NpcTalkPacket.Respond(npc, NpcType.Default, DialogType.Beauty, 1));
            return;
        }

        // Check if npc has an exploration quest
        QuestManager.OnTalkNpc(session.Player, npc.Value.Id, 0);

        // If npc has quests, send quests and talk option
        if (npcQuests.Count != 0)
        {
            // Check if npc has scripts available
            if (ScriptMetadataStorage.NpcHasScripts(npc.Value.Id))
            {
                npcTalk.ScriptId = 0;
                session.Send(QuestPacket.SendDialogQuest(objectId, npcQuests));
                session.Send(NpcTalkPacket.Respond(npc, NpcType.QuestOptions, DialogType.TalkOption, npcTalk.ScriptId));
                return;
            }

            // If npc has no scripts, send the quest script id
            npcTalk.IsQuest = true;
            npcTalk.QuestId = npcQuests.First().Id;

            ScriptMetadata questScript = ScriptMetadataStorage.GetQuestScriptMetadata(npcTalk.QuestId);
            npcTalk.ScriptId = GetNextScript(questScript, npcTalk, 0, script, session.Player);

            session.Send(QuestPacket.SendDialogQuest(objectId, npcQuests));
            session.Send(NpcTalkPacket.Respond(npc, NpcType.Quest, DialogType.CloseNext, npcTalk.ScriptId));
            return;
        }

        ScriptMetadata scriptMetadata = ScriptMetadataStorage.GetNpcScriptMetadata(npc.Value.Id);
        if (!scriptMetadata.Options.Exists(x => x.Type == ScriptType.Script))
        {
            return;
        }

        int firstScriptId = GetFirstScriptId(script, scriptMetadata);
        npcTalk.ScriptId = firstScriptId;

        Option option = scriptMetadata.Options.First(x => x.Id == firstScriptId);

        DialogType dialogType = option.Contents[0].Distractor is null ? DialogType.Close1 : DialogType.CloseNextWithDistractor;

        session.Send(NpcTalkPacket.Respond(npc, NpcType.NormalTalk, dialogType, firstScriptId));

        // If npc has buttonset roulette, send roulette id 13.
        // TODO: Send the correct roulette id
        if (scriptMetadata.Options.Any(x => x.ButtonSet == "roulette"))
        {
            session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "RouletteDialog", "13"));
        }
    }

    private static void HandleContinue(GameSession session, int index)
    {
        NpcTalk npcTalk = session.Player.NpcTalk;
        if (npcTalk.Npc.IsBeauty())
        {
            HandleBeauty(session);
            return;
        }

        Script script = ScriptLoader.GetScript($"Npcs/{npcTalk.Npc.Id}", session);

        // index is quest
        if (index <= npcTalk.Quests.Count - 1 && npcTalk.ScriptId == 0)
        {
            npcTalk.QuestId = npcTalk.Quests[index].Basic.Id;
            npcTalk.IsQuest = true;
        }

        ScriptMetadata scriptMetadata = npcTalk.IsQuest ? ScriptMetadataStorage.GetQuestScriptMetadata(npcTalk.QuestId) : ScriptMetadataStorage.GetNpcScriptMetadata(npcTalk.Npc.Id);
        ResponseType responseType = npcTalk.IsQuest ? ResponseType.Quest : ResponseType.Dialog;

        if (npcTalk.ScriptId != 0)
        {
            Option option = scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId);

            QuestManager.OnTalkNpc(session.Player, npcTalk.Npc.Id, npcTalk.ScriptId);

            // If npc has no more options, close dialog
            if (option.Contents.Count <= npcTalk.ContentIndex + 1 && option.Contents[npcTalk.ContentIndex].Distractor is null)
            {
                session.Send(NpcTalkPacket.Close());
                return;
            }
        }

        int nextScriptId = GetNextScript(scriptMetadata, npcTalk, index, script, session.Player);

        // If last script is different from next, reset content index, else increment content index
        if (npcTalk.ScriptId != nextScriptId)
        {
            npcTalk.ContentIndex = 0;
        }
        else
        {
            npcTalk.ContentIndex++;
        }

        Option nextScript = scriptMetadata.Options.FirstOrDefault(x => x.Id == nextScriptId);
        if (nextScript is null)
        {
            session.Send(NpcTalkPacket.Close());
            return;
        }

        bool hasNextScript = nextScript.Contents[npcTalk.ContentIndex].Distractor is not null;
        if (nextScript.Contents.Count > npcTalk.ContentIndex + 1)
        {
            hasNextScript = true;
        }

        npcTalk.ScriptId = nextScriptId;

        DialogType dialogType = GetDialogType(scriptMetadata, npcTalk, hasNextScript);

        session.Send(NpcTalkPacket.ContinueChat(nextScriptId, responseType, dialogType, npcTalk.ContentIndex, npcTalk.QuestId));
        // It appears if content has buttonset roulette, it's send again on every continue chat, unsure why since it doesn't break anything
    }

    private static void HandleNextQuest(GameSession session, PacketReader packet)
    {
        int questId = packet.ReadInt();
        short mode = packet.ReadShort();

        // Complete quest
        if (mode == 2)
        {
            session.Player.QuestData.TryGetValue(questId, out QuestStatus questStatus);
            questStatus?.Condition.ForEach(x => x.Completed = true);
        }
        else
        {
            session.Player.QuestData.TryGetValue(questId, out QuestStatus quest);
            session.Player.NpcTalk.Quests = quest is not null
                ? new() { quest }
                : new() { new(session.Player, QuestMetadataStorage.GetMetadata(questId)) };
        }

        session.Player.NpcTalk.ScriptId = 0;
        HandleContinue(session, 0);
    }

    private static void HandleBeauty(GameSession session)
    {
        MapPortal portal = MapEntityMetadataStorage.GetPortals(session.Player.MapId).FirstOrDefault(portal => portal.Id == 99); // unsure how the portalId is determined
        if (portal is null)
        {
            return;
        }

        session.Send(NpcTalkPacket.Action(ActionType.Portal, "", "", portal.Id));
        NpcMetadata npcTarget = NpcMetadataStorage.GetNpcMetadata(session.Player.NpcTalk.Npc.Id);
        session.Player.ShopId = npcTarget.ShopId;

        switch (npcTarget.ShopId)
        {
            case 500: // Dr Dixon
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "face")); // unsure how these strings are determined
                break;
            case 501: // Dr Zenko
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "skin"));
                break;
            case 504: // Rosetta
            case 509: //Lolly
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "hair,style"));
                break;
            case 505: // Ren
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "makeup"));
                break;
            case 506: // Douglas
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "itemcolor"));
                break;
            case 507: // Mirror
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "mirror"));
                break;
            case 508: // Paulie
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "hair,random"));
                break;
            case 510: // Mino
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "hair,styleSave"));
                break;
        }

        session.Send(UserMoveByPortalPacket.Move(session.Player.FieldPlayer, portal.Coord.ToFloat(), portal.Rotation.ToFloat()));
    }

    private static DialogType GetDialogType(ScriptMetadata scriptMetadata, NpcTalk npcTalk, bool hasNextScript)
    {
        Option option = scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId);
        Content content = option.Contents[npcTalk.ContentIndex];

        // If npc has buttonSet by xmls, use it
        if (content.ButtonSet != DialogType.None)
        {
            return content.ButtonSet;
        }

        if (!hasNextScript)
        {
            // Get the correct dialog type for the last quest content
            ScriptIdType type = (ScriptIdType) (npcTalk.ScriptId / 100);
            if (npcTalk.IsQuest)
            {
                return type switch
                {
                    ScriptIdType.Start => DialogType.AcceptDecline,
                    ScriptIdType.End => DialogType.QuestReward,
                    _ => DialogType.Close1
                };
            }

            return DialogType.Close1;
        }

        return content.Distractor is null ? DialogType.CloseNext : DialogType.CloseNextWithDistractor;
    }

    private static int GetNextScript(ScriptMetadata scriptMetadata, NpcTalk npcTalk, int index, Script script, Player player)
    {
        if (npcTalk.IsQuest && npcTalk.ScriptId == 0)
        {
            QuestStatus questStatus = npcTalk.Quests[index];
            if (questStatus.State is not QuestState.Started)
            {
                // Talking to npc that start the quest and isn't started
                return scriptMetadata.Options.FirstOrDefault(x => x.Id < 200 && x.JobId == (int) player.Job)?.Id ?? 100;
            }

            if (questStatus.StartNpcId == npcTalk.Npc.Id && questStatus.Condition.Count != questStatus.Condition.Count(x => x.Completed))
            {
                // Talking to npc that start the quest and condition is not completed
                return scriptMetadata.Options.FirstOrDefault(x => x.Id is >= 200 and <= 299 && x.JobId == (int) player.Job)?.Id ?? 200;
            }

            // Talking to npc that end the quest
            return scriptMetadata.Options.FirstOrDefault(x => x.Id >= 300 && x.JobId == (int) player.Job)?.Id ?? 300;
        }

        if (npcTalk.ScriptId == 0)
        {
            return GetFirstScriptId(script, scriptMetadata);
        }

        Option currentOption = scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId);
        Content content = currentOption.Contents[npcTalk.ContentIndex];
        if (content.Distractor is null || currentOption.Contents.Count > 1 && currentOption.Contents.Count > npcTalk.ContentIndex + 1)
        {
            return npcTalk.ScriptId;
        }

        // If content has any goto, use the lua scripts to check the requirements
        if (content.Distractor[index].Goto.Count > 0 && script is not null)
        {
            DynValue result = script.RunFunction("handleGoto", content.Distractor[index].Goto[0]);
            if (result is not null && (int) result.Number != -1)
            {
                return (int) result.Number;
            }
        }

        // If there is no script just return the selected index and first goto option
        return content.Distractor[index].Goto[0];
    }

    private static int GetFirstScriptId(Script script, ScriptMetadata scriptMetadata)
    {
        if (script is null)
        {
            return scriptMetadata.Options.First(x => x.Type == ScriptType.Script).Id;
        }

        // Usually hardcoded functions to get the first script id which
        // otherwise wouldn't be possible only with the xml data
        DynValue firstScriptResult = script.RunFunction("getFirstScriptId");
        if (firstScriptResult is not null && (int) firstScriptResult.Number != -1)
        {
            return (int) firstScriptResult.Number;
        }

        return scriptMetadata.Options.First(x => x.Type == ScriptType.Script).Id;
    }
}
