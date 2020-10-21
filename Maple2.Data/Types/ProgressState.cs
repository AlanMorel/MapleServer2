using System.Collections.Generic;

namespace Maple2.Data.Types {
    public class ProgressState {
        public HashSet<int> VisitedMaps;
        public HashSet<int> Taxis;
        public HashSet<int> Titles;
        public HashSet<int> Emotes;

        public ProgressState() {
            VisitedMaps = new HashSet<int>();
            Taxis = new HashSet<int>();
            Titles = new HashSet<int>();
            Emotes = new HashSet<int>();
            VisitedMaps = new HashSet<int> {2000001, 2000023, 2000051, 2000062, 2000076, 2000100, 52000065};
            Taxis = new HashSet<int> {2000001, 2000023, 2000051, 2000062, 2000076, 2000100};
            Titles = new HashSet<int> {10000565, 10000251, 10000291, 10000292};
            Emotes = new HashSet<int> {
                90200057, 90200043, 90200092, 90200077, 90200073, 90200019, 90200020, 90200021,
                90200009, 90200027, 90200010, 90200028, 90200051, 90200015, 90200016, 90200055, 90200060, 90200017,
                90200018, 90200093
            };
        }
    }
}