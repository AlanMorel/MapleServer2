using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GameDataParser.Parsers
{
    public class ScriptParser : Exporter<List<ScriptMetadata>>
    {
        public ScriptParser(MetadataResources resources) : base(resources, "script") { }

        protected override List<ScriptMetadata> Parse()
        {
            List<ScriptMetadata> entities = this.parseNpc(resources);
            entities.AddRange(this.parseQuest(resources));
            return entities;
        }

        private List<ScriptMetadata> parseNpc(MetadataResources resources)
        {
            List<ScriptMetadata> scripts = new List<ScriptMetadata>();
            foreach (PackFileEntry entry in resources.xmlFiles)
            {

                if (!entry.Name.StartsWith("script/npc"))
                {
                    continue;
                }

                ScriptMetadata metadata = new ScriptMetadata();
                string npcID = Path.GetFileNameWithoutExtension(entry.Name);
                XmlDocument document = resources.xmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    int id = int.Parse(node.Attributes["id"].Value);
                    string feature = node.Attributes["feature"].Value;
                    int randomPick = string.IsNullOrEmpty(node.Attributes["randomPick"]?.Value) ? 0 : int.Parse(node.Attributes["randomPick"].Value);
                    int popupState = string.IsNullOrEmpty(node.Attributes["popupState"]?.Value) ? 0 : int.Parse(node.Attributes["popupState"].Value);
                    int popupProp = string.IsNullOrEmpty(node.Attributes["popupProp"]?.Value) ? 0 : int.Parse(node.Attributes["popupProp"].Value);
                    List<int> gotoConditionTalkID = new List<int>();
                    if (!string.IsNullOrEmpty(node.Attributes["popupProp"]?.Value))
                    {
                        foreach (string item in node.Attributes["popupProp"].Value.Split(","))
                        {
                            gotoConditionTalkID.Add(int.Parse(item));
                        }
                    }

                    List<Content> contents = new List<Content>();
                    foreach (XmlNode content in node.ChildNodes)
                    {
                        long contentScriptID = string.IsNullOrEmpty(content.Attributes["text"]?.Value) ? 0 : long.Parse(content.Attributes["text"].Value.Substring(8, 16));
                        string voiceID = string.IsNullOrEmpty(content.Attributes["voiceID"]?.Value) ? "" : content.Attributes["voiceID"].Value;
                        int otherNpcTalk = string.IsNullOrEmpty(content.Attributes["otherNpcTalk"]?.Value) ? 0 : int.Parse(content.Attributes["otherNpcTalk"].Value);
                        string leftIllust = string.IsNullOrEmpty(content.Attributes["leftIllust"]?.Value) ? "" : content.Attributes["leftIllust"].Value;
                        string illust = string.IsNullOrEmpty(content.Attributes["illust"]?.Value) ? "" : content.Attributes["illust"].Value;
                        string speakerIllust = string.IsNullOrEmpty(content.Attributes["speakerIllust"]?.Value) ? "" : content.Attributes["speakerIllust"].Value;
                        bool myTalk = !string.IsNullOrEmpty(content.Attributes["myTalk"]?.Value);
                        byte functionID = string.IsNullOrEmpty(content.Attributes["functionID"]?.Value) ? 0 : byte.Parse(content.Attributes["functionID"].Value);

                        List<Distractor> distractors = new List<Distractor>();
                        List<Event> events = new List<Event>();
                        foreach (XmlNode distractor in content.ChildNodes)
                        {
                            if (distractor.Name == "event")
                            {
                                int eventid = string.IsNullOrEmpty(content.Attributes["id"]?.Value) ? 0 : int.Parse(content.Attributes["id"].Value);

                                List<Content> contents2 = new List<Content>();
                                foreach (XmlNode item in distractor.ChildNodes)
                                {
                                    long contentScriptID2 = long.Parse(item.Attributes["text"].Value.Substring(8, 16));
                                    string voiceID2 = string.IsNullOrEmpty(item.Attributes["voiceID"]?.Value) ? "" : item.Attributes["voiceID"].Value;
                                    int otherNpcTalk2 = string.IsNullOrEmpty(item.Attributes["otherNpcTalk"]?.Value) ? 0 : int.Parse(item.Attributes["otherNpcTalk"].Value);
                                    string leftIllust2 = string.IsNullOrEmpty(item.Attributes["leftIllust"]?.Value) ? "" : item.Attributes["leftIllust"].Value;
                                    string illust2 = string.IsNullOrEmpty(item.Attributes["illust"]?.Value) ? "" : item.Attributes["illust"].Value;
                                    string speakerIllust2 = string.IsNullOrEmpty(item.Attributes["speakerIllust"]?.Value) ? "" : item.Attributes["speakerIllust"].Value;
                                    bool myTalk2 = !string.IsNullOrEmpty(item.Attributes["myTalk"]?.Value);
                                    byte functionID2 = string.IsNullOrEmpty(item.Attributes["functionID"]?.Value) ? 0 : byte.Parse(item.Attributes["functionID"].Value);

                                    contents2.Add(new Content(contentScriptID2, voiceID2, functionID2, leftIllust2, speakerIllust2, otherNpcTalk2, myTalk2, illust2, null));
                                }
                                events.Add(new Event(eventid, contents2));
                            }
                            else
                            {
                                long distractorScriptID = long.Parse(distractor.Attributes["text"].Value.Substring(8, 16));
                                List<int> goTo = new List<int>();
                                if (!string.IsNullOrEmpty(distractor.Attributes["goto"]?.Value))
                                {
                                    foreach (string item in distractor.Attributes["goto"].Value.Split(","))
                                    {
                                        goTo.Add(int.Parse(item));
                                    }
                                }
                                List<int> goToFail = new List<int>();
                                if (!string.IsNullOrEmpty(distractor.Attributes["gotoFail"]?.Value))
                                {
                                    foreach (string item in distractor.Attributes["gotoFail"].Value.Split(","))
                                    {
                                        goToFail.Add(int.Parse(item));
                                    }
                                }

                                distractors.Add(new Distractor(distractorScriptID, goTo, goToFail));
                            }
                        }

                        contents.Add(new Content(contentScriptID, voiceID, functionID, leftIllust, speakerIllust, otherNpcTalk, myTalk, illust, distractors));
                    }

                    metadata.Id = int.Parse(npcID);
                    metadata.IsQuestScript = false;

                    if (node.Name == "select")
                    {
                        metadata.Selects[id] = new Select(id, contents);
                    }
                    else if (node.Name == "script")
                    {
                        metadata.Scripts[id] = new Script(id, feature, randomPick, gotoConditionTalkID, contents);
                    }
                    else if (node.Name == "monologue")
                    {
                        metadata.Monologues[id] = new Monologue(id, popupState, popupProp, contents);
                    }
                }

                scripts.Add(metadata);
            }

            return scripts;
        }

        private List<ScriptMetadata> parseQuest(MetadataResources resources)
        {
            List<ScriptMetadata> scripts = new List<ScriptMetadata>();
            foreach (PackFileEntry entry in resources.xmlFiles)
            {

                if (!entry.Name.StartsWith("script/quest"))
                {
                    continue;
                }

                XmlDocument document = resources.xmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode questNode in document.DocumentElement.ChildNodes)
                {
                    ScriptMetadata metadata = new ScriptMetadata();
                    int questID = int.Parse(questNode.Attributes["id"].Value);

                    foreach (XmlNode node in questNode.ChildNodes)
                    {
                        int id = int.Parse(node.Attributes["id"].Value);
                        string feature = node.Attributes["feature"].Value;
                        int randomPick = string.IsNullOrEmpty(node.Attributes["randomPick"]?.Value) ? 0 : int.Parse(node.Attributes["randomPick"].Value);
                        int popupState = string.IsNullOrEmpty(node.Attributes["popupState"]?.Value) ? 0 : int.Parse(node.Attributes["popupState"].Value);
                        int popupProp = string.IsNullOrEmpty(node.Attributes["popupProp"]?.Value) ? 0 : int.Parse(node.Attributes["popupProp"].Value);
                        List<int> gotoConditionTalkID = new List<int>();
                        if (!string.IsNullOrEmpty(node.Attributes["popupProp"]?.Value))
                        {
                            foreach (string item in node.Attributes["popupProp"].Value.Split(","))
                            {
                                gotoConditionTalkID.Add(int.Parse(item));
                            }
                        }

                        List<Content> contents = new List<Content>();
                        foreach (XmlNode content in node.ChildNodes)
                        {
                            long contentScriptID = string.IsNullOrEmpty(content.Attributes["text"]?.Value) ? 0 : long.Parse(content.Attributes["text"].Value.Substring(8, 16));
                            string voiceID = string.IsNullOrEmpty(content.Attributes["voiceID"]?.Value) ? "" : content.Attributes["voiceID"].Value;
                            int otherNpcTalk = string.IsNullOrEmpty(content.Attributes["otherNpcTalk"]?.Value) ? 0 : int.Parse(content.Attributes["otherNpcTalk"].Value);
                            string leftIllust = string.IsNullOrEmpty(content.Attributes["leftIllust"]?.Value) ? "" : content.Attributes["leftIllust"].Value;
                            string illust = string.IsNullOrEmpty(content.Attributes["illust"]?.Value) ? "" : content.Attributes["illust"].Value;
                            string speakerIllust = string.IsNullOrEmpty(content.Attributes["speakerIllust"]?.Value) ? "" : content.Attributes["speakerIllust"].Value;
                            bool myTalk = !string.IsNullOrEmpty(content.Attributes["myTalk"]?.Value);
                            byte functionID = string.IsNullOrEmpty(content.Attributes["functionID"]?.Value) ? 0 : byte.Parse(content.Attributes["functionID"].Value);

                            List<Distractor> distractors = new List<Distractor>();
                            List<Event> events = new List<Event>();
                            foreach (XmlNode distractor in content.ChildNodes)
                            {
                                if (distractor.Name == "event")
                                {
                                    int eventid = string.IsNullOrEmpty(content.Attributes["id"]?.Value) ? 0 : int.Parse(content.Attributes["id"].Value);

                                    List<Content> contents2 = new List<Content>();
                                    foreach (XmlNode item in distractor.ChildNodes)
                                    {
                                        long contentScriptID2 = long.Parse(item.Attributes["text"].Value.Substring(8, 16));
                                        string voiceID2 = string.IsNullOrEmpty(item.Attributes["voiceID"]?.Value) ? "" : item.Attributes["voiceID"].Value;
                                        int otherNpcTalk2 = string.IsNullOrEmpty(item.Attributes["otherNpcTalk"]?.Value) ? 0 : int.Parse(item.Attributes["otherNpcTalk"].Value);
                                        string leftIllust2 = string.IsNullOrEmpty(item.Attributes["leftIllust"]?.Value) ? "" : item.Attributes["leftIllust"].Value;
                                        string illust2 = string.IsNullOrEmpty(item.Attributes["illust"]?.Value) ? "" : item.Attributes["illust"].Value;
                                        string speakerIllust2 = string.IsNullOrEmpty(item.Attributes["speakerIllust"]?.Value) ? "" : item.Attributes["speakerIllust"].Value;
                                        bool myTalk2 = !string.IsNullOrEmpty(item.Attributes["myTalk"]?.Value);
                                        byte functionID2 = string.IsNullOrEmpty(item.Attributes["functionID"]?.Value) ? 0 : byte.Parse(item.Attributes["functionID"].Value);

                                        contents2.Add(new Content(contentScriptID2, voiceID2, functionID2, leftIllust2, speakerIllust2, otherNpcTalk2, myTalk2, illust2, null));
                                    }
                                    events.Add(new Event(eventid, contents2));
                                }
                                else
                                {
                                    long distractorScriptID = long.Parse(distractor.Attributes["text"].Value.Substring(8, 16));
                                    List<int> goTo = new List<int>();
                                    if (!string.IsNullOrEmpty(distractor.Attributes["goto"]?.Value))
                                    {
                                        foreach (string item in distractor.Attributes["goto"].Value.Split(","))
                                        {
                                            goTo.Add(int.Parse(item));
                                        }
                                    }
                                    List<int> goToFail = new List<int>();
                                    if (!string.IsNullOrEmpty(distractor.Attributes["gotoFail"]?.Value))
                                    {
                                        foreach (string item in distractor.Attributes["gotoFail"].Value.Split(","))
                                        {
                                            goToFail.Add(int.Parse(item));
                                        }
                                    }

                                    distractors.Add(new Distractor(distractorScriptID, goTo, goToFail));
                                }
                            }

                            contents.Add(new Content(contentScriptID, voiceID, functionID, leftIllust, speakerIllust, otherNpcTalk, myTalk, illust, distractors));
                        }

                        metadata.Id = questID;
                        metadata.IsQuestScript = true;

                        if (node.Name == "select")
                        {
                            metadata.Selects[id] = new Select(id, contents);
                        }
                        else if (node.Name == "script")
                        {
                            metadata.Scripts[id] = new Script(id, feature, randomPick, gotoConditionTalkID, contents);
                        }
                        else if (node.Name == "monologue")
                        {
                            metadata.Monologues[id] = new Monologue(id, popupState, popupProp, contents);
                        }

                        scripts.Add(metadata);
                    }
                }
            }
            return scripts;
        }
    }
}
