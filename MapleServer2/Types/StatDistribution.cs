using System.Collections.Generic;
using System.Linq;
using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class StatDistribution
    {
        public int TotalStatPoints { get; private set; }
        public Dictionary<OtherStatsIndex, int> OtherStats { get; private set; }
        public Dictionary<byte, int> AllocatedStats { get; private set; }
        // key = index representing the stat type (ie. a value of 00 corresponds to Str)
        // value = number of points allocated to the stat

        public StatDistribution(int totalStats = 0, Dictionary<byte, int> allocatedStats = null, Dictionary<OtherStatsIndex, int> otherStats = null)
        {
            // hardcode the amount of stat points the character starts with temporarily
            TotalStatPoints = totalStats;
            AllocatedStats = allocatedStats ?? new Dictionary<byte, int>();
            OtherStats = otherStats ?? new Dictionary<OtherStatsIndex, int>();
        }

        public void AddTotalStatPoints(int amount, OtherStatsIndex pointSrc = 0)
        {
            TotalStatPoints += amount;
            if (pointSrc == 0)
            {
                return;
            }
            if (!OtherStats.ContainsKey(pointSrc))
            {
                OtherStats[pointSrc] = 0;
            }
            OtherStats[pointSrc] += amount;
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
