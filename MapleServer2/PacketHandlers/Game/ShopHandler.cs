using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ShopHandler : GamePacketHandler<ShopHandler>
{
    public override RecvOp OpCode => RecvOp.Shop;

    private enum Mode : byte
    {
        PurchaseBuyBack = 3,
        Buy = 4,
        Sell = 5,
        InstantRestock = 9,
        Refresh = 10,
        LoadNew = 13
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.PurchaseBuyBack:
                HandlePurchaseBuyBack(session, packet);
                break;
            case Mode.Buy:
                HandleBuy(session, packet);
                break;
            case Mode.Sell:
                HandleSell(session, packet);
                break;
            case Mode.InstantRestock:
                HandleInstantRestock(session, packet);
                break;
            case Mode.Refresh:
                HandleRefresh(session);
                break;
            case Mode.LoadNew:
                HandleLoadNew(session);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandlePurchaseBuyBack(GameSession session, PacketReader packet)
    {
        int index = packet.ReadInt();

        BuyBackItem item = session.Player.BuyBackItems.ElementAtOrDefault(index);
        if (item is null)
        {
            return;
        }

        if (!session.Player.Wallet.Meso.Modify(-item.Price))
        {
            return;
        }

        session.Player.BuyBackItems[index] = null;
        session.Send(ShopPacket.RemoveBuyBackItem(index));
        session.Player.Inventory.AddItem(session, item.Item, true);
    }

    private static void HandleSell(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        int quantity = packet.ReadInt();

        Shop shop = DatabaseManager.Shops.FindById(session.Player.ShopId);
        if (shop is null)
        {
            return;
        }

        if (shop.DisableBuyback)
        {
            session.Send(ShopPacket.Notice(ShopNotice.CantSellItemsInThisShop));
            return;
        }

        Item item = session.Player.Inventory.GetByUid(itemUid);
        if (item == null)
        {
            return;
        }

        if (!ItemMetadataStorage.GetLimitMetadata(item.Id).Sellable)
        {
            session.Send(ShopPacket.Notice(ShopNotice.CannotBeSold));
            return;
        }

        long price = ItemMetadataStorage.GetSellPrice(item.Id, item.Rarity, item.GetItemType());
        session.Player.Wallet.Meso.Modify(price * quantity);

        BuyBackItem?[] buyBackItems = session.Player.BuyBackItems;
        int designatedSlot = Array.FindIndex(buyBackItems, x => x == null);
        if (designatedSlot == -1)
        {
            BuyBackItem oldestItem = buyBackItems.MinBy(x => x.AddedTimestamp);
            designatedSlot = Array.FindIndex(buyBackItems, x => x == oldestItem);
            buyBackItems[designatedSlot] = null;
            session.Send(ShopPacket.RemoveBuyBackItem(designatedSlot));
        }

        if (quantity < item.Amount)
        {
            if (item.TrySplit(quantity, out Item splitItem))
            {
                session.Send(ItemInventoryPacket.UpdateAmount(item.Uid, item.Amount));
                item = splitItem;
            }
        }
        else
        {
            session.Player.Inventory.RemoveItem(session, itemUid, out Item _);
        }
        BuyBackItem buyBackItem = new(item, price);
        buyBackItems[designatedSlot] = buyBackItem;
        session.Send(ShopPacket.AddBuyBackItem(buyBackItem, designatedSlot));
    }

    private static void HandleBuy(GameSession session, PacketReader packet)
    {
        int shopItemUid = packet.ReadInt();
        int quantity = packet.ReadInt();

        if (session.Player.Shops.TryGetValue(session.Player.ShopId, out Shop shop))
        {
            BuyFromInstanceShop(session, shop, shopItemUid, quantity);
            return;
        }

        ShopItem shopItem = DatabaseManager.ShopItems.FindByUid(shopItemUid);

        if (!shopItem.CanPurchase(session))
        {
            return;
        }

        if (!Pay(session, shopItem.CurrencyType, shopItem.Price, shopItem.Quantity, shopItem.RequiredItemId))
        {
            return;
        }

        // add item to inventory
        Item item = new(shopItem.ItemId, quantity * shopItem.Quantity, shopItem.Rarity);
        session.Player.Inventory.AddItem(session, item, true);

        // complete purchase
        session.Send(ShopPacket.Buy(shopItem.ItemId, quantity, shopItem.Price, shopItem.Rarity));
    }

    private static void HandleInstantRestock(GameSession session, PacketReader packet)
    {
        int cost = packet.ReadInt();

        if (!session.Player.Shops.TryGetValue(session.Player.ShopId, out Shop instanceShop))
        {
            return;
        }

        if (!instanceShop.CanRestock || instanceShop.DisableBuyback)
        {
            return;
        }

        if (!Pay(session, instanceShop.RestockCurrencyType, instanceShop.RestockCost, 1))
        {
            return;
        }

        // TODO: Implement restock cost multiplier

        long restockTime = TimeInfo.Now() + (instanceShop.RestockMinInterval * 60);
        Shop serverShop = DatabaseManager.Shops.FindById(instanceShop.Id);
        instanceShop = ShopHelper.RecreateShop(session.Player, serverShop);
        instanceShop.RestockTime = restockTime;

        DatabaseManager.ShopLogs.Update(session.Player.ShopLogs[instanceShop.Id]);
        session.Send(ShopPacket.InstantRestock());
        ShopHelper.LoadShop(session, instanceShop);
    }

    private static void HandleRefresh(GameSession session)
    {
        Shop serverShop = DatabaseManager.Shops.FindById(session.Player.ShopId);
        if (serverShop is null)
        {
            return;
        }

        ShopHelper.LoadShop(session, serverShop);
    }

    private static void HandleLoadNew(GameSession session)
    {
        // This doesn't seem to work properly... It didn't exist in GMS2.
        session.Send(ShopPacket.LoadNew(session.Player.Shops[session.Player.ShopId].Items));
    }

    private static void BuyFromInstanceShop(GameSession session, Shop shop, int shopItemUid, int quantity)
    {
        ShopItem shopItem = shop.Items.FirstOrDefault(x => x.ShopItemUid == shopItemUid);
        if (shopItem is null)
        {
            session.Send(ShopPacket.Notice(ShopNotice.ItemNotFound));
            return;
        }

        if (!shopItem.CanPurchase(session))
        {
            return;
        }

        if (shopItem.IsOutOfStock(quantity))
        {
            session.Send(ShopPacket.Notice(ShopNotice.NotEnoughSupplies));
            return;
        }

        if (!Pay(session, shopItem.CurrencyType, shopItem.Price, shopItem.Quantity, shopItem.RequiredItemId))
        {
            return;
        }

        PlayerShopItemLog itemLog = session.Player.ShopItemLogs[shopItem.ShopItemUid];
        shopItem.StockPurchased += quantity;
        itemLog.StockPurchased += quantity;

        DatabaseManager.ShopItemLogs.Update(itemLog);
        Item item = new(shopItem.Item)
        {
            Amount = quantity * shopItem.Quantity
        };
        item.Uid = DatabaseManager.Items.Insert(item);

        // add item to inventory
        session.Player.Inventory.AddItem(session, item, true);

        // complete purchase
        session.Send(ShopPacket.UpdateProduct(shopItem, shopItem.StockPurchased * shopItem.Quantity));
        session.Send(ShopPacket.Buy(shopItem.ItemId, quantity, shopItem.Price, shopItem.Rarity));
    }

    private static bool Pay(GameSession session, ShopCurrencyType currencyType, int price, int quantity, int itemId = 0)
    {
        switch (currencyType)
        {
            case ShopCurrencyType.Meso:
                if (!session.Player.Wallet.Meso.Modify(-(price * quantity)))
                {
                    session.Send(ShopPacket.Notice(ShopNotice.NotEnoughMesos));
                    return false;
                }
                return true;
            case ShopCurrencyType.ValorToken:
                return session.Player.Wallet.ValorToken.Modify(-(price * quantity));
            case ShopCurrencyType.Treva:
                return session.Player.Wallet.Treva.Modify(-(price * quantity));
            case ShopCurrencyType.Rue:
                return session.Player.Wallet.Rue.Modify(-(price * quantity));
            case ShopCurrencyType.HaviFruit:
                session.Player.Wallet.HaviFruit.Modify(-(price * quantity));
                break;
            case ShopCurrencyType.Meret:
            case ShopCurrencyType.GameMeret:
            case ShopCurrencyType.EventMeret:
                if (!session.Player.Account.RemoveMerets(price * quantity))
                {
                    session.Send(ShopPacket.Notice(ShopNotice.NotEnoughMerets));
                    return false;
                }
                return true;
            case ShopCurrencyType.Item:
                Item itemCost = session.Player.Inventory.GetById(itemId);
                if (itemCost is null || itemCost.Amount < price)
                {
                    return false;
                }
                session.Player.Inventory.ConsumeItem(session, itemCost.Uid, price);
                return true;
            default:
                session.Send(ShopPacket.Notice(ShopNotice.NotEnoughSupplies));
                Logger.Warning("Unknown currency: {CurrencyType}", currencyType);
                return false;
        }
        return false;
    }
}
