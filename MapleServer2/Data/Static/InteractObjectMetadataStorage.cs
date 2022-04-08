using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class InteractObjectMetadataStorage
{
    private static readonly Dictionary<int, InteractObjectMetadata> Interacts = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.InteractObject);
        List<InteractObjectMetadata> interactList = Serializer.Deserialize<List<InteractObjectMetadata>>(stream);
        foreach (InteractObjectMetadata interact in interactList)
        {
            Interacts[interact.Id] = interact;
        }
    }

    public static InteractObjectMetadata GetInteractObjectMetadata(int interactId)
    {
        return Interacts.GetValueOrDefault(interactId);
    }
}
