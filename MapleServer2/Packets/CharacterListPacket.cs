using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class CharacterListPacket
{
    private enum Mode : byte
    {
        AddEntries = 0x00,
        AppendEntry = 0x01,
        DeleteCharacter = 0x02,
        StartList = 0x03,
        EndList = 0x04,
        DeletePending = 0x05,
        DeleteCancel = 0x06,
        NameChange = 0x07
    }

    public static PacketWriter AddEntries(List<Player> players)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharList);
        pWriter.Write(Mode.AddEntries);
        pWriter.WriteByte((byte) players.Count);
        foreach (Player player in players)
        {
            pWriter.WriteCharacterEntry(player);
        }

        return pWriter;
    }

    // Sent after creating a character to append to list
    public static PacketWriter AppendEntry(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharList);
        pWriter.Write(Mode.AppendEntry);
        WriteCharacterEntry(pWriter, player);

        return pWriter;
    }

    public static PacketWriter DeleteCharacter(long playerId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharList);
        pWriter.Write(Mode.DeleteCharacter);
        pWriter.WriteInt(); // unk
        pWriter.WriteLong(playerId);

        return pWriter;
    }

    public static PacketWriter DeletePending(long playerId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharList);
        pWriter.Write(Mode.DeletePending);
        pWriter.WriteLong(playerId);
        pWriter.WriteInt(); // unk
        pWriter.WriteLong(); // delete timestamp

        return pWriter;
    }

    public static PacketWriter DeleteCancel(long playerId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharList);
        pWriter.Write(Mode.DeleteCancel);
        pWriter.WriteLong(playerId);
        pWriter.WriteInt(); // unk

        return pWriter;
    }

    public static PacketWriter SetMax(int unlocked)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharMaxCount);
        pWriter.WriteInt(unlocked);
        pWriter.WriteInt(int.Parse(ConstantsMetadataStorage.GetConstant("MaxCharacterSlots")));

        return pWriter;
    }

    private static void WriteCharacterEntry(this PacketWriter pWriter, Player player)
    {
        pWriter.WriteClass(player);

        pWriter.WriteUnicodeString(player.ProfileUrl);
        pWriter.WriteLong(player.DeletionTime);

        WriteEquipsAndCosmetics(pWriter, player);

        WriteBadges(pWriter, player);

        bool boolValue = false;
        pWriter.WriteBool(boolValue);
        if (boolValue)
        {
            pWriter.WriteLong();
            pWriter.WriteLong();
            bool otherBoolValue = true;
            pWriter.WriteBool(otherBoolValue);
            if (otherBoolValue)
            {
                pWriter.WriteInt();
                pWriter.WriteLong();
                pWriter.WriteUnicodeString("abc");
                pWriter.WriteInt();
            }
        }
    }

    // Note, the client actually uses item id to determine type
    public static void WriteEquip(ItemSlot slot, Item item, PacketWriter pWriter)
    {
        pWriter.WriteInt(item.Id);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteUnicodeString(slot.ToString());
        pWriter.WriteInt(item.Rarity);
        pWriter.WriteItem(item);
    }

    public static void WriteEquipsAndCosmetics(PacketWriter pWriter, Player player)
    {
        pWriter.WriteByte((byte) (player.Inventory.Equips.Count + player.Inventory.Cosmetics.Count));
        foreach ((ItemSlot slot, Item equip) in player.Inventory.Equips)
        {
            WriteEquip(slot, equip, pWriter);
        }
        foreach ((ItemSlot slot, Item equip) in player.Inventory.Cosmetics)
        {
            WriteEquip(slot, equip, pWriter);
        }
    }

    public static void WriteBadges(PacketWriter pWriter, Player player)
    {
        pWriter.WriteByte((byte) player.Inventory.Badges.Count(x => x != null));
        for (int i = 0; i < player.Inventory.Badges.Length; i++)
        {
            Item badge = player.Inventory.Badges[i];
            if (player.Inventory.Badges[i] != null)
            {
                pWriter.WriteByte((byte) i);
                pWriter.WriteInt(badge.Id);
                pWriter.WriteLong(badge.Uid);
                pWriter.WriteInt(badge.Rarity);
                pWriter.WriteItem(badge);
            }
        }
    }

    public static PacketWriter StartList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharList);
        pWriter.Write(Mode.StartList);

        return pWriter;
    }

    // This only needs to be sent if char count > 0
    public static PacketWriter EndList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharList);
        pWriter.Write(Mode.EndList);
        pWriter.WriteBool(false);

        return pWriter;
    }

    public static PacketWriter NameChanged(long characterId, string characterName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharList);
        pWriter.Write(Mode.NameChange);
        pWriter.WriteInt(1);
        pWriter.WriteLong(characterId);
        pWriter.WriteUnicodeString(characterName);
        return pWriter;
    }
}
