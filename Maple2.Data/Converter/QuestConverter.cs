using System.Collections.Generic;
using Maple2.Data.Types;
using Maple2.Data.Utils;
using Maple2Storage.Utils;

namespace Maple2.Data.Converter {
    public class QuestConverter : IModelConverter<Quest, Maple2.Sql.Model.Quest> {
        public Maple2.Sql.Model.Quest ToModel(Quest value, Maple2.Sql.Model.Quest quest) {
            if (value == null) return null;

            quest ??= new Maple2.Sql.Model.Quest();
            quest.QuestId = value.Id;
            quest.State = value.State;
            quest.CompletionCount = value.CompletionCount;
            quest.StartTime = value.StartTime.FromEpochSeconds();
            quest.EndTime = value.EndTime.FromEpochSeconds();
            quest.Accepted = value.Accepted;
            quest.Conditions = value.Conditions.SerializeCollection();

            return quest;
        }

        public Quest FromModel(Maple2.Sql.Model.Quest value) {
            if (value == null) return null;

            var quest = new Quest(value.QuestId);
            quest.State = value.State;
            quest.CompletionCount = value.CompletionCount;
            quest.StartTime = value.StartTime.ToEpochSeconds();
            quest.EndTime = value.EndTime.ToEpochSeconds();
            quest.Accepted = value.Accepted;
            quest.Conditions = value.Conditions.DeserializeCollection<List<int>, int>();

            return quest;
        }
    }
}