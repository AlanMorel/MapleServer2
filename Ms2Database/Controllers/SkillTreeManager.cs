using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ms2Database.DbClasses;

namespace Ms2Database.Controllers
{
    public class SkillTreeManager
    {
        public void AddSkill(long characterId, long skillId, string skillName = "", int level = 0, bool learned = false)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                SkillTree Skill = new SkillTree()
                {
                    CharacterId = characterId,
                    SkillId = skillId,
                    SkillName = skillName,
                    Level = level,
                    Learned = learned
                };
                Context.Add(Skill);
                Context.SaveChanges();
            }
        }

        public void DeleteSkill(long characterId, long skillId)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                SkillTree Skill = Context.SkillTrees.Where(c => c.CharacterId == characterId)
                                                   .FirstOrDefault(s => s.SkillId == skillId);

                Context.Remove(Skill);
                Context.SaveChanges();
            }
        }

        public void EditSkill(SkillTree skill)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                SkillTree Skill = skill;
                Context.SaveChanges();
            }
        }

        public SkillTree FindSkill(long characterId, long skillId)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                SkillTree Skill = Context.SkillTrees.Where(c => c.CharacterId == characterId)
                                                    .FirstOrDefault(s => s.SkillId == skillId);

                return Skill;
            }
        }
    }
}
