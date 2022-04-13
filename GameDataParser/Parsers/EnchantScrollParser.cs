using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class EnchantScrollParser : Exporter<List<EnchantScrollMetadata>>
{
    public EnchantScrollParser(MetadataResources resources) : base(resources, MetadataName.EnchantScroll) { }

    protected override List<EnchantScrollMetadata> Parse()
    {
        List<EnchantScrollMetadata> items = new();

        PackFileEntry entry = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/enchantscroll"));
        if (entry is null)
        {
            return items;
        }

        // Parse XML
        XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
        XmlNodeList nodes = document.SelectNodes("/ms2/scroll");

        foreach (XmlNode node in nodes)
        {
            List<int> intItemSlots = node.Attributes["slot"].Value.Split(",").ToList().ConvertAll(int.Parse);
            List<ItemType> itemSlots = intItemSlots.Select(itemSlot => (ItemType) itemSlot).ToList();

            EnchantScrollMetadata metadata = new()
            {
                Id = int.Parse(node.Attributes["id"].Value),
                MinLevel = short.Parse(node.Attributes["minLv"].Value),
                MaxLevel = short.Parse(node.Attributes["maxLv"].Value),
                ItemTypes = itemSlots,
                ScrollType = (EnchantScrollType) int.Parse(node.Attributes["scrollType"].Value),
                Rarities = node.Attributes["rank"].Value.Split(",").ToList().ConvertAll(int.Parse),
                EnchantLevels = node.Attributes["grade"].Value.Split(",").ToList().ConvertAll(int.Parse),
            };

            items.Add(metadata);
        }
        return items;
    }
}
