using System;
using System.Collections.Generic;
using System.Linq;
using MapleServer2.Data.Static;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Tools
{
    public class SpawnGenerator
    {
        public static List<CoordF> Points(int spawnRadius = Block.BLOCK_SIZE)
        {
            List<CoordF> spawnOffsets = new List<CoordF>();
            int spawnSize = (spawnRadius / Block.BLOCK_SIZE) * 2;
            for (int i = 0; i <= spawnSize; i++)
            {
                for (int j = 0; j <= spawnSize; j++)
                {
                    spawnOffsets.Add(CoordF.From(i * Block.BLOCK_SIZE - spawnRadius, j * Block.BLOCK_SIZE - spawnRadius, 0));
                }
            }
            Random offsetRNG = new Random();
            return spawnOffsets.OrderBy(x => offsetRNG.Next()).ToList();
        }

        public static List<CoordF> Points(int count, int spawnRadius)
        {
            return Points(spawnRadius).Take(count).ToList();
        }

        public static List<NpcMetadata> Mobs(int difficulty, int minDifficulty, string[] tags)
        {
            // Look into optimizing this.
            HashSet<NpcMetadata> matchedNpcs = new HashSet<NpcMetadata>();
            foreach (string tag in tags)
            {
                foreach (NpcMetadata data in NpcMetadataStorage.GetNpcsByMainTag(tag))
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
