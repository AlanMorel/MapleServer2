namespace MapleServer2.Database.Types;

public class GameEvent
{
    public int Id;
    public bool Active;
    public GameEventType Type;
    public List<MapleopolyEvent> Mapleopoly;
    public List<StringBoardEvent> StringBoard;
    public UGCMapContractSaleEvent UGCMapContractSale;
    public UGCMapExtensionSaleEvent UGCMapExtensionSale;
    public FieldPopupEvent FieldPopupEvent;
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
    DungeonBonusReward,
    EventFieldPopup
}
