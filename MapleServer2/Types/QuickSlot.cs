using System.Runtime.InteropServices;
using Maple2.Data.Types.Items;

namespace MapleServer2.Types {
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct QuickSlot {
        public int SkillId { get; private set; }
        public int ItemId { get; private set; }
        public long ItemUid { get; private set; }

        public static QuickSlot From(int skillId, Item item) {
            return new QuickSlot {
                SkillId = skillId,
                ItemId = item.Id,
                ItemUid = item.Uid
            };
        }

        public static QuickSlot From(int skillId, int itemId = 0, long itemUid = 0) {
            return new QuickSlot {
                SkillId = skillId,
                ItemId = itemId,
                ItemUid = itemUid
            };
        }

        public override string ToString() => $"QuickSlot({SkillId}, {ItemId}, {ItemUid})";
    }
}
