using Maple2.Trigger.Enum;

namespace MapleServer2.Types
{
    public class Widget
    {
        public int Id;
        public WidgetType Type;
        public string State;

        public Widget(WidgetType type)
        {
            Type = type;
        }
    }
}
