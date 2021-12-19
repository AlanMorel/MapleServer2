using Maple2Storage.Enums;
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

    public MeretMarketHandler() : base() { }

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
        OpenPremium = 0x1B,
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
                HandleLoadSales(session, packet);
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
            case MeretMarketMode.OpenPremium:
                HandleOpenPremium(session, packet);
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
        List<UGCMarketItem> items = new();
        items = GameServer.UGCMarketManager.GetItemsByCharacterId(session.Player.CharacterId);
        session.Send(MeretMarketPacket.LoadPersonalListings(items));
    }

    private static void HandleLoadSales(GameSession session, PacketReader packet)
    {
        //TODO get sales from DB
        session.Send(MeretMarketPacket.LoadSales(new()));
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

        if (item.UGC is null)
        {
            return;
        }

        UGCMarketItem marketItem = new UGCMarketItem(item, salePrice, session.Player, tags, description, promote);

        session.Send(MeretMarketPacket.ListItem(marketItem));
        session.Send(MeretMarketPacket.UpdateExpiration(marketItem));
    }

    private static void HandleRemoveListing(GameSession session, PacketReader packet)
    {
        packet.ReadInt(); // 0
        long ugcMarketItemId = packet.ReadLong();
        packet.ReadLong(); // duplicate id read?

        UGCMarketItem item = GameServer.UGCMarketManager.FindById(ugcMarketItemId);
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

        UGCMarketItem item = GameServer.UGCMarketManager.FindById(ugcMarketItemId);
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

        UGCMarketItem item = GameServer.UGCMarketManager.FindById(ugcMarketItemId);
        if (item is null || item.SellerCharacterId != session.Player.CharacterId || item.ListingExpirationTimestamp < TimeInfo.Now())
        {
            return;
        }

        // TODO: Handle fee

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

    }

    private static void HandleInitialize(GameSession session)
    {
        session.Send(MeretMarketPacket.Initialize());
    }

    private static void HandleOpenPremium(GameSession session, PacketReader packet)
    {
        MeretMarketCategory category = (MeretMarketCategory) packet.ReadInt();
        List<MeretMarketItem> marketItems = DatabaseManager.MeretMarket.FindAllByCategoryId(category);
        if (marketItems is null)
        {
            return;
        }
        session.Send(MeretMarketPacket.Premium(marketItems));
    }

    private static void HandlePurchase(GameSession session, PacketReader packet)
    {
        byte quantity = packet.ReadByte();
        int marketItemId = packet.ReadInt();
        long ugcItemId = packet.ReadLong();
        if (ugcItemId != 0)
        {
            PurchaseUGCItem(session, packet, ugcItemId);
            return;
        }

        PurchasePremiumItem(session, packet, marketItemId);
        return;
    }

    private static void PurchaseUGCItem(GameSession session, PacketReader packet, long ugcMarketItemId)
    {
        Console.WriteLine("This is a UGC Item");
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

        if (childMarketItemId == 0)
        {

        }
        else
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
        session.Send(MeretMarketPacket.Purchase(marketItem, itemIndex, totalQuantity));
    }

    private static bool HandleMarketItemPay(GameSession session, long price, MeretMarketCurrencyType currencyType)
    {
        switch (currencyType)
        {
            case MeretMarketCurrencyType.Meret:
                return session.Player.Account.RemoveMerets(price);
            case MeretMarketCurrencyType.Meso:
                return session.Player.Wallet.Meso.Modify(price);
        }
        return false;
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
        session.Send(MeretMarketPacket.Premium(meretMarketItems));
    }
}
