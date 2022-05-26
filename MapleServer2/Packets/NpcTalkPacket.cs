using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class NpcTalkPacket
{
    private enum NpcTalkMode : byte
    {
        Close = 0x00,
        Respond = 0x01,
        Continue = 0x02,
        Action = 0x03
    }

    public static PacketWriter Respond(IFieldObject<NpcMetadata> npc, NpcTalkType npcTalkType, DialogType dialogType, int scriptId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NPCTalk);
        pWriter.Write(NpcTalkMode.Respond);
        pWriter.WriteInt(npc.ObjectId);
        pWriter.Write(npcTalkType);
        pWriter.WriteInt(scriptId);
        pWriter.WriteInt();
        pWriter.Write(dialogType);

        return pWriter;
    }

    public static PacketWriter ContinueChat(int scriptId, ResponseType responseType, DialogType dialogType, int contentIndex, int questId = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NPCTalk);
        pWriter.Write(NpcTalkMode.Continue);
        pWriter.Write(responseType);
        pWriter.WriteInt(questId);
        pWriter.WriteInt(scriptId);
        pWriter.WriteInt(contentIndex); // used when there is multiple contents for the same script id
        pWriter.Write(dialogType);

        return pWriter;
    }

    public static PacketWriter Action(ActionType actionType, string window = "", string parameters = "", int function = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NPCTalk);
        pWriter.Write(NpcTalkMode.Action);
        pWriter.Write(actionType);
        switch (actionType)
        {
            case ActionType.Portal:
                pWriter.WriteInt(function);
                break;
            case ActionType.OpenWindow:
                pWriter.WriteUnicodeString(window);
                pWriter.WriteUnicodeString(parameters);
                break;
        }

        return pWriter;
    }

    public static PacketWriter Close()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NPCTalk);
        pWriter.Write(NpcTalkMode.Close);

        return pWriter;
    }
}
