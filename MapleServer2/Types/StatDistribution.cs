using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapleServer2.Types
{
    public class StatDistribution
    {
        public int TotalStatPoints { get; private set; }
        // TODO: implement Dictionary to keep track of points earned from quest, trophy, exploration, prestige
        // naming convention: PointsFromQuest, PointsFromTrophy, PointsFromExploration, PointsFromPrestige

        public Dictionary<byte, int> AllocatedStats { get; private set; }
        // key = index representing the stat type (ie. a value of 00 corresponds to Str)
        // value = number of points allocated to the stat

        public StatDistribution()
        {
            // hardcode the amount of stat points the character starts with temporarily
            this.TotalStatPoints = 18;
            this.AllocatedStats = new Dictionary<byte, int>();
        }

        public StatDistribution(Dictionary<byte, int> AllocatedStats)
        {
            this.AllocatedStats = AllocatedStats;
        }

        public void AddPoint(byte statType)
        {
            if (AllocatedStats.ContainsKey(statType))
            {
                AllocatedStats[statType] += 1;
            }
            else
            {
                AllocatedStats[statType] = 1;
            }
        }

        public void ResetPoints()
        {
            AllocatedStats.Clear();
        }

        public byte GetStatTypeCount()
        {
            // returns a count of how many types of Stats have had points added to them
            // ex. a character has Strength and Intelligence points allocated - function returns 2 

            return (byte) AllocatedStats.Count;
        }

        public static string ToDebugString<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }
    }
}
