using Maple2Storage.Tools;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types;

public class MobSpawn
{
    public readonly int Id;
    public readonly CoordF SpawnPosition;
    public readonly int SpawnRadius;
    public readonly int MaxPopulation;
    public readonly List<NpcMetadata> SpawnMobs;
    public readonly SpawnMetadata SpawnData;
    public List<IFieldObject<NpcMetadata>> Mobs;

    public MobSpawn(int id, CoordF pos, int spawnRadius, int maxPopulation, SpawnMetadata spawnData /*, List<int> mobIDs = null*/)
    {
        Id = id;
        SpawnPosition = pos;
        SpawnRadius = spawnRadius;
        MaxPopulation = maxPopulation;
        SpawnMobs = SelectMobs(spawnData.Difficulty, spawnData.MinDifficulty, spawnData.Tags);
        SpawnData = spawnData;
        Mobs = new();
    }

    public MobSpawn(MapMobSpawn mapSpawnData) : this(mapSpawnData.Id, mapSpawnData.Coord.ToFloat(), mapSpawnData.SpawnRadius, mapSpawnData.NpcCount, mapSpawnData.SpawnData /*, mapSpawnData.NpcList*/)
    {

    }

    public static List<NpcMetadata> SelectMobs(int difficulty, int minDifficulty, IEnumerable<string> tags)
    {
        // Look into optimizing this.
        HashSet<NpcMetadata> matchedNpcs = new();
        foreach (string tag in tags)
        {
            foreach (NpcMetadata mob in NpcMetadataStorage.GetNpcsByMainTag(tag))
            {
                if (mob.NpcMetadataBasic.Difficulty >= minDifficulty && mob.NpcMetadataBasic.Difficulty <= difficulty)
                {
                    matchedNpcs.Add(mob);
                }
            }
        }
        if (matchedNpcs.Count == 0)
        {
            matchedNpcs.Add(NpcMetadataStorage.GetNpcMetadata(21000001));
        }
        return matchedNpcs.ToList();
    }
}
