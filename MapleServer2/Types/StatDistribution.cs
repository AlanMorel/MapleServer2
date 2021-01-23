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
        public Dictionary<byte, int> OtherStats { get; private set; }
        // key: 01 PointsFromTrophy, 02 PointsFromQuest, 03 PointsFromExploration, 04 PointsFromPrestige
        // value: attribute points received from that category

        public Dictionary<byte, int> AllocatedStats { get; private set; }
        // key = index representing the stat type (ie. a value of 00 corresponds to Str)
        // value = number of points allocated to the stat

        public StatDistribution()
        {
            // hardcode the amount of stat points the character starts with temporarily
            this.TotalStatPoints = 18;
            this.OtherStats = new Dictionary<byte, int>();
            this.AllocatedStats = new Dictionary<byte, int>();
            AddTotalStatPoints(1, 0x2);
            AddTotalStatPoints(2, 0x1);
            AddTotalStatPoints(3, 0x3);
            AddTotalStatPoints(4, 0x4);
        }

        public StatDistribution(Dictionary<byte, int> AllocatedStats)
        {
            this.AllocatedStats = AllocatedStats;
            this.OtherStats = new Dictionary<byte, int>();
        }

        public StatDistribution(Dictionary<byte, int> AllocatedStats, Dictionary<byte, int> OtherStats)
        {
            this.AllocatedStats = AllocatedStats;
            this.OtherStats = OtherStats;
        }

        public void AddTotalStatPoints(int amount)
        {
            this.TotalStatPoints += amount;
        }

        // invoked whenever stat points are gained when not leveling up
        public void AddTotalStatPoints(int amount, byte pointSrc)
        {
            if (OtherStats.ContainsKey(pointSrc))
            {
                OtherStats[pointSrc] += amount;
                
            }
            else
            {
                OtherStats[pointSrc] = amount;
            }
            TotalStatPoints += amount;
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