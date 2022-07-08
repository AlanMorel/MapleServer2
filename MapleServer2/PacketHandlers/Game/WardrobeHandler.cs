using Maple2.Trigger.Enum;
using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using static MapleServer2.Packets.TriggerPacket;

namespace MapleServer2.PacketHandlers.Game;

public class WardrobeHandler : GamePacketHandler<WardrobeHandler>
{
    public override RecvOp OpCode => RecvOp.Wardrobe;

    private enum Mode : byte
    {
        Save = 0,
        Equip = 1,
        Reset = 2,
        SetShortcut = 4,
        Rename = 6,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.Save:
                HandleSave(session, packet);
                break;
            case Mode.Equip:
                HandleEquip(session, packet);
                break;
            case Mode.Reset:
                HandleReset(session, packet);
                break;
            case Mode.SetShortcut:
                HandleSetShortcut(session, packet);
                break;
            case Mode.Rename:
                HandleRename(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleSave(GameSession session, PacketReader packet)
    {
        int index = packet.ReadInt();
        int type = packet.ReadInt();
        Dictionary<ItemSlot, Item> equips = type == 0 ? session.Player.Inventory.Equips : session.Player.Inventory.Cosmetics;

        if (!session.Player.TryGetWardrobe(index, out Wardrobe wardrobe))
        {
            return;
        }

        foreach (Item equip in equips.Values)
        {
            // remove to not save Hair, Face, Ears, or Face Decoration
            if (equip.ItemSlot is ItemSlot.HR or ItemSlot.ER or ItemSlot.FD or ItemSlot.FA)
            {
                equips.Remove(equip.ItemSlot);
                continue;
            }
        }

        wardrobe.Equips = new(equips);
        wardrobe.Type = type;

        DatabaseManager.Wardrobes.Update(wardrobe, session.Player.CharacterId);
        session.Send(WardrobePacket.Load(wardrobe));
    }

    private static void HandleEquip(GameSession session, PacketReader packet)
    {
        int index = packet.ReadInt();
        int type = packet.ReadInt();

        if (!session.Player.TryGetWardrobe(index, out Wardrobe wardrobe))
        {
            return;
        }

        int failEquipCount = 0;
        foreach (KeyValuePair<ItemSlot, Item> item in wardrobe.Equips)
        {
            if (!session.Player.Inventory.TryEquip(session, item.Value.Uid, item.Key))
            {
                failEquipCount++;
            }
        }

        SystemNotice notice = failEquipCount == 0 ? SystemNotice.ClosetMsgSuccess : SystemNotice.ClosetMsgItemNotExist;
        session.Send(NoticePacket.Notice(notice, NoticeType.Chat | NoticeType.FastText));
    }

    private static void HandleReset(GameSession session, PacketReader packet)
    {
        int index = packet.ReadInt();
        if (!session.Player.TryGetWardrobe(index, out Wardrobe wardrobe))
        {
            return;
        }

        wardrobe.Equips.Clear();

        DatabaseManager.Wardrobes.Update(wardrobe, session.Player.CharacterId);
        session.Send(WardrobePacket.Load(wardrobe));
    }

    private static void HandleSetShortcut(GameSession session, PacketReader packet)
    {
        int index = packet.ReadInt();
        int keyCode = packet.ReadInt();

        if (!session.Player.TryGetWardrobe(index, out Wardrobe wardrobe))
        {
            return;
        }

        wardrobe.Key = keyCode;
    }

    private static void HandleRename(GameSession session, PacketReader packet)
    {
        int index = packet.ReadInt();
        string name = packet.ReadUnicodeString();

        if (!session.Player.TryGetWardrobe(index, out Wardrobe wardrobe))
        {
            session.Player.Wardrobes.Insert(index, new(0, 0, index, name, new(), session.Player));
            return;
        }

        wardrobe.Name = name;
        DatabaseManager.Wardrobes.Update(wardrobe, session.Player.CharacterId);
    }
}
