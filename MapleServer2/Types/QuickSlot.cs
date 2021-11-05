using System.Runtime.InteropServices;

namespace MapleServer2.Types;

[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
public struct QuickSlot
{
    public int SkillId { get; set; }
    public int ItemId { get; set; }
    public long ItemUid { get; set; }

    public static QuickSlot From(int skillId, Item item)
    {
        return new()
        {
            SkillId = skillId, ItemId = item.Id, ItemUid = item.Uid
        };
    }

    public static QuickSlot From(int skillId, int itemId = 0, long itemUid = 0)
    {
        return new()
        {
            SkillId = skillId, ItemId = itemId, ItemUid = itemUid
        };
    }

    public override string ToString()
    {
        return $"QuickSlot({SkillId}, {ItemId}, {ItemUid})";
    }
}
