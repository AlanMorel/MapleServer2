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

            // Parse skill id order
            XmlAttribute oAttr = xmlDoc.DocumentElement.Attributes["order"];
            int[] order = oAttr != null ? Array.ConvertAll(oAttr.Value.Split(","), Int32.Parse) : null;

            // Create new skill tab with name and skill order
            SkillTab skillTab = new SkillTab(name, order);

            // Parse skills and add to skillTab
            XmlNodeList skills = xmlDoc.SelectNodes("/ms2/key");
            IEnumerator ienum = skills.GetEnumerator();
            while (ienum.MoveNext())
            {
                XmlNode currentNode = (XmlNode)ienum.Current;

                // Skill id
                int id = Int32.Parse(currentNode.Attributes["id"].Value);
                //Default
                XmlAttribute dAttr = currentNode.Attributes["default"];
                int DefaultSkill = dAttr != null ? Int32.Parse(dAttr.Value) : 0;
                // Skill feature (awakening)
                XmlAttribute fAttr = currentNode.Attributes["feature"];
                string feature = fAttr != null ? fAttr.Value : "";
                // Skill sub skills
                XmlAttribute subAttr = currentNode.Attributes["sub"];
                int[] sub = subAttr != null ? Array.ConvertAll(subAttr.Value.Split(","), Int32.Parse) : null;

                skillTab.AddOrUpdate(new Skill(id, 1, 0, feature, sub));
            }

            return skillTab;
        }
    }
}