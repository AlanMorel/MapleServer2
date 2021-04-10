using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Maple2Storage.Types
{
    [XmlType]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct Color
    {
        [XmlElement(Order = 1)]
        public byte Blue { get; private set; }
        [XmlElement(Order = 2)]
        public byte Green { get; private set; }
        [XmlElement(Order = 3)]
        public byte Red { get; private set; }
        [XmlElement(Order = 4)]
        public byte Alpha { get; private set; }

        public static Color Argb(byte alpha, byte red, byte green, byte blue)
        {
            return new Color
            {
                Alpha = alpha,
                Red = red,
                Green = green,
                Blue = blue
            };
        }

        public override string ToString() => $"ARGB({Alpha:X2}, {Red:X2}, {Green:X2}, {Blue:X2})";

        public static Color FromBytes(byte[] byteArray)
        {
            return Argb(byteArray[0], byteArray[1], byteArray[2], byteArray[3]);
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 8)]
    public struct SkinColor
    {
        public Color Primary { get; set; }
        public Color Secondary { get; private set; }

        public static SkinColor Argb(Color color)
        {
            return new SkinColor
            {
                Primary = color,
                Secondary = color
            };
        }

        public override string ToString() => $"Primary:{Primary}|Secondary:{Secondary}";
    }

    [XmlType]
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct MixedColor
    {
        [XmlElement(Order = 1)]
        public Color Primary { get; private set; }
        [XmlElement(Order = 2)]
        public Color Secondary { get; private set; }
        [XmlElement(Order = 3)]
        public Color Tertiary { get; private set; }

        public static MixedColor Argb(Color color)
        {
            return new MixedColor
            {
                Primary = color,
                Secondary = color,
                Tertiary = color
            };
        }

        public static MixedColor Custom(Color primary, Color secondary, Color tertiary)
        {
            return new MixedColor
            {
                Primary = primary,
                Secondary = secondary,
                Tertiary = tertiary,
            };
        }

        public override string ToString() =>
            $"Primary:{Primary}, Secondary:{Secondary}, Tertiary:{Tertiary}";
    }

    [XmlType]
    [StructLayout(LayoutKind.Sequential, Size = 20)]
    public struct EquipColor
    {
        [XmlElement(Order = 1)]
        public MixedColor Color { get; private set; }
        [XmlElement(Order = 2)]
        public int Index { get; private set; }
        [XmlElement(Order = 2)]
        public int Palette { get; private set; }

        public static EquipColor Argb(MixedColor color, int index, int palette)
        {
            return new EquipColor
            {
                Color = color,
                Index = index,
                Palette = palette
            };
        }

        public static EquipColor Custom(MixedColor color, int index, int palette)
        {
            return new EquipColor
            {
                Color = color,
                Index = index,
                Palette = palette
            };
        }

        public override string ToString() =>
            $"Color:{Color}, Index:{Index}, Palette:{Palette}";
    }
}
