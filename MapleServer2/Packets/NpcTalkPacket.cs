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
                .WriteByte(15) // Type? 1 = shop, 2 = dialog
                .WriteInt(scriptId)
                .WriteInt()
                .WriteInt(5); // DialogType
        }

        private enum NpcType : byte
        {
            Dialog = 10,
        }

        private enum DialogType : int // All this worked with NpcType 10
        {
            NoOptions = 1, // no options to continue or exit
            Broken = 2, // appears "Quit:ESC" option
            Close1 = 3, // Close - Espace
            CloseNext = 4, // Close - Esc || Next - Space
            TalkOption = 5, // Option talk on the npc menu with: Close - Esc || Next - Space
            AcceptDecline = 6, // Decline - Esc || Accept - Space
            NoOptions2 = 7, // same as 1
            Close2 = 8, // same as 3 and 9
            Close3 = 9, // same as 3 and 8
            JobAdv = 11, // Nevermind - ESC || Perform Job Advancement
            AcceptDecline2 = 12, // Decline - Esc || Accept - Space
            GetTreatment = 13, // Decline - ESC || Get Treatment - Space
            StayGo = 14, // Stay - ESC || Go - Space
            AcceptDecline3 = 15, // Decline - Esc || Accept - Space
            Spin = 16, // Spin - Space
            Skip = 17, // Skip - Space
            GetTreatment2 = 18, // Decline - ESC || Get Treatment - Space

        }

        public static Packet Close()
        {
            return PacketWriter.Of(SendOp.NPC_TALK).WriteByte(0x00);
        }
    }
}
