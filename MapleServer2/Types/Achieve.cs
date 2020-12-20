using System.Collections.Generic;

namespace MapleServer2.Types {
    public class Achieve {
        public readonly int Id;
        public byte Type;
        public int StartGrade;
        public int CurrentGrade;
        public int EndGrade;
        public long Count;
        // Grade -> Timestamp of completed achievements
        public IDictionary<int, long> Record;

        public Achieve(int id) {
            Id = id;
            Record = new Dictionary<int, long>();
        }
    }
}