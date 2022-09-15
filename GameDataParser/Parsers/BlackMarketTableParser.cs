﻿using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class BlackMarketTableParser : Exporter<List<BlackMarketTableMetadata>>
{
    public BlackMarketTableParser(MetadataResources resources) : base(resources, MetadataName.BlackMarketTable) { }

    protected override List<BlackMarketTableMetadata> Parse()
    {
        List<BlackMarketTableMetadata> tables = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/blackmarkettable"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? childNodes = document.DocumentElement?.ChildNodes;
            if (childNodes is null)
            {
                continue;
            }

            foreach (XmlNode node in childNodes)
            {
                if (node.Name != "category")
                {
                    continue;
                }

                foreach (XmlNode tabNode in node)
                {
                    if (tabNode.Attributes?["category"] is not null)
                    {
                        if (ParserHelper.CheckForNull(tabNode, "id", "category"))
                        {
                            continue;
                        }

                        BlackMarketTableMetadata metadata = new()
                        {
                            CategoryId = int.Parse(tabNode.Attributes["id"]!.Value),
                            ItemCategories = tabNode.Attributes["category"]!.Value.Split(",").ToList()
                        };

                        tables.Add(metadata);
                    }

                    foreach (XmlNode subtabNode in tabNode.ChildNodes)
                    {
                        if (subtabNode.Attributes?["category"] != null)
                        {
                            if (ParserHelper.CheckForNull(subtabNode, "id", "category"))
                            {
                                continue;
                            }

                            BlackMarketTableMetadata metadata = new()
                            {
                                CategoryId = int.Parse(subtabNode.Attributes["id"]!.Value),
                                ItemCategories = subtabNode.Attributes["category"]!.Value.Split(",").ToList()
                            };

                            tables.Add(metadata);
                        }

                        if (!subtabNode.HasChildNodes)
                        {
                            continue;
                        }

                        foreach (XmlNode subsubNode in subtabNode.ChildNodes)
                        {
                            if (subsubNode.Attributes?["category"] == null)
                            {
                                continue;
                            }

                            if (ParserHelper.CheckForNull(subtabNode, "id", "category"))
                            {
                                continue;
                            }

                            BlackMarketTableMetadata metadata = new()
                            {
                                CategoryId = int.Parse(subsubNode.Attributes["id"]!.Value),
                                ItemCategories = subsubNode.Attributes["category"]!.Value.Split(",").ToList()
                            };

                            tables.Add(metadata);
                        }
                    }
                }
            }
        }

        return tables;
    }
}
