﻿using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Database.Classes;
using MapleServer2.Database.Types;
using MapleServer2.Packets.Helpers;

namespace MapleServer2.Types;

public class PlayerShopItemLog
{
    public readonly long Uid;
    public int ShopItemUid;
    public int ShopId;
    public long CharacterId;
    public long AccountId;
    public long ItemUid;
    public int StockPurchased;
    public bool IsPersistant; // Account wide

    public PlayerShopItemLog(dynamic data)
    {
        Uid = data.uid;
        ShopItemUid = data.shop_item_uid;
        ShopId = data.shop_id;
        CharacterId = data.character_id;
        AccountId = data.account_id;
        ItemUid = data.item_uid;
        StockPurchased = data.purchase_count;
        IsPersistant = data.is_persistant;
    }

    public PlayerShopItemLog(ShopItem shopItem, Player player, Shop shop, out Item item)
    {
        item = new(shopItem.ItemId, shopItem.Quantity, shopItem.Rarity);
        shopItem.Item = item;
        ShopItemUid = shopItem.ShopItemUid;
        ShopId = shop.Id;
        IsPersistant = shop.PersistantInventory;
        ItemUid = item.Uid;
        CharacterId = player.CharacterId;
        AccountId = player.AccountId;
        Uid = DatabaseManager.ShopItemLogs.Insert(this);
        player.ShopItemLogs.Add(shopItem.ShopItemUid, this);
    }
}
