using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class KeyTablePacket
{
    private enum KeyTablePacketMode : byte
    {
        SendFullOptions = 0x00,
        SendHotbars = 0x07,
        AskKeyboardOrMouse = 0x09,
    }

    public static PacketWriter SendFullOptions(GameOptions options)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.KEY_TABLE);
        pWriter.Write(KeyTablePacketMode.SendFullOptions);
        pWriter.WriteBool(false); // if true, load DefaultKey.xml

        // Key bindings
        pWriter.WriteInt(options.KeyBinds.Count);
        foreach (KeyBind keyBind in options.KeyBinds.Values)
        {
            pWriter.Write(keyBind);
        }

        // Hotbars
        pWriter.WriteHotbars(options);

        return pWriter;
    }

    public static PacketWriter SendHotbars(GameOptions options)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.KEY_TABLE);
        pWriter.Write(KeyTablePacketMode.SendHotbars);
        pWriter.WriteHotbars(options);

        return pWriter;
    }

    public static PacketWriter AskKeyboardOrMouse()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.KEY_TABLE);
        pWriter.Write(KeyTablePacketMode.AskKeyboardOrMouse);

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
