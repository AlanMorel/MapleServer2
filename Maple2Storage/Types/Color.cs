using System.Runtime.InteropServices;

namespace Maple2Storage.Types {
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct Color {
        public byte Blue { get; private set; }
        public byte Green { get; private set; }
        public byte Red { get; private set; }
        public byte Alpha { get; private set; }

        public static Color Argb(byte alpha, byte red, byte green, byte blue) {
            return new Color {
                Alpha = alpha,
                Red = red,
                Green = green,
                Blue = blue
            };
        }

        public override string ToString() => $"ARGB({Alpha:X2}, {Red:X2}, {Green:X2}, {Blue:X2})";
    }

    [StructLayout(LayoutKind.Sequential, Size = 8)]
    public struct SkinColor {
        public Color Primary;
        public Color Secondary { get; private set; }

        public static SkinColor Argb(Color color) {
            return new SkinColor {
                Primary = color,
                Secondary = color
            };
        }

        public override string ToString() => $"Primary:{Primary}|Secondary:{Secondary}";
    }

    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public struct EquipColor {
        public Color Primary { get; private set; }
        public Color Secondary { get; private set; }
        public Color Tertiary { get; private set; }
        public int Index { get; private set; }

        public static EquipColor Argb(Color color) {
            return new EquipColor {
                Primary = color,
                Secondary = color,
                Tertiary = color,
                Index = -1
            };
        }

        public static EquipColor Custom(Color primary, Color secondary, Color tertiary, int index = -1) {
            return new EquipColor {
                Primary = primary,
                Secondary = secondary,
                Tertiary = tertiary,
                Index = -1
            };
        }

        public override string ToString() =>
            $"Primary:{Primary}|Secondary:{Secondary}|Tertiary:{Tertiary}|Index:{Index}";
    }
}