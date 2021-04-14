using Maple2Storage.Types;

namespace MapleServer2.Types
{
    public class Portal
    {
        public int Id;
        public bool IsVisible;
        public bool IsEnabled;
        public bool IsMinimapVisible;
        public CoordF Rotation;
        public int TargetMapId;
        public int Duration;
        public string Effect;
        public string Host;
        public bool IsPassEnabled;
        public string Passcode;

        public Portal(int id)
        {
            Id = id;
        }
    }
}
