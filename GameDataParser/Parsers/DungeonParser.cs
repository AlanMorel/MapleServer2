using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class DungeonParser : Exporter<List<DungeonMetadata>>
    {
        public DungeonParser(MetadataResources resources) : base(resources, "dungeon") { }

        protected override List<DungeonMetadata> Parse()
        {
            List<DungeonMetadata> dungeons = new List<DungeonMetadata>();
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
                    DungeonMetadata metadata = new DungeonMetadata();

                    metadata.DungeonRoomId = int.Parse(dungeonNode.Attributes["dungeonRoomID"]?.Value ?? "0");
                    metadata.DungeonLevelRequirement = int.Parse(dungeonNode.Attributes["limitPlayerLevel"]?.Value ?? "0");
                    metadata.GroupType = dungeonNode.Attributes["groupType"]?.Value ?? "";
                    metadata.CooldownType = dungeonNode.Attributes["cooldownType"]?.Value ?? "";
                    metadata.UnionRewardId = int.Parse(dungeonNode.Attributes["unionRewardID"]?.Value ?? "0");
                    metadata.RewardCount = short.Parse(dungeonNode.Attributes["rewardCount"]?.Value ?? "0");
                    metadata.RewardExp = int.Parse(dungeonNode.Attributes["rewardExp"]?.Value ?? "0");
                    metadata.RewardMeso = int.Parse(dungeonNode.Attributes["rewardMeso"]?.Value ?? "0");
                    metadata.LobbyFieldId = int.Parse(dungeonNode.Attributes["lobbyFieldID"]?.Value ?? "0");
                    metadata.FieldIds = dungeonNode.Attributes["fieldIDs"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
                    metadata.MaxUserCount = byte.Parse(dungeonNode.Attributes["maxUserCount"]?.Value ?? "0");
                    metadata.LimitPlayerLevel = byte.Parse(dungeonNode.Attributes["limitPlayerLevel"]?.Value ?? "0");

                    dungeons.Add(metadata);
                }
            }
            return dungeons;
        }
    }
}
