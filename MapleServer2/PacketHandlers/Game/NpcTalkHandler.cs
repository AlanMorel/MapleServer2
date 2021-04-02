﻿using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class NpcTalkHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.NPC_TALK;

        public NpcTalkHandler(ILogger<NpcTalkHandler> logger) : base(logger) { }

        private enum NpcTalkMode : byte
        {
            Close = 0,
            Respond = 1,
            Continue = 2
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
                case NpcTalkMode.Close: // Cancel
                    return;
                case NpcTalkMode.Respond:
                    HandleRespond(session, packet);
                    break;
                case NpcTalkMode.Continue: // Continue chat?
                    HandleContinue(session, packet);
                    break;
            }
        }

        private static void HandleRespond(GameSession session, PacketReader packet)
        {
            List<QuestStatus> npcQuests = new List<QuestStatus>();
            int objectId = packet.ReadInt();
            if (!session.FieldManager.State.Npcs.TryGetValue(objectId, out IFieldObject<Npc> npc))
            {
                return; // Invalid NPC
            }
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

            foreach (QuestStatus item in session.Player.QuestList.Where(x => !x.Completed))
            {
                if (npc.Value.Id == item.StartNpcId)
                {
                    npcQuests.Add(item);
                }
                if (item.Started && npc.Value.Id == item.CompleteNpcId)
                {
                    if (!npcQuests.Contains(item))
                    {
                        npcQuests.Add(item);
                    }
                }
            }

            session.Player.NpcTalk = new NpcTalk(npc.Value, npcQuests);

            if (npcQuests.Count != 0)
            {
                session.Player.NpcTalk.ScriptId = 0;
                session.Send(QuestPacket.SendDialogQuest(objectId, npcQuests));
                session.Send(NpcTalkPacket.Respond(npc, NpcType.Unk2, DialogType.TalkOption, session.Player.NpcTalk.ScriptId));
            }
            else
            {
                ScriptMetadata scriptMetadata = ScriptMetadataStorage.GetNpcScriptMetadata(npc.Value.Id);
                int firstScript = scriptMetadata.Options.First(x => x.Type == ScriptType.Script).Id;
                session.Player.NpcTalk.ScriptId = firstScript;
                bool hasNextScript = scriptMetadata.Options.First(x => x.Id == firstScript).Goto.Count != 0;
                DialogType dialogType = DialogType.CloseNext1;
                if (scriptMetadata.Options.First(x => x.Id == firstScript).Goto.Count == 0)
                {
                    dialogType = DialogType.CloseNext;
                }

                if (!hasNextScript)
                {
                    dialogType = DialogType.Close1;
                }
                if (scriptMetadata.Options.First(x => x.Id == firstScript).AmountContent > 1)
                {
                    session.Player.NpcTalk.ContentIndex++;
                    dialogType = DialogType.CloseNext;
                }

                session.Send(NpcTalkPacket.Respond(npc, NpcType.Unk3, dialogType, firstScript));
            }
        }

        private static void HandleContinue(GameSession session, PacketReader packet)
        {
            NpcTalk npcTalk = session.Player.NpcTalk;
            if (npcTalk.Npc.IsBeauty())
            {
                HandleBeauty(session);
            }

            int index = packet.ReadInt(); // selection index

            // index is quest
            if (index <= npcTalk.Quests.Count - 1 && npcTalk.ScriptId == 0)
            {
                npcTalk.QuestId = npcTalk.Quests[index].Basic.Id;
                npcTalk.IsQuest = true;
            }

            ScriptMetadata scriptMetadata = npcTalk.IsQuest ? ScriptMetadataStorage.GetQuestScriptMetadata(npcTalk.QuestId) : ScriptMetadataStorage.GetNpcScriptMetadata(npcTalk.Npc.Id);
            ResponseType responseType = npcTalk.IsQuest ? ResponseType.Quest : ResponseType.Dialog;

            if (npcTalk.ScriptId != 0
                && scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId).AmountContent <= npcTalk.ContentIndex
                && scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId).Goto.Count == 0)
            {
                session.Send(NpcTalkPacket.Close());
                return;
            }

            // Find next script id
            int nextScript = GetNextScript(scriptMetadata, npcTalk, index);

            bool hasNextScript = scriptMetadata.Options.First(x => x.Id == nextScript).Goto.Count != 0;
            npcTalk.ContentIndex++;

            if (scriptMetadata.Options.First(x => x.Id == nextScript).AmountContent > npcTalk.ContentIndex)
            {
                hasNextScript = true;
            }
            else
            {
                if (npcTalk.ScriptId != nextScript)
                {
                    npcTalk.ContentIndex = 1;
                }
            }
            npcTalk.ScriptId = nextScript;

            DialogType dialogType = GetDialogType(scriptMetadata, npcTalk, hasNextScript);

            session.Send(NpcTalkPacket.ContinueChat(nextScript, responseType, dialogType, npcTalk.ContentIndex - 1, npcTalk.QuestId));
        }

        private static void HandleBeauty(GameSession session)
        {
            MapPortal portal = MapEntityStorage.GetPortals(session.Player.MapId).FirstOrDefault(portal => portal.Id == 99); // unsure how the portalId is determined
            session.Send(NpcTalkPacket.Action(ActionType.Portal, "", "", portal.Id));
            NpcMetadata npcTarget = NpcMetadataStorage.GetNpc(session.Player.NpcTalk.Npc.Id);

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
            session.Send(UserMoveByPortalPacket.Move(session, portal.Coord.ToFloat(), portal.Rotation.ToFloat()));
        }

        private static DialogType GetDialogType(ScriptMetadata scriptMetadata, NpcTalk npcTalk, bool hasNextScript)
        {
            DialogType dialogType = DialogType.CloseNext1;
            if (scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId).Goto.Count == 0)
            {
                dialogType = DialogType.CloseNext;
            }

            if (!hasNextScript)
            {
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

        private static int GetNextScript(ScriptMetadata scriptMetadata, NpcTalk npcTalk, int index)
        {
            if (npcTalk.IsQuest && npcTalk.ScriptId == 0)
            {
                if (npcTalk.Quests[index].Started)
                {
                    // Talking to npc that start the quest and is started
                    if (npcTalk.Quests[index].StartNpcId == npcTalk.Npc.Id)
                    {
                        return 200;
                    }
                    else // Talking to npc that end the quest
                    {
                        return 300;
                    }
                }
                else // Talking to npc that start the quest and isn't started
                {
                    return 100;
                }
            }
            else
            {
                if (npcTalk.ScriptId == 0)
                {
                    return scriptMetadata.Options.First(x => x.Id > npcTalk.ScriptId).Id;
                }
                else
                {
                    if (scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId).Goto.Count == 0)
                    {
                        return npcTalk.ScriptId;
                    }
                    else
                    {
                        return scriptMetadata.Options.First(x => x.Id == npcTalk.ScriptId).Goto[index];
                    }
                }
            }
        }
    }
}
