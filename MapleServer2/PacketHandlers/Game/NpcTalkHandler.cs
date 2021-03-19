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

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();
            switch (function)
            {
                case 0: // Cancel
                    return;
                case 1:
                    int objectId = packet.ReadInt();
                    if (!session.FieldManager.State.Npcs.TryGetValue(objectId, out IFieldObject<Npc> npc))
                    {
                        return; // Invalid NPC
                    }
                    session.Player.NpcTalk = npc;
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
                    // Stellar Chest: 11004215
                    session.Send(NpcTalkPacket.Respond(npc, NpcType.Unk2, DialogType.TalkOption, 0));
                    break;
                case 2: // Continue chat?
                    int index = packet.ReadInt(); // selection index

                    if (session.Player.NpcTalk.Value.IsBeauty()) // This may need a cleaner method
                    {
                        MapPortal portal = MapEntityStorage.GetPortals(session.Player.MapId).FirstOrDefault(portal => portal.Id == 99); // unsure how the portalId is determined
                        session.Send(NpcTalkPacket.Action(ActionType.Portal, "", "", portal.Id));
                        NpcMetadata npcTarget = NpcMetadataStorage.GetNpc(session.Player.NpcTalk.Value.Id);

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
                    session.Send(NpcTalkPacket.Close());
                    session.Player.NpcTalk = null;
                    break;
            }
        }
    }
}
