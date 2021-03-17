﻿using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ColorPaletteMetadataStorage
    {
        private static readonly Dictionary<int, ColorPaletteMetadata> palette = new Dictionary<int, ColorPaletteMetadata>();

        static ColorPaletteMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-color-palette-metadata");
            List<ColorPaletteMetadata> items = Serializer.Deserialize<List<ColorPaletteMetadata>>(stream);
            foreach (ColorPaletteMetadata item in items)
            {
                palette[item.PaletteId] = item;
            }
        }

        public static bool IsValid(int paletteId)
        {
            return palette.ContainsKey(paletteId);
        }

        public static ColorPaletteMetadata GetMetadata(int paletteId)
        {
            return palette.GetValueOrDefault(paletteId);
        }
    }
}
