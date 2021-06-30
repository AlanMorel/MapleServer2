using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class DungeonStorage
    {
        private static readonly Dictionary<int, DungeonMetadata> Dungeons = new Dictionary<int, DungeonMetadata>();

        static DungeonStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-dungeon-metadata");
            List<DungeonMetadata> dungeons = Serializer.Deserialize<List<DungeonMetadata>>(stream);
            foreach (DungeonMetadata dungeon in dungeons)
            {
                Dungeons.Add(dungeon.DungeonRoomId, dungeon);
            }
        }

        public static DungeonMetadata GetDungeonByDungeonId(int dungeonId)
        {
            return Dungeons.GetValueOrDefault(dungeonId);
        }

        //public static short GetSequenceIdBySequenceName(string actorId, string sequenceName)
        //{
        //    List<SequenceMetadata> sequences = GetSequencesByActorId(actorId);
        //    SequenceMetadata metadata = sequences.FirstOrDefault(s => s.SequenceName == sequenceName);

        //    if (metadata != default)
        //    {
        //        return metadata.SequenceId;
        //    }

        //    return 0;
        //}
    }
}
