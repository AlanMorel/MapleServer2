using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class ShopItem : IPacketSerializable
{
    public readonly int ShopItemUid;
    public readonly int ItemId;
    public readonly ShopCurrencyType CurrencyType;
    public readonly int RequiredItemId;
    public readonly int Price;
    public readonly int SalePrice;
    public readonly byte Rarity;
    public int StockCount;
    public int StockPurchased;
    public readonly int GuildTrophy;
    public readonly string Category;
    public readonly int RequiredAchievementId;
    public readonly int RequiredAchievementGrade;
    public readonly byte RequiredChampionshipGrade;
    public readonly short RequiredChampionshipJoinCount;
    public readonly byte RequiredGuildMerchantType;
    public readonly short RequiredGuildMerchantLevel;
    public readonly short Quantity;
    public readonly ShopItemLabel Label;
    public readonly string CurrencyId;
    public readonly short RequiredQuestAlliance;
    public readonly int RequiredFameGrade;
    public readonly bool AutoPreviewEquip;
    public Item? Item;

    public ShopItem(dynamic data)
    {
        ShopItemUid = data.uid;
        AutoPreviewEquip = data.auto_preview_equip;
        Category = data.category;
        Label = (ShopItemLabel) data.label;
        GuildTrophy = data.guild_trophy;
        ItemId = data.item_id;
        Rarity = data.rarity;
        Price = data.price;
        RequiredAchievementGrade = data.required_achievement_grade;
        RequiredAchievementId = data.required_achievement_id;
        RequiredChampionshipGrade = data.required_championship_grade;
        RequiredChampionshipJoinCount = data.required_championship_join_count;
        RequiredFameGrade = data.required_fame_grade;
        RequiredGuildMerchantLevel = data.required_guild_merchant_level;
        RequiredGuildMerchantType = data.required_guild_merchant_type;
        RequiredItemId = data.required_item_id;
        RequiredQuestAlliance = data.required_quest_alliance;
        SalePrice = data.sale_price;
        StockCount = data.stock_count;
        CurrencyId = data.currency_id;
        CurrencyType = (ShopCurrencyType) data.currency_type;
        Quantity = data.quantity;
    }

    public bool IsOutOfStock(int quantity)
    {
        // No stock on the DB means it's unlimited stock
        ShopItem serverShopItem = DatabaseManager.ShopItems.FindByUid(ShopItemUid);
        if (serverShopItem.StockCount == 0)
        {
            return false;
        }

        return quantity > StockCount - StockPurchased;
    }

    public bool CanPurchase(GameSession session)
    {
        if (RequiredAchievementId != 0)
        {
            return session.Player.HasTrophy(RequiredAchievementId, RequiredAchievementGrade);
        }
        return true;

        //TODO: Add championship, guild merchant, alliance, and reputation checks
    }
    
    public void WriteTo(PacketWriter pWriter)
    {
        pWriter.WriteInt(ShopItemUid);
        pWriter.WriteInt(ItemId);
        pWriter.Write(CurrencyType);
        pWriter.WriteInt(RequiredItemId);
        pWriter.WriteInt();
        pWriter.WriteInt(Price);
        pWriter.WriteInt(SalePrice);
        pWriter.WriteByte(Rarity);
        pWriter.WriteInt();
        pWriter.WriteInt(StockCount);
        pWriter.WriteInt(StockPurchased * Quantity);
        pWriter.WriteInt(GuildTrophy);
        pWriter.WriteString(Category);
        pWriter.WriteInt(RequiredAchievementId);
        pWriter.WriteInt(RequiredAchievementGrade);
        pWriter.WriteByte(RequiredChampionshipGrade);
        pWriter.WriteShort(RequiredChampionshipJoinCount);
        pWriter.WriteByte(RequiredGuildMerchantType);
        pWriter.WriteShort(RequiredGuildMerchantLevel);
        pWriter.WriteBool(false);
        pWriter.WriteShort(Quantity);
        pWriter.WriteByte();
        pWriter.Write(Label);
        pWriter.WriteString(CurrencyId);
        pWriter.WriteShort(RequiredQuestAlliance);
        pWriter.WriteInt(RequiredFameGrade);
        pWriter.WriteBool(AutoPreviewEquip);
        bool hasBuyPeriod = false;
        pWriter.WriteBool(hasBuyPeriod);
        if (hasBuyPeriod)
        {
            bool timeSpecific = true;
            pWriter.WriteBool(timeSpecific); 
            pWriter.WriteLong(1666337871); // start timestamp
            pWriter.WriteLong(1686357871); // end timestamp
            pWriter.WriteBool(true); // unk bool
            pWriter.WriteByte(1); // amount of buy periods. loop
            // loop start
            pWriter.WriteInt(1200); // time begin in seconds. ex 1200 = 12:20 AM
            pWriter.WriteInt(10600); // time end in seconds. ex 10600 = 2:56 AM
            // loop end
            
            pWriter.WriteByte(1); // days of the week you can buy at. loop
            // loop start
            pWriter.WriteByte(4); // 1 = Sunday, 7 = Saturday
            // loop end
        }
        pWriter.WriteItem(Item ?? new(ItemId, Quantity, Rarity, false));
    }
}
