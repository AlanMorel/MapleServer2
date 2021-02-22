using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Linq;
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
                metadata.NpcMetadataBasic.Class = byte.Parse(npcBasicNode.Attributes["class"].Value);
                metadata.NpcMetadataBasic.Kind = ushort.Parse(npcBasicNode.Attributes["kind"].Value);
                metadata.NpcMetadataBasic.HpBar = byte.Parse(npcBasicNode.Attributes["hpBar"].Value);

                metadata.Stats = GetNpcStats(statsCollection);
                metadata.SkillIds = string.IsNullOrEmpty(npcSkillIdsNode.Attributes["ids"].Value) ? Array.Empty<int>() : Array.ConvertAll(npcSkillIdsNode.Attributes["ids"].Value.Split(","), int.Parse);
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

            npcStats.Str.Total = int.Parse(collection["str"].Value);
            npcStats.Dex.Total = int.Parse(collection["dex"].Value);
            npcStats.Int.Total = int.Parse(collection["int"].Value);
            npcStats.Luk.Total = int.Parse(collection["luk"].Value);
            npcStats.Hp.Total = long.Parse(collection["hp"].Value);
            npcStats.HpRegen.Total = int.Parse(collection["hp_rgp"].Value);
            npcStats.HpInv.Total = int.Parse(collection["hp_inv"].Value);
            npcStats.Sp.Total = int.Parse(collection["sp"].Value);
            npcStats.SpRegen.Total = int.Parse(collection["sp_rgp"].Value);
            npcStats.SpInv.Total = int.Parse(collection["sp_inv"].Value);
            npcStats.Ep.Total = int.Parse(collection["ep"].Value);
            npcStats.EpRegen.Total = int.Parse(collection["ep_rgp"].Value);
            npcStats.EpInv.Total = int.Parse(collection["ep_inv"].Value);
            npcStats.AtkSpd.Total = int.Parse(collection["asp"].Value);
            npcStats.MoveSpd.Total = int.Parse(collection["msp"].Value);
            npcStats.Attack.Total = int.Parse(collection["atp"].Value);
            npcStats.Evasion.Total = int.Parse(collection["evp"].Value);
            npcStats.Cap.Total = int.Parse(collection["cap"].Value);
            npcStats.Cad.Total = int.Parse(collection["cad"].Value);
            npcStats.Car.Total = int.Parse(collection["car"].Value);
            npcStats.Ndd.Total = int.Parse(collection["ndd"].Value);
            npcStats.Abp.Total = int.Parse(collection["abp"].Value);
            npcStats.JumpHeight.Total = int.Parse(collection["jmp"].Value);
            npcStats.PhysAtk.Total = int.Parse(collection["pap"].Value);
            npcStats.MagAtk.Total = int.Parse(collection["map"].Value);
            npcStats.PhysRes.Total = int.Parse(collection["par"].Value);
            npcStats.MagRes.Total = int.Parse(collection["mar"].Value);
            npcStats.MinAtk.Total = int.Parse(collection["wapmin"].Value);
            npcStats.MaxAtk.Total = int.Parse(collection["wapmax"].Value);
            npcStats.Damage.Total = int.Parse(collection["dmg"].Value);
            npcStats.Pierce.Total = int.Parse(collection["pen"].Value);
            npcStats.MountSpeed.Total = int.Parse(collection["rmsp"].Value);

            return npcStats;
        }
    }
}
