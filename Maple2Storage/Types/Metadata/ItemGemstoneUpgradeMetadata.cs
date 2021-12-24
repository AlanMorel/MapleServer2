using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemGemstoneUpgradeMetadata
{
    [XmlElement(Order = 1)]
    public int ItemId;
    [XmlElement(Order = 2)]
    public byte GemLevel;
    [XmlElement(Order = 3)]
    public int NextItemId;
    [XmlElement(Order = 4)]
    public List<string> IngredientItems = new(); // by item tag
    [XmlElement(Order = 5)]
    public List<int> IngredientAmounts = new();

    public override string ToString()
    {
        return $"ItemGemstoneUpgradeMetadata(ItemId:{ItemId},GemLevel:{GemLevel},NextItemId{NextItemId})";
    }
}
