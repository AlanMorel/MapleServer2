using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

public class MeretMarketHandler : GamePacketHandler<MeretMarketHandler>
{
    public override RecvOp OpCode => RecvOp.MeretMarket;

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
        OpenFeatured = 0x65,
        OpenDesignShop = 0x66,
        Search = 0x68,
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
            case MeretMarketMode.OpenFeatured:
                HandleOpenFeatured(session, packet);
                break;
            case MeretMarketMode.OpenDesignShop:
                HandleOpenDesignShop(session);
                break;
            case MeretMarketMode.Search:
                HandleSearch(session, packet);
                break;
            case MeretMarketMode.LoadCart:
                HandleLoadCart(session);
                break;
            case MeretMarketMode.SendMarketRequest:
                HandleSendMarketRequest(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleLoadPersonalListings(GameSession session)
    {
        session.Player.GetMeretMarketPersonalListings();
    }

    private static void HandleLoadSales(GameSession session)
    {
        session.Player.GetMeretMarketSales();
    }

    private static void HandleListItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        long salePrice = packet.ReadLong();
        bool promote = packet.ReadBool();
        List<string> tags = packet.ReadUnicodeString().Split(",").ToList();
        string description = packet.ReadUnicodeString();
        long listingFee = packet.ReadLong();

        Item item = null;

        if (session.Player.Inventory.HasItem(itemUid))
        {
            item = session.Player.Inventory.GetByUid(itemUid);
        }
        else if (session.Player.Account.Home.WarehouseInventory.ContainsKey(itemUid))
        {
            item = session.Player.Account.Home.WarehouseInventory[itemUid];
        }

        if (item is null)
        {
            return;
        }

        if (item.Ugc is null || item.Ugc.CharacterId != session.Player.CharacterId)
        {
            return;
        }

        if (salePrice < item.Ugc.SalePrice || salePrice > long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopSellMaxPrice")))
        {
            return;
        }

        long totalFee = GetListingFee(session.Player.CharacterId, promote);
        if (!HandleMarketItemPay(session, totalFee, MeretMarketCurrencyType.Meret))
        {
            return;
        }

        UgcMarketItem marketItem = new(item, salePrice, session.Player, tags, description, promote);

        session.Send(MeretMarketPacket.ListItem(marketItem));
        session.Send(MeretMarketPacket.UpdateExpiration(marketItem));
    }

    private static long GetListingFee(long characterId, bool promote)
    {
        int activeListingsCount = GameServer.UgcMarketManager.GetItemsByCharacterId(characterId).Count;
        ScriptLoader scriptLoader = new("Functions/calcMeretMarketRegisterFee");
        DynValue feeDynValue = scriptLoader.Call("calcMeretMarketRegisterFee", activeListingsCount);
        long fee = (long) feeDynValue.Number;

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

        UgcMarketItem item = GameServer.UgcMarketManager.FindItemById(ugcMarketItemId);
        if (item is null || item.SellerCharacterId != session.Player.CharacterId)
        {
            return;
        }

        session.Send(MeretMarketPacket.RemoveListing(item.MarketId));
        DatabaseManager.UgcMarketItems.Delete(item.MarketId);
        GameServer.UgcMarketManager.RemoveListing(item);
    }

    private static void HandleUnlistItem(GameSession session, PacketReader packet)
    {
        packet.ReadInt(); // 0
        long ugcMarketItemId = packet.ReadLong();
        packet.ReadLong(); // duplicate id read?

        UgcMarketItem item = GameServer.UgcMarketManager.FindItemById(ugcMarketItemId);
        if (item is null || item.SellerCharacterId != session.Player.CharacterId)
        {
            return;
        }

        item.ListingExpirationTimestamp = long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopExpiredListingRemovalHour")) * 3600 + TimeInfo.Now();
        item.PromotionExpirationTimestamp = 0;
        item.Status = UgcMarketListingStatus.Expired;
        DatabaseManager.UgcMarketItems.Update(item);
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

        UgcMarketItem item = GameServer.UgcMarketManager.FindItemById(ugcMarketItemId);
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
        item.Status = UgcMarketListingStatus.Active;
        item.Description = description;
        item.Tags = tags;
        DatabaseManager.UgcMarketItems.Update(item);
        session.Send(MeretMarketPacket.RelistItem(item));
    }

    private static void HandleCollectProfit(GameSession session, PacketReader packet)
    {
        long saleId = packet.ReadLong();

        List<UgcMarketSale> sales = GameServer.UgcMarketManager.GetSalesByCharacterId(session.Player.CharacterId);
        long profitDelayTime = long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopProfitDelayInDays"));
        long totalProfit = 0;
        foreach (UgcMarketSale sale in sales)
        {
            if (!(sale.SoldTimestamp + profitDelayTime < TimeInfo.Now()))
            {
                continue;
            }
            totalProfit += sale.Profit;
            GameServer.UgcMarketManager.RemoveSale(sale);
            DatabaseManager.UgcMarketSales.Delete(saleId);
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
        GenderFlag gender = (GenderFlag) packet.ReadByte();
        JobFlag job = (JobFlag) packet.ReadInt();
        MeretMarketSort sortBy = (MeretMarketSort) packet.ReadByte();
        string searchString = packet.ReadUnicodeString();
        int startPage = packet.ReadInt();
        packet.ReadInt(); // repeat page?
        MeretMarketSection section = ReadMarketSection(packet.ReadByte());

        packet.ReadByte();
        byte itemsPerPage = packet.ReadByte();

        MeretMarketTab metadata = MeretMarketCategoryMetadataStorage.GetTabMetadata(section, (int) category);
        if (metadata is null)
        {
            return;
        }

        List<MeretMarketItem> items = new();

        switch (section)
        {
            case MeretMarketSection.UgcMarket:
                items.AddRange(GameServer.UgcMarketManager.FindItems(metadata.ItemCategories, gender, job, searchString));
                break;
            case MeretMarketSection.PremiumMarket:
            case MeretMarketSection.RedMeretMarket:
                items.AddRange(DatabaseManager.MeretMarket.FindAllByCategory(section, category, gender, job, searchString));
                break;
        }

        int totalItems = items.Count;

        items = MeretMarketHelper.MarketItemsSorted(items, sortBy);
        items = MeretMarketHelper.TakeLimit(items, startPage, itemsPerPage);

        session.Send(MeretMarketPacket.LoadShopCategory(items, totalItems));
    }

    private static void HandlePurchase(GameSession session, PacketReader packet)
    {
        byte quantity = packet.ReadByte();
        int marketItemId = packet.ReadInt();
        long ugcItemId = packet.ReadLong();
        if (ugcItemId != 0)
        {
            PurchaseUgcItem(session, ugcItemId);
            return;
        }

        PurchasePremiumItem(session, packet, marketItemId);
    }

    private static void PurchaseUgcItem(GameSession session, long ugcMarketItemId)
    {
        UgcMarketItem marketItem = GameServer.UgcMarketManager.FindItemById(ugcMarketItemId);
        if (marketItem is null || marketItem.ListingExpirationTimestamp < TimeInfo.Now())
        {
            return;
        }

        if (!HandleMarketItemPay(session, marketItem.Price, MeretMarketCurrencyType.Meret))
        {
            return;
        }

        marketItem.SalesCount++;
        DatabaseManager.UgcMarketItems.Update(marketItem);
        _ = new UgcMarketSale(marketItem.Price, marketItem.Item.Ugc.Name, marketItem.SellerCharacterId);

        Item item = new(marketItem.Item)
        {
            CreationTime = TimeInfo.Now()
        };
        item.Uid = DatabaseManager.Items.Insert(item);

        session.Player.Inventory.AddItem(session, item, true);
        session.Send(MeretMarketPacket.Purchase(0, marketItem.MarketId, marketItem.Price, 1));
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

        PremiumMarketItem marketItem = DatabaseManager.MeretMarket.FindById(marketItemId);
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

        long itemPrice = marketItem.Price;

        if (marketItem.SalePrice != 0)
        {
            itemPrice = marketItem.SalePrice;
        }
        if (!HandleMarketItemPay(session, itemPrice, marketItem.TokenType))
        {
            SystemNotice noticeId = SystemNotice.EmptyString;
            switch (marketItem.TokenType)
            {
                case MeretMarketCurrencyType.Meso:
                    noticeId = SystemNotice.ErrorInsufficientMeso;
                    break;
                case MeretMarketCurrencyType.Meret:
                    noticeId = SystemNotice.ErrorInsufficientMeret;
                    break;
                case MeretMarketCurrencyType.RedMeret:
                    noticeId = SystemNotice.ErrorInsufficientMeretRed;
                    break;
            }

            session.Send(NoticePacket.Notice(noticeId, NoticeType.Popup));
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
        session.Send(MeretMarketPacket.Purchase((int) marketItem.MarketId, 0, marketItem.Price, totalQuantity, itemIndex));
    }

    private static bool HandleMarketItemPay(GameSession session, long price, MeretMarketCurrencyType currencyType)
    {
        return currencyType switch
        {
            MeretMarketCurrencyType.Meret => session.Player.Account.RemoveMerets(price),
            MeretMarketCurrencyType.Meso => session.Player.Wallet.Meso.Modify(price),
            _ => false
        };
    }

    private static void HandleOpenFeatured(GameSession session, PacketReader reader)
    {
        byte section = reader.ReadByte();
        byte tab = reader.ReadByte(); // 0A = Featured, 0B = New
        List<MeretMarketItem> marketItems = new();
        switch (section)
        {
            // Front page
            case 0:
                marketItems.AddRange(DatabaseManager.MeretMarket.FindAllByCategory(MeretMarketSection.PremiumMarket, MeretMarketCategory.Promo, (GenderFlag) 3, JobFlag.All, ""));
                session.Send(MeretMarketPacket.Promos(marketItems));
                break;
            // Premium Featured (if implemented)
            case 1:
            // Red Market Featured (if implemented)
            case 2:
                break;
        }
    }

    private static void HandleOpenDesignShop(GameSession session)
    {
        List<UgcMarketItem> promoItems = GameServer.UgcMarketManager.GetPromoItems();
        List<UgcMarketItem> newestItems = GameServer.UgcMarketManager.GetNewestItems();
        session.Send(MeretMarketPacket.OpenDesignShop(promoItems, newestItems));
    }

    private static void HandleLoadCart(GameSession session)
    {
        session.Send(MeretMarketPacket.LoadCart());
    }

    private static void HandleSearch(GameSession session, PacketReader packet)
    {
        packet.ReadInt(); // 1
        GenderFlag genderFlag = (GenderFlag) packet.ReadByte();
        JobFlag jobFlag = (JobFlag) packet.ReadInt();
        MeretMarketSort sortBy = (MeretMarketSort) packet.ReadByte();
        string searchString = packet.ReadUnicodeString();
        int startPage = packet.ReadInt(); // 1
        packet.ReadInt(); // 1
        packet.ReadByte();
        packet.ReadByte();
        byte itemsPerPage = packet.ReadByte();
        MeretMarketSection marketSection = ReadMarketSection(packet.ReadByte());

        List<MeretMarketItem> items = new();

        switch (marketSection)
        {
            case MeretMarketSection.PremiumMarket:
            case MeretMarketSection.RedMeretMarket:
                items.AddRange(DatabaseManager.MeretMarket.FindAllByCategory(marketSection, MeretMarketCategory.None, genderFlag, jobFlag, searchString));
                break;
            case MeretMarketSection.UgcMarket:
                items.AddRange(GameServer.UgcMarketManager.FindItems(new(), genderFlag, jobFlag, searchString));
                break;
            case MeretMarketSection.All:
                items.AddRange(DatabaseManager.MeretMarket.FindAllByCategory(marketSection, MeretMarketCategory.None, genderFlag, jobFlag, searchString));
                items.AddRange(GameServer.UgcMarketManager.FindItems(new(), genderFlag, jobFlag, searchString));
                break;
        }

        int totalItems = items.Count;

        items = MeretMarketHelper.MarketItemsSorted(items, sortBy);
        items = MeretMarketHelper.TakeLimit(items, startPage, itemsPerPage);
        session.Send(MeretMarketPacket.LoadShopCategory(items, totalItems));
    }

    private static void HandleSendMarketRequest(GameSession session, PacketReader packet)
    {
        packet.ReadByte(); //constant 1
        int meretMarketItemUid = packet.ReadInt();
        List<MeretMarketItem> meretMarketItems = new()
        {
            DatabaseManager.MeretMarket.FindById(meretMarketItemUid)
        };
        session.Send(MeretMarketPacket.LoadShopCategory(meretMarketItems, meretMarketItems.Count));
    }

    private static MeretMarketSection ReadMarketSection(byte section)
    {
        return section switch
        {
            0 => MeretMarketSection.All,
            1 => MeretMarketSection.PremiumMarket,
            2 => MeretMarketSection.RedMeretMarket,
            3 => MeretMarketSection.UgcMarket,
            _ => MeretMarketSection.All
        };
    }
}
