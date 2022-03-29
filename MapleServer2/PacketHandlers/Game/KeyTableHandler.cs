using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class KeyTableHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.KeyTable;

    private enum KeyTableEnum : byte
    {
        SetMacroKeybind = 0x01,
        SetKeyBind = 0x02,
        MoveQuickSlot = 0x03,
        AddToFirstSlot = 0x04,
        RemoveQuickSlot = 0x05,
        SetActiveHotbar = 0x08
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        KeyTableEnum requestType = (KeyTableEnum) packet.ReadByte();

        switch (requestType)
        {
            case KeyTableEnum.SetMacroKeybind:
            case KeyTableEnum.SetKeyBind:
                SetKeyBinds(session, packet);
                break;
            case KeyTableEnum.MoveQuickSlot:
                MoveQuickSlot(session, packet);
                break;
            case KeyTableEnum.AddToFirstSlot:
                AddToQuickSlot(session, packet);
                break;
            case KeyTableEnum.RemoveQuickSlot:
                RemoveQuickSlot(session, packet);
                break;
            case KeyTableEnum.SetActiveHotbar:
                SetActiveHotbar(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(requestType);
                break;
        }
    }

    private static void AddToQuickSlot(GameSession session, PacketReader packet)
    {
        short hotbarId = packet.ReadShort();
        if (!session.Player.GameOptions.TryGetHotbar(hotbarId, out Hotbar targetHotbar))
        {
            Logger.Warning("Invalid hotbar id {hotbarId}", hotbarId);
            return;
        }

        QuickSlot quickSlot = packet.Read<QuickSlot>();
        int targetSlot = packet.ReadInt();
        if (targetHotbar.AddToFirstSlot(quickSlot))
        {
            session.Send(KeyTablePacket.SendHotbars(session.Player.GameOptions));
        }
    }

    private static void SetKeyBinds(GameSession session, PacketReader packet)
    {
        int numBindings = packet.ReadInt();

        for (int i = 0; i < numBindings; i++)
        {
            KeyBind keyBind = packet.Read<KeyBind>();
            session.Player.GameOptions.SetKeyBind(ref keyBind);
        }
    }

    private static void MoveQuickSlot(GameSession session, PacketReader packet)
    {
        short hotbarId = packet.ReadShort();
        if (!session.Player.GameOptions.TryGetHotbar(hotbarId, out Hotbar targetHotbar))
        {
            Logger.Warning("Invalid hotbar id {hotbarId}", hotbarId);
            return;
        }

        // Adds or moves a quickslot around
        QuickSlot quickSlot = packet.Read<QuickSlot>();
        int targetSlot = packet.ReadInt();
        targetHotbar.MoveQuickSlot(targetSlot, quickSlot);

        session.Send(KeyTablePacket.SendHotbars(session.Player.GameOptions));
    }

    private static void RemoveQuickSlot(GameSession session, PacketReader packet)
    {
        short hotbarId = packet.ReadShort();
        if (!session.Player.GameOptions.TryGetHotbar(hotbarId, out Hotbar targetHotbar))
        {
            Logger.Warning("Invalid hotbar id {hotbarId}", hotbarId);
            return;
        }

        int skillId = packet.ReadInt();
        long itemUid = packet.ReadLong();
        if (targetHotbar.RemoveQuickSlot(skillId, itemUid))
        {
            session.Send(KeyTablePacket.SendHotbars(session.Player.GameOptions));
        }
    }

    private static void SetActiveHotbar(GameSession session, PacketReader packet)
    {
        short hotbarId = packet.ReadShort();

        session.Player.GameOptions.SetActiveHotbar(hotbarId);
    }
}
