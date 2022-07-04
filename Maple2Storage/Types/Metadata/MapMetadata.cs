using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MapMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public string Name = "";
    [XmlElement(Order = 3)]
    public string XBlockName = "";
    [XmlElement(Order = 4)]
    public MapProperty Property;
    [XmlElement(Order = 5)]
    public MapDrop Drop;
    [XmlElement(Order = 6)]
    public MapCashCall CashCall;
    [XmlElement(Order = 7)]
    public MapUi Ui;
    [XmlElement(Order = 8)]
    public Dictionary<CoordS, MapBlock> Blocks = new();
    [XmlElement(Order = 9)]
    public MapEntityMetadata Entities = new();
}

[XmlType]
public class MapBlock
{
    [XmlElement(Order = 1)]
    public CoordS Coord;
    [XmlElement(Order = 2)]
    public string Attribute;
    [XmlElement(Order = 3)]
    public string Type;
    [XmlElement(Order = 4)]
    public int SaleableGroup;
}

[XmlType]
public class MapProperty
{
    [XmlElement(Order = 1)]
    public int RevivalReturnMapId;
    [XmlElement(Order = 2)]
    public int EnterReturnMapId;
    [XmlElement(Order = 3)]
    public int Capacity;
    [XmlElement(Order = 4)]
    public bool IsTutorialMap;
    [XmlElement(Order = 5)]
    public bool DeathPenalty;
    [XmlElement(Order = 6)]
    public bool HomeReturnable;
    [XmlElement(Order = 7)]
    public bool RecoverFullHp;
    [XmlElement(Order = 8)]
    public bool DisableFly;
    [XmlElement(Order = 9)]
    public List<int> EnterBuffIds = new();
    [XmlElement(Order = 10)]
    public List<int> EnterBuffLevels = new();
}

[XmlType]
public class MapDrop
{
    [XmlElement(Order = 1)]
    public int MapLevel;
    [XmlElement(Order = 2)]
    public int DropRank;
    [XmlElement(Order = 3)]
    public List<int> GlobalDropBoxIds = new();
}

[XmlType]
public class MapCashCall
{
    [XmlElement(Order = 1)]
    public bool DisableExitWithTaxi;
    [XmlElement(Order = 2)]
    public bool DisableEnterWithTaxi;
    [XmlElement(Order = 3)]
    public bool DisableUseDoctor;
    [XmlElement(Order = 4)]
    public bool DisableUseMarket; // need to confirm what this bool does
    [XmlElement(Order = 5)]
    public bool DisableRecallPlayers;
}

[XmlType]
public class MapUi
{
    [XmlElement(Order = 1)]
    public bool EnableFallDamage;
    [XmlElement(Order = 2)]
    public bool EnableStaminaSkillUse;
    [XmlElement(Order = 3)]
    public bool EnableMount;
    [XmlElement(Order = 4)]
    public bool EnablePet;
}
