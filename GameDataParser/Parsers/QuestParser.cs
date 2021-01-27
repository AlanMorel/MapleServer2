using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace GameDataParser.Parsers
{
    public static class QuestParser
    {
        public static List<QuestMetadata> Parse(MemoryMappedFile m2dFile, IEnumerable<PackFileEntry> entries)
        {
            List<QuestMetadata> quests = new List<QuestMetadata>();
            foreach (PackFileEntry entry in entries)
            {

                if (!entry.Name.StartsWith("quest/"))
                {
                    continue;
                }

                QuestMetadata metadata = new QuestMetadata();

                using XmlReader reader = m2dFile.GetReader(entry.FileHeader);
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }

                    if (reader.Name == "environment" && (reader["locale"] != "NA" && reader["locale"] != "")) // only reading quests for NA or all environments
                    {
                        reader.Skip();
                    }
                    else if (reader.Name == "environment")
                    {
                        metadata.Feature = reader["feature"];
                        metadata.Locale = reader["locale"];
                    }
                    else if (reader.Name == "basic")
                    {
                        metadata.Basic.ChapterID = string.IsNullOrEmpty(reader["chapterID"]) ? 0 : int.Parse(reader["chapterID"]);
                        metadata.Basic.QuestID = string.IsNullOrEmpty(reader["questID"]) ? 0 : int.Parse(reader["questID"]);
                        metadata.Basic.QuestType = string.IsNullOrEmpty(reader["questType"]) ? 0 : byte.Parse(reader["questType"]);
                        metadata.Basic.Account = string.IsNullOrEmpty(reader["account"]) ? 0 : byte.Parse(reader["account"]);
                        metadata.Basic.StandardLevel = string.IsNullOrEmpty(reader["standardLevel"]) ? 0 : int.Parse(reader["standardLevel"]);
                        metadata.Basic.AutoStart = string.IsNullOrEmpty(reader["autoStart"]) ? 0 : byte.Parse(reader["autoStart"]);
                        metadata.Basic.DisableGiveup = string.IsNullOrEmpty(reader["disableGiveup"]) ? 0 : byte.Parse(reader["disableGiveup"]);
                        metadata.Basic.ExceptChapterClear = string.IsNullOrEmpty(reader["exceptChapterClear"]) ? 0 : int.Parse(reader["exceptChapterClear"]);
                        metadata.Basic.Repeatable = string.IsNullOrEmpty(reader["repeatable"]) ? 0 : byte.Parse(reader["repeatable"]);
                        metadata.Basic.UsePeriod = reader["usePeriod"];
                        metadata.Basic.EventTag = reader["eventTag"];
                        metadata.Basic.Locking = string.IsNullOrEmpty(reader["locking"]) ? 0 : byte.Parse(reader["locking"]);
                        metadata.Basic.TabIndex = string.IsNullOrEmpty(reader["tabIndex"]) ? 0 : int.Parse(reader["tabIndex"]);
                        metadata.Basic.ForceRegistGuide = string.IsNullOrEmpty(reader["forceRegistGuide"]) ? 0 : byte.Parse(reader["forceRegistGuide"]);
                        metadata.Basic.UseNavigation = reader["useNavi"] == "FALSE" ? false : true;
                    }
                    else if (reader.Name == "notify")
                    {
                        metadata.Notify.CompleteUiEffect = reader["completeUiEffect"];
                        metadata.Notify.AcceptSoundKey = reader["acceptSoundKey"];
                        metadata.Notify.CompleteSoundKey = reader["completeSoundKey"];
                    }
                    else if (reader.Name == "require")
                    {
                        metadata.Require.Level = string.IsNullOrEmpty(reader["level"]) ? 0 : short.Parse(reader["level"]);
                        metadata.Require.MaxLevel = string.IsNullOrEmpty(reader["maxLevel"]) ? 0 : short.Parse(reader["maxLevel"]);

                        if (!string.IsNullOrEmpty(reader["job"]))
                        {
                            List<string> temp = new List<string>(reader["job"].Split(","));
                            foreach (string item in temp)
                            {
                                metadata.Require.Job.Add(short.Parse(item));
                            }
                        }

                        if (!string.IsNullOrEmpty(reader["quest"]))
                        {
                            List<string> temp = new List<string>(reader["quest"].Split(","));
                            foreach (string item in temp)
                            {
                                metadata.Require.RequiredQuests.Add(int.Parse(item));
                            }
                        }

                        if (!string.IsNullOrEmpty(reader["selectableQuest"]))
                        {
                            List<string> temp = new List<string>(reader["selectableQuest"].Split(","));
                            foreach (string item in temp)
                            {
                                metadata.Require.SelectableQuest.Add(int.Parse(item));
                            }
                        }

                        if (!string.IsNullOrEmpty(reader["unrequire"]))
                        {
                            List<string> temp = new List<string>(reader["unrequire"].Split(","));
                            foreach (string item in temp)
                            {
                                metadata.Require.Unrequire.Add(int.Parse(item));
                            }
                        }

                        metadata.Require.Field = string.IsNullOrEmpty(reader["field"]) ? 0 : int.Parse(reader["field"]);
                        metadata.Require.Achievement = string.IsNullOrEmpty(reader["achievement"]) ? 0 : int.Parse(reader["achievement"]);

                        if (!string.IsNullOrEmpty(reader["unreqAchievement"]))
                        {
                            List<string> temp = new List<string>(reader["unreqAchievement"].Split(","));
                            foreach (string item in temp)
                            {
                                metadata.Require.UnreqAchievement.Add(int.Parse(item));
                            }
                        }

                        metadata.Require.GroupID = string.IsNullOrEmpty(reader["groupID"]) ? 0 : int.Parse(reader["groupID"]);
                        metadata.Require.DayOfWeek = reader["dayOfWeek"];
                        metadata.Require.GearScore = string.IsNullOrEmpty(reader["gearScore"]) ? 0 : int.Parse(reader["gearScore"]);
                    }
                    else if (reader.Name == "start")
                    {
                        metadata.StartNpc = int.Parse(reader["npc"]);
                    }
                    else if (reader.Name == "complete")
                    {
                        metadata.CompleteNpc = int.Parse(reader["npc"]);
                    }
                    else if (reader.Name == "completeReward")
                    {
                        metadata.Reward.Exp = string.IsNullOrEmpty(reader["exp"]) ? 0 : int.Parse(reader["exp"]);
                        metadata.Reward.RelativeExp = reader["relativeExp"];
                        metadata.Reward.Money = string.IsNullOrEmpty(reader["money"]) ? 0 : int.Parse(reader["money"]);
                        metadata.Reward.Karma = string.IsNullOrEmpty(reader["karma"]) ? 0 : int.Parse(reader["karma"]);
                        metadata.Reward.Lu = string.IsNullOrEmpty(reader["lu"]) ? 0 : int.Parse(reader["lu"]);
                    }
                    else if (reader.Name == "essentialJobItem" || reader.Name == "globalEssentialItem" || reader.Name == "globalEssentialJobItem")
                    {
                        int itemid = string.IsNullOrEmpty(reader["code"]) ? 0 : int.Parse(reader["code"]);
                        byte rank = string.IsNullOrEmpty(reader["rank"]) ? 0 : byte.Parse(reader["rank"]);
                        int count = string.IsNullOrEmpty(reader["count"]) ? 0 : int.Parse(reader["count"]);
                        QuestRewardItem item = new QuestRewardItem(itemid, rank, count);
                        metadata.RewardItem.Add(item);
                    }
                    else if (reader.Name == "progressMap")
                    {
                        if (!string.IsNullOrEmpty(reader["progressMap"]))
                        {
                            List<string> temp = new List<string>(reader["progressMap"].Split(","));
                            foreach (string item in temp)
                            {
                                metadata.ProgressMap.Add(int.Parse(item));
                            }
                        }
                    }
                    else if (reader.Name == "guide")
                    {
                        metadata.Guide.Type = reader["guideType"];
                        metadata.Guide.Icon = reader["guideIcon"];
                        metadata.Guide.MinLevel = string.IsNullOrEmpty(reader["guideMinLevel"]) ? 0 : byte.Parse(reader["guideMinLevel"]);
                        metadata.Guide.MaxLevel = string.IsNullOrEmpty(reader["guideMaxLevel"]) ? 0 : byte.Parse(reader["guideMaxLevel"]);
                        metadata.Guide.Group = reader["guideGroup"];
                    }
                    else if (reader.Name == "gotoNpc")
                    {
                        metadata.Npc.Enable = string.IsNullOrEmpty(reader["enable"]) ? 0 : byte.Parse(reader["enable"]);
                        metadata.Npc.GoToField = string.IsNullOrEmpty(reader["gotoField"]) ? 0 : int.Parse(reader["gotoField"]);
                        metadata.Npc.GoToPortal = string.IsNullOrEmpty(reader["gotoPortal"]) ? 0 : int.Parse(reader["gotoPortal"]);
                    }
                    else if (reader.Name == "gotoDungeon")
                    {
                        metadata.Dungeon.State = string.IsNullOrEmpty(reader["state"]) ? 0 : byte.Parse(reader["state"]);
                        metadata.Dungeon.GoToDungeon = string.IsNullOrEmpty(reader["gotoDungeon"]) ? 0 : int.Parse(reader["gotoDungeon"]);
                        metadata.Dungeon.GoToInstanceID = string.IsNullOrEmpty(reader["gotoInstanceID"]) ? 0 : int.Parse(reader["gotoInstanceID"]);
                    }
                    else if (reader.Name == "remoteAccept")
                    {
                        metadata.RemoteAccept.UseRemote = reader["useRemote"];
                        metadata.RemoteAccept.RequireField = string.IsNullOrEmpty(reader["requireField"]) ? 0 : int.Parse(reader["requireField"]);
                    }
                    else if (reader.Name == "remoteComplete")
                    {
                        metadata.RemoteComplete.UseRemote = reader["useRemote"];
                        metadata.RemoteComplete.RequireField = string.IsNullOrEmpty(reader["requireField"]) ? 0 : int.Parse(reader["requireField"]);
                        metadata.RemoteComplete.RequireDungeonClear = string.IsNullOrEmpty(reader["requireDungeonClear"]) ? 0 : int.Parse(reader["requireDungeonClear"]);
                    }
                    else if (reader.Name == "summonPortal")
                    {
                        metadata.SummonPortal.FieldID = string.IsNullOrEmpty(reader["fieldID"]) ? 0 : int.Parse(reader["fieldID"]);
                        metadata.SummonPortal.PortalID = string.IsNullOrEmpty(reader["portalID"]) ? 0 : int.Parse(reader["portalID"]);
                    }
                    else if (reader.Name == "eventMission")
                    {
                        metadata.Event = reader["event"];
                    }
                    else if (reader.Name == "condition")
                    {
                        string Type = reader["type"];
                        string Code = reader["code"];
                        int Value = string.IsNullOrEmpty(reader["value"]) ? 0 : int.Parse(reader["value"]);
                        List<string> temp = null;
                        if (!string.IsNullOrEmpty(reader["target"]))
                        {
                            temp = new List<string>(reader["target"].Split(","));

                        }
                        metadata.Condition.Add(new QuestCondition(Type, Code, Value, temp));
                    }
                    else if (reader.Name == "navi")
                    {
                        string NaviType = reader["type"];
                        string NaviCode = reader["code"];
                        int NaviMap = string.IsNullOrEmpty(reader["map"]) ? 0 : int.Parse(reader["map"]);

                        metadata.Navigation.Add(new QuestNavigation(NaviType, NaviCode, NaviMap));
                    }
                }

                quests.Add(metadata);
            }

            return quests;
        }

        public static void Write(List<QuestMetadata> entities)
        {
            using (FileStream writeStream = File.Create($"{Paths.OUTPUT}/ms2-quest-metadata"))
            {
                Serializer.Serialize(writeStream, entities);
            }
            using (FileStream readStream = File.OpenRead($"{Paths.OUTPUT}/ms2-quest-metadata"))
            {
            }
            Console.WriteLine("\rSuccessfully parsed quest metadata!");
        }
    }
}
