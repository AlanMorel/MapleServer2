using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class MeretMarketCategoryParser : Exporter<List<MeretMarketCategoryMetadata>>
{
    public MeretMarketCategoryParser(MetadataResources resources) : base(resources, "meret-market-category") { }

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


                foreach (XmlNode tabNode in node)
                {
                    MeretMarketCategoryMetadata metadata = new();

                    metadata.Section = section;
                    metadata.CategoryId = int.Parse(tabNode.Attributes["id"].Value);

                    foreach (XmlNode subTabNode in tabNode.ChildNodes)
                    {
                        MeretMarketCategoryMetadata subTab = new();
                        subTab.Section = section;
                        subTab.CategoryId = int.Parse(subTabNode.Attributes["id"].Value);
                        if (subTabNode.Attributes["category"] is not null)
                        {
                            List<string> itemCategories = new();
                            itemCategories.AddRange(subTabNode.Attributes["category"].Value.Split(",").ToList());
                            subTab.ItemCategories = itemCategories;
                            metadata.ItemCategories.AddRange(itemCategories);
                        }

                        categories.Add(subTab);
                    }
                    categories.Add(metadata);
                }
            }
        }
        return categories;
    }
}
