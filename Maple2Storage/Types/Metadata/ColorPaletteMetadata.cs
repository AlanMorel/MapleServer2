using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ColorPaletteMetadata
{
    [XmlElement(Order = 1)]
    public int PaletteId;
    [XmlElement(Order = 2)]
    public List<MixedColor> DefaultColors = new();

    public override string ToString()
    {
        return $"ColorPaletteMetadata(PaletteId:{PaletteId},DefaultColors:{DefaultColors})";
    }
}
