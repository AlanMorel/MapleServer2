using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
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
            foreach (PackFileEntry entry in resources.XmlReader.Files)
            {

                if (!entry.Name.StartsWith("script/npc"))
                {
                    continue;
                }

                ScriptMetadata metadata = new ScriptMetadata();
                int npcId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));
                XmlDocument document = resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "monologue")
                    {
                        continue;
                    }
                    // Skip locales other than NA and null
                    string locale = string.IsNullOrEmpty(node.Attributes["locale"]?.Value) ? "" : node.Attributes["locale"].Value;

                    if (locale != "NA" && locale != "")
                    {
                        continue;
                    }

                    ScriptType type = ScriptType.Script;
                    if (node.Name == "select")
                    {
                        type = ScriptType.Select;
                    }
                    int id = int.Parse(node.Attributes["id"].Value);
                    string buttonSet = node.Attributes["buttonSet"]?.Value;
                    List<Content> contents = new List<Content>();
                    if (type == ScriptType.Script)
                    {
                        foreach (XmlNode content in node.ChildNodes)
                        {
                            if (content.Name != "content")
                            {
                                continue;
                            }

                            List<int> gotoList = new List<int>();
                            List<int> gotoFailList = new List<int>();
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

                            string functionId = content.Attributes["functionID"]?.Value;
                            DialogType dialogType = string.IsNullOrEmpty(content.Attributes["buttonSet"]?.Value) ? DialogType.None : (DialogType) int.Parse(content.Attributes["buttonSet"].Value);
                            contents.Add(new Content(gotoList, gotoFailList, functionId, dialogType));
                        }
                    }
                    metadata.Options.Add(new Option(type, id, contents, buttonSet));
                }

                metadata.Id = npcId;
                scripts.Add(metadata);
            }

            return scripts;
        }

        private static List<ScriptMetadata> ParseQuest(MetadataResources resources)
        {
            List<ScriptMetadata> scripts = new List<ScriptMetadata>();
            foreach (PackFileEntry entry in resources.XmlReader.Files)
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

                XmlDocument document = resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode questNode in document.DocumentElement.ChildNodes)
                {
                    // Skip locales other than NA and null
                    string locale = string.IsNullOrEmpty(questNode.Attributes["locale"]?.Value) ? "" : questNode.Attributes["locale"].Value;

                    if (locale != "NA" && locale != "")
                    {
                        continue;
                    }

                    ScriptMetadata metadata = new ScriptMetadata();
                    int questId = int.Parse(questNode.Attributes["id"].Value);
                    string buttonSet = questNode.Attributes["buttonSet"]?.Value;
                    foreach (XmlNode script in questNode.ChildNodes)
                    {
                        int id = int.Parse(script.Attributes["id"].Value);
                        List<Content> contents = new List<Content>();
                        foreach (XmlNode content in script.ChildNodes)
                        {
                            if (content.Name != "content")
                            {
                                continue;
                            }

                            List<int> gotoList = new List<int>();
                            List<int> gotoFailList = new List<int>();
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

                            string functionId = content.Attributes["functionID"]?.Value;
                            DialogType dialogType = string.IsNullOrEmpty(content.Attributes["buttonSet"]?.Value) ? DialogType.None : (DialogType) int.Parse(content.Attributes["buttonSet"].Value);
                            contents.Add(new Content(gotoList, gotoFailList, functionId, dialogType));
                        }
                        metadata.Options.Add(new Option(ScriptType.Script, id, contents, buttonSet));
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
