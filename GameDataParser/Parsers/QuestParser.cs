using System.Xml;
using GameDataParser.Files;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class QuestParser : Exporter<List<QuestMetadata>>
{
    public QuestParser(MetadataResources resources) : base(resources, MetadataName.Quest) { }

    protected override List<QuestMetadata> Parse()
    {
        List<QuestMetadata> quests = new();
        Dictionary<int, string> questNames = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("string/en/questdescription_"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            foreach (XmlNode quest in document.DocumentElement.ChildNodes)
            {
                int id = int.Parse(quest.Attributes["questID"].Value);
                string name = quest.Attributes["name"].Value;
                questNames[id] = name;
            }
        }

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

                QuestMetadata metadata = new();
                XmlNodeList quest = envi.ChildNodes[0].ChildNodes;
                foreach (XmlNode node in quest)
                {
                    switch (node.Name)
                    {
                        case "basic":
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
                            metadata.Basic.UseNavigation = node.Attributes["useNavi"].Value != "FALSE";
                            break;
                        case "notify":
                            metadata.Notify.CompleteUiEffect = node.Attributes["completeUiEffect"].Value;
                            metadata.Notify.AcceptSoundKey = node.Attributes["acceptSoundKey"].Value;
                            metadata.Notify.CompleteSoundKey = node.Attributes["completeSoundKey"].Value;
                            break;
                        case "require":
                            metadata.Require.Level = short.Parse(node.Attributes["level"]?.Value ?? "0");
                            metadata.Require.MaxLevel = short.Parse(node.Attributes["maxLevel"]?.Value ?? "0");

                            metadata.Require.Job = node.Attributes["job"]?.Value.SplitAndParseToShort(',').ToList();
                            metadata.Require.RequiredQuests = node.Attributes["quest"]?.Value.SplitAndParseToInt(',').ToList();
                            metadata.Require.SelectableQuest = node.Attributes["selectableQuest"]?.Value.SplitAndParseToInt(',').ToList();
                            metadata.Require.Unrequire = node.Attributes["unrequire"]?.Value.SplitAndParseToInt(',').ToList();

                            _ = int.TryParse(node.Attributes["field"]?.Value ?? "0", out metadata.Require.Field);
                            metadata.Require.Achievement = int.Parse(node.Attributes["achievement"]?.Value ?? "0");

                            metadata.Require.UnreqAchievement = node.Attributes["unreqAchievement"]?.Value.SplitAndParseToInt(',').ToList();

                            metadata.Require.GroupID = int.Parse(node.Attributes["groupID"]?.Value ?? "0");
                            metadata.Require.DayOfWeek = node.Attributes["dayOfWeek"].Value;
                            metadata.Require.GearScore = int.Parse(node.Attributes["gearScore"]?.Value ?? "0");
                            break;
                        case "start":
                            metadata.StartNpc = int.Parse(node.Attributes["npc"].Value);
                            break;
                        case "complete":
                            metadata.CompleteNpc = int.Parse(node.Attributes["npc"].Value);
                            break;
                        case "completeReward":
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

                                QuestRewardItem item = new(itemId, rank, count);
                                metadata.RewardItem.Add(item);
                            }

                            break;
                        case "progressMap":
                            metadata.ProgressMap = node.Attributes["progressMap"]?.Value.SplitAndParseToInt(',').ToList();
                            break;
                        case "guide":
                            metadata.Guide.Type = node.Attributes["guideType"].Value;
                            metadata.Guide.Icon = node.Attributes["guideIcon"].Value;
                            metadata.Guide.MinLevel = byte.Parse(node.Attributes["guideMinLevel"]?.Value ?? "0");
                            metadata.Guide.MaxLevel = byte.Parse(node.Attributes["guideMaxLevel"]?.Value ?? "0");
                            metadata.Guide.Group = node.Attributes["guideGroup"]?.Value;
                            break;
                        case "gotoNpc":
                            metadata.Npc.Enable = byte.Parse(node.Attributes["enable"]?.Value ?? "0");
                            metadata.Npc.GoToField = int.Parse(node.Attributes["gotoField"]?.Value ?? "0");
                            metadata.Npc.GoToPortal = int.Parse(node.Attributes["gotoPortal"]?.Value ?? "0");
                            break;
                        case "gotoDungeon":
                            metadata.Dungeon.State = byte.Parse(node.Attributes["state"]?.Value ?? "0");
                            metadata.Dungeon.GoToDungeon = int.Parse(node.Attributes["gotoDungeon"]?.Value ?? "0");
                            metadata.Dungeon.GoToInstanceID = int.Parse(node.Attributes["gotoInstanceID"]?.Value ?? "0");
                            break;
                        case "remoteAccept":
                            metadata.RemoteAccept.UseRemote = node.Attributes["useRemote"].Value;
                            metadata.RemoteAccept.RequireField = int.Parse(node.Attributes["requireField"]?.Value ?? "0");
                            break;
                        case "remoteComplete":
                            metadata.RemoteComplete.UseRemote = node.Attributes["useRemote"].Value;
                            metadata.RemoteComplete.RequireField = int.Parse(node.Attributes["requireField"]?.Value ?? "0");
                            metadata.RemoteComplete.RequireDungeonClear = int.Parse(node.Attributes["requireDungeonClear"]?.Value ?? "0");
                            break;
                        case "summonPortal":
                            metadata.SummonPortal.FieldID = int.Parse(node.Attributes["fieldID"]?.Value ?? "0");
                            metadata.SummonPortal.PortalID = int.Parse(node.Attributes["portalID"]?.Value ?? "0");
                            break;
                        case "eventMission":
                            metadata.Event = node.Attributes["event"].Value;
                            break;
                        case "condition":
                            string type = node.Attributes["type"].Value;
                            string code = node.Attributes["code"]?.Value ?? "0";
                            int value = int.Parse(node.Attributes["value"]?.Value ?? "0");
                            string target = node.Attributes["target"]?.Value ?? "0";

                            metadata.Condition.Add(new(type, code, value, target));
                            break;
                        case "navi":
                            string naviType = node.Attributes["type"].Value;
                            string naviCode = node.Attributes["code"].Value;
                            int naviMap = int.Parse(node.Attributes["map"]?.Value ?? "0");

                            metadata.Navigation.Add(new(naviType, naviCode, naviMap));
                            break;
                        case "dispatch":
                            string portal = node.Attributes["portal"]?.Value;
                            string script = node.Attributes["script"]?.Value;

                            metadata.Dispatch.Type = node.Attributes["type"].Value;
                            metadata.Dispatch.FieldId = int.Parse(node.Attributes["field"]?.Value ?? "0");
                            metadata.Dispatch.PortalId = string.IsNullOrEmpty(portal) ? (short) 0 : short.Parse(portal);
                            metadata.Dispatch.ScriptId = string.IsNullOrEmpty(script) ? 0 : int.Parse(script);
                            break;
                    }
                }

                questNames.TryGetValue(metadata.Basic.Id, out metadata.Name);
                quests.Add(metadata);
            }
        }

        return quests;
    }
}
