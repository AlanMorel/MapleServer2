using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Npc : NpcMetadata
    {
        public short ZRotation; // In degrees * 10
        public CoordF Velocity;

        public Npc(int id)
        {
            NpcMetadata npc = NpcMetadataStorage.GetNpcMetadata(id);
            if (npc != null)
            {
                Id = npc.Id;
                Animation = 255;
                Friendly = npc.Friendly;
                Kind = npc.Kind;
                ShopId = npc.ShopId;
            }
        }

        public bool IsShop()
        {
            return Kind == 13;
        }

        public bool IsBank()
        {
            return Kind == 2;
        }

        public bool IsBeauty()
        {
            return IsHair() || IsMakeUp() || IsSkin() || IsDye() || IsMirror();
        }

        public bool IsMakeUp()
        {
            return Kind == 30;
        }

        public bool IsSkin()
        {
            return Kind == 32;
        }

        public bool IsHair()
        {
            return Kind == 33;
        }

        public bool IsDye()
        {
            return Kind == 34;
        }

        public bool IsMirror()
        {
            return Kind == 35;
        }
    }
}
