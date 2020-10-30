using Maple2.Data.Types;
using Maple2Storage.Utils;

namespace Maple2.Data.Converter {
    public class AchieveConverter : IModelConverter<Achieve, Maple2.Sql.Model.Achieve> {
        public Maple2.Sql.Model.Achieve ToModel(Achieve value, Maple2.Sql.Model.Achieve achieve) {
            if (value == null) return null;

            achieve ??= new Maple2.Sql.Model.Achieve();
            achieve.AchieveId = value.Id;
            achieve.Type = value.Type;
            achieve.StartGrade = value.StartGrade;
            achieve.CurrentGrade = value.CurrentGrade;
            achieve.EndGrade = value.EndGrade;
            achieve.Count = value.Count;
            achieve.Record = value.Record.SerializeDictionary();

            return achieve;
        }

        public Achieve FromModel(Maple2.Sql.Model.Achieve value) {
            if (value == null) return null;

            var achieve = new Achieve(value.AchieveId);
            achieve.Type = value.Type;
            achieve.StartGrade = value.StartGrade;
            achieve.CurrentGrade = value.CurrentGrade;
            achieve.EndGrade = value.EndGrade;
            achieve.Count = value.Count;
            achieve.Record = value.Record.DeserializeDictionary<int,long>();

            return achieve;
        }
    }
}