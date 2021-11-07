using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ChatStickerMetadata
{
    [XmlElement(Order = 1)]
    public int StickerId;
    [XmlElement(Order = 2)]
    public byte GroupId;
    [XmlElement(Order = 3)]
    public short CategoryId;
}
