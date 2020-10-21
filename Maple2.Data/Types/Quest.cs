using System.Collections.Generic;

namespace Maple2.Data.Types {
    public class Quest {
        public readonly int Id;
        public int State;
        public int CompletionCount;
        public long StartTime;
        public long EndTime;
        public bool Accepted;
        public IList<int> Conditions;

        public Quest(int id) {
            Id = id;
            Conditions = new List<int>();
        }
    }
}