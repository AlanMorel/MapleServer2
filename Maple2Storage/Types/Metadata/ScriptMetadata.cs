using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class ScriptMetadata
    {
        [XmlElement(Order = 1)]
        public bool IsQuestScript;
        [XmlElement(Order = 2)]
        public int Id;
        [XmlElement(Order = 3)]
        public List<Option> Options = new List<Option>();

        public ScriptMetadata() { }

        public override string ToString() => $"IsQuestScript: {IsQuestScript}, Id: {Id}, Options: ({string.Join(", ", Options)})";
    }

    [XmlType]
    public class Option
    {
        [XmlElement(Order = 1)]
        public ScriptType Type;
        [XmlElement(Order = 2)]
        public int Id;
        [XmlElement(Order = 3)]
        public List<Content> Contents = new List<Content>();
        [XmlElement(Order = 4)]
        public string ButtonSet;

        public Option() { }

        public Option(ScriptType type, int id, List<Content> contents, string buttonSet)
        {
            Type = type;
            Id = id;
            Contents = contents;
            ButtonSet = buttonSet;
        }

        public override string ToString() => $"Type: {Type}, Id: {Id}, AmountContent: {Contents.Count}, Contents: {string.Join("\n", Contents)})\r\n";
    }

    [XmlType]
    public class Content
    {
        [XmlElement(Order = 1)]
        public string FunctionId;
        [XmlElement(Order = 2)]
        public DialogType ButtonSet;
        [XmlElement(Order = 3)]
        public List<int> Goto = new List<int>();
        [XmlElement(Order = 4)]
        public List<int> GotoFail = new List<int>();

        public Content() { }

        public Content(List<int> gotos, List<int> gotoFail, string functionId, DialogType buttonSet)
        {
            Goto = gotos;
            GotoFail = gotoFail;
            FunctionId = functionId;
            ButtonSet = buttonSet;
        }

        public override string ToString() => $"FunctionId: {FunctionId}, ButtonSet: {ButtonSet}, GoTo: ({string.Join(",", Goto)}), GotoFail: ({string.Join(",", GotoFail)}";
    }

    public enum ScriptType
    {
        Select = 0,
        Script = 1
    }
}
