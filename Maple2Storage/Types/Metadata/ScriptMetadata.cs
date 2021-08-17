using System.Xml.Serialization;

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
        public List<int> Goto = new List<int>();
        [XmlElement(Order = 4)]
        public List<int> GotoFail = new List<int>();
        [XmlElement(Order = 5)]
        public int AmountContent;

        public Option() { }

        public Option(ScriptType type, int id, List<int> goTo, List<int> gotoFail, int amountContent)
        {
            Type = type;
            Id = id;
            Goto = goTo;
            GotoFail = gotoFail;
            AmountContent = amountContent;
        }

        public override string ToString()
        {
            return $"Type: {Type}, Id: {Id}, AmountContent: {AmountContent}, GoTo: ({string.Join(",", Goto)}), GotoFail: ({string.Join(",", GotoFail)})\r\n";
        }
    }

    public enum ScriptType
    {
        Select = 0,
        Script = 1
    }
}
