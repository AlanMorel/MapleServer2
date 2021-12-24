using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ChatStickerMetadataStorage
{
    private static readonly Dictionary<int, ChatStickerMetadata> ChatSticker = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-chat-sticker-metadata");
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
