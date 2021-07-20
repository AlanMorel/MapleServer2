using System.Collections.Generic;
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
                            metadata.Basic.ChapterID = string.IsNullOrEmpty(node.Attributes["chapterID"]?.Value) ? 0 : int.Parse(node.Attributes["chapterID"].Value);
                            metadata.Basic.Id = string.IsNullOrEmpty(node.Attributes["questID"]?.Value) ? 0 : int.Parse(node.Attributes["questID"].Value);
                            metadata.Basic.QuestType = (QuestType) (string.IsNullOrEmpty(node.Attributes["questType"]?.Value) ? 0 : byte.Parse(node.Attributes["questType"].Value));
                            metadata.Basic.Account = (byte) (string.IsNullOrEmpty(node.Attributes["account"]?.Value) ? 0 : byte.Parse(node.Attributes["account"].Value));
                            metadata.Basic.StandardLevel = string.IsNullOrEmpty(node.Attributes["standardLevel"]?.Value) ? 0 : int.Parse(node.Attributes["standardLevel"].Value);
                            metadata.Basic.AutoStart = (byte) (string.IsNullOrEmpty(node.Attributes["autoStart"]?.Value) ? 0 : byte.Parse(node.Attributes["autoStart"].Value));
                            metadata.Basic.DisableGiveup = (byte) (string.IsNullOrEmpty(node.Attributes["disableGiveup"]?.Value) ? 0 : byte.Parse(node.Attributes["disableGiveup"].Value));
                            metadata.Basic.ExceptChapterClear = string.IsNullOrEmpty(node.Attributes["exceptChapterClear"]?.Value) ? 0 : int.Parse(node.Attributes["exceptChapterClear"].Value);
                            metadata.Basic.Repeatable = (byte) (string.IsNullOrEmpty(node.Attributes["repeatable"]?.Value) ? 0 : byte.Parse(node.Attributes["repeatable"].Value));
                            metadata.Basic.UsePeriod = node.Attributes["usePeriod"].Value;
                            metadata.Basic.EventTag = node.Attributes["eventTag"].Value;
                            metadata.Basic.Locking = (byte) (string.IsNullOrEmpty(node.Attributes["locking"]?.Value) ? 0 : byte.Parse(node.Attributes["locking"].Value));
                            metadata.Basic.TabIndex = string.IsNullOrEmpty(node.Attributes["tabIndex"]?.Value) ? 0 : int.Parse(node.Attributes["tabIndex"].Value);
                            metadata.Basic.ForceRegistGuide = (byte) (string.IsNullOrEmpty(node.Attributes["forceRegistGuide"]?.Value) ? 0 : byte.Parse(node.Attributes["forceRegistGuide"].Value));
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
                            metadata.Require.Level = (short) (string.IsNullOrEmpty(node.Attributes["level"]?.Value) ? 0 : short.Parse(node.Attributes["level"].Value));
                            metadata.Require.MaxLevel = (short) (string.IsNullOrEmpty(node.Attributes["maxLevel"]?.Value) ? 0 : short.Parse(node.Attributes["maxLevel"].Value));

                            if (!string.IsNullOrEmpty(node.Attributes["job"]?.Value))
                            {
                                List<string> temp = new List<string>(node.Attributes["job"].Value.Split(","));
                                foreach (string item in temp)
                                {
                                    metadata.Require.Job.Add(short.Parse(item));
                                }
                            }

                            if (!string.IsNullOrEmpty(node.Attributes["quest"]?.Value))
                            {
                                List<string> temp = new List<string>(node.Attributes["quest"].Value.Split(","));
                                foreach (string item in temp)
                                {
                                    metadata.Require.RequiredQuests.Add(int.Parse(item));
                                }
                            }

                            if (!string.IsNullOrEmpty(node.Attributes["selectableQuest"]?.Value))
                            {
                                List<string> temp = new List<string>(node.Attributes["selectableQuest"].Value.Split(","));
                                foreach (string item in temp)
                                {
                                    metadata.Require.SelectableQuest.Add(int.Parse(item));
                                }
                            }

                            if (!string.IsNullOrEmpty(node.Attributes["unrequire"]?.Value))
                            {
                                List<string> temp = new List<string>(node.Attributes["unrequire"].Value.Split(","));
                                foreach (string item in temp)
                                {
                                    metadata.Require.Unrequire.Add(int.Parse(item));
                                }
                            }

                            metadata.Require.Field = string.IsNullOrEmpty(node.Attributes["field"]?.Value) ? 0 : int.Parse(node.Attributes["field"].Value);
                            metadata.Require.Achievement = string.IsNullOrEmpty(node.Attributes["achievement"]?.Value) ? 0 : int.Parse(node.Attributes["achievement"].Value);

                            if (!string.IsNullOrEmpty(node.Attributes["unreqAchievement"]?.Value))
                            {
                                List<string> temp = new List<string>(node.Attributes["unreqAchievement"].Value.Split(","));
                                foreach (string item in temp)
                                {
                                    metadata.Require.UnreqAchievement.Add(int.Parse(item));
                                }
                            }

                            metadata.Require.GroupID = string.IsNullOrEmpty(node.Attributes["groupID"]?.Value) ? 0 : int.Parse(node.Attributes["groupID"].Value);
                            metadata.Require.DayOfWeek = node.Attributes["dayOfWeek"].Value;
                            metadata.Require.GearScore = string.IsNullOrEmpty(node.Attributes["gearScore"]?.Value) ? 0 : int.Parse(node.Attributes["gearScore"].Value);
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
                            metadata.Reward.Exp = string.IsNullOrEmpty(node.Attributes["exp"]?.Value) ? 0 : int.Parse(node.Attributes["exp"].Value);
                            metadata.Reward.RelativeExp = node.Attributes["relativeExp"]?.Value;
                            metadata.Reward.Money = string.IsNullOrEmpty(node.Attributes["money"]?.Value) ? 0 : int.Parse(node.Attributes["money"].Value);
                            metadata.Reward.Karma = string.IsNullOrEmpty(node.Attributes["karma"]?.Value) ? 0 : int.Parse(node.Attributes["karma"].Value);
                            metadata.Reward.Lu = string.IsNullOrEmpty(node.Attributes["lu"]?.Value) ? 0 : int.Parse(node.Attributes["lu"].Value);

                            foreach (XmlNode reward in node.ChildNodes)
                            {
                                if (!reward.Name.Contains("global") && metadata.Basic.QuestType != QuestType.Navigator)
                                {
                                    continue;
                                }
                                int itemId = string.IsNullOrEmpty(reward.Attributes["code"]?.Value) ? 0 : int.Parse(reward.Attributes["code"].Value);
                                if (itemId == 0)
                                {
                                    continue;
                                }

                                byte rank = (byte) (string.IsNullOrEmpty(reward.Attributes["rank"]?.Value) ? 0 : byte.Parse(reward.Attributes["rank"].Value));
                                int count = string.IsNullOrEmpty(reward.Attributes["count"]?.Value) ? 0 : int.Parse(reward.Attributes["count"].Value);

                                QuestRewardItem item = new QuestRewardItem(itemId, rank, count);
                                metadata.RewardItem.Add(item);
                            }
                        }
                        else if (node.Name == "progressMap")
                        {
                            if (!string.IsNullOrEmpty(node.Attributes["progressMap"]?.Value))
                            {
                                List<string> temp = new List<string>(node.Attributes["progressMap"].Value.Split(","));
                                foreach (string item in temp)
                                {
                                    metadata.ProgressMap.Add(int.Parse(item));
                                }
                            }
                        }
                        else if (node.Name == "guide")
                        {
                            metadata.Guide.Type = node.Attributes["guideType"].Value;
                            metadata.Guide.Icon = node.Attributes["guideIcon"].Value;
                            metadata.Guide.MinLevel = (short) (string.IsNullOrEmpty(node.Attributes["guideMinLevel"]?.Value) ? 0 : byte.Parse(node.Attributes["guideMinLevel"].Value));
                            metadata.Guide.MaxLevel = (short) (string.IsNullOrEmpty(node.Attributes["guideMaxLevel"]?.Value) ? 0 : byte.Parse(node.Attributes["guideMaxLevel"].Value));
                            metadata.Guide.Group = node.Attributes["guideGroup"]?.Value;
                        }
                        else if (node.Name == "gotoNpc")
                        {
                            metadata.Npc.Enable = (byte) (string.IsNullOrEmpty(node.Attributes["enable"]?.Value) ? 0 : byte.Parse(node.Attributes["enable"].Value));
                            metadata.Npc.GoToField = string.IsNullOrEmpty(node.Attributes["gotoField"]?.Value) ? 0 : int.Parse(node.Attributes["gotoField"].Value);
                            metadata.Npc.GoToPortal = string.IsNullOrEmpty(node.Attributes["gotoPortal"]?.Value) ? 0 : int.Parse(node.Attributes["gotoPortal"].Value);
                        }
                        else if (node.Name == "gotoDungeon")
                        {
                            metadata.Dungeon.State = (byte) (string.IsNullOrEmpty(node.Attributes["state"]?.Value) ? 0 : byte.Parse(node.Attributes["state"].Value));
                            metadata.Dungeon.GoToDungeon = string.IsNullOrEmpty(node.Attributes["gotoDungeon"]?.Value) ? 0 : int.Parse(node.Attributes["gotoDungeon"].Value);
                            metadata.Dungeon.GoToInstanceID = string.IsNullOrEmpty(node.Attributes["gotoInstanceID"]?.Value) ? 0 : int.Parse(node.Attributes["gotoInstanceID"].Value);
                        }
                        else if (node.Name == "remoteAccept")
                        {
                            metadata.RemoteAccept.UseRemote = node.Attributes["useRemote"].Value;
                            metadata.RemoteAccept.RequireField = string.IsNullOrEmpty(node.Attributes["requireField"]?.Value) ? 0 : int.Parse(node.Attributes["requireField"].Value);
                        }
                        else if (node.Name == "remoteComplete")
                        {
                            metadata.RemoteComplete.UseRemote = node.Attributes["useRemote"].Value;
                            metadata.RemoteComplete.RequireField = string.IsNullOrEmpty(node.Attributes["requireField"]?.Value) ? 0 : int.Parse(node.Attributes["requireField"].Value);
                            metadata.RemoteComplete.RequireDungeonClear = string.IsNullOrEmpty(node.Attributes["requireDungeonClear"]?.Value) ? 0 : int.Parse(node.Attributes["requireDungeonClear"].Value);
                        }
                        else if (node.Name == "summonPortal")
                        {
                            metadata.SummonPortal.FieldID = string.IsNullOrEmpty(node.Attributes["fieldID"]?.Value) ? 0 : int.Parse(node.Attributes["fieldID"].Value);
                            metadata.SummonPortal.PortalID = string.IsNullOrEmpty(node.Attributes["portalID"]?.Value) ? 0 : int.Parse(node.Attributes["portalID"].Value);
                        }
                        else if (node.Name == "eventMission")
                        {
                            metadata.Event = node.Attributes["event"].Value;
                        }
                        else if (node.Name == "condition")
                        {
                            string type = node.Attributes["type"].Value;
                            string[] codes = node.Attributes["code"]?.Value.Split(",");
                            int value = string.IsNullOrEmpty(node.Attributes["value"]?.Value) ? 0 : int.Parse(node.Attributes["value"].Value);
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
                            int naviMap = string.IsNullOrEmpty(node.Attributes["map"]?.Value) ? 0 : int.Parse(node.Attributes["map"].Value);

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
