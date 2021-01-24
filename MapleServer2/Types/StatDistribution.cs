using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public StatDistribution(Dictionary<byte, int> AllocatedStats = null, Dictionary<OtherStatsIndex, int> OtherStats = null)
        {
            // hardcode the amount of stat points the character starts with temporarily
            this.TotalStatPoints = 18;
            this.AllocatedStats = AllocatedStats == null ? new Dictionary<byte, int>() : AllocatedStats;
            this.OtherStats = OtherStats == null ? new Dictionary<OtherStatsIndex, int>() : OtherStats;
            
            AddTotalStatPoints(1, OtherStatsIndex.Quest);
            AddTotalStatPoints(2, OtherStatsIndex.Trophy);
            AddTotalStatPoints(3, OtherStatsIndex.Exploration);
            AddTotalStatPoints(4, OtherStatsIndex.Prestige);
        }

        public void AddTotalStatPoints(int amount)
        {
            this.TotalStatPoints += amount;
        }

        public void AddTotalStatPoints(int amount, OtherStatsIndex pointSrc)
        {
            if (OtherStats.ContainsKey(pointSrc))
            {
                OtherStats[pointSrc] += amount;
            }
            else
            {
                OtherStats[pointSrc] = amount;
            }
            this.TotalStatPoints += amount;
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