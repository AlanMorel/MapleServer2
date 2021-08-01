using Maple2.Trigger.Enum;

namespace MapleServer2.Types
{
    public class Widget
    {
        public int Id;
        public WidgetType Type;
        public string State;
        public string Arg; // unclear what this is

        public Widget(WidgetType type)
        {
            Type = type;
        }
    }
}
