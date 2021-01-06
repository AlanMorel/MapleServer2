using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class Currency
    {

        public long Meso { get; set; }
        public long Meret { get; set; }
        public long ValorToken { get; set; }
        public long Treva { get; set; }
        public long Rue { get; set; }
        public long HaviFruit { get; set; }
        public long MesoToken { get; set; }

        public Currency()
        {
            Meso = 20000;
            Meret = 20000;
            ValorToken = 20000;
            Treva = 20000;
            Rue = 20000;
            HaviFruit = 20000;
            MesoToken = 20000;
        }
    }
}
