namespace Maple2Storage.Enums
{
    public enum NpcType : byte
    {
        Default = 1,
        NormalTalk = 2,
        Quest = 4,
        QuestOptions = 14,
    }

    public enum DialogType : int // All this worked with NpcType 10
    {
        None = 0,
        NoOptions = 1, // no options to continue or exit
        Broken = 2, // appears "Quit:ESC" option
        Close1 = 3, // Close - Espace
        CloseNext = 4, // Close - Esc || Next - Space // don't accept distractors
        TalkOption = 5, // Option talk on the npc menu with: Close - Esc || Next - Space
        AcceptDecline = 6, // Decline - Esc || Accept - Space
        QuestReward = 7,
        Close2 = 8, // Close - Escape
        CloseNextWithDistractor = 9, // Close - Esc || Next - Space // accept distractors
        Beauty = 10,
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

    public enum ActionType : byte
    {
        Portal = 3,
        OpenWindow = 4,
    }
}
