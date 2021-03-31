using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ScriptParser : Exporter<List<ScriptMetadata>>
    {
        public ScriptParser(MetadataResources resources) : base(resources, "script") { }

        protected override List<ScriptMetadata> Parse()
        {
            List<ScriptMetadata> entities = ParseNpc(Resources);
            entities.AddRange(ParseQuest(Resources));
            return entities;
        }

        private static List<ScriptMetadata> ParseNpc(MetadataResources resources)
        {
            List<ScriptMetadata> scripts = new List<ScriptMetadata>();
            foreach (PackFileEntry entry in resources.XmlFiles)
            {

                if (!entry.Name.StartsWith("script/npc"))
                {
                    continue;
                }

                ScriptMetadata metadata = new ScriptMetadata();
                int npcId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));
                XmlDocument document = resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "monologue")
                    {
                        continue;
                    }
                    ScriptType type = ScriptType.Script;
                    List<int> gotoList = new List<int>();
                    List<int> gotoFailList = new List<int>();
                    if (node.Name == "select")
                    {
                        type = ScriptType.Select;
                    }
                    int id = int.Parse(node.Attributes["id"].Value);
                    int amountContent = 0;
                    if (type == ScriptType.Script)
                    {
                        foreach (XmlNode content in node.ChildNodes)
                        {
                            if (content.Name != "content")
                            {
                                continue;
                            }
                            amountContent++;

                            foreach (XmlNode distractor in content.ChildNodes)
                            {
                                if (distractor.Name != "distractor")
                                {
                                    continue;
                                }

                                if (!string.IsNullOrEmpty(distractor.Attributes["goto"]?.Value))
                                {
                                    gotoList.AddRange(distractor.Attributes["goto"].Value.Split(",").Select(int.Parse).ToList());
                                }
                                if (!string.IsNullOrEmpty(distractor.Attributes["gotoFail"]?.Value))
                                {
                                    gotoFailList.AddRange(distractor.Attributes["gotoFail"].Value.Split(",").Select(int.Parse).ToList());
                                }
                            }
                        }
                    }
                    metadata.Options.Add(new Option(type, id, gotoList, gotoFailList, amountContent));
                }

                metadata.Id = npcId;
                scripts.Add(metadata);
            }

            return scripts;
        }

        private static List<ScriptMetadata> ParseQuest(MetadataResources resources)
        {
            List<ScriptMetadata> scripts = new List<ScriptMetadata>();
            foreach (PackFileEntry entry in resources.XmlFiles)
            {

                if (!entry.Name.StartsWith("script/quest"))
                {
                    continue;
                }
                string filename = Path.GetFileNameWithoutExtension(entry.Name);
                if (filename.Contains("eventjp") || filename.Contains("eventkr") || filename.Contains("eventcn"))
                {
                    continue;
                }

                XmlDocument document = resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode questNode in document.DocumentElement.ChildNodes)
                {
                    ScriptMetadata metadata = new ScriptMetadata();
                    int questId = int.Parse(questNode.Attributes["id"].Value);
                    foreach (XmlNode script in questNode.ChildNodes)
                    {
                        List<int> gotoList = new List<int>();
                        List<int> gotoFailList = new List<int>();
                        int id = int.Parse(script.Attributes["id"].Value);
                        int amountContent = 0;
                        foreach (XmlNode content in script.ChildNodes)
                        {
                            if (content.Name != "content")
                            {
                                continue;
                            }
                            amountContent++;

                            foreach (XmlNode distractor in content.ChildNodes)
                            {
                                if (distractor.Name != "distractor")
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(distractor.Attributes["goto"]?.Value))
                                {
                                    gotoList.AddRange(distractor.Attributes["goto"].Value.Split(",").Select(int.Parse).ToList());
                                }
                                if (!string.IsNullOrEmpty(distractor.Attributes["gotoFail"]?.Value))
                                {
                                    gotoFailList.AddRange(distractor.Attributes["gotoFail"].Value.Split(",").Select(int.Parse).ToList());
                                }
                            }
                        }
                        metadata.Options.Add(new Option(ScriptType.Script, id, gotoList, gotoFailList, amountContent));
                    }
                    metadata.Id = questId;
                    metadata.IsQuestScript = true;
                    scripts.Add(metadata);
                }
            }
            return scripts;
        }
    }
}
