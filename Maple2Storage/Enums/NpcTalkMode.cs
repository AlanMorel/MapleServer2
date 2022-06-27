namespace Maple2Storage.Enums;

[Flags]
public enum DialogType : byte
{
    None = 0,
    UI = 1,
    Talk = 2,
    Quest = 4,
    Options = 8,
    Cinematic = 16,
}
public enum ResponseSelection
{
    None = 0,
    Empty = 1, // no options to continue or exit
    Stop = 2, // appears "Quit:ESC" option
    Close = 3, // Close - Espace
    Next = 4, // Close - Esc || Next - Space // don't accept distractors
    SelectableTalk = 5, // Option talk on the npc menu with: Close - Esc || Next - Space
    QuestAccept = 6, // Decline - Esc || Accept - Space
    QuestComplete = 7,
    QuestProgress = 8, // Close - Escape
    SelectableDistractor = 9, // Close - Esc || Next - Space // accept distractors
    Beauty = 10,
    JobAdvance = 11, // Nevermind - ESC || Perform Job Advancement
    UGCSign = 12, // Decline - Esc || Accept - Space
    GetTreatment = 13, // Decline - ESC || Get Treatment - Space
    TakeBoat = 14, // Stay - ESC || Go - Space
    SellUGCMap = 15, // Decline - Esc || Accept - Space
    Roulette = 16, // Spin - Space
    RouletteSkip = 17, // Skip - Space
    GetHomeTreatment = 18 // Decline - ESC || Get Treatment - Space
}

public enum ActionType : byte
{
    Portal = 3,
    OpenWindow = 4,
    ItemReward = 5,
    MoveMap = 99
}

public enum NpcTalkEventType : short
{
    Begin = 1,
    InProgress = 2,
    Result = 3
}
