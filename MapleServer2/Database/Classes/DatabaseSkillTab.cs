using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseSkillTab
    {
        private readonly string TableName = "SkillTabs";

        public long CreateSkillTab(SkillTab skillTab, long characterId)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                skillTab.TabId,
                skillTab.Name,
                SkillLevels = JsonConvert.SerializeObject(skillTab.SkillLevels),
                characterId
            });
        }

        public List<SkillTab> FindAllByCharacterId(long characterId, int jobId)
        {
            IEnumerable<dynamic> skillTabsResult = DatabaseManager.QueryFactory.Query(TableName).Where("CharacterId", characterId).Get();
            List<SkillTab> skillTabs = new List<SkillTab>();
            foreach (dynamic item in skillTabsResult)
            {
                skillTabs.Add(new SkillTab(item.Name, jobId, item.TabId, item.Uid, JsonConvert.DeserializeObject<Dictionary<int, int>>(item.SkillLevels)));
            }
            return skillTabs;
        }

        public void Update(SkillTab skillTab)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Uid", skillTab.Uid).Update(new
            {
                skillTab.TabId,
                skillTab.Name,
                SkillLevels = JsonConvert.SerializeObject(skillTab.SkillLevels),
            });
        }

        public bool Delete(long uid) => DatabaseManager.QueryFactory.Query(TableName).Where("Uid", uid).Delete() == 1;
    }
}
