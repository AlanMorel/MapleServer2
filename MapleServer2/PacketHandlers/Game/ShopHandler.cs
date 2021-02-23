using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
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

        public static void HandleOpen(GameSession session, IFieldObject<Npc> npcFieldObject)
        {
            NpcMetadata metadata = NpcMetadataStorage.GetNpc(npcFieldObject.Value.Id);
            
            List<NpcShopItem> products = new()
            {
                new NpcShopItem
                {
                    UniqueId = GuidGenerator.Int(),
                    ItemId = 20001697,
                    TokenType = 0x1,
                    Price = 10000,
                    SalePrice = 0,
                    ItemRank = 1,
                    Quantity = 100,
                    StockCount = 0,
                    StockPurchased = 0,
                    Category = "CoinEtc",
                    RequiredItemId = 30000447,
                    Flag = 0x1
                }
            };
            
            session.Send(ShopPacket.Open(metadata.TemplateId, metadata.ShopId, 43, "eventshop"));
            session.Send(ShopPacket.LoadProducts(session.Player, products));
            session.Send(ShopPacket.Reload());
            session.Send(NpcTalkPacket.Respond(npcFieldObject, NpcType.Default, DialogType.None, 0));
        }

        private static void HandleClose(GameSession session)
        {
            session.Send(ShopPacket.Close());
        }

        private static void HandleLoadProducts(GameSession session, List<NpcShopItem> products)
        {
            session.Send(ShopPacket.LoadProducts(session.Player, products));
        }

        private static void HandleSell(GameSession session, PacketReader packet)
        {
            // sell to shop
            long itemUid = packet.ReadLong();
            int quantity = packet.ReadInt();

            // get item
            if (session.Player.Inventory.Items.TryGetValue(itemUid, out Item item))
            {
                // get random selling price from price points
                Random rng = new();
                int[] pricePoints = ItemMetadataStorage.GetPricePoints(item.Id);
                if (pricePoints.Any())
                {
                    int rand = rng.Next(0, pricePoints.Length);
                    int price = pricePoints[rand];
                    session.Send(ShopPacket.Sell(itemUid, quantity, price));
                }
            }
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
