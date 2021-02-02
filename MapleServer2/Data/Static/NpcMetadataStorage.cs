using System.Collections.Generic;
using System.IO;
using MapleServer2.Constants;
using Maple2Storage.Types.Metadata;
using ProtoBuf;
using System.Linq;

namespace MapleServer2.Data.Static
{
    public static class NpcMetadataStorage
    {
        private static readonly Dictionary<int, NpcMetadata> Npcs = new Dictionary<int, NpcMetadata>();

        static NpcMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-npc-metadata");
            List<NpcMetadata> npcList = Serializer.Deserialize<List<NpcMetadata>>(stream);
            foreach (NpcMetadata npc in npcList)
            {
                Npcs.Add(npc.Id, npc);
            }
        }

        public static NpcMetadata GetNpc(int id)
        {
            return Npcs.GetValueOrDefault(id);
        }

        public static NpcMetadata GetNpcMetadata(int id)
        {
            NpcMetadata newNpc = Npcs.GetValueOrDefault(id);
            if (newNpc != null)
            {
                if (newNpc.Friendly == 2)
                {
                    return Npcs.Select(x => x.Value).Where(x => x.Friendly == 2 && x.Id == id).FirstOrDefault();
                }
                else
                {
                    return Npcs.Select(x => x.Value).Where(x => x.Friendly == 0 && x.Id == id).FirstOrDefault();
                }
            }
            else
            {
                return Npcs.GetValueOrDefault(11000010);
            }
        }
    }
}
