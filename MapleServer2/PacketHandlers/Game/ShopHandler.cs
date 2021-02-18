using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ShopHandler : GamePacketHandler
    {
        
        public override RecvOp OpCode => RecvOp.SHOP;
        
        public ShopHandler(ILogger<ShopHandler> logger) : base(logger) { }

        private enum ShopMode : byte
        {
            Open = 0x0,
            GetProducts = 0x1,
            Buy = 0x4,
            Sell = 0x5,
            Close = 0x6
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ShopMode mode = (ShopMode) packet.ReadByte();

            switch (mode)
            {
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleOpen(Player player, PacketReader packet) {
            //Logger.LogInformation("Opening Shop ID %d;", packet);
            // List<NpcShopProduct> products = shopData.get(npc.getTemplate().getShopID());
            // user.sendPacket(FieldPacket.onSendShop(0, npc.getTemplateID(), null));
            // user.sendPacket(FieldPacket.onSendShop(1, npc.getTemplateID(), products == null ? new ArrayList<NpcShopProduct>() : products));
            // user.sendPacket(FieldPacket.onSendShop(6, npc.getTemplateID(), null));
            // user.sendPacket(FieldPacket.onSendNpcTalk(npc.getEntityID(), 1, 0, NpcTalkFlag.NONE));
        }
    }
}
