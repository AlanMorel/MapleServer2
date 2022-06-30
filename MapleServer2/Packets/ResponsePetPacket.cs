using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ResponsePetPacket
{
    private enum ResponsePetMode : byte
    {
        Add = 0x00,
        Remove = 0x01,
        UpdateName = 0x04,
        PotionSettings = 0x05,
        LootSettings = 0x06,
        Album = 0x07,
        LoadPetSettings = 0x09
    }

    public static PacketWriter Add(Pet pet)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ResponsePet);
        pWriter.Write(ResponsePetMode.Add);
        pWriter.WriteInt(pet.Owner.ObjectId);
        pWriter.WriteInt(pet.ObjectId);
        pWriter.WriteBool(true);
        if (false) // TODO: find when it should stop here
        {
            return pWriter;
        }

        pWriter.WriteInt(pet.Item.Id);
        pWriter.WriteLong(pet.Item.Uid);
        pWriter.WriteInt(pet.Item.Rarity);
        pWriter.WriteItem(pet.Item);
        pWriter.WriteLong(pet.Item.Uid);

        return pWriter;
    }

    public static PacketWriter Remove(Pet pet)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ResponsePet);
        pWriter.Write(ResponsePetMode.Remove);
        pWriter.WriteInt(pet.Owner.ObjectId);
        pWriter.WriteLong(pet.Item.Uid);

        return pWriter;
    }

    public static PacketWriter UpdateName(Pet pet)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ResponsePet);
        pWriter.Write(ResponsePetMode.UpdateName);
        pWriter.WriteInt(pet.Owner.ObjectId);
        pWriter.WriteUnicodeString(pet.Item.PetInfo.Name);
        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteShort();
        pWriter.WriteShort();
        pWriter.WriteShort();

        return pWriter;
    }

    public static PacketWriter LoadPetSettings(Pet pet)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ResponsePet);
        pWriter.Write(ResponsePetMode.LoadPetSettings);
        pWriter.WriteInt(pet.Owner.ObjectId);
        pWriter.WriteUnicodeString(pet.Item.PetInfo.Name);
        pWriter.WriteLong(pet.Item.PetInfo.Exp);
        pWriter.WriteInt();
        pWriter.WriteShort();
        pWriter.WriteShort();
        pWriter.WriteShort();
        pWriter.WriteClass(pet.Item.PetInfo.PotionSettings);
        pWriter.WriteClass(pet.Item.PetInfo.LootSettings);

        return pWriter;
    }

    public static PacketWriter UpdatePotions(Pet pet)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ResponsePet);
        pWriter.Write(ResponsePetMode.PotionSettings);
        pWriter.WriteInt(pet.Owner.ObjectId);
        pWriter.WriteClass(pet.Item.PetInfo.PotionSettings);

        return pWriter;
    }

    public static PacketWriter UpdateLoot(Pet pet)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ResponsePet);
        pWriter.Write(ResponsePetMode.LootSettings);
        pWriter.WriteInt(pet.Owner.ObjectId);
        pWriter.WriteClass(pet.Item.PetInfo.LootSettings);

        return pWriter;
    }

    public static PacketWriter LoadAlbum()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ResponsePet);
        pWriter.Write(ResponsePetMode.Album);
        pWriter.WriteInt(); // count
        for (int i = 0; i < 0; i++)
        {
            pWriter.WriteInt(); // pet id
            pWriter.WriteShort(); // catch count
        }

        return pWriter;
    }
}
