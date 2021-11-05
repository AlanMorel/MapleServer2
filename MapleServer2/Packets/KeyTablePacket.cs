using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class KeyTablePacket
{
    public static PacketWriter AskKeyboardOrMouse()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.KEY_TABLE);
        pWriter.WriteByte(0x09);

        return pWriter;
    }

    // Tells client to load DefaultKey.xml
    public static PacketWriter RequestDefault()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.KEY_TABLE);
        pWriter.WriteByte(0x00);
        pWriter.WriteBool(true);

        return pWriter;
    }

    public static PacketWriter SendFullOptions(GameOptions options)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.KEY_TABLE);
        pWriter.WriteByte(0x00);
        pWriter.WriteBool(false);

        // Key bindings
        pWriter.WriteInt(options.KeyBinds.Count);
        foreach (KeyBind keyBind in options.KeyBinds.Values)
        {
            pWriter.Write<KeyBind>(keyBind);
        }

        // Hotbars
        pWriter.WriteHotbars(options);

        return pWriter;
    }

    public static PacketWriter SendHotbars(GameOptions options)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.KEY_TABLE);
        pWriter.WriteByte(0x7); // Type
        pWriter.WriteHotbars(options);

        return pWriter;
    }

    private static PacketWriter WriteHotbars(this PacketWriter pWriter, GameOptions options)
    {
        pWriter.WriteShort(options.ActiveHotbarId);
        pWriter.WriteShort((short) options.Hotbars.Count);

        foreach (Hotbar hotbar in options.Hotbars)
        {
            pWriter.WriteInt(hotbar.Slots.Length);
            for (int slotIndex = 0; slotIndex < hotbar.Slots.Length; slotIndex++)
            {
                pWriter.WriteInt(slotIndex);
                pWriter.Write(hotbar.Slots[slotIndex]);
            }
        }

        return pWriter;
    }
}
