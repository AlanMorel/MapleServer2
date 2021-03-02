using MaplePacketLib2.Tools;
using MapleServer2.Constants;
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
                    // Stellar Chest: 11004215
                    session.Send(NpcTalkPacket.Respond(npc, NpcType.Unk2, DialogType.TalkOption, 0));
                    break;
                case 2: // Continue chat?
                    int index = packet.ReadInt(); // selection index
                    session.Send(NpcTalkPacket.Close());
                    break;
            }
        }
    }
}
