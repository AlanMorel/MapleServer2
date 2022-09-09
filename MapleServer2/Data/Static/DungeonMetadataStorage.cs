using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class DungeonStorage
{
    private static readonly Dictionary<int, DungeonMetadata> Dungeons = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Dungeon);
        List<DungeonMetadata> dungeons = Serializer.Deserialize<List<DungeonMetadata>>(stream);
        foreach (DungeonMetadata dungeon in dungeons)
        {
            Dungeons.Add(dungeon.DungeonRoomId, dungeon);
        }
    }

    public static DungeonMetadata? GetDungeonByDungeonId(int dungeonId)
    {
        return Dungeons.GetValueOrDefault(dungeonId);
    }
}
