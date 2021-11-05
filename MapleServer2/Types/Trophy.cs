using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using static MapleServer2.Packets.TrophyPacket;

namespace MapleServer2.Types;

public class Trophy
{
    public long Uid { get; private set; }
    public int Id { get; private set; }
    public int NextGrade { get; private set; }
    public int MaxGrade { get; private set; }
    public long Counter { get; private set; }
    public long Condition { get; private set; }
    public string ConditionType { get; private set; }
    public string[] ConditionCodes { get; private set; }
    public string[] ConditionTargets { get; private set; }
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
        TrophyGradeMetadata trophyGradeMetadata = trophyMetadata.Grades.FirstOrDefault(x => x.Grade == NextGrade);
        Condition = trophyGradeMetadata.Condition;
        ConditionType = trophyGradeMetadata.ConditionType;
        ConditionCodes = trophyGradeMetadata.ConditionCodes;
        ConditionTargets = trophyGradeMetadata.ConditionTargets;
        Type = trophyMetadata.Categories[0];
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
        TrophyGradeMetadata trophyGradeMetadata = trophyMetadata.Grades.FirstOrDefault(x => x.Grade == NextGrade);
        Condition = trophyGradeMetadata.Condition;
        ConditionType = trophyGradeMetadata.ConditionType;
        ConditionCodes = trophyGradeMetadata.ConditionCodes;
        ConditionTargets = trophyGradeMetadata.ConditionTargets;
        Type = trophyMetadata.Categories[0];
        Counter = counter;
        IsDone = isDone;
        LastReward = lastReward;
        Timestamps = timestamps;
        CharacterId = characterId;
        AccountId = accountId;
    }

    public GradeStatus GetGradeStatus()
    {
        return IsDone ? GradeStatus.Finished : GradeStatus.InProgress;
    }

    public void AddCounter(GameSession session, long amount)
    {
        Counter += amount;
        if (IsDone)
        {
            return;
        }

        if (Counter < Condition)
        {
            return;
        }

        TrophyGradeMetadata grade = TrophyMetadataStorage.GetMetadata(Id).Grades.FirstOrDefault(x => x.Grade == NextGrade);
        if (!RewardTypeRequiresClaim(grade.RewardType) && LastReward == NextGrade)
        {
            // Add stat points and skill points
            switch (grade.RewardType)
            {
                case RewardType.statPoint:
                    session.Player.AddStatPoint(grade.RewardValue, OtherStatsIndex.Trophy);
                    break;
                case RewardType.skillPoint:
                    // TODO: Add skill points
                    break;
            }
            LastReward++;
        }
        NextGrade++;
        Timestamps.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        // level up but not completed
        if (NextGrade <= MaxGrade)
        {
            // Update condition
            TrophyGradeMetadata trophyGradeMetadata = TrophyMetadataStorage.GetMetadata(Id).Grades.FirstOrDefault(x => x.Grade == NextGrade);
            Condition = trophyGradeMetadata.Condition;
            ConditionType = trophyGradeMetadata.ConditionType;
            ConditionCodes = trophyGradeMetadata.ConditionCodes;
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
                    session.Player.TrophyCount[0] += 1;
                    break;
                case string s when s.Contains("adventure"):
                    session.Player.TrophyCount[1] += 1;
                    break;
                case string s when s.Contains("lifestyle"):
                    session.Player.TrophyCount[2] += 1;
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
