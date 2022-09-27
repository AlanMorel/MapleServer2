using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class SetItemInfoMetadataStorage
{
    private static readonly Dictionary<int, SetItemInfoMetadata> SetItemInfo = new();
    private static readonly Dictionary<int, SetItemInfoMetadata> ItemIdMap = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.SetItemInfo);
        List<SetItemInfoMetadata> sets = Serializer.Deserialize<List<SetItemInfoMetadata>>(stream);
        foreach (SetItemInfoMetadata set in sets)
        {
            SetItemInfo[set.Id] = set;
            
            foreach (int itemId in set.ItemIds)
            {
                ItemIdMap[itemId] = set;
            }
        }
    }

    public static SetItemInfoMetadata? GetMetadata(int id)
    {
        return SetItemInfo.Values.FirstOrDefault(x => x.Id == id);
    }

    public static SetItemInfoMetadata? GetMetadataFromItem(int id)
    {
        return ItemIdMap.TryGetValue(id, out SetItemInfoMetadata? info) ? info : null;
    }
}
