using System.Collections.Generic;
using Maple2.Data.Types;
using Maple2Storage.Utils;

namespace Maple2.Data.Converter {
    public class CharacterProgressConverter : IModelConverter<ProgressState, Maple2.Sql.Model.CharacterProgress> {
        public Maple2.Sql.Model.CharacterProgress ToModel(ProgressState value, Maple2.Sql.Model.CharacterProgress unlock) {
            if (value == null) return null;

            unlock ??= new Maple2.Sql.Model.CharacterProgress();
            unlock.VisitedMaps = value.VisitedMaps.SerializeCollection<int>();
            unlock.Taxis = value.Taxis.SerializeCollection<int>();
            unlock.Titles = value.Titles.SerializeCollection<int>();
            unlock.Emotes = value.Emotes.SerializeCollection<int>();

            return unlock;
        }

        public ProgressState FromModel(Maple2.Sql.Model.CharacterProgress value) {
            if (value == null) return null;

            var unlock = new ProgressState();
            unlock.VisitedMaps = value.VisitedMaps.DeserializeCollection<HashSet<int>, int>();
            unlock.Taxis = value.Taxis.DeserializeCollection<HashSet<int>, int>();
            unlock.Titles = value.Titles.DeserializeCollection<HashSet<int>, int>();
            unlock.Emotes = value.Emotes.DeserializeCollection<HashSet<int>, int>();

            return unlock;
        }
    }
}