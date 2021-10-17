using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class BlackMarketHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.BLACK_MARKET;

        public BlackMarketHandler() : base() { }

        private enum BlackMarketMode : byte
        {
            Open = 0x1,
            CreateListing = 0x2,
            CancelListing = 0x3,
            Search = 0x4,
            Purchase = 0x5,
            PrepareListing = 0x8,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            BlackMarketMode mode = (BlackMarketMode) packet.ReadByte();

            switch (mode)
            {
                case BlackMarketMode.Open:
                    HandleOpen(session, packet);
                    break;
                case BlackMarketMode.CreateListing:
                    HandleCreateListing(session, packet);
                    break;
                case BlackMarketMode.CancelListing:
                    HandleCancelListing(session, packet);
                    break;
                case BlackMarketMode.Search:
                    HandleSearch(session, packet);
                    break;
                case BlackMarketMode.Purchase:
                    HandlePurchase(session, packet);
                    break;
                case BlackMarketMode.PrepareListing:
                    HandlePrepareListing(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleOpen(GameSession session, PacketReader packet)
        {
            session.Send(BlackMarketPacket.Open(session.Player.BlackMarketListings));
        }

        private static void HandlePrepareListing(GameSession session, PacketReader packet)
        {
            int itemId = packet.ReadInt();
            int rarity = packet.ReadInt();

            if (!session.Player.Inventory.Items.Any(x => x.Value.Id == itemId && x.Value.Rarity == rarity))
            {
                return;
            }

            int npcShopPrice = 0;

            ShopItem shopItem = DatabaseManager.ShopItems.FindByItemId(itemId);
            if (shopItem != null)
            {
                npcShopPrice = shopItem.Price;
            }

            session.Send(BlackMarketPacket.PrepareListing(itemId, rarity, npcShopPrice));
        }

        private static void HandleCreateListing(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();
            long price = packet.ReadLong();
            int quantity = packet.ReadInt();

            if (session.Player.BlackMarketListings.Count >= 28) // Max amount of listings
            {
                return;
            }

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                return;
            }

            Item item = session.Player.Inventory.Items[itemUid];
            if (item.Amount < quantity)
            {
                return;
            }

            Item listingItem;
            listingItem = item;

            if (item.Amount > quantity)
            {
                InventoryController.Split(session, itemUid, quantity, out Item newStack);
                listingItem = newStack;
            }
            // TODO :Fix consuming correct item

            double depositRate = 0.01; // 1% deposit rate
            int maxDeposit = 100000;

            int calculatedDeposit = (int) (depositRate * (price * listingItem.Amount));
            int deposit = Math.Min(calculatedDeposit, maxDeposit);

            // User does not have enough mesos for deposit
            if (!session.Player.Wallet.Meso.Modify(-deposit))
            {
                return;
            }

            BlackMarketListing listing = new BlackMarketListing(session.Player, listingItem, listingItem.Amount, price, deposit);
            session.Player.BlackMarketListings.Add(listing);

            InventoryController.Consume(session, listingItem.Uid, quantity);
            session.Send(BlackMarketPacket.CreateListing(listing));
        }

        private static void HandleCancelListing(GameSession session, PacketReader packet)
        {
            long listingId = packet.ReadLong();

            BlackMarketListing listing = session.Player.BlackMarketListings.FirstOrDefault(x => x.Id == listingId);
            if (listing == null)
            {
                session.Send(BlackMarketPacket.CancelListing(listing, true));
                return;
            }

            session.Send(BlackMarketPacket.CancelListing(listing, false));
            MailHelper.BlackMarketCancellation(listing);
        }

        private static void HandleSearch(GameSession session, PacketReader packet)
        {
            int minCategoryId = packet.ReadInt();
            int maxCategoryId = packet.ReadInt();
            int minLevel = packet.ReadInt();
            int maxLevel = packet.ReadInt();
            JobFlag job = (JobFlag) packet.ReadInt();
            int rarity = packet.ReadInt();
            int minEnchantLevel = packet.ReadInt();
            int maxEnchantLevel = packet.ReadInt();
            byte minSockets = packet.ReadByte();
            byte maxSockets = packet.ReadByte();
            string name = packet.ReadUnicodeString().ToLower();
            int startPage = packet.ReadInt();
            packet.ReadLong(); // 21?
            packet.ReadShort();
            bool additionalOptionsEnabled = packet.ReadBool();
            // TODO: Figure out how additional options are read

            List<string> itemCategories = BlackMarketTableMetadataStorage.GetItemCategories(minCategoryId, maxCategoryId);
            List<BlackMarketListing> searchResults = GameServer.BlackMarketManager.GetSearchedListings(itemCategories, minLevel, maxLevel, rarity, name, job,
                minEnchantLevel, maxEnchantLevel, minSockets, maxSockets, startPage);

            session.Send(BlackMarketPacket.SearchResults(searchResults));
        }

        private static void HandlePurchase(GameSession session, PacketReader packet)
        {
            long listingId = packet.ReadLong();
            int amount = packet.ReadInt();

            BlackMarketListing listing = GameServer.BlackMarketManager.GetListingById(listingId);
            if (listing == null)
            {
                return;
            }

            if (listing.Item.Amount < amount)
            {
                return;
            }

            if (!session.Player.Wallet.Meso.Modify(-listing.Price * amount))
            {
                return;
            }

            Item purchasedItem;
            if (listing.Item.Amount == amount)
            {
                purchasedItem = listing.Item;
                GameServer.BlackMarketManager.RemoveListing(listing);
                DatabaseManager.BlackMarketListings.Delete(listing.Id);
            }
            else
            {
                InventoryController.Split(session, listing.Item.Uid, amount, out Item newStack);
                purchasedItem = newStack;
            }


            MailHelper.BlackMarketTransaction(purchasedItem, listing, session.Player.CharacterId, listing.Price);
            session.Send(BlackMarketPacket.Purchase(listingId, amount));
        }
    }
}
