using System.ComponentModel;

namespace Maple2Storage.Enums;

public enum GemSlot : byte
{
    [Description("None")]
    NONE = 0,
    [Description("Transparency Badge")]
    TRANS = 1,
    [Description("Damage Font")]
    DAMAGE = 2,
    [Description("Chat Bubble")]
    CHAT = 3,
    [Description("Name Tag")]
    NAME = 4,
    [Description("Tombstone Style")]
    TOMBSTONE = 5,
    [Description("Swim Tube")]
    SWIM = 6,
    [Description("Buddy Badge")]
    BUDDY = 7,
    [Description("Fishing Accessories")]
    FISHING = 8,
    [Description("Auto-Gathering Badge")]
    GATHER = 9,
    [Description("Effects Badge")]
    EFFECT = 10,
    [Description("Pet Skin")]
    PET = 11,
    [Description("Meta")]
    META = 12
}
