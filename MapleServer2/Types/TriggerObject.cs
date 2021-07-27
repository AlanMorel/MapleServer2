

namespace MapleServer2.Types
{
    public class TriggerObject
    {
        public int Id;

        public TriggerObject(int id)
        {
            Id = id;
        }
    }

    public class TriggerMesh : TriggerObject
    {
        public bool IsVisible;
        public TriggerMesh(int id, bool isVisible) : base(id)
        {
            IsVisible = isVisible;
        }
    }

    public class TriggerEffect : TriggerObject
    {
        public TriggerEffect(int id) : base(id)
        {
        }
    }

    public class TriggerCamera : TriggerObject
    {
        public bool IsEnabled;
        public TriggerCamera(int id, bool isEnabled) : base(id)
        {
            IsEnabled = isEnabled;
        }
    }
}
