using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;
using System;


namespace GameDataParser.Parsers
{
    public class DungeonParser : Exporter<List<DungeonMetadata>>
    {
        public DungeonParser(MetadataResources resources) : base(resources, "dungeon") { }

        protected override List<DungeonMetadata> Parse()
        {
            List<DungeonMetadata> dungeons = new List<DungeonMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/na/dungeonroom"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode dungeonNode in document.DocumentElement.ChildNodes)
                {
                    DungeonMetadata metadata = new DungeonMetadata();

                    if (dungeonNode.Name == "dungeonRoom")
                    {
                        metadata.DungeonRoomId = int.Parse(dungeonNode.Attributes["dungeonRoomID"]?.Value);
                        if (dungeonNode.Attributes["limitPlayerLevel"] != null)
                        {
                            metadata.DungeonLevelRequirement = int.Parse(dungeonNode.Attributes["limitPlayerLevel"].Value);
                        }
                        if (dungeonNode.Attributes["groupType"] != null)
                        {
                            metadata.GroupType = dungeonNode.Attributes["groupType"].Value;
                        }
                        if (dungeonNode.Attributes["cooldownType"] != null)
                        {
                            metadata.CooldownType = dungeonNode.Attributes["cooldownType"]?.Value;
                        }
                        if (dungeonNode.Attributes["unionRewardID"] != null)
                        {
                            metadata.UnionRewardId = int.Parse(dungeonNode.Attributes["unionRewardID"].Value);
                        }
                        if (dungeonNode.Attributes["rewardCount"] != null)
                        {
                            metadata.RewardCount = short.Parse(dungeonNode.Attributes["rewardCount"].Value);
                        }
                        if (dungeonNode.Attributes["rewardExp"] != null)
                        {
                            metadata.RewardExp = int.Parse(dungeonNode.Attributes["rewardExp"].Value);
                        }
                        if (dungeonNode.Attributes["rewardMeso"] != null)
                        {
                            metadata.RewardMeso = int.Parse(dungeonNode.Attributes["rewardMeso"].Value);
                        }
                        if (dungeonNode.Attributes["lobbyFieldID"] != null)
                        {
                            metadata.LobbyFieldId = int.Parse(dungeonNode.Attributes["lobbyFieldID"].Value);
                        }
                        if (dungeonNode.Attributes["fieldIDs"] != null)
                        {
                            int[] fieldIDsList = Array.ConvertAll(dungeonNode.Attributes["fieldIDs"].Value.Split(","), int.Parse);
                            foreach (int fieldId in fieldIDsList)
                            {
                                metadata.FieldIds.Add(fieldId);
                            }
                        }
                        if (dungeonNode.Attributes["maxUserCount"] != null)
                        {
                            metadata.MaxUserCount = byte.Parse(dungeonNode.Attributes["maxUserCount"].Value);
                        }
                        if (dungeonNode.Attributes["limitPlayerLevel"] != null)
                        {
                            metadata.LimitPlayerLevel = byte.Parse(dungeonNode.Attributes["limitPlayerLevel"].Value);
                        }
                    }

                    dungeons.Add(metadata);
                }
            }
            return dungeons;
        }
    }
}
