using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseSkillTab : DatabaseTable
    {
        public DatabaseSkillTab() : base("skilltabs") { }

        public long Insert(SkillTab skillTab, long characterId)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                skillTab.TabId,
                skillTab.Name,
                skilllevels = JsonConvert.SerializeObject(skillTab.SkillLevels),
                characterId
            });
        }

        public List<SkillTab> FindAllByCharacterId(long characterId, int jobId)
        {
            IEnumerable<dynamic> skillTabsResult = QueryFactory.Query(TableName).Where("characterid", characterId).Get();
            List<SkillTab> skillTabs = new List<SkillTab>();
            foreach (dynamic data in skillTabsResult)
            {
                skillTabs.Add(new SkillTab(data.name, jobId, data.tabid, data.uid, JsonConvert.DeserializeObject<Dictionary<int, short>>(data.skilllevels)));
            }
            return skillTabs;
        }

        public void Update(SkillTab skillTab)
        {
            QueryFactory.Query(TableName).Where("uid", skillTab.Uid).Update(new
            {
                skillTab.TabId,
                skillTab.Name,
                skilllevels = JsonConvert.SerializeObject(skillTab.SkillLevels),
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
    }
}
