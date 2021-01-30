using System.Linq;
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
                SkillTree skill = context.SkillTrees.Where(c => c.CharacterId == characterId).FirstOrDefault(s => s.SkillId == skillId);

                context.Remove(skill);
                context.SaveChanges();
            }
        }

        public static void EditSkill(/*SkillTree skill*/)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                context.SaveChanges();
            }
        }

        public static SkillTree FindSkill(long characterId, long skillId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                SkillTree skill = context.SkillTrees.Where(c => c.CharacterId == characterId).FirstOrDefault(s => s.SkillId == skillId);

                return skill;
            }
        }
    }
}
