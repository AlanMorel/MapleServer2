using System.Collections.Generic;

namespace MapleServer2.Types
{
    public class NpcShop
    {
        public int TemplateId { get; set; }
        public int Id { get; set; }
        public int Category { get; set; }
        public string Name { get; set; }
        public bool RestrictSales { get; set; }
        public bool CanRestock { get; set; }
        public long NextRestock { get; set; }
        public bool AllowBuyback { get; set; }
        public List<NpcShopItem> Items { get; set; }
    }
}
