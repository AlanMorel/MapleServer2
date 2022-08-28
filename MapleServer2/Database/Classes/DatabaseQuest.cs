using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseQuest : DatabaseTable
{
    public DatabaseQuest() : base("quests") { }

    public long Insert(QuestStatus questStatus)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            questStatus.Id,
            questStatus.State,
            start_timestamp = questStatus.StartTimestamp,
            complete_timestamp = questStatus.CompleteTimestamp,
            accepted = questStatus.Accepted,
            amount_completed = questStatus.AmountCompleted,
            condition = JsonConvert.SerializeObject(questStatus.Condition),
            character_id = questStatus.CharacterId
        });
    }

    public Dictionary<int, QuestStatus> FindAllByCharacterId(long characterId)
    {
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        Dictionary<int, QuestStatus> questStatusList = new();
        foreach (dynamic data in results)
        {
            QuestStatus questStatus = (QuestStatus) ReadQuest(data);

            if (questStatus == null)
            {
                continue;
            }

            questStatusList.Add(questStatus.Id, questStatus);
        }

        return questStatusList;
    }

    public void Update(QuestStatus questStatus)
    {
        QueryFactory.Query(TableName).Where("uid", questStatus.Uid).Update(new
        {
            questStatus.State,
            start_timestamp = questStatus.StartTimestamp,
            complete_timestamp = questStatus.CompleteTimestamp,
            accepted = questStatus.Accepted,
            condition = JsonConvert.SerializeObject(questStatus.Condition),
            character_id = questStatus.CharacterId,
            amount_completed = questStatus.AmountCompleted,
        });
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
    }

    private static QuestStatus ReadQuest(dynamic data)
    {
        if (QuestMetadataStorage.GetMetadata(data.id) == null)
        {
            return null;
        }

        return new QuestStatus(data.uid, data.id, data.character_id, data.accepted, data.start_timestamp, data.complete_timestamp, JsonConvert.DeserializeObject<List<Condition>>(data.condition), (QuestState) data.state, data.amount_completed);
    }
}
