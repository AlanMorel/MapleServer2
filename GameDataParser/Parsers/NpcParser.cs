using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class NpcParser : Exporter<List<NpcMetadata>>
    {
        public NpcParser(MetadataResources resources) : base(resources, "npc") { }

        protected override List<NpcMetadata> Parse()
        {
            // Parse EXP tables
            Dictionary<int, ExpMetadata> levelExp = new Dictionary<int, ExpMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/expbasetable"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "table")
                    {
                        if (int.Parse(node.Attributes["expTableID"].Value) != 1)
                        {
                            continue;
                        }
                        foreach (XmlNode tableNode in node.ChildNodes)
                        {
                            if (tableNode.Name == "base")
                            {
                                ExpMetadata expTable = new ExpMetadata();

                                byte level = byte.Parse(tableNode.Attributes["level"].Value);
                                if (level != 0)
                                {
                                    expTable.Level = level;
                                    expTable.Experience = long.Parse(tableNode.Attributes["exp"].Value);
                                    levelExp[level] = expTable;
                                }
                            }
                        }
                    }
                }
            }

            Dictionary<int, string> npcIdToName = new Dictionary<int, string> { };
            List<NpcMetadata> npcs = new List<NpcMetadata>();

            // Parse the NpcId -> Names first.
            foreach (PackFileEntry entry in Resources.XmlFiles.Where(entry => entry.Name.Equals("string/en/npcname.xml")))
            {
                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);

                foreach (XmlNode node in document.SelectNodes("ms2/key"))
                {
                    int id = int.Parse(node.Attributes["id"].Value);
                    if (!npcIdToName.ContainsKey(id))
                    {
                        npcIdToName.Add(id, node.Attributes["name"].Value);
                    }
                }
            }

            // Handle /npc files second, to setup the NpcMetadata
            foreach (PackFileEntry entry in Resources.XmlFiles.Where(entry => entry.Name.StartsWith("npc/")))
            {
                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);

                XmlNode npcModelNode = document.SelectSingleNode("ms2/environment/model") ?? document.SelectSingleNode("ms2/model");
                XmlNode npcBasicNode = document.SelectSingleNode("ms2/environment/basic") ?? document.SelectSingleNode("ms2/basic");
                XmlNode npcStatsNode = document.SelectSingleNode("ms2/environment/stat") ?? document.SelectSingleNode("ms2/stat");
                XmlNode npcSkillIdsNode = document.SelectSingleNode("ms2/environment/skill") ?? document.SelectSingleNode("ms2/skill");
                XmlNode npcExpNode = document.SelectSingleNode("ms2/environment/exp") ?? document.SelectSingleNode("ms2/exp");
                XmlNode npcAiInfoNode = document.SelectSingleNode("ms2/environment/aiInfo") ?? document.SelectSingleNode("ms2/aiInfo");
                XmlNode npcDeadNode = document.SelectSingleNode("ms2/environment/dead") ?? document.SelectSingleNode("ms2/dead");
                XmlNode npcDropItemNode = document.SelectSingleNode("ms2/environment/dropiteminfo") ?? document.SelectSingleNode("ms2/dropiteminfo");
                XmlAttributeCollection statsCollection = npcStatsNode.Attributes;

                // Metadata
                NpcMetadata metadata = new NpcMetadata();
                metadata.Id = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));
                metadata.Name = npcIdToName.ContainsKey(metadata.Id) ? npcIdToName[metadata.Id] : "";
                metadata.Model = npcModelNode.Attributes["kfm"].Value;

                // Parse basic attribs.
                metadata.TemplateId = int.TryParse(npcBasicNode.Attributes["illust"]?.Value, out _) ? int.Parse(npcBasicNode.Attributes["illust"].Value) : 0;
                metadata.Friendly = byte.Parse(npcBasicNode.Attributes["friendly"].Value);
                metadata.Level = byte.Parse(npcBasicNode.Attributes["level"].Value);
                if (npcBasicNode.Attributes["npcAttackGroup"] != null)
                {
                    metadata.NpcMetadataBasic.NpcAttackGroup = sbyte.Parse(npcBasicNode.Attributes["npcAttackGroup"].Value);
                }
                if (npcBasicNode.Attributes["npcDefenseGroup"] != null)
                {
                    metadata.NpcMetadataBasic.NpcDefenseGroup = sbyte.Parse(npcBasicNode.Attributes["npcDefenseGroup"].Value);
                }
                if (npcBasicNode.Attributes["difficulty"] != null)
                {
                    metadata.NpcMetadataBasic.Difficulty = ushort.Parse(npcBasicNode.Attributes["difficulty"].Value);
                }
                if (npcBasicNode.Attributes["maxSpawnCount"] != null)
                {
                    metadata.NpcMetadataBasic.MaxSpawnCount = byte.Parse(npcBasicNode.Attributes["maxSpawnCount"].Value);
                }
                if (npcBasicNode.Attributes["groupSpawnCount"] != null)
                {
                    metadata.NpcMetadataBasic.GroupSpawnCount = byte.Parse(npcBasicNode.Attributes["groupSpawnCount"].Value);
                }
                metadata.NpcMetadataBasic.MainTags = string.IsNullOrEmpty(npcBasicNode.Attributes["mainTags"].Value) ? Array.Empty<string>() : npcBasicNode.Attributes["mainTags"].Value.Split(",").Select(p => p.Trim()).ToArray();
                metadata.NpcMetadataBasic.SubTags = string.IsNullOrEmpty(npcBasicNode.Attributes["subTags"].Value) ? Array.Empty<string>() : npcBasicNode.Attributes["subTags"].Value.Split(",").Select(p => p.Trim()).ToArray();
                metadata.NpcMetadataBasic.Class = byte.Parse(npcBasicNode.Attributes["class"].Value);
                metadata.NpcMetadataBasic.Kind = ushort.Parse(npcBasicNode.Attributes["kind"].Value);
                metadata.NpcMetadataBasic.HpBar = byte.Parse(npcBasicNode.Attributes["hpBar"].Value);

                metadata.Stats = GetNpcStats(statsCollection);
                metadata.SkillIds = string.IsNullOrEmpty(npcSkillIdsNode.Attributes["ids"].Value) ? Array.Empty<int>() : Array.ConvertAll(npcSkillIdsNode.Attributes["ids"].Value.Split(","), int.Parse);
                int customExpValue = int.Parse(npcExpNode.Attributes["customExp"].Value);
                metadata.Experience = (customExpValue > 0) ? customExpValue : (int) levelExp[metadata.Level].Experience;
                metadata.AiInfo = npcAiInfoNode.Attributes["path"].Value;
                metadata.NpcMetadataDead.Time = float.Parse(npcDeadNode.Attributes["time"].Value);
                metadata.NpcMetadataDead.Actions = npcDeadNode.Attributes["defaultaction"].Value.Split(",");
                metadata.GlobalDropBoxIds = string.IsNullOrEmpty(npcDropItemNode.Attributes["globalDropBoxId"].Value) ? Array.Empty<int>() : Array.ConvertAll(npcDropItemNode.Attributes["globalDropBoxId"].Value.Split(","), int.Parse);
                metadata.Kind = short.Parse(npcBasicNode.Attributes["kind"].Value);
                metadata.ShopId = int.Parse(npcBasicNode.Attributes["shopId"].Value);
                npcs.Add(metadata);
            }

            return npcs;
        }

        private static NpcStats GetNpcStats(XmlAttributeCollection collection)
        {
            // MUST be in ORDER
            NpcStats npcStats = new NpcStats();

            npcStats.Str = new NpcStat<int>(int.Parse(collection["str"].Value));
            npcStats.Dex = new NpcStat<int>(int.Parse(collection["dex"].Value));
            npcStats.Int = new NpcStat<int>(int.Parse(collection["int"].Value));
            npcStats.Luk = new NpcStat<int>(int.Parse(collection["luk"].Value));
            npcStats.Hp = new NpcStat<long>(long.Parse(collection["hp"].Value));
            npcStats.HpRegen = new NpcStat<int>(int.Parse(collection["hp_rgp"].Value));
            npcStats.HpInv = new NpcStat<int>(int.Parse(collection["hp_inv"].Value));
            npcStats.Sp = new NpcStat<int>(int.Parse(collection["sp"].Value));
            npcStats.SpRegen = new NpcStat<int>(int.Parse(collection["sp_rgp"].Value));
            npcStats.SpInv = new NpcStat<int>(int.Parse(collection["sp_inv"].Value));
            npcStats.Ep = new NpcStat<int>(int.Parse(collection["ep"].Value));
            npcStats.EpRegen = new NpcStat<int>(int.Parse(collection["ep_rgp"].Value));
            npcStats.EpInv = new NpcStat<int>(int.Parse(collection["ep_inv"].Value));
            npcStats.AtkSpd = new NpcStat<int>(int.Parse(collection["asp"].Value));
            npcStats.MoveSpd = new NpcStat<int>(int.Parse(collection["msp"].Value));
            npcStats.Accuracy = new NpcStat<int>(int.Parse(collection["atp"].Value));
            npcStats.Evasion = new NpcStat<int>(int.Parse(collection["evp"].Value));
            npcStats.Cap = new NpcStat<int>(int.Parse(collection["cap"].Value));
            npcStats.Cad = new NpcStat<int>(int.Parse(collection["cad"].Value));
            npcStats.Car = new NpcStat<int>(int.Parse(collection["car"].Value));
            npcStats.Ndd = new NpcStat<int>(int.Parse(collection["ndd"].Value));
            npcStats.Abp = new NpcStat<int>(int.Parse(collection["abp"].Value));
            npcStats.JumpHeight = new NpcStat<int>(int.Parse(collection["jmp"].Value));
            npcStats.PhysAtk = new NpcStat<int>(int.Parse(collection["pap"].Value));
            npcStats.MagAtk = new NpcStat<int>(int.Parse(collection["map"].Value));
            npcStats.PhysRes = new NpcStat<int>(int.Parse(collection["par"].Value));
            npcStats.MagRes = new NpcStat<int>(int.Parse(collection["mar"].Value));
            npcStats.MinAtk = new NpcStat<int>(int.Parse(collection["wapmin"].Value));
            npcStats.MaxAtk = new NpcStat<int>(int.Parse(collection["wapmax"].Value));
            npcStats.Damage = new NpcStat<int>(int.Parse(collection["dmg"].Value));
            npcStats.Pierce = new NpcStat<int>(int.Parse(collection["pen"].Value));
            npcStats.MountSpeed = new NpcStat<int>(int.Parse(collection["rmsp"].Value));

            return npcStats;
        }
    }
}
