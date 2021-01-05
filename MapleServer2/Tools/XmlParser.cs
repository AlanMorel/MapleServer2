using System;
using System.Xml;
using System.Collections;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public static class XmlParser
    {
        // Parses skills_{job}
        public static SkillTab ParseSkills(int job, string name = "test")
        {
            // Load skill xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load($"MapleServer2/Constants/Skills/skills_{job}.xml");

            // Parse skill id order and count split num
            XmlAttribute oAttr = xmlDoc.DocumentElement.Attributes["order"];
            int[] order = oAttr != null ? Array.ConvertAll(oAttr.Value.Split(","), int.Parse) : null;
            XmlAttribute splitAttr = xmlDoc.DocumentElement.Attributes["split"];
            byte split  = splitAttr != null ? byte.Parse(splitAttr.Value) : (byte) 8;

            // Create new skill tab with name and skill order
            SkillTab skillTab = new SkillTab(name, order, split);

            // Parse skills and add to skillTab
            XmlNodeList skills = xmlDoc.SelectNodes("/ms2/key");
            IEnumerator ienum = skills.GetEnumerator();
            while (ienum.MoveNext())
            {
                XmlNode currentNode = (XmlNode)ienum.Current;

                // Skill id
                int id = int.Parse(currentNode.Attributes["id"].Value);
                // Default
                XmlAttribute dAttr = currentNode.Attributes["default"];
                int DefaultSkill = dAttr != null ? int.Parse(dAttr.Value) : 0;
                // Skill feature (awakening)
                XmlAttribute fAttr = currentNode.Attributes["feature"];
                string feature = fAttr != null ? fAttr.Value : "";
                // Skill sub skills
                XmlAttribute subAttr = currentNode.Attributes["sub"];
                int[] sub = subAttr != null ? Array.ConvertAll(subAttr.Value.Split(","), int.Parse) : null;

                if (DefaultSkill > 0)
                {
                    skillTab.AddOrUpdate(new Skill(id, 1, 1, feature, sub));
                }
                else
                {
                    skillTab.AddOrUpdate(new Skill(id, 1, 0, feature, sub));
                }
            }

            return skillTab;
        }
    }
}