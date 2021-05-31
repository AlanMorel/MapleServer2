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

                    if (key.Attributes["IngredientItemID1"] != null)
                    {
                        metadata.IngredientItems.Add(Regex.Match(key.Attributes["IngredientItemID1"].Value, @"[a-zA-Z]+").Value);
                        metadata.IngredientAmounts.Add(int.Parse(key.Attributes["IngredientCount1"].Value));
                    }

                    if (key.Attributes["IngredientItemID2"] != null)
                    {
                        metadata.IngredientItems.Add(Regex.Match(key.Attributes["IngredientItemID2"].Value, @"[a-zA-Z]+").Value);
                        metadata.IngredientAmounts.Add(int.Parse(key.Attributes["IngredientCount2"].Value));
                    }

                    if (key.Attributes["IngredientItemID3"] != null)
                    {
                        metadata.IngredientItems.Add(Regex.Match(key.Attributes["IngredientItemID3"].Value, @"[a-zA-Z]+").Value);
                        metadata.IngredientAmounts.Add(int.Parse(key.Attributes["IngredientCount3"].Value));
                    }

                    if (key.Attributes["IngredientItemID4"] != null)
                    {
                        metadata.IngredientItems.Add(Regex.Match(key.Attributes["IngredientItemID4"].Value, @"[a-zA-Z]+").Value);
                        metadata.IngredientAmounts.Add(int.Parse(key.Attributes["IngredientCount4"].Value));
                    }

                    gems.Add(metadata);
                }
            }
            return gems;
        }
    }
}
