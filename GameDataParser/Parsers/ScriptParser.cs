using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Tools;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.Script;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using NpcScript = Maple2.File.Parser.Xml.Script.NpcScript;

namespace GameDataParser.Parsers;

public class ScriptParser : Exporter<List<ScriptMetadata>>
{
    public ScriptParser(MetadataResources resources) : base(resources, MetadataName.Script) { }

    protected override List<ScriptMetadata> Parse()
    {
        List<ScriptMetadata> scripts = new();
        Filter.Load(Resources.XmlReader, "NA", "Live");
        Maple2.File.Parser.ScriptParser parser = new(Resources.XmlReader);

        // Parse NPC Scripts
        foreach ((int id, NpcScript script) in parser.ParseNpc())
        {
            ScriptMetadata metadata = new();
            foreach (TalkScript talkScript in script.select)
            {
                int scriptId = talkScript.id;
                int jobCondition = talkScript.jobCondition;
                List<ScriptContent> contents = ParseContents(talkScript.content);
                bool randomPick = talkScript.randomPick;
                metadata.NpcScripts.Add(new(ScriptType.Select, scriptId, contents, jobCondition, randomPick));
            }

            foreach (TalkScript talkScript in script.script)
            {

                int scriptId = talkScript.id;
                int jobCondition = talkScript.jobCondition;
                List<ScriptContent> contents = ParseContents(talkScript.content);
                bool randomPick = talkScript.randomPick;
                metadata.NpcScripts.Add(new(ScriptType.Script, scriptId, contents, jobCondition, randomPick));
            }

            // job script
            TalkScript jobScript = script.job;
            if (jobScript is not null)
            {
                List<ScriptContent> jobContents = ParseContents(jobScript.content);
                bool randomPick = jobScript.randomPick;
                metadata.NpcScripts.Add(new(ScriptType.Job, jobScript.id, jobContents, jobScript.jobCondition, randomPick));
            }

            metadata.Id = id;
            scripts.Add(metadata);
        }

        // Parse Quest Scripts
        foreach (QuestScript questScript in parser.ParseQuest())
        {
            ScriptMetadata metadata = new();
            int questId = questScript.id;
            foreach (TalkScript talkScript in questScript.script)
            {
                int scriptId = talkScript.id;
                int jobCondition = talkScript.jobCondition;
                List<ScriptContent> contents = ParseContents(talkScript.content);
                bool randomPick = talkScript.randomPick;

                metadata.NpcScripts.Add(new(ScriptType.Script, scriptId, contents, jobCondition, randomPick));
            }

            metadata.Id = questId;
            metadata.IsQuestScript = true;
            scripts.Add(metadata);
        }
        return scripts;
    }

    private static List<ScriptContent> ParseContents(List<Content> contents)
    {
        List<ScriptContent> scriptContents = new();
        foreach (Content content in contents)
        {
            int functionId = content.functionID;
            ResponseSelection responseSelection = (ResponseSelection) content.buttonSet;

            List<ScriptDistractor> distractors = new();
            foreach (Distractor distractor in content.distractor)
            {
                List<int> gotoList = distractor.@goto.SplitAndParseToInt(',').ToList();
                List<int> gotoFailList = distractor.gotoFail.SplitAndParseToInt(',').ToList();
                distractors.Add(new(gotoList, gotoFailList));
            }
            List<ScriptEvent> scriptEvents = new();
            foreach (Event eventContent in content.@event)
            {
                int eventId = eventContent.id;
                List<EventContent> eventContents = new();
                foreach (Content subContent in eventContent.content)
                {
                    eventContents.Add(new(subContent.voiceID, subContent.illust, subContent.text));
                }
                scriptEvents.Add((new(eventId, eventContents)));
            }

            scriptContents.Add(new(functionId, responseSelection, scriptEvents, distractors));
        }
        return scriptContents;
    }
}
