using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class QuestMetadata
{
    [XmlElement(Order = 1)]
    public QuestBasic Basic = new();
    [XmlElement(Order = 2)]
    public QuestRequire Require = new();
    [XmlElement(Order = 3)]
    public int StartNpc;
    [XmlElement(Order = 4)]
    public int CompleteNpc;
    [XmlElement(Order = 5)]
    public QuestReward Reward = new();
    [XmlElement(Order = 6)]
    public List<QuestRewardItem> RewardItem = new();
    [XmlElement(Order = 7)]
    public List<int> ProgressMap = new();
    [XmlElement(Order = 8)]
    public QuestNpc Npc = new();
    [XmlElement(Order = 9)]
    public QuestDungeon Dungeon = new();
    [XmlElement(Order = 10)]
    public QuestSummonPortal SummonPortal = new();
    [XmlElement(Order = 11)]
    public List<QuestCondition> Condition = new();
    [XmlElement(Order = 12)]
    public QuestDispatch Dispatch = new();
    [XmlElement(Order = 13)]
    public string Name = "";
}

[XmlType]
public class QuestBasic
{
    [XmlElement(Order = 1)]
    public int ChapterID;
    [XmlElement(Order = 2)]
    public int Id;
    [XmlElement(Order = 3)]
    public QuestType QuestType;
    [XmlElement(Order = 4)]
    public bool AutoStart;
}

[XmlType]
public class QuestRequire
{
    [XmlElement(Order = 1)]
    public short Level;
    [XmlElement(Order = 2)]
    public short MaxLevel;
    [XmlElement(Order = 3)]
    public List<short> Job = new();
    [XmlElement(Order = 4)]
    public List<int> RequiredQuests = new();
}

[XmlType]
public class QuestReward
{
    [XmlElement(Order = 1)]
    public int Exp;
    [XmlElement(Order = 2)]
    public string RelativeExp;
    [XmlElement(Order = 3)]
    public int Money;
    [XmlElement(Order = 4)]
    public int Karma;
    [XmlElement(Order = 5)]
    public int Lu;
}

[XmlType]
public class QuestRewardItem
{
    [XmlElement(Order = 1)]
    public int Code;
    [XmlElement(Order = 2)]
    public byte Rank;
    [XmlElement(Order = 3)]
    public int Count;

    public QuestRewardItem() { }

    public QuestRewardItem(int pItemId, byte pRank, int pCount)
    {
        Code = pItemId;
        Rank = pRank;
        Count = pCount;
    }
}

[XmlType]
public class QuestNpc
{
    [XmlElement(Order = 1)]
    public bool Enable;
    [XmlElement(Order = 2)]
    public int GoToField;
    [XmlElement(Order = 3)]
    public int GoToPortal;
}

[XmlType]
public class QuestDungeon
{
    [XmlElement(Order = 1)]
    public byte State;
    [XmlElement(Order = 2)]
    public int GoToDungeon;
    [XmlElement(Order = 3)]
    public int GoToInstanceID;
}

[XmlType]
public class QuestSummonPortal
{
    [XmlElement(Order = 1)]
    public int FieldID;
    [XmlElement(Order = 2)]
    public int PortalID;
}

[XmlType]
public class QuestCondition
{
    [XmlElement(Order = 1)]
    public string Type;
    [XmlElement(Order = 2)]
    public string Code;
    [XmlElement(Order = 3)]
    public int Goal;
    [XmlElement(Order = 4)]
    public string Target;

    public QuestCondition() { }

    public QuestCondition(string type, string code, int goal, string target)
    {
        Type = type;
        Code = code;
        Goal = goal;
        Target = target;
    }
}

[XmlType]
public class QuestDispatch
{
    [XmlElement(Order = 1)]
    public string Type;
    [XmlElement(Order = 2)]
    public int FieldId;
    [XmlElement(Order = 3)]
    public short PortalId;
    [XmlElement(Order = 4)]
    public int ScriptId;
}
