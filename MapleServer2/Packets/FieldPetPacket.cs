using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Managers.Actors;

namespace MapleServer2.Packets;

public static class FieldPetPacket
{
    public static PacketWriter AddPet(Pet pet)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldAddPet);

        pWriter.WriteInt(pet.ObjectId);
        pWriter.WriteInt(pet.Value.Id); // pet skin id
        pWriter.WriteInt(pet.Value.Id); // pet npc id
        pWriter.Write(pet.Coord);
        pWriter.Write(pet.Rotation);
        pWriter.WriteFloat(pet.Value.NpcMetadataModel.Scale);
        pWriter.WriteInt(pet.Owner.ObjectId);

        pWriter.DefaultStatsMob(pet);

        pWriter.WriteLong(pet.Item.Uid);
        pWriter.WriteByte();
        pWriter.WriteShort(pet.Item.PetInfo?.Level);
        pWriter.WriteShort(); // pet taming? calls lua calc_PetTamingRank
        pWriter.WriteInt();
        pWriter.WriteUnicodeString(pet.Item.PetInfo?.Name);

        return pWriter;
    }

    public static PacketWriter RemovePet(Pet pet)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldRemovePet);
        pWriter.WriteInt(pet.ObjectId);
        return pWriter;
    }
}
