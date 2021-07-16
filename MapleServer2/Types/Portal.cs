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
        public byte PortalType;

        public Portal(int id)
        {
            Id = id;
        }

        public void Update(bool visible, bool enabled, bool minimapVisible)
        {
            IsVisible = visible;
            IsEnabled = enabled;
            IsMinimapVisible = minimapVisible;
        }
    }
}
