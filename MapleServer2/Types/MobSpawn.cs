using System;
using System.Collections.Generic;
using System.Linq;
using MapleServer2.Data.Static;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types
{
    public class MobSpawn
    {
        public readonly int Id;
        public readonly CoordF SpawnPosition;
        public readonly int SpawnRadius;
        public readonly int MaxPopulation;
        public readonly List<NpcMetadata> SpawnMobs;
        public readonly SpawnMetadata SpawnData;
        public List<IFieldObject<Mob>> Mobs;

        public MobSpawn(int id, CoordF pos, int spawnRadius, int maxPopulation, SpawnMetadata spawnData, List<int> mobIDs = null)
        {
            Id = id;
            SpawnPosition = pos;
            SpawnRadius = spawnRadius;
            MaxPopulation = maxPopulation;
            SpawnMobs = SelectMobs(spawnData.Difficulty, spawnData.MinDifficulty, spawnData.Tags);
            //mobs.ForEach(Console.WriteLine);
            SpawnData = spawnData;
            Mobs = new List<IFieldObject<Mob>>();
        }

        public MobSpawn(MapMobSpawn mapSpawnData) : this(mapSpawnData.Id, mapSpawnData.Coord.ToFloat(), mapSpawnData.SpawnRadius, mapSpawnData.NpcCount, mapSpawnData.SpawnData, mapSpawnData.NpcList)
        {

        }

        public void Spawn(int count)
        {
            //List<CoordF> spawnPoints = SelectPoints(SpawnRadius);
            //foreach (NpcMetadata mob in SpawnMobs)
            //{
            //    int spawnCount = mob.NpcMetadataBasic.GroupSpawnCount;  // Spawn count changes due to field effect (?)
            //    if (spawnCount > SpawnData.Population)
            //    {
            //        break;
            //    }

            //    for (int i = 0; i < count && i < MaxPopulation; i++)
            //    {
            //        IFieldObject<Mob> fieldMob = RequestFieldObject(new Mob(mob.Id));
            //        fieldMob.Coord = SpawnPosition + spawnPoints[Mobs.Count % spawnPoints.Count];
            //        AddMob(fieldMob);
            //        Mobs.Add(fieldMob);
            //    }
            //}
        }

        public static List<CoordF> SelectPoints(int spawnRadius = Block.BLOCK_SIZE)
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

        public static List<CoordF> SelectPoints(int count, int spawnRadius)
        {
            return SelectPoints(spawnRadius).Take(count).ToList();
        }

        public static List<NpcMetadata> SelectMobs(int difficulty, int minDifficulty, string[] tags)
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
