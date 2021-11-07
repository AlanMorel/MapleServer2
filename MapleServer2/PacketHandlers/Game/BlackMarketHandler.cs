using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

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
        PrepareListing = 0x8
    }

    private enum BlackMarketError : int
    {
        FailedToListItem = 0x05,
        ItemNotInInventory = 0x0E,
        ItemCannotBeListed = 0x20,
        OneMinuteRestriction = 0x25,
        Fatigue = 0x26,
        CannotUseBlackMarket = 0x27,
        QuantityNotAvailable = 0x29,
        CannotPurchaseOwnItems = 0x2A,
        RequiredLevelToList = 0x2B,
        RequiredLevelToBuy = 0x2C
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        BlackMarketMode mode = (BlackMarketMode) packet.ReadByte();

        switch (mode)
        {
            case BlackMarketMode.Open:
                HandleOpen(session);
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

    private static void HandleOpen(GameSession session)
    {
        List<BlackMarketListing> listings = GameServer.BlackMarketManager.GetListingsByCharacterId(session.Player.CharacterId);
        session.Send(BlackMarketPacket.Open(listings));
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

        if (!session.Player.Inventory.Items.ContainsKey(itemUid))
        {
            session.Send(BlackMarketPacket.Error((int) BlackMarketError.ItemNotInInventory));
            return;
        }

        double depositRate = 0.01; // 1% deposit rate
        int maxDeposit = 100000;

        int calculatedDeposit = (int) (depositRate * (price * quantity));
        int deposit = Math.Min(calculatedDeposit, maxDeposit);

        if (!session.Player.Wallet.Meso.Modify(-deposit))
        {
            return;
        }

        Item item = session.Player.Inventory.Items[itemUid];
        if (item.Amount < quantity)
        {
            return;
        }

        if (item.Amount > quantity)
        {
            item.TrySplit(quantity, out Item newStack);
            session.Send(ItemInventoryPacket.Update(item.Uid, item.Amount));
            item = newStack;
        }
        else
        {
            session.Player.Inventory.ConsumeItem(session, item.Uid, quantity);
        }

        BlackMarketListing listing = new(session.Player, item, quantity, price, deposit);
        session.Send(BlackMarketPacket.CreateListing(listing));
    }

    private static void HandleCancelListing(GameSession session, PacketReader packet)
    {
        long listingId = packet.ReadLong();

        BlackMarketListing listing = GameServer.BlackMarketManager.GetListingById(listingId);
        if (listing == null)
        {
            return;
        }

        if (listing.OwnerCharacterId != session.Player.CharacterId)
        {
            return;
        }

        DatabaseManager.BlackMarketListings.Delete(listingId);
        GameServer.BlackMarketManager.RemoveListing(listing);
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
        string name = packet.ReadUnicodeString();
        int startPage = packet.ReadInt();
        long sort = packet.ReadLong();
        packet.ReadShort();
        bool additionalOptionsEnabled = packet.ReadBool();

        List<ItemStat> stats = new List<ItemStat>();
        if (additionalOptionsEnabled)
        {
            packet.ReadByte(); // always 1
            for (int i = 0; i < 3; i++)
            {
                int statId = packet.ReadInt();
                int value = packet.ReadInt();
                if (value == 0)
                {
                    continue;
                }

                ItemStat stat = ReadStat(statId, value);
                if (stat == null)
                {
                    continue;
                }
                stats.Add(stat);
            }
        }

        List<string> itemCategories = BlackMarketTableMetadataStorage.GetItemCategories(minCategoryId, maxCategoryId);
        List<BlackMarketListing> searchResults = GameServer.BlackMarketManager.GetSearchedListings(itemCategories, minLevel, maxLevel, rarity, name, job,
            minEnchantLevel, maxEnchantLevel, minSockets, maxSockets, startPage, sort, additionalOptionsEnabled, stats);

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

        if (listing.OwnerAccountId == session.Player.AccountId)
        {
            session.Send(BlackMarketPacket.Error((int) BlackMarketError.CannotPurchaseOwnItems));
            return;
        }

        if (listing.Item.Amount < amount)
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
            string name = packet.ReadUnicodeString();
            int startPage = packet.ReadInt();
            long sort = packet.ReadLong();
            packet.ReadShort();
            bool additionalOptionsEnabled = packet.ReadBool();


            List<ItemStat> stats = new List<ItemStat>();
            if (additionalOptionsEnabled)
            {
                packet.ReadByte(); // always 1
                for (int i = 0; i < 3; i++)
                {
                    int statId = packet.ReadInt();
                    int value = packet.ReadInt();
                    if (value == 0)
                    {
                        continue;
                    }

                    ItemStat stat = ReadStat(statId, value);
                    if (stat == null)
                    {
                        continue;
                    }
                    stats.Add(stat);
                }
            }

            List<string> itemCategories = BlackMarketTableMetadataStorage.GetItemCategories(minCategoryId, maxCategoryId);
            List<BlackMarketListing> searchResults = GameServer.BlackMarketManager.GetSearchedListings(itemCategories, minLevel, maxLevel, rarity, name, job,
                minEnchantLevel, maxEnchantLevel, minSockets, maxSockets, startPage, sort, additionalOptionsEnabled, stats);

            session.Send(BlackMarketPacket.SearchResults(searchResults));
        }
    }

    private static ItemStat ReadStat(int statId, int value)
    {
        // Normal Stat with percent value
        if (statId >= 1000 && statId < 11000)
        {
            NormalStat normalStat = new NormalStat();
            float percent = (float) Math.Round(value * 0.0001f + 0.0005f, 4);
            normalStat.ItemAttribute = (ItemAttribute) statId - 1000;
            normalStat.Percent = percent;
            return normalStat;
        }
        // Special Stat with percent value
        else if (statId >= 11000)
        {
            SpecialStat specialStat = new SpecialStat();
            specialStat.ItemAttribute = (SpecialItemAttribute) statId - 11000;
            float percent = (float) Math.Round(value * 0.0001f + 0.0005f, 4);
            specialStat.Percent = percent;
            return specialStat;
        }
        // Normal Stat with flat value
        else
        {
            NormalStat normalStat = new NormalStat();
            normalStat.ItemAttribute = (ItemAttribute) statId;
            normalStat.Flat = value;
            return normalStat;
        }
    }
}
