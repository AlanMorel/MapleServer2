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
    public List<Option> Options = new();

    public override string ToString()
    {
        return $"IsQuestScript: {IsQuestScript}, Id: {Id}, Options: ({string.Join(", ", Options)})";
    }
}
[XmlType]
public class Option
{
    [XmlElement(Order = 1)]
    public ScriptType Type;
    [XmlElement(Order = 2)]
    public int Id;
    [XmlElement(Order = 3)]
    public List<Content> Contents = new();
    [XmlElement(Order = 4)]
    public string ButtonSet;
    [XmlElement(Order = 5)]
    public int JobId;

    public Option() { }

    public Option(ScriptType type, int id, List<Content> contents, string buttonSet, int jobId)
    {
        Type = type;
        Id = id;
        Contents = contents;
        ButtonSet = buttonSet;
        JobId = jobId;
    }

    public override string ToString()
    {
        return $"Type: {Type}, Id: {Id}, AmountContent: {Contents.Count}, Contents: {string.Join("\n", Contents)})\r\n";
    }
}
[XmlType]
public class Content
{
    [XmlElement(Order = 1)]
    public string FunctionId;
    [XmlElement(Order = 2)]
    public DialogType ButtonSet;
    [XmlElement(Order = 3)]
    public List<Distractor> Distractor;

    public Content() { }

    public Content(string functionId, DialogType buttonSet, List<Distractor> distractor)
    {
        Distractor = distractor;
        FunctionId = functionId;
        ButtonSet = buttonSet;
    }

    public override string ToString()
    {
        return $"FunctionId: {FunctionId}, ButtonSet: {ButtonSet}, Distractor: ({string.Join("\r\n", Distractor)})";
    }
}
[XmlType]
public class Distractor
{
    [XmlElement(Order = 1)]
    public List<int> Goto = new();
    [XmlElement(Order = 2)]
    public List<int> GotoFail = new();

    public Distractor() { }

    public Distractor(List<int> gotos, List<int> gotoFail)
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
    Script = 1
}
