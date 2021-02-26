using Maple2Storage.Enums;

namespace MapleServer2.Types
{
    public class InteractActor
    {
        public string Uuid;
        public string Name;
        public InteractActorType Type;

        public InteractActor(string uuid, string name, InteractActorType type)
        {
            Uuid = uuid;
            Name = name;
            Type = type;
        }
    }
}
