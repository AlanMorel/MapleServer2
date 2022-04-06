using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class MeretMarketCategoryParser : Exporter<List<MeretMarketCategoryMetadata>>
{
    public MeretMarketCategoryParser(MetadataResources resources) : base(resources, MetadataName.MeretMarketCategory) { }

    protected override List<MeretMarketCategoryMetadata> Parse()
    {
        List<MeretMarketCategoryMetadata> categories = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/na/meratmarketcategory"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.Name != "category")
                {
                    continue;
                }

                MeretMarketSection section = (MeretMarketSection) int.Parse(node.Attributes["id"].Value);

                MeretMarketCategoryMetadata metadata = new()
                {
                    Section = section,
                };

                foreach (XmlNode tabNode in node)
                {
                    MeretMarketTab tab = new()
                    {
                        Id = int.Parse(tabNode.Attributes["id"].Value),
                    };

                    foreach (XmlNode subTabNode in tabNode.ChildNodes)
                    {
                        if (subTabNode.Attributes["category"] is not null)
                        {
                            List<string> categoryList = subTabNode.Attributes["category"].Value.Split(",").ToList();
                            MeretMarketTab subTab = new()
                            {
                                Id = int.Parse(subTabNode.Attributes["id"].Value),
                                ItemCategories = categoryList
                            };
                            tab.ItemCategories.AddRange(categoryList);
                            metadata.Tabs.Add(subTab);
                        }
                    }
                    metadata.Tabs.Add(tab);
                }
                categories.Add(metadata);
            }
        }
        return categories;
    }
}
