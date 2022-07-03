using System.ComponentModel;

// NOTE: These enum are case sensitive
namespace Maple2Storage.Enums;

public enum ItemSlot : byte
{
    [Description("None")]
    NONE = 0,
    [Description("Hair")]
    HR = 1,
    [Description("Face")]
    FA = 2,
    [Description("Face Decoration")]
    FD = 3,
    [Description("Left Hand")]
    LH = 4,
    [Description("Right Hand")]
    RH = 5,
    [Description("Cap")]
    CP = 6,
    [Description("Mantle")]
    MT = 7,
    [Description("Clothes")]
    CL = 8,
    [Description("Pants")]
    PA = 9,
    [Description("Gloves")]
    GL = 10,
    [Description("Shoes")]
    SH = 11,
    [Description("Face Accessory")]
    FH = 12,
    [Description("Eyewear")]
    EY = 13,
    [Description("Earring")]
    EA = 14,
    [Description("Pendant")]
    PD = 15,
    [Description("Ring")]
    RI = 16,
    [Description("Belt")]
    BE = 17,
    [Description("Ear")]
    ER = 18,
    [Description("Off Hand")]
    OH = 19
}
