using Maple2Storage.Enums;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class StatDistribution
{
    public int TotalStatPoints;
    public Dictionary<OtherStatsIndex, int> OtherStats { get; } // Dictionary of OtherStatsIndex and amount of points allocated to it
    public Dictionary<StatAttribute, int> AllocatedStats { get; } // Dictionary of StatId and amount of stat points allocated to it

    public StatDistribution(int totalStats = 0, Dictionary<StatAttribute, int> allocatedStats = null, Dictionary<OtherStatsIndex, int> otherStats = null)
    {
        TotalStatPoints = totalStats;
        AllocatedStats = allocatedStats ?? new Dictionary<StatAttribute, int>();
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

    public void AddPoint(StatAttribute statType)
    {
        if (AllocatedStats.ContainsKey(statType))
        {
            AllocatedStats[statType] += 1;
            return;
        }

        AllocatedStats[statType] = 1;
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
}
