using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseSkillTab : DatabaseTable
{
    public DatabaseSkillTab() : base("skill_tabs") { }

    public long Insert(SkillTab skillTab, long characterId)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            tab_id = skillTab.TabId, skillTab.Name, skill_levels = JsonConvert.SerializeObject(skillTab.SkillLevels), character_id = characterId
        });
    }

    public List<SkillTab> FindAllByCharacterId(long characterId, int jobId)
    {
        IEnumerable<dynamic> skillTabsResult = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        List<SkillTab> skillTabs = new();
        foreach (dynamic data in skillTabsResult)
        {
            skillTabs.Add(new SkillTab(data.name, jobId, data.tab_id, data.uid, JsonConvert.DeserializeObject<Dictionary<int, short>>(data.skill_levels)));
        }
        return skillTabs;
    }

    public void Update(SkillTab skillTab)
    {
        QueryFactory.Query(TableName).Where("uid", skillTab.Uid).Update(new
        {
            tab_id = skillTab.TabId, skillTab.Name, skill_levels = JsonConvert.SerializeObject(skillTab.SkillLevels)
        });
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
    }
}
