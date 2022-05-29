using Maple2Storage.Enums;

namespace MapleServer2.Database.Types;

public class RouletteGameItem
{
    public int Uid;
    public int RouletteId;
    public int ItemId;
    public byte ItemRarity;
    public int ItemAmount;

    public RouletteGameItem(dynamic data)
    {
        Uid = data.uid;
        RouletteId = data.roulette_id;
        ItemId = data.item_id;
        ItemRarity = (byte) data.item_rarity;
        ItemAmount = data.item_amount;
    }
}
