using MaplePacketLib2.Tools;
using MapleServer2.Database;

namespace MapleServer2.Types;

public class PetInfo : IPacketSerializable
{
    public readonly long Uid;
    public string Name { get; set; }
    public long Exp { get; set; }
    public short Level { get; set; }

    public PetPotionSettings PotionSettings { get; set; }

    public PetLootSettings LootSettings { get; set; }

    public PetInfo(long uid)
    {
        Uid = uid;
    }

    public PetInfo()
    {
        Name = "";
        Exp = 0;
        Level = 1;
        PotionSettings = new();
        LootSettings = new();

        Uid = DatabaseManager.Pets.Insert(this);
    }

    public PetInfo(PetInfo otherPetInfo)
    {
        Name = otherPetInfo.Name;
        Exp = otherPetInfo.Exp;
        Level = otherPetInfo.Level;
        PotionSettings = otherPetInfo.PotionSettings;
        LootSettings = otherPetInfo.LootSettings;

        Uid = DatabaseManager.Pets.Insert(this);
    }

    public void WriteTo(PacketWriter pWriter)
    {
        pWriter.WriteUnicodeString(Name);
        pWriter.WriteLong(Exp);
        pWriter.WriteInt();
        pWriter.WriteInt(Level);
        pWriter.WriteByte();
    }
}

public class PetPotionSettings : IPacketSerializable, IPacketDeserializable
{
    private const byte PotionCount = 2;
    public (int thresholdIndex, float threshold, int itemId)[] Potions = new (int, float, int)[PotionCount];

    public void WriteTo(PacketWriter pWriter)
    {
        pWriter.WriteByte((byte) Potions.Length);
        foreach ((int thresholdIndex, float threshold, int itemId) in Potions)
        {
            pWriter.WriteInt(thresholdIndex);
            pWriter.WriteFloat(threshold);
            pWriter.WriteInt(itemId);
        }
    }

    public void ReadFrom(PacketReader reader)
    {
        Potions = new (int, float, int)[PotionCount];

        byte count = reader.ReadByte();
        if (count != PotionCount)
        {
            throw new ArgumentException("Invalid potion count.");
        }

        for (int i = 0; i < count; i++)
        {
            Potions[i] = (reader.ReadInt(), reader.ReadFloat(), reader.ReadInt());
        }
    }
}

public class PetLootSettings : IPacketSerializable, IPacketDeserializable
{
    public bool Mesos;
    public bool Merets;
    public bool Other;
    public bool Currency;
    public bool Equipment;
    public bool Consumable;
    public bool Gemstone;
    public bool Dropped;
    public int MinRarity;
    public bool Enabled;

    public void WriteTo(PacketWriter pWriter)
    {
        pWriter.WriteBool(Mesos);
        pWriter.WriteBool(Merets);
        pWriter.WriteBool(Other);
        pWriter.WriteBool(Currency);
        pWriter.WriteBool(Equipment);
        pWriter.WriteBool(Consumable);
        pWriter.WriteBool(Gemstone);
        pWriter.WriteBool(Dropped);
        pWriter.WriteInt(MinRarity);
        pWriter.WriteBool(Enabled);
    }

    public void ReadFrom(PacketReader reader)
    {
        Mesos = reader.ReadBool();
        Merets = reader.ReadBool();
        Other = reader.ReadBool();
        Currency = reader.ReadBool();
        Equipment = reader.ReadBool();
        Consumable = reader.ReadBool();
        Gemstone = reader.ReadBool();
        Dropped = reader.ReadBool();
        MinRarity = reader.ReadInt();
        Enabled = reader.ReadBool();
    }
}
