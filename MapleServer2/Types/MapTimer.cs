namespace MapleServer2.Types
{
    public class MapTimer
    {
        public readonly string Id;
        public int StartTick;
        public int EndTick;

        public MapTimer(string id, int endTick)
        {
            Id = id;
            StartTick = Environment.TickCount;
            EndTick = endTick;
        }
    }
}
