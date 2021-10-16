using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class BlackMarketTableParser : Exporter<List<BlackMarketTableMetadata>>
    {
        public BlackMarketTableParser(MetadataResources resources) : base(resources, "black-market-table") { }

        protected override List<BlackMarketTableMetadata> Parse()
        {
            List<BlackMarketTableMetadata> tables = new List<BlackMarketTableMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {

                if (!entry.Name.StartsWith("table/blackmarkettable"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "category")
                    {

                        foreach (XmlNode tabNode in node)
                        {
                            if (tabNode.Attributes["category"] != null)
                            {
                                BlackMarketTableMetadata metadata = new BlackMarketTableMetadata();

                                metadata.CategoryId = int.Parse(tabNode.Attributes["id"].Value);
                                metadata.ItemCategories = tabNode.Attributes["category"].Value.Split(",").ToList();

                                tables.Add(metadata);
                            }

                            foreach (XmlNode subtabNode in tabNode.ChildNodes)
                            {
                                Console.WriteLine(subtabNode.Attributes["name"].Value);
                                if (subtabNode.Attributes["category"] != null)
                                {
                                    BlackMarketTableMetadata metadata = new BlackMarketTableMetadata();

                                    metadata.CategoryId = int.Parse(subtabNode.Attributes["id"].Value);
                                    metadata.ItemCategories = subtabNode.Attributes["category"].Value.Split(",").ToList();

                                    tables.Add(metadata);
                                }

                                if (subtabNode.HasChildNodes)
                                {
                                    foreach (XmlNode subsubNode in subtabNode.ChildNodes)
                                    {
                                        Console.WriteLine(subsubNode.Attributes["name"].Value);
                                        if (subsubNode.Attributes["category"] != null)
                                        {
                                            BlackMarketTableMetadata metadata = new BlackMarketTableMetadata();

                                            metadata.CategoryId = int.Parse(subsubNode.Attributes["id"].Value);
                                            metadata.ItemCategories = subsubNode.Attributes["category"].Value.Split(",").ToList();

                                            tables.Add(metadata);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return tables;
        }
    }
}
