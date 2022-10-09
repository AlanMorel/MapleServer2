using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class Hotbar
{
    public readonly long Id;
    private const int MaxSlots = 22;
    public QuickSlot[] Slots { get; private set; }

    public Hotbar(long gameOptionsId)
    {
        Slots = new QuickSlot[MaxSlots];

        for (int i = 0; i < MaxSlots; i++)
        {
            Slots[i] = new();
        }

        Id = DatabaseManager.Hotbars.Insert(this, gameOptionsId);
    }

    public Hotbar(long gameOptionsId, JobCode jobCode)
    {
        Slots = new QuickSlot[MaxSlots];

        for (int i = 0; i < MaxSlots; i++)
        {
            Slots[i] = new();
        }

        AddDefaultSkills();

        Id = DatabaseManager.Hotbars.Insert(this, gameOptionsId);

        void AddDefaultSkills()
        {
            JobMetadata jobMetadata = JobMetadataStorage.GetJobMetadata(jobCode);
            if (jobMetadata is null)
            {
                return;
            }

            List<int> skillIds = new();
            jobMetadata.LearnedSkills.ForEach(x => skillIds.AddRange(x.SkillIds));

            List<(int skillId, byte slotPriority)> hotbarSkills = new();
            foreach (int skillId in skillIds)
            {
                JobSkillMetadata jobSkillMetadata = jobMetadata.Skills.First(x => x.SkillId == skillId);
                if (jobSkillMetadata.QuickSlotPriority != 99 && jobSkillMetadata.SubJobCode == 0)
                {
                    hotbarSkills.Add((skillId, jobSkillMetadata.QuickSlotPriority));
                }
            }

            foreach ((int skillId, byte _) in hotbarSkills.OrderBy(x => x.slotPriority))
            {
                AddToFirstSlot(QuickSlot.From(skillId));
            }
        }
    }

    public Hotbar(QuickSlot[] slots, long id)
    {
        Slots = slots;
        Id = id;
    }

    public bool AddToFirstSlot(QuickSlot quickSlot)
    {
        if (Slots.Contains(quickSlot))
        {
            return false;
        }

        for (int i = 0; i < MaxSlots; i++)
        {
            if (Slots[i].ItemId != 0 || Slots[i].SkillId != 0)
            {
                continue;
            }

            Slots[i] = quickSlot;
            return true;
        }

        return false;
    }

    public void MoveQuickSlot(int targetSlotIndex, QuickSlot quickSlot)
    {
        if (targetSlotIndex is < 0 or >= MaxSlots)
        {
            // This should never occur
            throw new ArgumentException($"Invalid target slot {targetSlotIndex}");
        }

        int sourceSlotIndex = FindQuickSlotIndex(quickSlot.SkillId, quickSlot.ItemUid);
        if (sourceSlotIndex != -1)
        {
            // Swapping with an existing slot on the hotbar
            QuickSlot sourceQuickSlot = Slots[targetSlotIndex];
            Slots[sourceSlotIndex] = QuickSlot.From(
                sourceQuickSlot.SkillId,
                sourceQuickSlot.ItemId,
                sourceQuickSlot.ItemUid
            );
        }

        Slots[targetSlotIndex] = quickSlot;
    }

    private int FindQuickSlotIndex(int skillId, long itemUid = 0)
    {
        for (int i = 0; i < MaxSlots; i++)
        {
            QuickSlot currentSlot = Slots[i];
            if (currentSlot.SkillId == skillId && currentSlot.ItemUid == itemUid)
            {
                return i;
            }
        }

        return -1;
    }

    public bool RemoveQuickSlot(int skillId, long itemUid)
    {
        int targetSlotIndex = FindQuickSlotIndex(skillId, itemUid);
        if (targetSlotIndex is < 0 or >= MaxSlots)
        {
            // TODO - There is either a) hotbar desync or b) something unintended occuring
            return false;
        }

        Slots[targetSlotIndex] = new(); // Clear
        return true;
    }

    public bool RemoveQuickSlot(QuickSlot quickSlot)
    {
        int targetSlotIndex = FindQuickSlotIndex(quickSlot.SkillId, quickSlot.ItemUid);
        if (targetSlotIndex is < 0 or >= MaxSlots)
        {
            // TODO - There is either a) hotbar desync or b) something unintended occuring
            return false;
        }

        Slots[targetSlotIndex] = new(); // Clear
        return true;
    }
}
