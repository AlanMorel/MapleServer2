using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Managers;
using MapleServer2.Managers.Actors;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;
using Ubiety.Dns.Core;

namespace MapleServer2.PacketHandlers.Game;

public class NpcTalkHandler : GamePacketHandler<NpcTalkHandler>
{
    public override RecvOp OpCode => RecvOp.NpcTalk;

    private enum NpcTalkMode : byte
    {
        Close = 0,
        Begin = 1,
        Continue = 2,
        Enchant = 6,
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
            case NpcTalkMode.Begin:
                HandleBegin(session, packet);
                break;
            case NpcTalkMode.Continue:
                HandleContinue(session, packet.ReadInt());
                break;
            case NpcTalkMode.Enchant:
                HandleEnchant(session, packet);
                break;
            case NpcTalkMode.NextQuest:
                HandleNextQuest(session, packet);
                break;
            default:
                LogUnknownMode(function);
                break;
        }
    }

    private static void HandleBegin(GameSession session, PacketReader packet)
    {
        int objectId = packet.ReadInt();
        List<QuestStatus> npcQuests = new();
        int contentIndex = 0;

        // Find if npc object id exists in field manager
        if (!session.FieldManager.State.Npcs.TryGetValue(objectId, out Npc npc))
        {
            return;
        }

        // Get all quests for this npc
        foreach (QuestStatus item in session.Player.QuestData.Values.Where(x => x.State is not QuestState.Completed))
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

        ScriptMetadata scriptMetadata = ScriptMetadataStorage.GetNpcScriptMetadata(npc.Value.Id);
        NpcKind kind = npc.Value.NpcMetadataBasic.Kind;

        // need to find script properly before continuing
        NpcScript npcScript = GetFirstScript(session, scriptMetadata, npcTalk, npcQuests);

        switch (kind)
        {
            // reputation NPCs only have UI Dialog Type even if they have quests to accept/accepted.
            case NpcKind.SkyLumiknightCommander:
            case NpcKind.SkyGreenHoodCommander:
            case NpcKind.SkyDarkWindCommander:
            case NpcKind.SkyMapleAllianceCommander:
            case NpcKind.SkyRoyalGuardCommander:
            case NpcKind.KritiasLumiknightCommander:
            case NpcKind.KritiasGreenHoodCommander:
            case NpcKind.KritiasMapleAllianceCommander:
            case NpcKind.Humanitas:
                npcTalk.DialogType = DialogType.UI;
                ShopHandler.HandleOpen(session, npc, npc.Value.Id);
                break;
            case NpcKind.BalmyShop:
            case NpcKind.FixedShop:
            case NpcKind.RotatingShop:
                ShopHandler.HandleOpen(session, npc, npc.Value.Id);
                break;
            case NpcKind.Storage:
            case NpcKind.BlackMarket:
            case NpcKind.Birthday: // TODO: needs a special case? select if birthday. script if not
                npcTalk.DialogType = DialogType.UI;
                break;
        }

        npcTalk.ScriptId = npcScript?.Id ?? 0;
        ResponseSelection responseSelection = GetResponseSelection(kind, npcTalk.DialogType, contentIndex, npcScript);

        if (npcTalk.DialogType.HasFlag(DialogType.Quest))
        {
            session.Send(QuestPacket.SendDialogQuest(objectId, npcQuests));
        }
        QuestManager.OnTalkNpc(session.Player, npc.Value.Id, npcTalk.ScriptId);
        session.Send(NpcTalkPacket.Respond(npc, npcTalk.DialogType, contentIndex, responseSelection, npcTalk.ScriptId));

        if (npcScript != null)
        {
            npcTalk.TalkFunction(session, npcScript.Contents[npcTalk.ContentIndex].FunctionId, "preTalkActions");
        }
    }

    private static ResponseSelection GetResponseSelection(NpcKind npcKind, DialogType type, int contentIndex, NpcScript npcScript)
    {
        if (npcScript?.Contents[contentIndex]?.ButtonSet != 0 && npcScript != null)
        {
            return npcScript.Contents[contentIndex].ButtonSet;
        }

        if (type.HasFlag(DialogType.Options))
        {
            if (npcScript?.Contents.Count > 0)
            {
                return ResponseSelection.SelectableTalk;
            }
            return ResponseSelection.None;
        }

        if (type.HasFlag(DialogType.UI) && npcScript?.Type == ScriptType.Select)
        {
            return ResponseSelection.None;
        }

        if (npcScript?.Contents[contentIndex].Distractor.Count > 0)
        {
            return ResponseSelection.SelectableDistractor;
        }

        if (npcScript?.Contents.ElementAtOrDefault(contentIndex + 1) is not null)
        {
            return ResponseSelection.Next;
        }

        switch (npcScript?.Type)
        {
            case ScriptType.Job:
                switch (npcKind)
                {
                    case NpcKind.Travel:
                        return ResponseSelection.TakeBoat;
                    case NpcKind.BeautyDye:
                    case NpcKind.BeautyHair:
                    case NpcKind.BeautyFaceAndSkin:
                    case NpcKind.BeautyFaceDecoration:
                    case NpcKind.Mirror:
                        return ResponseSelection.Beauty;
                    case NpcKind.JobAdvancement:
                        return ResponseSelection.JobAdvance;
                    case NpcKind.Wheel:
                        return ResponseSelection.Roulette;
                }
                break;
            case ScriptType.Select:
                switch (npcKind)
                {
                    case NpcKind.Storage:
                    case NpcKind.FixedShop:
                    case NpcKind.BalmyShop:
                    case NpcKind.RotatingShop:
                        return ResponseSelection.None;

                }
                break;
        }

        if (type == DialogType.Quest)
        {
            ScriptIdType questScriptIdType = (ScriptIdType) (npcScript.Id / 100);
            return questScriptIdType switch
            {
                ScriptIdType.Start => ResponseSelection.QuestAccept,
                ScriptIdType.Continue => ResponseSelection.QuestProgress,
                ScriptIdType.End => ResponseSelection.QuestComplete,
                _ => ResponseSelection.Close
            };
        }

        return ResponseSelection.Close;
    }

    private static void HandleContinue(GameSession session, int selectedIndex)
    {
        NpcTalk npcTalk = session.Player.NpcTalk;
        NpcScript currentScript = npcTalk.GetCurrentScript();
        NpcScript nextScript = currentScript;
        NpcKind kind = npcTalk.Npc.NpcMetadataBasic.Kind;
        DialogType currentDialogType = npcTalk.DialogType;
        npcTalk.DialogType = DialogType.None;
        ResponseSelection responseSelection = ResponseSelection.None;
        ScriptMetadata metadata = ScriptMetadataStorage.GetNpcScriptMetadata(npcTalk.Npc.Id);

        npcTalk.TalkFunction(session, currentScript.Contents[npcTalk.ContentIndex].FunctionId, "postTalkActions");

        if (currentDialogType.HasFlag(DialogType.Options))
        {
            // if npc had a shop option
            if (currentDialogType.HasFlag(DialogType.UI))
            {
                if (selectedIndex == 0)
                {
                    // User selected to open UI
                    npcTalk.DialogType = DialogType.UI;
                    session.Send(NpcTalkPacket.ContinueChat(0, npcTalk.DialogType, responseSelection, npcTalk.ContentIndex));
                    return;
                }

                npcTalk.ContentIndex = 0;
                npcTalk.DialogType = DialogType.Talk;
                nextScript = GetBasicTalkScript(session, metadata);
                npcTalk.ScriptId = nextScript.Id;
                responseSelection = GetResponseSelection(kind, npcTalk.DialogType, npcTalk.ContentIndex, nextScript);
                session.Send(NpcTalkPacket.ContinueChat(npcTalk.ScriptId, npcTalk.DialogType, responseSelection, npcTalk.ContentIndex));
                return;
            }

            // if npc is had a quest
            if (currentDialogType.HasFlag(DialogType.Quest))
            {
                if (selectedIndex <= npcTalk.Quests.Count - 1)
                {
                    npcTalk.QuestId = npcTalk.Quests[selectedIndex].Basic.Id;
                    metadata = ScriptMetadataStorage.GetQuestScriptMetadata(npcTalk.QuestId);
                    npcTalk.DialogType |= DialogType.Quest;
                    nextScript = GetNextScript(session, metadata, npcTalk);
                    npcTalk.ScriptId = nextScript.Id;
                    responseSelection = GetResponseSelection(kind, npcTalk.DialogType, npcTalk.ContentIndex, nextScript);
                    session.Send(NpcTalkPacket.ContinueChat(npcTalk.ScriptId, npcTalk.DialogType, responseSelection, npcTalk.ContentIndex, npcTalk.QuestId));
                    return;
                }

                npcTalk.ContentIndex = 0;
                npcTalk.DialogType = DialogType.Talk;
                nextScript = GetBasicTalkScript(session, metadata);
                npcTalk.ScriptId = nextScript.Id;
                responseSelection = GetResponseSelection(kind, npcTalk.DialogType, npcTalk.ContentIndex, nextScript);
                session.Send(NpcTalkPacket.ContinueChat(npcTalk.ScriptId, npcTalk.DialogType, responseSelection, npcTalk.ContentIndex));
                return;
            }
        }
        else
        {
            if (currentScript?.Contents[npcTalk.ContentIndex].Distractor.Count > 0)
            {
                if (currentDialogType.HasFlag(DialogType.Quest))
                {
                    metadata = ScriptMetadataStorage.GetQuestScriptMetadata(npcTalk.QuestId);
                }

                // Handle GoTo
                nextScript = GoToScript(session, selectedIndex, currentScript, npcTalk, metadata);

                if (nextScript?.Type == ScriptType.Select)
                {
                    session.Send(NpcTalkPacket.Close());
                    return;
                }
                npcTalk.ScriptId = nextScript.Id;
                npcTalk.ContentIndex = 0;
            }
            else if (currentScript?.Contents.ElementAtOrDefault(npcTalk.ContentIndex + 1) is null)
            {
                session.Send(NpcTalkPacket.Close());
                return;
            }
            else
            {
                nextScript = currentScript;
                npcTalk.ContentIndex++;
            }
        }


        npcTalk.DialogType = GetDialogType(nextScript, npcTalk);
        responseSelection = GetResponseSelection(kind, npcTalk.DialogType, npcTalk.ContentIndex, nextScript);
        if (nextScript != null)
        {
            npcTalk.TalkFunction(session, nextScript.Contents[npcTalk.ContentIndex].FunctionId, "preTalkActions");
        }
        session.Send(NpcTalkPacket.ContinueChat(npcTalk.ScriptId, npcTalk.DialogType, responseSelection, npcTalk.ContentIndex, npcTalk.QuestId));
    }
    private static NpcScript GoToScript(GameSession session, int selectedIndex, NpcScript currentScript, NpcTalk npcTalk, ScriptMetadata metadata)
    {

        if (currentScript.Contents[npcTalk.ContentIndex].Distractor[selectedIndex].Goto.Count == 1 && currentScript.Contents[npcTalk.ContentIndex].Distractor[selectedIndex].GotoFail.Count == 0)
        {
            return metadata.NpcScripts.FirstOrDefault(x => x.Id == currentScript.Contents[npcTalk.ContentIndex].Distractor[selectedIndex].Goto.First());
        }
        if (currentScript.Contents[npcTalk.ContentIndex].Distractor[selectedIndex].Goto.Count != 0 && currentScript.Contents[npcTalk.ContentIndex].Distractor[selectedIndex].GotoFail.Count > 0)
        {
            Script luaScript = ScriptLoader.GetScript($"Npcs/{session.Player.NpcTalk.Npc.Id}", session);
            DynValue result = luaScript?.RunFunction("handleGoto", currentScript.Contents[npcTalk.ContentIndex].Distractor[selectedIndex].Goto[0]);
            if (result is not null && (int) result.Number != -1)
            {
                return metadata.NpcScripts.FirstOrDefault(x => x.Id == (int) result.Number);
            }
        }
        return metadata.NpcScripts.FirstOrDefault(x => x.Id == currentScript.Contents[npcTalk.ContentIndex].Distractor[selectedIndex].Goto[0]);
    }

    private static DialogType GetDialogType(NpcScript npcScript, NpcTalk npcTalk)
    {

        if (npcScript.Type == ScriptType.Job)
        {
            return DialogType.UI;
        }
        if (npcTalk.QuestId != 0)
        {
            return DialogType.Quest;
        }
        return DialogType.Talk;
    }

    private static void HandleEnchant(GameSession session, PacketReader packet)
    {
        int npcId = packet.ReadInt();
        int scriptId = packet.ReadInt();
        NpcTalkEventType eventType = (NpcTalkEventType) packet.ReadShort();
        long itemUid = 0;
        if (eventType is NpcTalkEventType.Begin)
        {
            itemUid = packet.ReadLong();
        }

        ScriptMetadata scriptMetadata = ScriptMetadataStorage.GetNpcScriptMetadata(npcId);
        NpcScript npcScript = scriptMetadata?.NpcScripts.FirstOrDefault(x => x.Id == scriptId);
        if (npcScript is null)
        {
            return;
        }

        ItemEnchant itemEnchant = session.Player.ItemEnchant;
        if (itemEnchant is null)
        {
            return;
        }

        Item item = session.Player.Inventory.GetFromInventoryOrEquipped(itemEnchant.ItemUid);
        if (item is null)
        {
            return;
        }

        Script script = ScriptLoader.GetScript($"Npcs/{npcId}", session);
        int eventId = 0;
        switch (eventType)
        {
            case NpcTalkEventType.Begin:
                eventId = (int) script.RunFunction("getBeginEventId", item.Rarity, item.EnchantLevel).Number;
                break;
            case NpcTalkEventType.InProgress:
                eventId = (int) script.RunFunction("getProcessEventId", EnchantHelper.PlayerHasIngredients(itemEnchant, session.Player.Inventory),
                    EnchantHelper.PlayerHasRequiredCatalysts(itemEnchant), itemEnchant.Rates.BaseSuccessRate + itemEnchant.Rates.CatalystTotalRate()).Number;
                break;
            case NpcTalkEventType.Result:
                eventId = (int) script.RunFunction("getResultEventId", item.EnchantLevel, itemEnchant.Success).Number;
                break;
            default:
                return;
        }

        if (eventId == 0)
        {
            return;
        }

        EnchantHelper.HandleNpcTalkEventType(session, npcScript, eventId);
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
                ? new()
                {
                    quest
                }
                : new()
                {
                    new(session.Player.CharacterId, questId)
                };
        }

        NpcTalk npcTalk = session.Player.NpcTalk;
        npcTalk.QuestId = questId;
        npcTalk.ScriptId = 0;
        npcTalk.ContentIndex = 0;
        ScriptMetadata metadata = ScriptMetadataStorage.GetQuestScriptMetadata(npcTalk.QuestId);
        DialogType dialogType = DialogType.Quest;
        NpcScript nextScript = GetNextScript(session, metadata, npcTalk);
        npcTalk.ScriptId = nextScript.Id;
        npcTalk.DialogType = dialogType;
        ResponseSelection responseSelection = GetResponseSelection(npcTalk.Npc.NpcMetadataBasic.Kind, npcTalk.DialogType, npcTalk.ContentIndex, nextScript);
        session.Send(NpcTalkPacket.ContinueChat(npcTalk.ScriptId, npcTalk.DialogType, responseSelection, npcTalk.ContentIndex, npcTalk.QuestId));
    }

    private static NpcScript GetNextScript(GameSession session, ScriptMetadata scriptMetadata, NpcTalk npcTalk, int index = 0)
    {
        if (npcTalk.DialogType.HasFlag(DialogType.Quest) && npcTalk.ScriptId == 0)
        {
            QuestStatus questStatus = npcTalk.Quests[index];
            if (questStatus.State is not QuestState.Started)
            {
                // Talking to npc that start the quest and isn't started
                return scriptMetadata.NpcScripts.FirstOrDefault(x => x.Id < 200 && x.JobId == (int) session.Player.Job) ??
                       scriptMetadata.NpcScripts.FirstOrDefault(x => x.Id == 100);
            }

            if (questStatus.State is QuestState.Started && questStatus.StartNpcId == npcTalk.Npc.Id)
            {
                // Talking to npc that start the quest and condition is not completed
                if (questStatus.Condition.Count != questStatus.Condition.Count(x => x.Completed) || questStatus.Condition.Count == 0)
                {
                    return scriptMetadata.NpcScripts.FirstOrDefault(x => x.Id is >= 200 and <= 299 && x.JobId == (int) session.Player.Job) ??
                           scriptMetadata.NpcScripts.FirstOrDefault(x => x.Id == 200);
                }
            }

            // Talking to npc that end the quest
            return scriptMetadata.NpcScripts.FirstOrDefault(x => x.Id >= 300 && x.JobId == (int) session.Player.Job) ??
                   scriptMetadata.NpcScripts.FirstOrDefault(x => x.Id == 300);
        }

        return GoToScript(session, index, npcTalk.GetCurrentScript(), npcTalk, scriptMetadata);
    }

    private static NpcScript GetFirstScript(GameSession session, ScriptMetadata scriptMetadata, NpcTalk npcTalk, List<QuestStatus> npcQuests)
    {
        // need to fix for quests
        if (scriptMetadata is null)
        {
            return null;
        }

        if (scriptMetadata.NpcScripts.Any(x => x.Type == ScriptType.Job))
        {
            Script luaScript = ScriptLoader.GetScript($"Npcs/{scriptMetadata.Id}", session);
            DynValue scriptResult = luaScript?.RunFunction("meetsJobScriptRequirement");
            if (scriptResult != null && scriptResult.Boolean)
            {
                npcTalk.DialogType = DialogType.UI;
                return scriptMetadata.NpcScripts.FirstOrDefault(x => x.Type == ScriptType.Job);
            }
        }

        NpcScript script = scriptMetadata?.NpcScripts.FirstOrDefault(x => x.Type == ScriptType.Select);

        // If npc has quests, send quests
        if (npcQuests.Count > 0)
        {
            npcTalk.DialogType |= DialogType.Quest;
            if (script is not null)
            {
                if (scriptMetadata.NpcScripts.Count(x => x.RandomPick) == 0)
                {
                    scriptMetadata = ScriptMetadataStorage.GetQuestScriptMetadata(npcQuests.First().Id);
                    return GetNextScript(session, scriptMetadata, npcTalk);
                }
                npcTalk.DialogType |= DialogType.Talk | DialogType.Options;
            }
            else
            {
                npcTalk.QuestId = npcQuests.First().Id;
                ScriptMetadata questScriptsMetadata = ScriptMetadataStorage.GetQuestScriptMetadata(npcTalk.QuestId);
                script = GetNextScript(session, questScriptsMetadata, npcTalk);
            }
            return script;
        }

        if (script is null)
        {
            return null;
        }

        NpcKind kind = npcTalk.Npc.NpcMetadataBasic.Kind;
        if (kind is NpcKind.RotatingShop or NpcKind.BalmyShop or NpcKind.FixedShop)
        {
            npcTalk.DialogType |= DialogType.UI;
        }

        int randomPickCount = scriptMetadata.NpcScripts.Count(x => x.RandomPick);
        if (randomPickCount > 0)
        {
            npcTalk.DialogType |= DialogType.Talk;
        }
        else
        {
            npcTalk.DialogType = DialogType.None;
            return script;
        }

        // give player option to choose ui or talk
        if (npcTalk.DialogType.HasFlag(DialogType.UI | DialogType.Talk))
        {
            npcTalk.DialogType |= DialogType.Options;
            return script;
        }

        return GetBasicTalkScript(session, scriptMetadata);
    }

    private static NpcScript GetBasicTalkScript(GameSession session, ScriptMetadata scriptMetadata)
    {
        int randomPickCount = scriptMetadata.NpcScripts.Count(x => x.RandomPick);
        switch (randomPickCount)
        {
            case 1:
                return scriptMetadata.NpcScripts.FirstOrDefault(x => x.RandomPick);
            default:
                Script luaScript = ScriptLoader.GetScript($"Npcs/{scriptMetadata.Id}", session);
                DynValue firstScriptResult = luaScript?.RunFunction("getFirstScriptId");
                if (firstScriptResult != null && (int) firstScriptResult.Number != -1)
                {
                    return scriptMetadata.NpcScripts.First(x => x.Id == (int) firstScriptResult.Number);
                }
                Logger.Warning($"Unhandled NPC {scriptMetadata.Id} with multiple possible talk scripts");
                return scriptMetadata.NpcScripts.FirstOrDefault(x => x.RandomPick);
        }
    }
}
