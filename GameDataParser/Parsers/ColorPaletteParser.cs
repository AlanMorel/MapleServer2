using System;
using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ColorPaletteParser : Exporter<List<ColorPaletteMetadata>>
    {
        public ColorPaletteParser(MetadataResources resources) : base(resources, "color-palette") { }

        protected override List<ColorPaletteMetadata> Parse()
        {
            List<ColorPaletteMetadata> palette = new List<ColorPaletteMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {

                if (!entry.Name.StartsWith("table/colorpalette"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    ColorPaletteMetadata metadata = new ColorPaletteMetadata();

                    if (node.Name == "colorPalette")
                    {
                        metadata.PaletteId = int.Parse(node.Attributes["id"].Value);

                        foreach (XmlNode colorNode in node)
                        {

                            int index = int.Parse(colorNode.Attributes["colorSN"].Value);
                            int primary = Convert.ToInt32(colorNode.Attributes["ch0"].Value, 16);
                            byte[] primaryBytes = BitConverter.GetBytes(primary);
                            Array.Reverse(primaryBytes);

                            int secondary = Convert.ToInt32(colorNode.Attributes["ch1"].Value, 16);
                            byte[] secondaryBytes = BitConverter.GetBytes(secondary);
                            Array.Reverse(secondaryBytes);

                            int tertiary = Convert.ToInt32(colorNode.Attributes["ch2"].Value, 16);
                            byte[] tertiaryBytes = BitConverter.GetBytes(tertiary);
                            Array.Reverse(tertiaryBytes);

                            int paletteColor = Convert.ToInt32(colorNode.Attributes["palette"].Value, 16);
                            byte[] paletteBytes = BitConverter.GetBytes(paletteColor);
                            Array.Reverse(paletteBytes);

                            MixedColor newColor = MixedColor.Custom(
                    Color.Argb(primaryBytes[0], primaryBytes[1], primaryBytes[2], primaryBytes[3]),
                    Color.Argb(secondaryBytes[0], secondaryBytes[1], secondaryBytes[2], secondaryBytes[3]),
                    Color.Argb(tertiaryBytes[0], tertiaryBytes[1], tertiaryBytes[2], tertiaryBytes[3])
                    );
                            metadata.DefaultColors.Add(newColor);
                        }
                    }
                    palette.Add(metadata);
                }
            }
            return palette;
        }
    }
}
