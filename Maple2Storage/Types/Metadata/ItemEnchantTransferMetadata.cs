using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemEnchantTransferMetadata
{
    [XmlElement(Order = 1)]
    public int InputRarity;
    [XmlElement(Order = 2)]
    public int InputItemLevel;
    [XmlElement(Order = 3)]
    public int InputEnchantLevel;
    [XmlElement(Order = 4)]
    public List<ItemType> InputSlots;
    [XmlElement(Order = 5)]
    public List<int> InputItemIds;
    [XmlElement(Order = 6)]
    public long MesoCost;
    [XmlElement(Order = 7)]
    public int OutputItemId;
    [XmlElement(Order = 8)]
    public int OutputRarity;
    [XmlElement(Order = 9)]
    public int OutputAmount;

    public ItemEnchantTransferMetadata() { }
}
