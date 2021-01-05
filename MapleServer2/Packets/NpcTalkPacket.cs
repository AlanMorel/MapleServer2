using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class NpcTalkPacket
    {
        // Unsure about how to handle the "DialogType" part of the packet.
        // When this is wrong, the game is stuck with an invisible dialog.
        public static Packet Respond(IFieldObject<Npc> npc, int scriptId)
        {
            return PacketWriter.Of(SendOp.NPC_TALK)
                .WriteByte(0x01)
                .WriteInt(npc.ObjectId)
                .WriteByte(10) // Type? 1 = shop, 2 = dialog
                .WriteInt(scriptId)
                .WriteInt()
                .WriteInt(9); // DialogType
        }

        public static Packet Close()
        {
            return PacketWriter.Of(SendOp.NPC_TALK).WriteByte(0x00);
        }
    }
}
