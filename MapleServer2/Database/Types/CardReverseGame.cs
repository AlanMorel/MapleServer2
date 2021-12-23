namespace MapleServer2.Database.Types;

public class CardReverseGame
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public byte ItemRarity { get; set; }
    public int ItemAmount { get; set; }

    // Temporarily hardcoding the item and cost
    public const int TOKEN_ITEM_ID = 30000782; // 2nd Anniversary Commemorative Coin
    public const int TOKEN_COST = 2;
}
