using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using MapleServer2.Types;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class PrestigeLevelMissionMetadataStorage
{
    private static readonly Dictionary<int, PrestigeLevelMissionMetadata> Rewards = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.PrestigeLevelMission);
        List<PrestigeLevelMissionMetadata> items = Serializer.Deserialize<List<PrestigeLevelMissionMetadata>>(stream);
        foreach (PrestigeLevelMissionMetadata item in items)
        {
            Rewards[item.Id] = item;
        }
    }

    public static List<PrestigeMission> GetPrestigeMissions => Rewards.Values.Select(metadata => new PrestigeMission(metadata.Id)).ToList();

    public static PrestigeLevelMissionMetadata? GetMetadata(int id) => Rewards.GetValueOrDefault(id);
}
