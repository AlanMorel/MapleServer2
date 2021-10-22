using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class ColorPaletteMetadata
    {
        [XmlElement(Order = 1)]
        public int PaletteId;
        [XmlElement(Order = 2)]
        public List<MixedColor> DefaultColors;

        public ColorPaletteMetadata()
        {
            DefaultColors = new List<MixedColor>();
        }

        public override string ToString() => $"ColorPaletteMetadata(PaletteId:{PaletteId},DefaultColors:{DefaultColors})";
    }
}
