namespace MapleServer2.Types
{
    public class Instrument
    {
        public int GmId;
        public int PercussionId;
        public bool IsCustomScore;
        public Item Score;
        public int InstrumentTick;
        public int PlayerObjectId;
        public bool Improvise;
        public bool Ensemble;

        public Instrument(int gmId, int percussionId, bool isCustomScore, int playerObjectId)
        {
            GmId = gmId;
            PercussionId = percussionId;
            IsCustomScore = isCustomScore;
            PlayerObjectId = playerObjectId;
        }
    }
}
