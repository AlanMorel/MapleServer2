using Maple2Storage.Types;

namespace MapleServer2.Types {
    public class Portal {
        public int Id;
        public bool IsVisible;
        public bool IsEnabled;
        public bool IsMinimapVisible;
        public CoordF Rotation;
        public int TargetMapId;

        public Portal(int id) {
            this.Id = id;
        }
    }
}