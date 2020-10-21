using System.Collections.Generic;
using Maple2.Data.Types;
using Maple2Storage.Utils;

namespace Maple2.Data.Converter {
    public class SkillTabConverter : IModelConverter<SkillTab, Maple2.Sql.Model.SkillTab> {
        public Maple2.Sql.Model.SkillTab ToModel(SkillTab value, Maple2.Sql.Model.SkillTab skillTab) {
            if (value == null) return null;

            skillTab ??= new Maple2.Sql.Model.SkillTab();
            skillTab.Id = value.Id;
            skillTab.Name = value.Name;
            skillTab.Skills = value.Skills.SerializeCollection();

            return skillTab;
        }

        public SkillTab FromModel(Maple2.Sql.Model.SkillTab value) {
            ICollection<SkillTabEntry> entries = value.Skills.DeserializeCollection<SkillTabEntry>();
            var skillTab = new SkillTab(value.Name, entries);
            skillTab.Id = value.Id;

            return skillTab;
        }
    }
}