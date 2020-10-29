using System.ComponentModel;

// NOTE: These enum are case sensitive
namespace Maple2Storage.Enums {
    public enum EquipSlot : byte {
        [Description("None")]
        NONE = 0,
        [Description("Hair")]
        HR = 102,
        [Description("Face")]
        FA = 103,
        [Description("Face Decoration")]
        FD = 104,
        [Description("Ear")]
        ER = 105,
        [Description("Face Accessory")]
        FH = 110,
        [Description("Eyewear")]
        EY = 111,
        [Description("Earring")]
        EA = 112,
        [Description("Cap")]
        CP = 113,
        [Description("Clothes")]
        CL = 114,
        [Description("Pants")]
        PA = 115,
        [Description("Gloves")]
        GL = 116,
        [Description("Shoes")]
        SH = 117,
        [Description("Mantle")]
        MT = 118,
        [Description("Pendant")]
        PD = 119,
        [Description("Ring")]
        RI = 120,
        [Description("Belt")]
        BE = 121,
        [Description("Right Hand")]
        RH = 1,
        [Description("Left Hand")]
        LH = 2,
        [Description("Off Hand")]
        OH = 3,
    }
}