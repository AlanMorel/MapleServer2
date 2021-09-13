using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game
{
    public class NpcTalkHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.NPC_TALK;

        public NpcTalkHandler() : base() { }

        private enum NpcTalkMode : byte
        {
            Close = 0,
            Respond = 1,
            Continue = 2,
            NextQuest = 7,
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
                    HandleContinue(session, index: packet.ReadInt());
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
            List<QuestStatus> npcQuests = new List<QuestStatus>();
            int objectId = packet.ReadInt();

            // Find if npc object id exists in field manager
            if (!session.FieldManager.State.Npcs.TryGetValue(objectId, out IFieldObject<Npc> npc))
            {
                return;
            }

            // Get all quests for this npc
            foreach (QuestStatus item in session.Player.QuestList.Where(x => !x.Completed))
            {
                if (npc.Value.Id == item.StartNpcId)
                {
                    npcQuests.Add(item);
                }
                if (item.Started && npc.Value.Id == item.CompleteNpcId && !npcQuests.Contains(item))
                {
                    npcQuests.Add(item);
                }
            }
            session.Player.NpcTalk = new NpcTalk(npc.Value, npcQuests);
            ScriptLoader scriptLoader = new ScriptLoader($"Npcs/{npc.Value.Id}", session);

            // If NPC is a shop, load and open the shop
            if (npc.Value.IsShop())
            {
                ShopHandler.HandleOpen(session, npc);
                return;
            }
            else if (npc.Value.IsBank())
            {
                session.Send(HomeBank.OpenBank());
                return;
            }
            else if (npc.Value.IsBeauty())
            {
                session.Send(NpcTalkPacket.Respond(npc, NpcType.Default, DialogType.Beauty, 1));
                return;
            }

            // Check if npc has an exploration quest
            QuestHelper.UpdateExplorationQuest(session, npc.Value.Id.ToString(), "talk_in");

            // If npc has quests, send quests and talk option
            if (npcQuests.Count != 0)
            {
                session.Player.NpcTalk.ScriptId = 0;
                session.Send(QuestPacket.SendDialogQuest(objectId, npcQuests));
                session.Send(NpcTalkPacket.Respond(npc, NpcType.Unk2, DialogType.TalkOption, session.Player.NpcTalk.ScriptId));
                return;
            }

            ScriptMetadata scriptMetadata = ScriptMetadataStorage.GetNpcScriptMetadata(npc.Value.Id);
            if (!scriptMetadata.Options.Exists(x => x.Type == ScriptType.Script))
            {
                return;
            }

            int firstScriptId = GetFirstScriptId(scriptLoader, scriptMetadata);
            session.Player.NpcTalk.ScriptId = firstScriptId;

            Option option = scriptMetadata.Options.First(x => x.Id == firstScriptId);

            DialogType dialogType = DialogType.None;
            if (option.Contents[0].Goto.Count == 0)
            {
                dialogType = DialogType.Close1;
            }
            else
            {
                dialogType = DialogType.CloseNextWithDistractor;
            }

            session.Send(NpcTalkPacket.Respond(npc, NpcType.Unk3, dialogType, firstScriptId));

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

            ScriptLoader scriptLoader = new ScriptLoader($"Npcs/{npcTalk.Npc.Id}", session);

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

                // Find if player has quest condition for type "talk_in" and option id
                QuestHelper.UpdateQuest(session, npcTalk.Npc.Id.ToString(), "talk_in", option.Id.ToString());

                // If npc has no more options, close dialog
                if (option.Contents.Count <= npcTalk.ContentIndex + 1 && option.Contents[npcTalk.ContentIndex].Goto.Count == 0)
                {
                    session.Send(NpcTalkPacket.Close());
                    return;
                }
            }

            int nextScriptId = GetNextScript(scriptMetadata, npcTalk, index, scriptLoader);

            // If last script is different from next, reset content index, else increment content index
            if (npcTalk.ScriptId != nextScriptId)
            {
                npcTalk.ContentIndex = 0;
            }
            else
            {
                npcTalk.ContentIndex++;
            }

            Option nextScript = scriptMetadata.Options.First(x => x.Id == nextScriptId);
            bool hasNextScript = nextScript.Contents[npcTalk.ContentIndex].Goto.Count != 0;
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
            session.Player.NpcTalk.Quests = new List<QuestStatus>() { new QuestStatus(session.Player, QuestMetadataStorage.GetMetadata(questId)) };
            session.Player.NpcTalk.ScriptId = 0;
            HandleContinue(session, index: 0);
        }

        private static void HandleBeauty(GameSession session)
        {
            MapPortal portal = MapEntityStorage.GetPortals(session.Player.MapId).FirstOrDefault(portal => portal.Id == 99); // unsure how the portalId is determined
            session.Send(NpcTalkPacket.Action(ActionType.Portal, "", "", portal.Id));
            NpcMetadata npcTarget = NpcMetadataStorage.GetNpc(session.Player.NpcTalk.Npc.Id);
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
            session.Send(UserMoveByPortalPacket.Move(session.FieldPlayer, portal.Coord.ToFloat(), portal.Rotation.ToFloat()));
        }

        private static DialogType GetDialogType(ScriptMetadata scriptMetadata, NpcTalk npcTalk, bool hasNextScript)
        {
            DialogType dialogType = DialogType.None;
            Option option = scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId);

            // If npc has buttonSet by xmls, use it
            if (option.Contents[npcTalk.ContentIndex].ButtonSet != DialogType.None)
            {
                return option.Contents[npcTalk.ContentIndex].ButtonSet;
            }

            if (option.Contents[npcTalk.ContentIndex].Goto.Count == 0)
            {
                dialogType = DialogType.CloseNext;
            }
            else
            {
                dialogType = DialogType.CloseNextWithDistractor;
            }

            if (!hasNextScript)
            {
                // Get the correct dialog type for the last quest content
                ScriptIdType type = (ScriptIdType) (npcTalk.ScriptId / 100);
                if (npcTalk.IsQuest)
                {
                    dialogType = type switch
                    {
                        ScriptIdType.Start => DialogType.AcceptDecline,
                        ScriptIdType.End => DialogType.QuestReward,
                        _ => DialogType.Close1,
                    };
                }
                else
                {
                    dialogType = DialogType.Close1;
                }
            }
            return dialogType;
        }

        private static int GetNextScript(ScriptMetadata scriptMetadata, NpcTalk npcTalk, int index, ScriptLoader scriptLoader)
        {
            if (npcTalk.IsQuest && npcTalk.ScriptId == 0)
            {
                QuestStatus questStatus = npcTalk.Quests[index];
                if (questStatus.Started)
                {
                    // Talking to npc that start the quest and condition is not completed
                    if (questStatus.StartNpcId == npcTalk.Npc.Id && questStatus.Condition.Count != questStatus.Condition.Count(x => x.Completed))
                    {
                        return 200;
                    }
                    return 300; // Talking to npc that end the quest
                }
                return 100; // Talking to npc that start the quest and isn't started
            }

            if (npcTalk.ScriptId == 0)
            {
                if (scriptLoader.Script != null)
                {
                    // Usually hardcoded functions to get the first script id which
                    // otherwise wouldn't be possible only with the xml data
                    DynValue result = scriptLoader.Call("getFirstScriptId");
                    if (result != null && result.Number != -1)
                    {
                        return (int) result.Number;
                    }
                }

                return scriptMetadata.Options.First(x => x.Id > npcTalk.ScriptId).Id;
            }

            Option currentOption = scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId);
            if (currentOption.Contents[npcTalk.ContentIndex].Goto.Count == 0
                || currentOption?.Contents.Count > 1 && currentOption?.Contents.Count > npcTalk.ContentIndex + 1)
            {
                return npcTalk.ScriptId;
            }

            // If content has goto fail, use the lua scripts to check the requirements
            if (currentOption.Contents[npcTalk.ContentIndex].GotoFail.Count > 0)
            {
                if (scriptLoader.Script != null)
                {
                    DynValue result = scriptLoader.Call("handleGotoFail", currentOption.Contents[npcTalk.ContentIndex].Goto[index]);
                    if (result == null)
                    {
                        return 0;
                    }

                    return (int) result.Number;
                }
            }

            // TODO: check for the requirements for goto

            return currentOption.Contents[npcTalk.ContentIndex].Goto[index];
        }

        private static int GetFirstScriptId(ScriptLoader scriptLoader, ScriptMetadata scriptMetadata)
        {
            if (scriptLoader.Script != null)
            {
                // Usually hardcoded functions to get the first script id which
                // otherwise wouldn't be possible only with the xml data
                DynValue result = scriptLoader.Call("getFirstScriptId");
                if (result != null && result.Number != -1)
                {
                    return (int) result.Number;
                }
            }

            return scriptMetadata.Options.First(x => x.Type == ScriptType.Script).Id;
        }
    }
}
