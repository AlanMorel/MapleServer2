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
        public bool IsVisible;
        public TriggerEffect(int id, bool isVisible) : base(id)
        {
            IsVisible = isVisible;
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

    public class TriggerActor : TriggerObject
    {
        public bool IsVisible;
        public string StateName;
        public TriggerActor(int id, bool isVisible, string stateName) : base(id)
        {
            IsVisible = isVisible;
            StateName = stateName;
        }
    }
}
