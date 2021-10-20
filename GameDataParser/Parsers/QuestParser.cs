using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class QuestParser : Exporter<List<QuestMetadata>>
    {
        public QuestParser(MetadataResources resources) : base(resources, "quest") { }

        protected override List<QuestMetadata> Parse()
        {
            List<QuestMetadata> quests = new List<QuestMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("quest/"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode envi in document.DocumentElement.ChildNodes)
                {
                    if (envi.Name == "environment")
                    {
                        string locale = string.IsNullOrEmpty(envi.Attributes["locale"]?.Value) ? null : envi.Attributes["locale"].Value;
                        if (locale != "NA" && locale != null)
                        {
                            continue;
                        }
                    }

                    QuestMetadata metadata = new QuestMetadata();
                    XmlNodeList quest = envi.ChildNodes[0].ChildNodes;
                    foreach (XmlNode node in quest)
                    {
                        if (node.Name == "basic")
                        {
                            metadata.Basic.ChapterID = int.Parse(node.Attributes["chapterID"]?.Value ?? "0");
                            metadata.Basic.Id = int.Parse(node.Attributes["questID"]?.Value ?? "0");
                            _ = byte.TryParse(node.Attributes["questType"]?.Value ?? "0", out byte questType);
                            metadata.Basic.QuestType = (QuestType) questType;
                            metadata.Basic.Account = byte.Parse(node.Attributes["account"]?.Value ?? "0");
                            metadata.Basic.StandardLevel = int.Parse(node.Attributes["standardLevel"]?.Value ?? "0");
                            metadata.Basic.AutoStart = byte.Parse(node.Attributes["autoStart"]?.Value ?? "0");
                            metadata.Basic.DisableGiveup = byte.Parse(node.Attributes["disableGiveup"]?.Value ?? "0");
                            metadata.Basic.ExceptChapterClear = int.Parse(node.Attributes["exceptChapterClear"]?.Value ?? "0");
                            metadata.Basic.Repeatable = byte.Parse(node.Attributes["repeatable"]?.Value ?? "0");
                            metadata.Basic.UsePeriod = node.Attributes["usePeriod"].Value;
                            metadata.Basic.EventTag = node.Attributes["eventTag"].Value;
                            metadata.Basic.Locking = byte.Parse(node.Attributes["locking"]?.Value ?? "0");
                            metadata.Basic.TabIndex = int.Parse(node.Attributes["tabIndex"]?.Value ?? "0");
                            metadata.Basic.ForceRegistGuide = byte.Parse(node.Attributes["forceRegistGuide"]?.Value ?? "0");
                            metadata.Basic.UseNavigation = !(node.Attributes["useNavi"].Value == "FALSE");

                        }
                        else if (node.Name == "notify")
                        {
                            metadata.Notify.CompleteUiEffect = node.Attributes["completeUiEffect"].Value;
                            metadata.Notify.AcceptSoundKey = node.Attributes["acceptSoundKey"].Value;
                            metadata.Notify.CompleteSoundKey = node.Attributes["completeSoundKey"].Value;
                        }
                        else if (node.Name == "require")
                        {
                            metadata.Require.Level = short.Parse(node.Attributes["level"]?.Value ?? "0");
                            metadata.Require.MaxLevel = short.Parse(node.Attributes["maxLevel"]?.Value ?? "0");

                            metadata.Require.Job = node.Attributes["job"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(short.Parse).ToList();
                            metadata.Require.RequiredQuests = node.Attributes["quest"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
                            metadata.Require.SelectableQuest = node.Attributes["selectableQuest"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
                            metadata.Require.Unrequire = node.Attributes["unrequire"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();

                            _ = int.TryParse(node.Attributes["field"]?.Value ?? "0", out metadata.Require.Field);
                            metadata.Require.Achievement = int.Parse(node.Attributes["achievement"]?.Value ?? "0");

                            metadata.Require.UnreqAchievement = node.Attributes["unreqAchievement"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();

                            metadata.Require.GroupID = int.Parse(node.Attributes["groupID"]?.Value ?? "0");
                            metadata.Require.DayOfWeek = node.Attributes["dayOfWeek"].Value;
                            metadata.Require.GearScore = int.Parse(node.Attributes["gearScore"]?.Value ?? "0");
                        }
                        else if (node.Name == "start")
                        {
                            metadata.StartNpc = int.Parse(node.Attributes["npc"].Value);
                        }
                        else if (node.Name == "complete")
                        {
                            metadata.CompleteNpc = int.Parse(node.Attributes["npc"].Value);
                        }
                        else if (node.Name == "completeReward")
                        {
                            metadata.Reward.Exp = int.Parse(node.Attributes["exp"]?.Value ?? "0");
                            metadata.Reward.RelativeExp = node.Attributes["relativeExp"]?.Value;
                            metadata.Reward.Money = int.Parse(node.Attributes["money"]?.Value ?? "0");
                            metadata.Reward.Karma = int.Parse(node.Attributes["karma"]?.Value ?? "0");
                            metadata.Reward.Lu = int.Parse(node.Attributes["lu"]?.Value ?? "0");

                            foreach (XmlNode reward in node.ChildNodes)
                            {
                                if (!reward.Name.Contains("global") && metadata.Basic.QuestType != QuestType.Navigator)
                                {
                                    continue;
                                }
                                int itemId = int.Parse(reward.Attributes["code"]?.Value ?? "0");
                                if (itemId == 0)
                                {
                                    continue;
                                }

                                byte rank = byte.Parse(reward.Attributes["rank"]?.Value ?? "0");
                                int count = int.Parse(reward.Attributes["count"]?.Value ?? "0");

                                QuestRewardItem item = new QuestRewardItem(itemId, rank, count);
                                metadata.RewardItem.Add(item);
                            }
                        }
                        else if (node.Name == "progressMap")
                        {
                            metadata.ProgressMap = node.Attributes["progressMap"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
                        }
                        else if (node.Name == "guide")
                        {
                            metadata.Guide.Type = node.Attributes["guideType"].Value;
                            metadata.Guide.Icon = node.Attributes["guideIcon"].Value;
                            metadata.Guide.MinLevel = byte.Parse(node.Attributes["guideMinLevel"]?.Value ?? "0");
                            metadata.Guide.MaxLevel = byte.Parse(node.Attributes["guideMaxLevel"]?.Value ?? "0");
                            metadata.Guide.Group = node.Attributes["guideGroup"]?.Value;
                        }
                        else if (node.Name == "gotoNpc")
                        {
                            metadata.Npc.Enable = byte.Parse(node.Attributes["enable"]?.Value ?? "0");
                            metadata.Npc.GoToField = int.Parse(node.Attributes["gotoField"]?.Value ?? "0");
                            metadata.Npc.GoToPortal = int.Parse(node.Attributes["gotoPortal"]?.Value ?? "0");
                        }
                        else if (node.Name == "gotoDungeon")
                        {
                            metadata.Dungeon.State = byte.Parse(node.Attributes["state"]?.Value ?? "0");
                            metadata.Dungeon.GoToDungeon = int.Parse(node.Attributes["gotoDungeon"]?.Value ?? "0");
                            metadata.Dungeon.GoToInstanceID = int.Parse(node.Attributes["gotoInstanceID"]?.Value ?? "0");
                        }
                        else if (node.Name == "remoteAccept")
                        {
                            metadata.RemoteAccept.UseRemote = node.Attributes["useRemote"].Value;
                            metadata.RemoteAccept.RequireField = int.Parse(node.Attributes["requireField"]?.Value ?? "0");
                        }
                        else if (node.Name == "remoteComplete")
                        {
                            metadata.RemoteComplete.UseRemote = node.Attributes["useRemote"].Value;
                            metadata.RemoteComplete.RequireField = int.Parse(node.Attributes["requireField"]?.Value ?? "0");
                            metadata.RemoteComplete.RequireDungeonClear = int.Parse(node.Attributes["requireDungeonClear"]?.Value ?? "0");
                        }
                        else if (node.Name == "summonPortal")
                        {
                            metadata.SummonPortal.FieldID = int.Parse(node.Attributes["fieldID"]?.Value ?? "0");
                            metadata.SummonPortal.PortalID = int.Parse(node.Attributes["portalID"]?.Value ?? "0");
                        }
                        else if (node.Name == "eventMission")
                        {
                            metadata.Event = node.Attributes["event"].Value;
                        }
                        else if (node.Name == "condition")
                        {
                            string type = node.Attributes["type"].Value;
                            string[] codes = node.Attributes["code"]?.Value.Split(",");
                            int value = int.Parse(node.Attributes["value"]?.Value ?? "0");
                            List<string> targets = null;
                            if (!string.IsNullOrEmpty(node.Attributes["target"]?.Value))
                            {
                                targets = new List<string>(node.Attributes["target"].Value.Split(","));
                            }
                            metadata.Condition.Add(new QuestCondition(type, codes, value, targets));
                        }
                        else if (node.Name == "navi")
                        {
                            string naviType = node.Attributes["type"].Value;
                            string naviCode = node.Attributes["code"].Value;
                            int naviMap = int.Parse(node.Attributes["map"]?.Value ?? "0");

                            metadata.Navigation.Add(new QuestNavigation(naviType, naviCode, naviMap));
                        }
                    }

                    quests.Add(metadata);
                }
            }
            return quests;
        }
    }
}