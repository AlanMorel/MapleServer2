using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Packets.Helpers;

namespace MapleServer2.Database.Types;

public class ShopItem : IPacketSerializable
{
    public readonly int Uid;
    public readonly int ItemId;
    public readonly ShopCurrencyType TokenType;
    public readonly int RequiredItemId;
    public readonly int Price;
    public readonly int SalePrice;
    public readonly byte ItemRank;
    public readonly int StockCount;
    public readonly int StockPurchased;
    public readonly int GuildTrophy;
    public readonly string Category;
    public readonly int RequiredAchievementId;
    public readonly int RequiredAchievementGrade;
    public readonly byte RequiredChampionshipGrade;
    public readonly short RequiredChampionshipJoinCount;
    public readonly byte RequiredGuildMerchantType;
    public readonly short RequiredGuildMerchantLevel;
    public readonly short Quantity;
    public readonly ShopItemFlag Flag;
    public readonly string TemplateName;
    public readonly short RequiredQuestAlliance;
    public readonly int RequiredFameGrade;
    public readonly bool AutoPreviewEquip;

    public ShopItem(dynamic data)
    {
        Uid = data.uid;
        AutoPreviewEquip = data.auto_preview_equip;
        Category = data.category;
        Flag = (ShopItemFlag) data.flag;
        GuildTrophy = data.guild_trophy;
        ItemId = data.item_id;
        ItemRank = data.item_rank;
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
        StockPurchased = data.stock_purchased;
        TemplateName = data.template_name;
        TokenType = (ShopCurrencyType) data.token_type;
        Quantity = data.quantity;
    }
    
    public void WriteTo(PacketWriter pWriter)
    {
        pWriter.WriteInt(Uid);
        pWriter.WriteInt(ItemId);
        pWriter.Write(TokenType);
        pWriter.WriteInt(RequiredItemId);
        pWriter.WriteInt();
        pWriter.WriteInt(Price);
        pWriter.WriteInt(SalePrice);
        pWriter.WriteByte(ItemRank);
        pWriter.WriteInt();
        pWriter.WriteInt(StockCount);
        pWriter.WriteInt(StockPurchased);
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
        pWriter.WriteByte(1);
        pWriter.Write(Flag);
        pWriter.WriteString(TemplateName); // currency ID STR
        pWriter.WriteShort(RequiredQuestAlliance);
        pWriter.WriteInt(RequiredFameGrade);
        pWriter.WriteBool(AutoPreviewEquip);
        pWriter.WriteByte(); // has buy period
        pWriter.WriteItem(new(ItemId, Quantity, ItemRank, false));
    }
}
