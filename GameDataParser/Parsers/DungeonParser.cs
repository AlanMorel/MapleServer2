using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class DungeonParser : Exporter<List<DungeonMetadata>>
{
    public DungeonParser(MetadataResources resources) : base(resources, "dungeon") { }

    protected override List<DungeonMetadata> Parse()
    {
        List<DungeonMetadata> dungeons = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/na/dungeonroom"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodes = document.SelectNodes("/ms2/dungeonRoom");

            foreach (XmlNode dungeonNode in nodes)
            {
                DungeonMetadata metadata = new()
                {
                    DungeonRoomId = int.Parse(dungeonNode.Attributes["dungeonRoomID"]?.Value ?? "0"),
                    DungeonLevelRequirement = int.Parse(dungeonNode.Attributes["limitPlayerLevel"]?.Value ?? "0"),
                    GroupType = dungeonNode.Attributes["groupType"]?.Value ?? "",
                    CooldownType = dungeonNode.Attributes["cooldownType"]?.Value ?? "",
                    UnionRewardId = int.Parse(dungeonNode.Attributes["unionRewardID"]?.Value ?? "0"),
                    RewardCount = short.Parse(dungeonNode.Attributes["rewardCount"]?.Value ?? "0"),
                    RewardExp = int.Parse(dungeonNode.Attributes["rewardExp"]?.Value ?? "0"),
                    RewardMeso = int.Parse(dungeonNode.Attributes["rewardMeso"]?.Value ?? "0"),
                    LobbyFieldId = int.Parse(dungeonNode.Attributes["lobbyFieldID"]?.Value ?? "0"),
                    FieldIds = dungeonNode.Attributes["fieldIDs"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList(),
                    MaxUserCount = byte.Parse(dungeonNode.Attributes["maxUserCount"]?.Value ?? "0"),
                    LimitPlayerLevel = byte.Parse(dungeonNode.Attributes["limitPlayerLevel"]?.Value ?? "0")
                };

                dungeons.Add(metadata);
            }
        }

        return dungeons;
    }
}
