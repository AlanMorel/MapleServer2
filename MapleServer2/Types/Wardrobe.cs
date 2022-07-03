using System.Data;
using System.Security.Policy;
using Maple2.Trigger.Enum;
using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Database.Types;

namespace MapleServer2.Types;

public class Wardrobe : IPacketSerializable
{
    public int Type;
    public int Key;
    public int Index;
    public string Name;
    public Dictionary<ItemSlot, Item> Equips;

    public Wardrobe() { }

    public Wardrobe(int type, int key, int index, string name, Dictionary<ItemSlot, Item> equips, Player player)
    {
        Type = type;
        Key = key;
        Index = index;
        Name = name;
        Equips = equips;
        DatabaseManager.Wardrobes.Insert(this, player.CharacterId);
        player.Wardrobes.Insert(index, this);
    }

    public void WriteTo(PacketWriter pWriter)
    {
        pWriter.WriteInt(Index);
        pWriter.WriteInt(Type);
        pWriter.WriteInt(Key);
        pWriter.WriteUnicodeString(Name);
        pWriter.WriteInt(Equips.Count);
        foreach (Item equip in Equips.Values)
        {
            pWriter.WriteLong(equip.Uid);
            pWriter.WriteInt(equip.Id);
            pWriter.WriteInt((int) equip.ItemSlot - 1); // wardrobe slots are offset by 1
            pWriter.WriteInt(equip.Rarity);
        }
    }
}
