using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ms2Database.DbClasses;

namespace Ms2Database.Controllers
{
    public class SkillTreeManager
    {
        public static void AddSkill(long characterId, long skillId, string skillName = "", int level = 0, bool learned = false)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                SkillTree skill = new SkillTree()
                {
                    CharacterId = characterId,
                    SkillId = skillId,
                    SkillName = skillName,
                    Level = level,
                    Learned = learned
                };
                context.Add(skill);
                context.SaveChanges();
            }
        }

        public static void DeleteSkill(long characterId, long skillId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                SkillTree skill = context.SkillTrees.Where(column => column.CharacterId == characterId)
                                                    .FirstOrDefault(column => column.SkillId == skillId);
                context.Remove(skill);
                context.SaveChanges();
            }
        }

        public void UpdateSkill(SkillTree skillObject)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                SkillTree skill = skillObject;
                context.SaveChanges();
            }
        }

        public SkillTree GetSkill(long characterId, long skillId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                SkillTree skill = context.SkillTrees.Where(column => column.CharacterId == characterId)
                                                    .FirstOrDefault(column => column.SkillId == skillId);

                return skill;
            }
        }

        public List<SkillTree> GetSkillTree(long characterId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                List<SkillTree> skillTree = context.SkillTrees.Where(column => column.CharacterId == characterId)
                                                              .ToList();
                return skillTree;
            }
        }
    }
}
