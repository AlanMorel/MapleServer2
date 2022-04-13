using System.Xml;
using GameDataParser.Files;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemEnchantTransferParser : Exporter<List<ItemEnchantTransferMetadata>>
{
    public ItemEnchantTransferParser(MetadataResources resources) : base(resources, MetadataName.ItemEnchantTransfer) { }

    protected override List<ItemEnchantTransferMetadata> Parse()
    {
        List<ItemEnchantTransferMetadata> items = new();

        PackFileEntry entry = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/itemenchanttransform"));
        if (entry is null)
        {
            return items;
        }

        // Parse XML
        XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
        XmlNodeList nodes = document.SelectNodes("/ms2/transform");

        foreach (XmlNode node in nodes)
        {
            string locale = node.Attributes["locale"]?.Value ?? "";
            if (locale != "NA" && locale != "")
            {
                continue;
            }

            List<int> intItemSlots = node.Attributes["inputSlot"].Value.SplitAndParseToInt(',').ToList();
            List<ItemType> itemSlots = intItemSlots.Select(itemSlot => (ItemType) itemSlot).ToList();

            ItemEnchantTransferMetadata metadata = new()
            {
                InputRarity = int.Parse(node.Attributes["inputRank"].Value),
                InputItemLevel = int.Parse(node.Attributes["inputLimitLevel"].Value),
                InputEnchantLevel = int.Parse(node.Attributes["inputEnchantLevel"].Value),
                InputSlots = itemSlots,
                InputItemIds = node.Attributes["inputItemId"].Value.SplitAndParseToInt(',').ToList(),
                MesoCost = long.Parse(node.Attributes["needMeso"].Value),
                OutputItemId = int.Parse(node.Attributes["output1Id"].Value),
                OutputRarity = int.Parse(node.Attributes["output1Rank"].Value),
                OutputAmount = int.Parse(node.Attributes["output1Count"].Value)
            };

            items.Add(metadata);
        }
        return items;
    }
}
