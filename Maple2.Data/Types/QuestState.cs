using System;
using System.Collections.Generic;

namespace Maple2.Data.Types {
    public class QuestState {
        public readonly IDictionary<int, Quest> Quests;
        public readonly ISet<int> SkyFortressQuests;
        public readonly ISet<int> KritiasQuests;

        public QuestState() {
            Quests = new Dictionary<int, Quest>();
            SkyFortressQuests = new HashSet<int>();
            KritiasQuests = new HashSet<int>();
        }

        public bool StartQuest(int questId) {
            if (!Quests.ContainsKey(questId)) {
                Quests[questId] = new Quest(questId);
            }

            Quest quest = Quests[questId];
            if (quest.Accepted) {
                return false;
            }

            quest.State = 1;
            quest.Accepted = true;
            quest.StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return true;
        }

        public bool FinishQuest(int questId) {
            if (!Quests.TryGetValue(questId, out Quest quest)) {
                return false;
            }

            quest.State = 2;
            quest.CompletionCount++;
            quest.EndTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return true;
        }
    }
}