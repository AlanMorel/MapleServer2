using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ItemGemstoneUpgradeParser : Exporter<List<ItemGemstoneUpgradeMetadata>>
    {
        public ItemGemstoneUpgradeParser(MetadataResources resources) : base(resources, "item-gemstone-upgrade") { }

        protected override List<ItemGemstoneUpgradeMetadata> Parse()
        {
            List<ItemGemstoneUpgradeMetadata> gems = new List<ItemGemstoneUpgradeMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/na/itemgemstoneupgrade"))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList keys = document.SelectNodes("/ms2/key");

                foreach (XmlNode key in keys)
                {
                    ItemGemstoneUpgradeMetadata metadata = new ItemGemstoneUpgradeMetadata();
                    metadata.ItemId = int.Parse(key.Attributes["ItemId"].Value);
                    metadata.GemLevel = byte.Parse(key.Attributes["GemLevel"].Value);

                    if (key.Attributes["NextItemID"] != null)
                    {
                        metadata.NextItemId = string.IsNullOrEmpty(key.Attributes["NextItemID"]?.Value) ? 0 : int.Parse(key.Attributes["NextItemID"].Value);
                    }

                    for (int i = 1; i < 5; i++)
                    {
                        if (key.Attributes["IngredientItemID" + i.ToString()] != null)
                        {
                            metadata.IngredientItems.Add(Regex.Match(key.Attributes["IngredientItemID" + i.ToString()].Value, @"[a-zA-Z]+").Value);
                            metadata.IngredientAmounts.Add(int.Parse(key.Attributes["IngredientCount" + i.ToString()].Value));
                        }
                    }
                    gems.Add(metadata);
                }
            }
            return gems;
        }
    }
}
