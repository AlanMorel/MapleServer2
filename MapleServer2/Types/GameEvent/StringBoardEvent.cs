namespace MapleServer2.Types
{
    public class StringBoardEvent
    {
        public int Id { get; set; }
        public int StringId { get; set; }
        public string String { get; set; }

        // if stringId = 0, a string is required to display custom text. Otherwise the id needs to match one in /table/stringboardtext.xml
        public StringBoardEvent() { }
    }
}
