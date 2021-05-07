using System;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

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
