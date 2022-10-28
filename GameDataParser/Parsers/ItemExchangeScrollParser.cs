using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemExchangeScrollParser : Exporter<List<ItemExchangeScrollMetadata>>
{
    public ItemExchangeScrollParser(MetadataResources resources) : base(resources, MetadataName.ItemExchangeScroll) { }

    protected override List<ItemExchangeScrollMetadata> Parse()
    {
        List<ItemExchangeScrollMetadata> exchangeScroll = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/itemexchangescrolltable"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            if (document.DocumentElement?.ChildNodes is null)
            {
                continue;
            }

            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                ItemExchangeScrollMetadata metadata = new();

                if (ParserHelper.CheckForNull(node, "id", "type"))
                {
                    continue;
                }

                if (node.Name == "exchange")
                {
                    metadata.ExchangeId = int.Parse(node.Attributes!["id"]!.Value);
                    metadata.Type = node.Attributes["type"]!.Value;
                }

                foreach (XmlNode childNode in node)
                {
                    if (childNode.Attributes is null)
                    {
                        continue;
                    }

                    switch (childNode.Name)
                    {
                        case "receipe":
                            if (ParserHelper.CheckForNull(childNode, "id", "rank", "count"))
                            {
                                break;
                            }

                            metadata.RecipeId = int.Parse(childNode.Attributes["id"]!.Value);
                            metadata.RecipeRarity = byte.Parse(childNode.Attributes["rank"]!.Value);
                            metadata.RecipeAmount = short.Parse(childNode.Attributes["count"]!.Value);
                            break;
                        case "exchange":
                            if (ParserHelper.CheckForNull(childNode, "id", "rank", "count"))
                            {
                                break;
                            }

                            metadata.RewardId = int.Parse(childNode.Attributes["id"]!.Value);
                            metadata.RewardRarity = byte.Parse(childNode.Attributes["rank"]!.Value);
                            metadata.RewardAmount = short.Parse(childNode.Attributes["count"]!.Value);
                            break;
                        case "require":
                            {
                                _ = int.TryParse(childNode.Attributes["meso"]?.Value ?? "0", out metadata.MesoCost);

                                foreach (XmlNode itemNode in childNode)
                                {
                                    if (itemNode.Name != "item")
                                    {
                                        continue;
                                    }

                                    if (ParserHelper.CheckForNull(itemNode, "id"))
                                    {
                                        break;
                                    }

                                    ItemRequirementMetadata item = new();
                                    string[] parameters = itemNode.Attributes!["id"]!.Value.Split(",");
                                    string[] parameter0 = parameters[0].Split(":");

                                    item.Id = int.Parse(parameter0[0]);
                                    item.Rarity = byte.Parse(parameters[1]);
                                    item.Amount = short.Parse(parameters[2]);
                                    if (parameter0.ElementAtOrDefault(1) is not null)
                                    {
                                        item.StringTag = parameter0[1];
                                    }

                                    metadata.ItemCost.Add(item);
                                }

                                break;
                            }
                    }
                }

                exchangeScroll.Add(metadata);
            }
        }

        return exchangeScroll;
    }
}
