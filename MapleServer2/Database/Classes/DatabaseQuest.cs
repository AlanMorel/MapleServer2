using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseQuest : DatabaseTable
    {
        public DatabaseQuest(string tableName) : base(tableName) { }

        public long Insert(QuestStatus questStatus)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                questStatus.Id,
                questStatus.Started,
                questStatus.Completed,
                questStatus.StartTimestamp,
                questStatus.CompleteTimestamp,
                Condition = JsonConvert.SerializeObject(questStatus.Condition),
                questStatus.CharacterId
            });
        }

        public List<QuestStatus> FindAllByCharacterId(long characterId)
        {
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("CharacterId", characterId).Get();
            List<QuestStatus> questStatusList = new List<QuestStatus>();
            foreach (dynamic data in results)
            {
                questStatusList.Add((QuestStatus) ReadQuest(data));
            }

            return questStatusList;
        }

        public void Update(QuestStatus questStatus)
        {
            QueryFactory.Query(TableName).Where("Id", questStatus.Id).Update(new
            {
                questStatus.Id,
                questStatus.Started,
                questStatus.Completed,
                questStatus.StartTimestamp,
                questStatus.CompleteTimestamp,
                Condition = JsonConvert.SerializeObject(questStatus.Condition),
                questStatus.CharacterId
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("Uid", uid).Delete() == 1;

        private static QuestStatus ReadQuest(dynamic data) => new QuestStatus(data.Uid, data.Id, data.CharacterId, data.Started, data.Completed, data.StartTimestamp, data.CompleteTimestamp, JsonConvert.DeserializeObject<List<Condition>>(data.Condition));
    }
}
