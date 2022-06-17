using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class Trophy
{
    public long Uid { get; private set; }
    public int Id { get; private set; }
    public int NextGrade { get; private set; }
    public int MaxGrade { get; private set; }
    public long Counter { get; private set; }
    public TrophyGradeMetadata GradeCondition { get; private set; }
    public bool IsDone { get; private set; }
    public string Type { get; private set; }
    public List<long> Timestamps { get; private set; }
    public byte LastReward { get; set; }
    public bool Favorited { get; set; }

    public readonly long CharacterId;
    public readonly long AccountId;

    public Trophy() { }

    public Trophy(long characterId, long accountId, int trophyId)
    {
        TrophyMetadata trophyMetadata = TrophyMetadataStorage.GetMetadata(trophyId);
        Id = trophyId;
        LastReward = 1;
        NextGrade = 1;
        Timestamps = new();
        MaxGrade = trophyMetadata.Grades.Count;
        GradeCondition = trophyMetadata.Grades.FirstOrDefault(x => x.Grade == NextGrade);
        Type = trophyMetadata.Categories?[0] ?? "";
        if (trophyMetadata.AccountWide)
        {
            AccountId = accountId;
        }
        else
        {
            CharacterId = characterId;
        }

        Uid = DatabaseManager.Trophies.Insert(this);
    }

    public Trophy(long uid, int trophyId, int nextGrade, long counter, bool isDone, byte lastReward, List<long> timestamps, long characterId, long accountId)
    {
        TrophyMetadata trophyMetadata = TrophyMetadataStorage.GetMetadata(trophyId);
        Uid = uid;
        Id = trophyId;
        NextGrade = nextGrade;
        MaxGrade = trophyMetadata.Grades.Count;
        GradeCondition = trophyMetadata.Grades.FirstOrDefault(x => x.Grade == NextGrade);
        Type = trophyMetadata.Categories?[0] ?? "";
        Counter = counter;
        IsDone = isDone;
        LastReward = lastReward;
        Timestamps = timestamps;
        CharacterId = characterId;
        AccountId = accountId;
    }

    public void AddCounter(Player player, long amount)
    {
        Counter += amount;
        if (IsDone)
        {
            return;
        }

        if (Counter < GradeCondition.Condition)
        {
            return;
        }

        if (!RewardTypeRequiresClaim(GradeCondition.RewardType) && LastReward == NextGrade)
        {
            // Add stat points and skill points
            switch (GradeCondition.RewardType)
            {
                case RewardType.statPoint:
                    player.AddStatPoint(GradeCondition.RewardValue, OtherStatsIndex.Trophy);
                    break;
                case RewardType.skillPoint:
                    player.AddSkillPoint(GradeCondition.RewardValue, GradeCondition.RewardSubJobLevel, OtherStatsIndex.Trophy);
                    break;
            }

            LastReward++;
        }

        NextGrade++;
        Timestamps.Add(TimeInfo.Now());

        // level up but not completed
        if (NextGrade <= MaxGrade)
        {
            // Update condition
            GradeCondition = TrophyMetadataStorage.GetMetadata(Id).Grades.FirstOrDefault(x => x.Grade == NextGrade);
            return;
        }

        // level up and completed
        IsDone = true;
        NextGrade--;
        string[] categories = TrophyMetadataStorage.GetMetadata(Id).Categories;
        foreach (string category in categories)
        {
            switch (category)
            {
                case string s when s.Contains("combat"):
                    player.TrophyCount[0] += 1;
                    break;
                case string s when s.Contains("adventure"):
                    player.TrophyCount[1] += 1;
                    break;
                case string s when s.Contains("lifestyle"):
                    player.TrophyCount[2] += 1;
                    break;
            }
        }
    }

    private static bool RewardTypeRequiresClaim(RewardType type)
    {
        return type switch
        {
            RewardType.item or RewardType.title => true,
            _ => false
        };
    }
}
