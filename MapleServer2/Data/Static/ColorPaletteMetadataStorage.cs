using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ColorPaletteMetadataStorage
{
    private static readonly Dictionary<int, ColorPaletteMetadata> Palette = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.ColorPalette}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
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
