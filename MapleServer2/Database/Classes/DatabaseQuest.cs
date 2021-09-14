using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseQuest : DatabaseTable
    {
        public DatabaseQuest() : base("quests") { }

        public long Insert(QuestStatus questStatus)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                questStatus.Id,
                questStatus.Started,
                questStatus.Completed,
                start_timestamp = questStatus.StartTimestamp,
                complete_timestamp = questStatus.CompleteTimestamp,
                questStatus.Tracked,
                condition = JsonConvert.SerializeObject(questStatus.Condition),
                character_id = questStatus.CharacterId
            });
        }

        public List<QuestStatus> FindAllByCharacterId(long characterId)
        {
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
            List<QuestStatus> questStatusList = new List<QuestStatus>();
            foreach (dynamic data in results)
            {
                questStatusList.Add((QuestStatus) ReadQuest(data));
            }

            return questStatusList;
        }

        public void Update(QuestStatus questStatus)
        {
            QueryFactory.Query(TableName).Where("id", questStatus.Id).Update(new
            {
                questStatus.Id,
                questStatus.Started,
                questStatus.Completed,
                start_timestamp = questStatus.StartTimestamp,
                complete_timestamp = questStatus.CompleteTimestamp,
                questStatus.Tracked,
                condition = JsonConvert.SerializeObject(questStatus.Condition),
                character_id = questStatus.CharacterId
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;

        private static QuestStatus ReadQuest(dynamic data) => new QuestStatus(data.uid, data.id, data.character_id, data.tracked, data.started, data.completed, data.start_timestamp, data.complete_timestamp, JsonConvert.DeserializeObject<List<Condition>>(data.condition));
    }
}
