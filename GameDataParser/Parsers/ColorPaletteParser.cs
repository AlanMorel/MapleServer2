using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ColorPaletteParser : Exporter<List<ColorPaletteMetadata>>
{
    public ColorPaletteParser(MetadataResources resources) : base(resources, MetadataName.ColorPalette) { }

    protected override List<ColorPaletteMetadata> Parse()
    {
        List<ColorPaletteMetadata> palette = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/colorpalette"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodes = document.SelectNodes("/ms2/colorPalette");
            if (nodes is null)
            {
                continue;
            }

            foreach (XmlNode node in nodes)
            {
                if (ParserHelper.CheckForNull(node, "id"))
                {
                    continue;
                }

                ColorPaletteMetadata metadata = new()
                {
                    PaletteId = int.Parse(node.Attributes!["id"]!.Value)
                };

                foreach (XmlNode colorNode in node)
                {
                    if (ParserHelper.CheckForNull(colorNode, "colorSN", "ch0", "ch1", "ch2", "palette"))
                    {
                        continue;
                    }

                    // int index = int.Parse(colorNode.Attributes!["colorSN"]!.Value);
                    int primary = Convert.ToInt32(colorNode.Attributes!["ch0"]!.Value, 16);
                    byte[] primaryBytes = BitConverter.GetBytes(primary);
                    Array.Reverse(primaryBytes);

                    int secondary = Convert.ToInt32(colorNode.Attributes["ch1"]!.Value, 16);
                    byte[] secondaryBytes = BitConverter.GetBytes(secondary);
                    Array.Reverse(secondaryBytes);

                    int tertiary = Convert.ToInt32(colorNode.Attributes["ch2"]!.Value, 16);
                    byte[] tertiaryBytes = BitConverter.GetBytes(tertiary);
                    Array.Reverse(tertiaryBytes);

                    int paletteColor = Convert.ToInt32(colorNode.Attributes["palette"]!.Value, 16);
                    byte[] paletteBytes = BitConverter.GetBytes(paletteColor);
                    Array.Reverse(paletteBytes);

                    MixedColor newColor = MixedColor.Custom(
                        Color.FromBytes(primaryBytes),
                        Color.FromBytes(secondaryBytes),
                        Color.FromBytes(tertiaryBytes)
                    );
                    metadata.DefaultColors.Add(newColor);
                }

                palette.Add(metadata);
            }
        }

        return palette;
    }
}
