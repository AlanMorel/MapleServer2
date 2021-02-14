using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Mob : NpcMetadata
    {
        public short ZRotation; // In degrees * 10

        public Mob(int id)
        {
            NpcMetadata mob = NpcMetadataStorage.GetNpcMetadata(id);
            if (mob != null)  // Temporary, while I figure out how to seperate Interactable Models from real NPCs.
            {
                Id = mob.Id;
                Animation = 255;
                Stats = mob.Stats;
                Friendly = mob.Friendly;
            }
        }
    }
}
