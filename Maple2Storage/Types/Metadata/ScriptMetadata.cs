using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ScriptMetadata
{
    [XmlElement(Order = 1)]
    public bool IsQuestScript;
    [XmlElement(Order = 2)]
    public int Id;
    [XmlElement(Order = 3)]
    public List<NpcScript> NpcScripts = new();

    public ScriptMetadata() { }

    public override string ToString()
    {
        return $"IsQuestScript: {IsQuestScript}, Id: {Id}, Options: ({string.Join(", ", NpcScripts)})";
    }
}

[XmlType]
public class NpcScript
{
    [XmlElement(Order = 1)]
    public ScriptType Type;
    [XmlElement(Order = 2)]
    public int Id;
    [XmlElement(Order = 3)]
    public List<ScriptContent> Contents = new();
    [XmlElement(Order = 4)]
    public int JobId;
    [XmlElement(Order = 5)]
    public bool RandomPick;

    public NpcScript() { }

    public NpcScript(ScriptType type, int id, List<ScriptContent> contents, int jobId, bool randomPick)
    {
        Type = type;
        Id = id;
        Contents = contents;
        JobId = jobId;
        RandomPick = randomPick;
    }

    public override string ToString()
    {
        return $"Type: {Type}, Id: {Id}, AmountContent: {Contents.Count}, Contents: {string.Join("\n", Contents)})\r\n";
    }
}

[XmlType]
public class ScriptContent
{
    [XmlElement(Order = 1)]
    public int FunctionId;
    [XmlElement(Order = 2)]
    public ResponseSelection ButtonSet;
    [XmlElement(Order = 3)]
    public List<ScriptDistractor> Distractor = new();
    [XmlElement(Order = 4)]
    public List<ScriptEvent> Events = new();

    public ScriptContent() { }

    public ScriptContent(int functionId, ResponseSelection buttonSet, List<ScriptEvent> events, List<ScriptDistractor> distractor)
    {
        Distractor = distractor;
        Events = events;
        FunctionId = functionId;
        ButtonSet = buttonSet;
    }

    public override string ToString()
    {
        return $"FunctionId: {FunctionId}, ButtonSet: {ButtonSet}, Distractor: ({string.Join("\r\n", Distractor)})";
    }
}

[XmlType]
public class ScriptEvent
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public List<EventContent> Contents = new();

    public ScriptEvent() { }

    public ScriptEvent(int id, List<EventContent> contents)
    {
        Id = id;
        Contents = contents;
    }
}

[XmlType]
public class EventContent
{
    [XmlElement(Order = 1)]
    public string VoiceId;
    [XmlElement(Order = 2)]
    public string Illustration;
    [XmlElement(Order = 3)]
    public string Text;

    public EventContent() { }
    public EventContent(string voiceId, string illustration, string text)
    {
        VoiceId = voiceId;
        Illustration = illustration;
        Text = text;
    }
}

[XmlType]
public class ScriptDistractor
{
    [XmlElement(Order = 1)]
    public List<int> Goto = new();
    [XmlElement(Order = 2)]
    public List<int> GotoFail = new();

    public ScriptDistractor() { }

    public ScriptDistractor(List<int> gotos, List<int> gotoFail)
    {
        Goto = gotos;
        GotoFail = gotoFail;
    }

    public override string ToString()
    {
        return $"Goto: {string.Join(", ", Goto)}, GotoFail: {string.Join(", ", GotoFail)}";
    }
}

public enum ScriptType
{
    Select = 0,
    Script = 1,
    Job = 2
}
