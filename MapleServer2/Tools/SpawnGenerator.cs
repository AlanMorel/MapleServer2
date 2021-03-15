using System;
using System.Collections.Generic;
using System.Linq;
using MapleServer2.Data.Static;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Tools
{
    class SpawnGenerator
    {
        public static List<CoordF> Points(int spawnRadius = 150)
        {
            HashSet<CoordF> spawnOffsets = new HashSet<CoordF> { CoordS.From(0, 0, 0).ToFloat() };
            int blockSize = 150;
            int size = spawnRadius / 150;
            for (int i = 0; i <= size; i++)
            {
                for (int j = 0; j <= size; j++)
                {
                    spawnOffsets.Add(CoordS.From((short) (i * blockSize), (short) (j * blockSize), 0).ToFloat());
                    spawnOffsets.Add(CoordS.From((short) (i * -blockSize), (short) (j * -blockSize), 0).ToFloat());
                }
            }
            Random offsetRNG = new Random();
            return spawnOffsets.ToList().OrderBy(x => offsetRNG.Next()).ToList();
        }

        public static List<CoordF> Points(int count, int spawnRadius = 150)
        {
            return Points(spawnRadius).Take(count).ToList();
        }

        public static List<NpcMetadata> Mobs(int difficulty, int minDifficulty, string[] tags)
        {
            HashSet<NpcMetadata> matchedNpcs = new HashSet<NpcMetadata>();
            foreach (string tag in tags)
            {
                foreach (NpcMetadata data in NpcMetadataStorage.GetMainNpcs(tag))
                {
                    if (data.NpcMetadataBasic.Difficulty >= minDifficulty && data.NpcMetadataBasic.Difficulty <= difficulty)
                    {
                        matchedNpcs.Add(data);
                    }
                }
            }
            if (matchedNpcs.Count == 0)
            {
                matchedNpcs.Add(NpcMetadataStorage.GetNpc(21000001));
            }
            return matchedNpcs.ToList();
        }
    }
}
