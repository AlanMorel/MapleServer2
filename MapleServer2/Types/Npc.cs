using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Npc : NpcMetadata
    {
        public short ZRotation; // In degrees * 10

        public Npc(int id)
        {
            NpcMetadata npc = NpcMetadataStorage.GetNpcMetadata(id);
            Id = npc.Id;
            Animation = 255;
            Friendly = npc.Friendly;
        }
    }
}
