namespace MapleServer2.Enums
{
    public enum NpcType : byte
    {
        Unk = 05,
        Dialog = 10,
        Unk2 = 14,
        Quest = 15,
    }

    public enum DialogType : int // All this worked with NpcType 10
    {
        NoOptions = 1, // no options to continue or exit
        Broken = 2, // appears "Quit:ESC" option
        Close1 = 3, // Close - Espace
        CloseNext = 4, // Close - Esc || Next - Space
        TalkOption = 5, // Option talk on the npc menu with: Close - Esc || Next - Space
        AcceptDecline = 6, // Decline - Esc || Accept - Space
        QuestReward = 7,
        Close2 = 8, // same as 3 and 9
        NextClose1 = 9, // same as 3 and 8
        JobAdv = 11, // Nevermind - ESC || Perform Job Advancement
        AcceptDecline2 = 12, // Decline - Esc || Accept - Space
        GetTreatment = 13, // Decline - ESC || Get Treatment - Space
        StayGo = 14, // Stay - ESC || Go - Space
        AcceptDecline3 = 15, // Decline - Esc || Accept - Space
        Spin = 16, // Spin - Space
        Skip = 17, // Skip - Space
        GetTreatment2 = 18 // Decline - ESC || Get Treatment - Space
    }

    public enum ResponseType : byte
    {
        Dialog = 2,
        Quest = 4,
    }

}
