using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class NpcTalkPacket
    {
        private enum NpcTalkMode : byte
        {
            Close = 0x00,
            Respond = 0x01,
            Continue = 0x02,
            Action = 0x03,
        }

        public static Packet Respond(IFieldObject<Npc> npc, NpcType npcType, DialogType dialogType, int scriptId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NPC_TALK);
            pWriter.WriteEnum(NpcTalkMode.Respond);
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
            pWriter.WriteEnum(NpcTalkMode.Continue);
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
            pWriter.WriteEnum(NpcTalkMode.Action);
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
            pWriter.WriteEnum(NpcTalkMode.Close);

            return pWriter;
        }
    }
}
