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
using Currency = Maple2Storage.Types.Currency;

namespace MapleServer2.PacketHandlers.Game
{
    public class ShopHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.SHOP;

        public ShopHandler(ILogger<ShopHandler> logger) : base(logger) { }

        private enum ShopMode : byte
        {
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

            ShopMetadata shop = ShopMetadataStorage.GetShop(metadata.ShopId);
            if (shop == null)
            {
                return;
            }

            session.Send(ShopPacket.Open(shop));
            session.Send(ShopPacket.LoadProducts(shop.Items));
            session.Send(ShopPacket.Reload());
            session.Send(NpcTalkPacket.Respond(npcFieldObject, NpcType.Default, DialogType.None, 0));
        }

        private static void HandleClose(GameSession session)
        {
            session.Send(ShopPacket.Close());
        }

        private static void HandleSell(GameSession session, PacketReader packet)
        {
            // sell to shop
            long itemUid = packet.ReadLong();
            int quantity = packet.ReadInt();
            
            session.SendNotice("Selling is not yet implemented.");
            return;

            // get item
            // if (session.Player.Inventory.Items.TryGetValue(itemUid, out Item item))
            // {
            //     // get random selling price from price points
            //     Random rng = new();
            //     int[] pricePoints = ItemMetadataStorage.GetPricePoints(item.Id);
            //     if (pricePoints.Any())
            //     {
            //         int rand = rng.Next(0, pricePoints.Length);
            //         int price = pricePoints[rand];
            //         session.Send(ShopPacket.Sell(itemUid, quantity, price));
            //     }
            // }
        }

        private static void HandleBuy(GameSession session, PacketReader packet)
        {
            int itemUid = packet.ReadInt();
            int quantity = packet.ReadInt();
            
            ShopItem shopItem = ShopMetadataStorage.GetItem(itemUid);

            switch (shopItem.TokenType)
            {
                case Currency.Meso:
                    if (!session.Player.Wallet.Meso.Modify(-(shopItem.Price * quantity)))
                    {
                        session.SendNotice("You don't have enough mesos.");
                        return;
                    }

                    break;
                case Currency.Meret:
                    if (!session.Player.Wallet.Meret.Modify(-(shopItem.Price * quantity)))
                    {
                        session.SendNotice("You don't have enough merets.");
                        return;
                    }
                    
                    break;
            }

            // add item to inventory
            Item item = new(shopItem.ItemId)
            {
                Amount = quantity
            };
            InventoryController.Add(session, item, true);
            
            // complete purchase
            session.Send(ShopPacket.Buy(shopItem.ItemId, quantity, shopItem.Price, shopItem.TokenType));
        }
    }
}
