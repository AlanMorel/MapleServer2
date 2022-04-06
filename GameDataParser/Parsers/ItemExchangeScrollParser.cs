using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Files;
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
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                ItemExchangeScrollMetadata metadata = new();

                if (node.Name == "exchange")
                {
                    metadata.ExchangeId = int.Parse(node.Attributes["id"].Value);
                    metadata.Type = node.Attributes["type"].Value;
                }

                foreach (XmlNode childNode in node)
                {
                    if (childNode.Name == "receipe")
                    {
                        metadata.RecipeId = int.Parse(childNode.Attributes["id"].Value);
                        metadata.RecipeRarity = byte.Parse(childNode.Attributes["rank"].Value);
                        metadata.RecipeAmount = short.Parse(childNode.Attributes["count"].Value);
                    }
                    else if (childNode.Name == "exchange")
                    {
                        metadata.RewardId = int.Parse(childNode.Attributes["id"].Value);
                        metadata.RewardRarity = byte.Parse(childNode.Attributes["rank"].Value);
                        metadata.RewardAmount = short.Parse(childNode.Attributes["count"].Value);
                    }
                    else if (childNode.Name == "require")
                    {
                        _ = int.TryParse(childNode.Attributes["meso"]?.Value ?? "0", out metadata.MesoCost);

                        foreach (XmlNode itemNode in childNode)
                        {
                            if (itemNode.Name != "item")
                            {
                                continue;
                            }

                            ItemRequirementMetadata item = new();
                            string[] parameters = itemNode.Attributes["id"].Value.Split(",");
                            parameters[0] = Regex.Match(parameters[0], @"\d+").Value; // remove text from item id

                            item.Id = int.Parse(parameters[0]);
                            item.Rarity = byte.Parse(parameters[1]);
                            item.Amount = short.Parse(parameters[2]);

                            metadata.ItemCost.Add(item);
                        }
                    }
                }

                exchangeScroll.Add(metadata);
            }
        }

        return exchangeScroll;
    }
}
