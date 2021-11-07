using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ConstantsMetadata
{
    [XmlElement(Order = 1)]
    public string Key;
    [XmlElement(Order = 2)]
    public string Value;

    public ConstantsMetadata() { }

    public ConstantsMetadata(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
