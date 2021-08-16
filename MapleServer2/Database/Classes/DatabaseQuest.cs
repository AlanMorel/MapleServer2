using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseQuest
    {
        public static long CreateQuest(QuestStatus questStatus)
        {
            return DatabaseManager.QueryFactory.Query("quests").InsertGetId<long>(new
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

        public static List<QuestStatus> FindAllByCharacterId(long characterId)
        {
            IEnumerable<dynamic> results = DatabaseManager.QueryFactory.Query("quests").Where("CharacterId", characterId).Get();
            List<QuestStatus> questStatusList = new List<QuestStatus>();
            foreach (dynamic data in results)
            {
                questStatusList.Add((QuestStatus) ReadQuest(data));
            }

            return questStatusList;
        }

        public static void Update(QuestStatus questStatus)
        {
            DatabaseManager.QueryFactory.Query("quests").Where("Id", questStatus.Id).Update(new
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

        public static bool Delete(long uid) => DatabaseManager.QueryFactory.Query("quests").Where("Uid", uid).Delete() == 1;

        private static QuestStatus ReadQuest(dynamic data) => new QuestStatus(data.Uid, data.Id, data.CharacterId, data.Started, data.Completed, data.StartTimestamp, data.CompleteTimestamp, JsonConvert.DeserializeObject<List<Condition>>(data.Condition));
    }
}
