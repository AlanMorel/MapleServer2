using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class NpcTalkPacket
    {
        // Unsure about how to handle the "DialogType" part of the packet.
        // When this is wrong, the game is stuck with an invisible dialog.
        public static Packet Respond(IFieldObject<Npc> npc, NpcType npcType, DialogType dialogType, int scriptId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NPC_TALK);
            pWriter.WriteByte(0x01);
            pWriter.WriteInt(npc.ObjectId);
            pWriter.WriteEnum(npcType);
            pWriter.WriteInt(scriptId);
            pWriter.WriteInt();
            pWriter.WriteEnum(dialogType);

            return pWriter;
        }

        public static Packet ContinueChat(int scriptId, ResponseType responseType, DialogType dialogType, int contentIndex, int questId = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NPC_TALK);
            pWriter.WriteByte(0x02);
            pWriter.WriteEnum(responseType);
            pWriter.WriteInt(questId);
            pWriter.WriteInt(scriptId);
            pWriter.WriteInt(contentIndex); // used when there is multiple contents for the same script id
            pWriter.WriteEnum(dialogType);

            return pWriter;
        }

        public static Packet Action(ActionType actionType, string window = "", string parameters = "", int function = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NPC_TALK);
            pWriter.WriteByte(0x03);
            pWriter.WriteEnum(actionType);
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

        public static Packet Close()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NPC_TALK);
            pWriter.WriteByte(0x00);

            return pWriter;
        }
    }
}
