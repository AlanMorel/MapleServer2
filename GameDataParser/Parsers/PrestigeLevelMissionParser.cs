﻿using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class PrestigeLevelMissionParser : Exporter<List<PrestigeLevelMissionMetadata>>
{
    public PrestigeLevelMissionParser(MetadataResources resources) : base(resources, MetadataName.PrestigeLevelMission) { }

    protected override List<PrestigeLevelMissionMetadata> Parse()
    {
        List<PrestigeLevelMissionMetadata> items = new();

        PackFileEntry? entry = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/adventurelevelmission"));
        if (entry is null)
        {
            return items;
        }

        // Parse XML
        XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
        XmlNodeList? nodes = document.SelectNodes("/ms2/mission");
        if (nodes is null)
        {
            return items;
        }

        foreach (XmlNode node in nodes)
        {
            if (ParserHelper.CheckForNull(node, "missionId", "missionCount", "itemId", "itemRank", "itemCount"))
            {
                continue;
            }

            PrestigeLevelMissionMetadata metadata = new()
            {
                Id = int.Parse(node.Attributes!["missionId"]!.Value),
                MissionCount = int.Parse(node.Attributes["missionCount"]!.Value),
                RewardItemId = int.Parse(node.Attributes["itemId"]!.Value),
                RewardItemRarity = int.Parse(node.Attributes["itemRank"]!.Value),
                RewardItemAmount = int.Parse(node.Attributes["itemCount"]!.Value)
            };

            items.Add(metadata);
        }

        return items;
    }
}
