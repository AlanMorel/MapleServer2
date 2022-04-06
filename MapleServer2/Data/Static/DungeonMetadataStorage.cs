using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class DungeonStorage
{
    private static readonly Dictionary<int, DungeonMetadata> Dungeons = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Dungeon}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
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
}
