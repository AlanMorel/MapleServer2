using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapleServer2.Types
{
    public class StatDistribution    {
        public Dictionary<byte, int> AllocatedStats { get; private set; }
        // key = index of the stat 
        // value = number of points allocated to the stat

        public StatDistribution()
        {
            this.AllocatedStats = new Dictionary<byte, int>();
        }

        public StatDistribution(Dictionary<byte, int> AllocatedStats)
        {
            this.AllocatedStats = AllocatedStats;
        }

        public void addPoint(byte StatType)
        {
            int statCount;
            if (AllocatedStats.TryGetValue(StatType, out statCount))
            {
                AllocatedStats[StatType] = statCount + 1;
            }
            else
            {
                AllocatedStats[StatType] = 1;
            }

            Console.WriteLine("Stat Distribution: " + ToDebugString(AllocatedStats));

        }

        public void resetPoints()
        {
            foreach (var StatType in AllocatedStats.Keys.ToList())
            {
                AllocatedStats[StatType] = 0;
            }
        }

        public static string ToDebugString<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }
    }
}
