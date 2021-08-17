using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
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
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/expbasetable"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
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
            foreach (PackFileEntry entry in Resources.XmlReader.Files.Where(entry => entry.Name.Equals("string/en/npcname.xml")))
            {
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);

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
            foreach (PackFileEntry entry in Resources.XmlReader.Files.Where(entry => entry.Name.StartsWith("npc/")))
            {
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);

                XmlNode npcModelNode = document.SelectSingleNode("ms2/environment/model") ?? document.SelectSingleNode("ms2/model");
                XmlNode npcBasicNode = document.SelectSingleNode("ms2/environment/basic") ?? document.SelectSingleNode("ms2/basic");
                XmlNode npcStatsNode = document.SelectSingleNode("ms2/environment/stat") ?? document.SelectSingleNode("ms2/stat");
                XmlNode npcSpeedNode = document.SelectSingleNode("ms2/environment/speed") ?? document.SelectSingleNode("ms2/speed");
                XmlNode npcDistanceNode = document.SelectSingleNode("ms2/environment/distance") ?? document.SelectSingleNode("ms2/distance");
                XmlNode npcSkillNode = document.SelectSingleNode("ms2/environment/skill") ?? document.SelectSingleNode("ms2/skill");
                XmlNode npcExpNode = document.SelectSingleNode("ms2/environment/exp") ?? document.SelectSingleNode("ms2/exp");
                XmlNode npcAiInfoNode = document.SelectSingleNode("ms2/environment/aiInfo") ?? document.SelectSingleNode("ms2/aiInfo");
                XmlNode npcNormalNode = document.SelectSingleNode("ms2/environment/normal") ?? document.SelectSingleNode("ms2/normal");
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
                metadata.WalkSpeed = float.Parse(npcSpeedNode.Attributes["walk"]?.Value ?? "0");
                metadata.RunSpeed = float.Parse(npcSpeedNode.Attributes["run"]?.Value ?? "0");

                // Parse distance
                // metadata.AttackRange = ;
                // metadata.AggroRange = ;
                // metadata.AggroRangeUp = ;
                // metadata.AggroRangeDown = ;

                // Parse skill
                metadata.SkillIds = string.IsNullOrEmpty(npcSkillNode.Attributes["ids"].Value) ? Array.Empty<int>() : Array.ConvertAll(npcSkillNode.Attributes["ids"].Value.Split(","), int.Parse);
                if (metadata.SkillIds.Length > 0)
                {
                    metadata.SkillLevels = string.IsNullOrEmpty(npcSkillNode.Attributes["levels"].Value) ? Array.Empty<byte>() : Array.ConvertAll(npcSkillNode.Attributes["levels"].Value.Split(","), byte.Parse);
                    metadata.SkillPriorities = string.IsNullOrEmpty(npcSkillNode.Attributes["priorities"].Value) ? Array.Empty<byte>() : Array.ConvertAll(npcSkillNode.Attributes["priorities"].Value.Split(","), byte.Parse);
                    metadata.SkillProbs = string.IsNullOrEmpty(npcSkillNode.Attributes["probs"].Value) ? Array.Empty<short>() : Array.ConvertAll(npcSkillNode.Attributes["probs"].Value.Split(","), short.Parse);
                    metadata.SkillCooldown = short.Parse(npcSkillNode.Attributes["coolDown"].Value);
                }

                // Parse normal state
                List<(string, NpcAction, short)> normalActions = new List<(string, NpcAction, short)>();
                string[] normalActionIds = string.IsNullOrEmpty(npcNormalNode.Attributes["action"].Value) ? Array.Empty<string>() : npcNormalNode.Attributes["action"].Value.Split(",");
                if (normalActionIds.Length > 0)
                {
                    short[] actionProbs = string.IsNullOrEmpty(npcNormalNode.Attributes["prob"].Value) ? Array.Empty<short>() : Array.ConvertAll(npcNormalNode.Attributes["prob"].Value.Split(","), short.Parse);
                    for (int i = 0; i < normalActionIds.Length; i++)
                    {
                        normalActions.Add((normalActionIds[i], GetNpcAction(normalActionIds[i]), actionProbs[i]));
                    }
                    metadata.StateActions[NpcState.Normal] = normalActions.ToArray();
                }
                metadata.MoveRange = short.Parse(npcNormalNode.Attributes["movearea"]?.Value ?? "0");

                // Parse dead state
                List<(string, NpcAction, short)> deadActions = new List<(string, NpcAction, short)>();
                string[] deadActionIds = string.IsNullOrEmpty(npcDeadNode.Attributes["defaultaction"].Value) ? Array.Empty<string>() : npcDeadNode.Attributes["defaultaction"].Value.Split(",");
                if (deadActionIds.Length > 0)
                {
                    int equalProb = 10000 / deadActionIds.Length;
                    int remainder = 10000 % (equalProb * deadActionIds.Length);
                    deadActions.Add((deadActionIds[0], GetNpcAction(deadActionIds[0]), (short) (equalProb + remainder)));
                    for (int i = 1; i < deadActionIds.Length; i++)
                    {
                        deadActions.Add((deadActionIds[i], GetNpcAction(deadActionIds[i]), (short) equalProb));
                    }
                    metadata.StateActions[NpcState.Dead] = deadActions.ToArray();
                }

                metadata.AiInfo = npcAiInfoNode.Attributes["path"].Value;
                int customExpValue = int.Parse(npcExpNode.Attributes["customExp"].Value);
                metadata.Experience = (customExpValue >= 0) ? customExpValue : (int) levelExp[metadata.Level].Experience;
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

        private static NpcAction GetNpcAction(string name)
        {
            string actionName = name.Split('_')[0];
            return actionName switch
            {
                "Idle" => NpcAction.Idle,
                "Bore" => NpcAction.Bore,
                "Walk" => NpcAction.Walk,
                "Run" => NpcAction.Run,
                "Dead" => NpcAction.Dead,
                _ => NpcAction.None
            };
        }
    }
}
