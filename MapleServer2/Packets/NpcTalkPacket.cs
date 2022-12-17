using System.Diagnostics;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class NpcTalkPacket
{
    private enum Mode : byte
    {
        Close = 0x00,
        Respond = 0x01,
        Continue = 0x02,
        Action = 0x03,
        CustomText = 0x04,
    }

    public static PacketWriter Respond(IFieldObject<NpcMetadata> npc, DialogType dialogType, int contentIndex, ResponseSelection responseSelection, int scriptId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NpcTalk);
        pWriter.Write(Mode.Respond);
        pWriter.WriteInt(npc.ObjectId);
        pWriter.Write(dialogType);
        pWriter.WriteInt(scriptId);
        pWriter.WriteInt(contentIndex);
        pWriter.Write(responseSelection);

        return pWriter;
    }

    public static PacketWriter ContinueChat(int scriptId, DialogType dialogType, ResponseSelection responseSelection, int contentIndex, int questId = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NpcTalk);
        pWriter.Write(Mode.Continue);
        pWriter.Write(dialogType);
        pWriter.WriteInt(questId);
        pWriter.WriteInt(scriptId);
        pWriter.WriteInt(contentIndex); // used when there is multiple contents for the same script id
        pWriter.Write(responseSelection);

        return pWriter;
    }

    public static PacketWriter Action(ActionType actionType, string window = "", string parameters = "", int portalId = 0, Item? item = null)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NpcTalk);
        pWriter.Write(Mode.Action);
        pWriter.Write(actionType);
        switch (actionType)
        {
            case ActionType.Portal:
                pWriter.WriteInt(portalId);
                break;
            case ActionType.OpenWindow:
                pWriter.WriteUnicodeString(window);
                pWriter.WriteUnicodeString(parameters);
                break;
            case ActionType.ItemReward:
                Debug.Assert(item != null, nameof(item) + " != null");
                pWriter.WriteInt(1); // item count. TODO: support multiple items
                pWriter.WriteInt(item.Id);
                pWriter.WriteByte((byte) item.Rarity);
                pWriter.WriteInt(item.Amount);
                pWriter.WriteItem(item);
                break;
        }

        return pWriter;
    }

    public static PacketWriter CustomText(string script, string voiceId, string illustration)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NpcTalk);
        pWriter.Write(Mode.CustomText);
        pWriter.WriteUnicodeString(script);
        pWriter.WriteUnicodeString(voiceId);
        pWriter.WriteUnicodeString(illustration);
        return pWriter;
    }

    public static PacketWriter Close()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NpcTalk);
        pWriter.Write(Mode.Close);

        return pWriter;
    }
}
