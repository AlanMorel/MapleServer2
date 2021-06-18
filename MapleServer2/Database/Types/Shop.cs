using System.Collections.Generic;
using Maple2Storage.Enums;

namespace MapleServer2.Database.Types
{
    public class Shop
    {
        public int Uid { get; set; }
        public int Id { get; set; }
        public int Category { get; set; }
        public string Name { get; set; }
        public ShopType ShopType { get; set; }
        public bool RestrictSales { get; set; }
        public bool CanRestock { get; set; }
        public long NextRestock { get; set; }
        public bool AllowBuyback { get; set; }
        public List<ShopItem> Items { get; set; }

        public Shop() { }
    }
}
