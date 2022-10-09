using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class SetItemOptionMetadataStorage
{
    private static readonly Dictionary<int, SetItemOptionMetadata> SetItemOption = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.SetItemOption);
        List<SetItemOptionMetadata> options = Serializer.Deserialize<List<SetItemOptionMetadata>>(stream);
        foreach (SetItemOptionMetadata option in options)
        {
            SetItemOption[option.Id] = option;
        }
    }

    public static SetItemOptionMetadata? GetMetadata(int id)
    {
        return SetItemOption.Values.FirstOrDefault(x => x.Id == id);
    }
}
