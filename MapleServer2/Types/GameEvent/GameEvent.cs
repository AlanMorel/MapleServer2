using System.Collections.Generic;

namespace MapleServer2.Types
{
    public class GameEvent
    {
        public int Id;
        public bool Active;
        public GameEventType Type;
        public List<MapleopolyEvent> Mapleopoly;
        public List<StringBoardEvent> StringBoard;

        public GameEvent() { }
    }

    public enum GameEventType
    {
        StringBoard, // Marquee text
        BlueMarble, // Mapleopoly
        ExchangeScrollSale,
        TrafficOptimizer,
        PetComposeSale,
        SaleRemakes,
        SaleEnchant,
        ItemTradeRestriction,
        HotTime,
        UGCMapContractSale,
        UGCMapExtensionSale,
        DungeonBonusReward
    }
}
