using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class MeretMarketHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.MERET_MARKET;

    private enum MeretMarketMode : byte
    {
        LoadPersonalListings = 0xB,
        LoadSales = 0xC,
        ListItem = 0xD,
        RemoveListing = 0xE,
        UnlistItem = 0xF,
        RelistItem = 0x12,
        CollectProfit = 0x14,
        Initialize = 0x16,
        OpenShop = 0x1B,
        SendMarketRequest = 0x1D,
        Purchase = 0x1E,
        Home = 0x65,
        OpenDesignShop = 0x66,
        LoadCart = 0x6B
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        MeretMarketMode mode = (MeretMarketMode) packet.ReadByte();

        switch (mode)
        {
            case MeretMarketMode.LoadPersonalListings:
                HandleLoadPersonalListings(session);
                break;
            case MeretMarketMode.LoadSales:
                HandleLoadSales(session);
                break;
            case MeretMarketMode.ListItem:
                HandleListItem(session, packet);
                break;
            case MeretMarketMode.RemoveListing:
                HandleRemoveListing(session, packet);
                break;
            case MeretMarketMode.UnlistItem:
                HandleUnlistItem(session, packet);
                break;
            case MeretMarketMode.RelistItem:
                HandleRelistItem(session, packet);
                break;
            case MeretMarketMode.CollectProfit:
                HandleCollectProfit(session, packet);
                break;
            case MeretMarketMode.Initialize:
                HandleInitialize(session);
                break;
            case MeretMarketMode.OpenShop:
                HandleOpenShop(session, packet);
                break;
            case MeretMarketMode.Purchase:
                HandlePurchase(session, packet);
                break;
            case MeretMarketMode.Home:
                HandleHome(session);
                break;
            case MeretMarketMode.OpenDesignShop:
                HandleOpenDesignShop(session);
                break;
            case MeretMarketMode.LoadCart:
                HandleLoadCart(session);
                break;
            case MeretMarketMode.SendMarketRequest:
                HandleSendMarketRequest(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleLoadPersonalListings(GameSession session)
    {
        List<UGCMarketItem> items = GameServer.UGCMarketManager.GetItemsByCharacterId(session.Player.CharacterId);

        // TODO: Possibly a better way to implement updating item status?
        foreach (UGCMarketItem item in items)
        {
            if (item.ListingExpirationTimestamp < TimeInfo.Now() && item.Status == UGCMarketListingStatus.Active)
            {
                item.Status = UGCMarketListingStatus.Expired;
                DatabaseManager.UGCMarketItems.Update(item);
            }
        }
        session.Send(MeretMarketPacket.LoadPersonalListings(items));
    }

    private static void HandleLoadSales(GameSession session)
    {
        List<UGCMarketSale> sales = GameServer.UGCMarketManager.GetSalesByCharacterId(session.Player.CharacterId);
        session.Send(MeretMarketPacket.LoadSales(sales));
    }

    private static void HandleListItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        long salePrice = packet.ReadLong();
        bool promote = packet.ReadBool();
        List<string> tags = packet.ReadUnicodeString().Split(",").ToList();
        string description = packet.ReadUnicodeString();
        long listingFee = packet.ReadLong();

        // TODO: Check if item is a ugc block and not an item. Find item from their block inventory
        if (!session.Player.Inventory.Items.ContainsKey(itemUid))
        {
            return;
        }

        Item item = session.Player.Inventory.Items[itemUid];

        if (item.UGC is null || item.UGC.CharacterId != session.Player.CharacterId)
        {
            return;
        }

        if (salePrice < item.UGC.SalePrice || salePrice > long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopSellMaxPrice")))
        {
            return;
        }

        long totalFee = GetListingFee(session.Player.CharacterId, promote);
        if (!HandleMarketItemPay(session, totalFee, MeretMarketCurrencyType.Meret))
        {
            return;
        }

        UGCMarketItem marketItem = new UGCMarketItem(item, salePrice, session.Player, tags, description, promote);

        session.Send(MeretMarketPacket.ListItem(marketItem));
        session.Send(MeretMarketPacket.UpdateExpiration(marketItem));
    }

    private static long GetListingFee(long characterId, bool promote)
    {
        int activeListingsCount = GameServer.UGCMarketManager.GetItemsByCharacterId(characterId).Count;
        long baseFee = long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopBaseListFee"));
        long fee = baseFee + activeListingsCount * 100;

        // Max fee being 390
        fee = Math.Min(fee, baseFee + 200);
        if (promote)
        {
            fee += long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopAdFeeMerat"));
        }
        return fee;
    }

    private static void HandleRemoveListing(GameSession session, PacketReader packet)
    {
        packet.ReadInt(); // 0
        long ugcMarketItemId = packet.ReadLong();
        packet.ReadLong(); // duplicate id read?

        UGCMarketItem item = GameServer.UGCMarketManager.FindItemById(ugcMarketItemId);
        if (item is null || item.SellerCharacterId != session.Player.CharacterId)
        {
            return;
        }

        session.Send(MeretMarketPacket.RemoveListing(item.Id));
        DatabaseManager.UGCMarketItems.Delete(item.Id);
        GameServer.UGCMarketManager.RemoveListing(item);
    }

    private static void HandleUnlistItem(GameSession session, PacketReader packet)
    {
        packet.ReadInt(); // 0
        long ugcMarketItemId = packet.ReadLong();
        packet.ReadLong(); // duplicate id read?

        UGCMarketItem item = GameServer.UGCMarketManager.FindItemById(ugcMarketItemId);
        if (item is null || item.SellerCharacterId != session.Player.CharacterId)
        {
            return;
        }

        item.ListingExpirationTimestamp = long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopExpiredListingRemovalHour")) * 3600 + TimeInfo.Now();
        item.PromotionExpirationTimestamp = 0;
        item.Status = UGCMarketListingStatus.Expired;
        DatabaseManager.UGCMarketItems.Update(item);
        session.Send(MeretMarketPacket.UpdateExpiration(item));
    }

    private static void HandleRelistItem(GameSession session, PacketReader packet)
    {
        long ugcMarketItemId = packet.ReadLong();
        long price = packet.ReadLong();
        bool promote = packet.ReadBool();
        List<string> tags = packet.ReadUnicodeString().Split(",").ToList();
        string description = packet.ReadUnicodeString();
        long listingFee = packet.ReadLong();

        UGCMarketItem item = GameServer.UGCMarketManager.FindItemById(ugcMarketItemId);
        if (item is null || item.SellerCharacterId != session.Player.CharacterId || item.ListingExpirationTimestamp < TimeInfo.Now())
        {
            return;
        }

        long totalFee = GetListingFee(session.Player.CharacterId, promote);
        if (!HandleMarketItemPay(session, totalFee, MeretMarketCurrencyType.Meret))
        {
            return;
        }

        item.Price = price;
        item.ListingExpirationTimestamp = long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopSaleDay")) * 86400 + TimeInfo.Now();
        if (promote)
        {
            item.PromotionExpirationTimestamp = long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopAdHour")) * 3600 + item.ListingExpirationTimestamp;
        }
        item.Status = UGCMarketListingStatus.Active;
        item.Description = description;
        item.Tags = tags;
        DatabaseManager.UGCMarketItems.Update(item);
        session.Send(MeretMarketPacket.RelistItem(item));
    }

    private static void HandleCollectProfit(GameSession session, PacketReader packet)
    {
        long saleId = packet.ReadLong();

        List<UGCMarketSale> sales = GameServer.UGCMarketManager.GetSalesByCharacterId(session.Player.CharacterId);
        long profitDelayTime = long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopProfitDelayInDays"));
        long totalProfit = 0;
        foreach (UGCMarketSale sale in sales)
        {
            if (!(sale.SoldTimestamp + profitDelayTime < TimeInfo.Now()))
            {
                continue;
            }
            totalProfit += sale.Profit;
            GameServer.UGCMarketManager.RemoveSale(sale);
            DatabaseManager.UGCMarketSales.Delete(saleId);
        }

        session.Player.Account.GameMeret.Modify(totalProfit);
        session.Send(MeretsPacket.UpdateMerets(session.Player.Account, totalProfit));
        session.Send(MeretMarketPacket.UpdateProfit(saleId));
    }

    private static void HandleInitialize(GameSession session)
    {
        session.Send(MeretMarketPacket.Initialize());
    }

    private static void HandleOpenShop(GameSession session, PacketReader packet)
    {
        MeretMarketCategory category = (MeretMarketCategory) packet.ReadInt();

        MeretMarketCategoryMetadata metadata = MeretMarketCategoryMetadataStorage.GetMetadata((int) category);
        if (metadata is null)
        {
            return;
        }

        switch (metadata.Section)
        {
            case MeretMarketSection.PremiumMarket:
                HandleOpenPremiumMarket(session, category);
                break;
            case MeretMarketSection.RedMeretMarket:
                HandleOpenRedMeretMarket();
                break;
            case MeretMarketSection.UGCMarket:
                HandleOpenUGCMarket(session, packet, metadata);
                break;
        }
    }

    private static void HandleOpenPremiumMarket(GameSession session, MeretMarketCategory category)
    {
        List<MeretMarketItem> marketItems = DatabaseManager.MeretMarket.FindAllByCategoryId((category));
        if (marketItems is null)
        {
            return;
        }
        session.Send(MeretMarketPacket.LoadPremiumShopCategory(marketItems));
    }

    private static void HandleOpenUGCMarket(GameSession session, PacketReader packet, MeretMarketCategoryMetadata metadata)
    {
        GenderFlag gender = (GenderFlag) packet.ReadByte();
        JobFlag job = (JobFlag) packet.ReadInt();
        short sortBy = packet.ReadByte();

        List<UGCMarketItem> items = GameServer.UGCMarketManager.FindItemsByCategory(metadata.ItemCategories, gender, job, sortBy);
        session.Send(MeretMarketPacket.LoadUGCShopCategory(items));
    }

    private static void HandleOpenRedMeretMarket()
    {
        // TODO: Red Meret Market
    }

    private static void HandlePurchase(GameSession session, PacketReader packet)
    {
        byte quantity = packet.ReadByte();
        int marketItemId = packet.ReadInt();
        long ugcItemId = packet.ReadLong();
        if (ugcItemId != 0)
        {
            PurchaseUGCItem(session, ugcItemId);
            return;
        }

        PurchasePremiumItem(session, packet, marketItemId);
    }

    private static void PurchaseUGCItem(GameSession session, long ugcMarketItemId)
    {
        UGCMarketItem marketItem = GameServer.UGCMarketManager.FindItemById(ugcMarketItemId);
        if (marketItem is null || marketItem.ListingExpirationTimestamp < TimeInfo.Now())
        {
            return;
        }

        if (!HandleMarketItemPay(session, marketItem.Price, MeretMarketCurrencyType.Meret))
        {
            return;
        }

        marketItem.SalesCount++;
        DatabaseManager.UGCMarketItems.Update(marketItem);
        _ = new UGCMarketSale(marketItem.Price, marketItem.Item.UGC.Name, marketItem.SellerCharacterId);

        Item item = new(marketItem.Item)
        {
            CreationTime = TimeInfo.Now(),
        };
        item.Uid = DatabaseManager.Items.Insert(item);

        session.Player.Inventory.AddItem(session, item, true);
        session.Send(MeretMarketPacket.Purchase(0, marketItem.Id, marketItem.Price, 1));
    }

    private static void PurchasePremiumItem(GameSession session, PacketReader packet, int marketItemId)
    {
        packet.ReadInt();
        int childMarketItemId = packet.ReadInt();
        long unk2 = packet.ReadLong();
        int itemIndex = packet.ReadInt();
        int totalQuantity = packet.ReadInt();
        int unk3 = packet.ReadInt();
        byte unk4 = packet.ReadByte();
        string unk5 = packet.ReadUnicodeString();
        string unk6 = packet.ReadUnicodeString();
        long price = packet.ReadLong();

        MeretMarketItem marketItem = DatabaseManager.MeretMarket.FindById(marketItemId);
        if (marketItem is null)
        {
            return;
        }

        if (childMarketItemId != 0)
        {
            marketItem = marketItem.AdditionalQuantities.FirstOrDefault(x => x.MarketId == childMarketItemId);
            if (marketItem is null)
            {
                return;
            }
        }

        if (!HandleMarketItemPay(session, marketItem.Price, marketItem.TokenType))
        {
            return;
        }

        Item item = new(marketItem.ItemId)
        {
            Amount = marketItem.Quantity + marketItem.BonusQuantity,
            Rarity = marketItem.Rarity
        };
        if (marketItem.Duration != 0)
        {
            item.ExpiryTime = TimeInfo.Now() + Environment.TickCount + marketItem.Duration * 24 * 60 * 60;
        }
        session.Player.Inventory.AddItem(session, item, true);
        session.Send(MeretMarketPacket.Purchase(marketItem.MarketId, 0, marketItem.Price, totalQuantity, itemIndex));
    }

    private static bool HandleMarketItemPay(GameSession session, long price, MeretMarketCurrencyType currencyType)
    {
        return currencyType switch
        {
            MeretMarketCurrencyType.Meret => session.Player.Account.RemoveMerets(price),
            MeretMarketCurrencyType.Meso => session.Player.Wallet.Meso.Modify(price),
            _ => false,
        };
    }

    private static void HandleHome(GameSession session)
    {
        List<MeretMarketItem> marketItems = DatabaseManager.MeretMarket.FindAllByCategoryId(MeretMarketCategory.Promo);
        if (marketItems is null)
        {
            return;
        }
        session.Send(MeretMarketPacket.Promos(marketItems));
    }

    private static void HandleOpenDesignShop(GameSession session)
    {
        List<UGCMarketItem> promoItems = GameServer.UGCMarketManager.GetPromoItems();
        List<UGCMarketItem> newestItems = GameServer.UGCMarketManager.GetNewestItems();
        session.Send(MeretMarketPacket.OpenDesignShop(promoItems, newestItems));
    }

    private static void HandleLoadCart(GameSession session)
    {
        session.Send(MeretMarketPacket.LoadCart());
    }

    private static void HandleSendMarketRequest(GameSession session, PacketReader packet)
    {
        packet.ReadByte(); //constant 1
        int meretMarketItemUid = packet.ReadInt();
        List<MeretMarketItem> meretMarketItems = new()
        {
            DatabaseManager.MeretMarket.FindById(meretMarketItemUid)
        };
        session.Send(MeretMarketPacket.LoadPremiumShopCategory(meretMarketItems));
    }
}
