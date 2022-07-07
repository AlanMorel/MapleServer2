using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class PetHandler : GamePacketHandler<PetHandler>
{
    public override RecvOp OpCode => RecvOp.RequestPet;

    private enum Mode : byte
    {
        Summon = 0x0,
        Dismiss = 0x1,
        Replace = 0x3,
        Rename = 0x4,
        PotionSettings = 0x5,
        LootSettings = 0x6,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.Summon:
                HandlePetSummon(session, packet);
                break;
            case Mode.Dismiss:
                HandlePetDismiss(session, packet);
                break;
            case Mode.Replace:
                HandlePetReplace(session, packet);
                break;
            case Mode.Rename:
                HandlePetRename(session, packet);
                break;
            case Mode.PotionSettings:
                HandlePetPotionSettings(session, packet);
                break;
            case Mode.LootSettings:
                HandlePetLootSettings(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandlePetSummon(GameSession session, PacketReader packet)
    {
        long uid = packet.ReadLong();

        AddPet(session, uid);
    }

    private static void HandlePetDismiss(GameSession session, PacketReader packet)
    {
        long uid = packet.ReadLong();

        Character fieldPlayer = session.Player.FieldPlayer;
        if (fieldPlayer.ActivePet is null || fieldPlayer.ActivePet.Item.Uid != uid)
        {
            return;
        }

        session.FieldManager.RemovePet(fieldPlayer.ActivePet);
    }

    private static void HandlePetRename(GameSession session, PacketReader packet)
    {
        string name = packet.ReadUnicodeString();

        Player player = session.Player;
        Character fieldPlayer = player.FieldPlayer;
        if (fieldPlayer.ActivePet is null || player.ActivePet is null)
        {
            return;
        }

        player.ActivePet.PetInfo.Name = name;
        fieldPlayer.ActivePet.Item = player.ActivePet;
        session.Send(PetPacket.UpdateName(fieldPlayer.ActivePet));

        DatabaseManager.Pets.Update(fieldPlayer.ActivePet.Item.PetInfo);
    }

    private static void HandlePetReplace(GameSession session, PacketReader packet)
    {
        long uid = packet.ReadLong();

        Character fieldPlayer = session.Player.FieldPlayer;
        if (fieldPlayer.ActivePet is null)
        {
            return;
        }

        session.FieldManager.RemovePet(fieldPlayer.ActivePet);

        AddPet(session, uid);
    }

    private static void HandlePetPotionSettings(GameSession session, PacketReader packet)
    {
        PetPotionSettings settings = packet.ReadClass<PetPotionSettings>();

        Player player = session.Player;
        Character fieldPlayer = player.FieldPlayer;
        if (fieldPlayer.ActivePet is null || player.ActivePet is null)
        {
            return;
        }

        player.ActivePet.PetInfo.PotionSettings = settings;
        fieldPlayer.ActivePet.Item = player.ActivePet;
        session.Send(PetPacket.UpdatePotions(fieldPlayer.ActivePet));

        DatabaseManager.Pets.Update(fieldPlayer.ActivePet.Item.PetInfo);
    }

    private static void HandlePetLootSettings(GameSession session, PacketReader packet)
    {
        PetLootSettings settings = packet.ReadClass<PetLootSettings>();

        Player player = session.Player;
        Character fieldPlayer = player.FieldPlayer;
        if (fieldPlayer.ActivePet is null || player.ActivePet is null)
        {
            return;
        }

        player.ActivePet.PetInfo.LootSettings = settings;
        fieldPlayer.ActivePet.Item = player.ActivePet;
        session.Send(PetPacket.UpdateLoot(fieldPlayer.ActivePet));

        DatabaseManager.Pets.Update(fieldPlayer.ActivePet.Item.PetInfo);
    }

    private static void AddPet(GameSession session, long uid)
    {
        Player player = session.Player;
        Item item = player.Inventory.GetByUid(uid);
        if (item is null)
        {
            return;
        }

        Pet pet = session.FieldManager.RequestPet(item, player.FieldPlayer);
        if (pet is null)
        {
            return;
        }

        if (item.TransferType == TransferType.BindOnSummonEnchantOrReroll & !item.IsBound())
        {
            item.BindItem(session.Player);
        }

        player.ActivePet = pet.Item;
        player.FieldPlayer.ActivePet = pet;

        session.Send(PetPacket.LoadPetSettings(pet));
        session.Send(NoticePacket.Notice(SystemNotice.PetSummonOn, NoticeType.Chat | NoticeType.FastText));
    }
}
