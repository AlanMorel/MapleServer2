namespace MapleServer2.Types
{
    public class Breakable
    {
        public string Id;
        public BreakableState State;
        public bool IsEnabled;
        public int TriggerId;

        public Breakable(string id, bool isEnabled)
        {
            Id = id;
            IsEnabled = isEnabled;
            State = BreakableState.Spawn;
        }
    }

    public enum BreakableState : byte
    {
        Spawn = 2,
        Break = 3,
        Despawn = 4
    }
}
