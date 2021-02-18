using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
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
            LoadProducts = 0x1,
            Buy = 0x4,
            Sell = 0x5,
            Close = 0x6
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ShopMode mode = (ShopMode) packet.ReadByte();

            switch (mode)
            {
                case ShopMode.Open:
                    HandleOpen(session, packet);
                    break;
                case ShopMode.Close:
                    HandleClose(session);
                    break;
                case ShopMode.Buy:
                    HandleBuy(session, packet);
                    break;
                case ShopMode.Sell:
                    HandleSell(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleOpen(GameSession session, PacketReader packet)
        {
            Console.WriteLine("Opening shop...");
            Console.WriteLine($"NPC: {packet.ReadInt()}");

            //Logger.LogInformation("Opening Shop ID %d;", packet);
            // List<NpcShopProduct> products = shopData.get(npc.getTemplate().getShopID());
            // user.sendPacket(FieldPacket.onSendShop(0, npc.getTemplateID(), null));
            // user.sendPacket(FieldPacket.onSendShop(1, npc.getTemplateID(), products == null ? new ArrayList<NpcShopProduct>() : products));
            // user.sendPacket(FieldPacket.onSendShop(6, npc.getTemplateID(), null));
            // user.sendPacket(FieldPacket.onSendNpcTalk(npc.getEntityID(), 1, 0, NpcTalkFlag.NONE));
        }

        private static void HandleClose(GameSession session)
        {
            Console.WriteLine("Closing Shop");
            session.Send(ShopPacket.Close());
        }

        private static void HandleLoadProducts(GameSession session, PacketReader packet)
        {
            // public static Packet LoadProducts(List<NpcShopProduct> products)
            // {
            //     PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            //     pWriter.WriteByte(products.size());
            //     foreach (NpcShopProduct product in products)
            //     {
            //         pWriter.Write(product);
            //     }
            // }
        }

        private static void HandleSell(GameSession session, PacketReader packet)
        {
            // sell to shop
            long itemUid = packet.ReadLong();
            int quantity = packet.ReadInt();

            session.Send(ShopPacket.Sell(itemUid, quantity));
        }

        private static void HandleBuy(GameSession session, PacketReader packet)
        {
            // buy from shop
            int itemId = packet.ReadInt();
            int quantity = packet.ReadInt();

            session.Send(ShopPacket.Buy(itemId, quantity));
        }
    }
}
