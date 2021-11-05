using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class InteractObjectMetadataStorage
{
    private static readonly Dictionary<int, InteractObjectMetadata> interacts = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-interact-object-metadata");
        List<InteractObjectMetadata> interactList = Serializer.Deserialize<List<InteractObjectMetadata>>(stream);
        foreach (InteractObjectMetadata interact in interactList)
        {
            interacts[interact.Id] = interact;
        }
    }

    public static InteractObjectMetadata GetInteractObjectMetadata(int interactId)
    {
        return interacts.GetValueOrDefault(interactId);
    }
}
