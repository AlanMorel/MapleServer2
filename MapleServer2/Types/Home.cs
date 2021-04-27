namespace MapleServer2.Types
{
    public class Home
    {
        public readonly long Id;
        public Player Owner { get; }
        public int MapId { get; set; }
        public int PlotId { get; set; }
        public int PlotNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public long Expiration { get; set; }
        public string Name { get; set; }
    }
}
