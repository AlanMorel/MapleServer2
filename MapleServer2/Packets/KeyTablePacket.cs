using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets {
    public static class KeyTablePacket {
        public static Packet AskKeyboardOrMouse() {
            return PacketWriter.Of(SendOp.KEY_TABLE)
                .WriteByte(0x09);
        }

        // Tells client to load DefaultKey.xml
        public static Packet RequestDefault() {
            return PacketWriter.Of(SendOp.KEY_TABLE)
                .WriteByte(0x00)
                .WriteBool(true);
        }

        public static Packet SendFullOptions(GameOptions options) {
            var pWriter = PacketWriter.Of(SendOp.KEY_TABLE)
                .WriteByte(0x00)
                .WriteBool(false);

            // Key bindings
            pWriter.WriteInt(options.KeyBinds.Count);
            foreach (KeyBind keyBind in options.KeyBinds.Values) {
                pWriter.Write<KeyBind>(keyBind);
            }

            // Hotbars
            pWriter.WriteHotbars(options);

            return pWriter;
        }

        public static Packet SendHotbars(GameOptions options) {
            return PacketWriter.Of(SendOp.KEY_TABLE)
                .WriteByte(0x7) // Type
                .WriteHotbars(options);
        }

        private static PacketWriter WriteHotbars(this PacketWriter pWriter, GameOptions options) {
            pWriter.WriteShort(options.ActiveHotbarId)
                .WriteShort((short)options.Hotbars.Count);

            foreach (Hotbar hotbar in options.Hotbars) {
                pWriter.WriteInt(hotbar.Slots.Length);
                for (int slotIndex = 0; slotIndex < hotbar.Slots.Length; slotIndex++) {
                    pWriter.WriteInt(slotIndex);
                    pWriter.Write<QuickSlot>(hotbar.Slots[slotIndex]);
                }
            }

            return pWriter;
        }
    }
}