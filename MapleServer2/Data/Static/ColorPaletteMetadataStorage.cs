using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ColorPaletteMetadataStorage
{
    private static readonly Dictionary<int, ColorPaletteMetadata> Palette = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-color-palette-metadata");
        List<ColorPaletteMetadata> items = Serializer.Deserialize<List<ColorPaletteMetadata>>(stream);
        foreach (ColorPaletteMetadata item in items)
        {
            Palette[item.PaletteId] = item;
        }
    }

    public static bool IsValid(int paletteId)
    {
        return Palette.ContainsKey(paletteId);
    }

    public static ColorPaletteMetadata GetMetadata(int paletteId)
    {
        return Palette.GetValueOrDefault(paletteId);
    }
}
