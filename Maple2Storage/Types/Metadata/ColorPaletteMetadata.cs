using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class ColorPaletteMetadata
    {
        [XmlElement(Order = 1)]
        public int PaletteId;
        [XmlElement(Order = 2)]
        public List<EquipColor> DefaultColors;

        public ColorPaletteMetadata()
        {
            DefaultColors = new List<EquipColor>();
        }

        public override string ToString() =>
$"ColorPaletteMetadata(PaletteId:{PaletteId},DefaultColors:{DefaultColors})";

        protected bool Equals(ColorPaletteMetadata other)
        {
            return PaletteId == other.PaletteId &&
                   DefaultColors.SequenceEqual(other.DefaultColors);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ColorPaletteMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PaletteId, DefaultColors);
        }

        public static bool operator ==(ColorPaletteMetadata left, ColorPaletteMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ColorPaletteMetadata left, ColorPaletteMetadata right)
        {
            return !Equals(left, right);
        }
    }
}
