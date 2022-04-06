using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ChatStickerMetadataStorage
{
    private static readonly Dictionary<int, ChatStickerMetadata> ChatSticker = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.ChatSticker}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<ChatStickerMetadata> items = Serializer.Deserialize<List<ChatStickerMetadata>>(stream);
        foreach (ChatStickerMetadata item in items)
        {
            ChatSticker[item.StickerId] = item;
        }
    }

    public static bool IsValid(int stickerId)
    {
        return ChatSticker.ContainsKey(stickerId);
    }

    public static ChatStickerMetadata GetMetadata(int stickerId)
    {
        return ChatSticker.GetValueOrDefault(stickerId);
    }

    public static byte GetGroupId(int stickerId)
    {
        return ChatSticker.GetValueOrDefault(stickerId).GroupId;
    }

    public static short GetCategoryId(int stickerId)
    {
        return ChatSticker.GetValueOrDefault(stickerId).CategoryId;
    }
}
