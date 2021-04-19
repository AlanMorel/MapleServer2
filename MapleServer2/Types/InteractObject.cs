using Maple2Storage.Enums;

namespace MapleServer2.Types
{
    public class InteractObject
    {
        public string Uuid;
        public string Name;
        public InteractObjectType Type;

        public InteractObject(string uuid, string name, InteractObjectType type)
        {
            Uuid = uuid;
            Name = name;
            Type = type;
        }
    }
}
