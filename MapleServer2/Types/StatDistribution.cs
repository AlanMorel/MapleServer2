using Maple2Storage.Enums;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public struct ExtraSkillPoints
{
    public Dictionary<short, int> ExtraPoints;

    public ExtraSkillPoints()
    {
        ExtraPoints = new Dictionary<short, int>();
    }
}

public enum SkillPointSource
{
    Trophy = 1,
    Chapter = 2,
    Unknown = 3
}

public class StatDistribution
{
    public int TotalStatPoints;
    public Dictionary<OtherStatsIndex, int> OtherStats { get; } // Dictionary of OtherStatsIndex and amount of points allocated to it
    public Dictionary<StatAttribute, int> AllocatedStats { get; } // Dictionary of StatId and amount of stat points allocated to it

    public const int MaxSkillSources = 3;
    public const short MaxSkillJobRanks = 2;
    public int TotalExtraSkillPoints;
    public Dictionary<SkillPointSource, ExtraSkillPoints> ExtraSkillPoints { get; }

    public StatDistribution(int totalStats = 0, Dictionary<StatAttribute, int> allocatedStats = null, Dictionary<OtherStatsIndex, int> otherStats = null)
    {
        TotalStatPoints = totalStats;
        AllocatedStats = allocatedStats ?? new Dictionary<StatAttribute, int>();
        OtherStats = otherStats ?? new Dictionary<OtherStatsIndex, int>();
        ExtraSkillPoints = new Dictionary<SkillPointSource, ExtraSkillPoints>();

        foreach (int source in Enumerable.Range(0, MaxSkillSources))
        {
            ExtraSkillPoints sourcePoints = new ExtraSkillPoints();

            foreach (short jobRank in Enumerable.Range(0, MaxSkillJobRanks))
            {
                sourcePoints.ExtraPoints[jobRank] = 0;
            }

            ExtraSkillPoints[(SkillPointSource) source] = sourcePoints;
        }
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

    public void AddTotalSkillPoints(int amount, int rank, SkillPointSource index)
    {
        TotalExtraSkillPoints += amount;
        ExtraSkillPoints[index].ExtraPoints[(short) rank] += amount;
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
