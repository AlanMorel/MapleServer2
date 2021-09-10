using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseSkillTab : DatabaseTable
    {
        public DatabaseSkillTab() : base("SkillTabs") { }

        public long Insert(SkillTab skillTab, long characterId)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                skillTab.TabId,
                skillTab.Name,
                SkillLevels = JsonConvert.SerializeObject(skillTab.SkillLevels),
                characterId
            });
        }

        public List<SkillTab> FindAllByCharacterId(long characterId, int jobId)
        {
            IEnumerable<dynamic> skillTabsResult = QueryFactory.Query(TableName).Where("CharacterId", characterId).Get();
            List<SkillTab> skillTabs = new List<SkillTab>();
            foreach (dynamic item in skillTabsResult)
            {
                skillTabs.Add(new SkillTab(item.Name, jobId, item.TabId, item.Uid, JsonConvert.DeserializeObject<Dictionary<int, short>>(item.SkillLevels)));
            }
            return skillTabs;
        }

        public void Update(SkillTab skillTab)
        {
            QueryFactory.Query(TableName).Where("Uid", skillTab.Uid).Update(new
            {
                skillTab.TabId,
                skillTab.Name,
                SkillLevels = JsonConvert.SerializeObject(skillTab.SkillLevels),
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("Uid", uid).Delete() == 1;
    }
}
