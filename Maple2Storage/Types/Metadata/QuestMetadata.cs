using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class QuestMetadata
    {
        [XmlElement(Order = 1)]
        public string Feature;
        [XmlElement(Order = 2)]
        public string Locale;
        [XmlElement(Order = 3)]
        public QuestBasic Basic = new QuestBasic();
        [XmlElement(Order = 4)]
        public QuestNotify Notify = new QuestNotify();
        [XmlElement(Order = 5)]
        public QuestRequire Require = new QuestRequire();
        [XmlElement(Order = 6)]
        public int StartNpc;
        [XmlElement(Order = 7)]
        public int CompleteNpc;
        [XmlElement(Order = 8)]
        public QuestReward Reward = new QuestReward();
        [XmlElement(Order = 9)]
        public List<QuestRewardItem> RewardItem = new List<QuestRewardItem>();
        [XmlElement(Order = 10)]
        public List<int> ProgressMap = new List<int>();
        [XmlElement(Order = 11)]
        public QuestGuide Guide = new QuestGuide();
        [XmlElement(Order = 12)]
        public QuestNpc Npc = new QuestNpc();
        [XmlElement(Order = 13)]
        public QuestDungeon Dungeon = new QuestDungeon();
        [XmlElement(Order = 14)]
        public QuestRemoteAccept RemoteAccept = new QuestRemoteAccept();
        [XmlElement(Order = 15)]
        public QuestRemoteComplete RemoteComplete = new QuestRemoteComplete();
        [XmlElement(Order = 16)]
        public QuestSummonPortal SummonPortal = new QuestSummonPortal();
        [XmlElement(Order = 17)]
        public string Event;
        [XmlElement(Order = 18)]
        public List<QuestCondition> Condition = new List<QuestCondition>();
        [XmlElement(Order = 19)]
        public List<QuestNavigation> Navigation = new List<QuestNavigation>();

        public QuestMetadata() { }

        public override string ToString() =>
            $"Feature: {Feature}, Locale: {Locale}, Basic: {Basic}, Notify: {Notify},  Require: {Require}," +
            $" StartNpc: {StartNpc}, CompleteNpc: {CompleteNpc},  Reward: {Reward}, QuestRewardItem: {string.Join(",", RewardItem)},  " +
            $"ProgressMap: {string.Join(",", ProgressMap)}, Guide: {Guide}, Npc: {Npc}, Dungeon: {Dungeon}, RemoteAccept: {RemoteAccept}, " +
            $"RemoteComplete: {RemoteComplete}, SummonPortal: {SummonPortal}, Event: {Event}, Condition: {string.Join(",", Condition)}, " +
            $"Navigation: {string.Join(",", Navigation)}";
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
        public byte Account;
        [XmlElement(Order = 5)]
        public int StandardLevel;
        [XmlElement(Order = 6)]
        public byte AutoStart;
        [XmlElement(Order = 7)]
        public byte DisableGiveup;
        [XmlElement(Order = 8)]
        public int ExceptChapterClear;
        [XmlElement(Order = 9)]
        public byte Repeatable;
        [XmlElement(Order = 10)]
        public string UsePeriod;
        [XmlElement(Order = 11)]
        public string EventTag;
        [XmlElement(Order = 12)]
        public byte Locking;
        [XmlElement(Order = 13)]
        public int TabIndex;
        [XmlElement(Order = 14)]
        public byte ForceRegistGuide;
        [XmlElement(Order = 15)]
        public bool UseNavigation;

        public QuestBasic() { }

        public override string ToString() =>
            $"\r\nchapterID: {ChapterID}, questID: {Id}, questType: {QuestType}, account: {Account}, standardLevel: {StandardLevel}, " +
            $"autoStart: {AutoStart}, disableGiveup: {DisableGiveup}, exceptChapterClear: {ExceptChapterClear}, repeatable: {Repeatable}, " +
            $"usePeriod: {UsePeriod}, eventTag: {EventTag}, locking: {Locking}, tabIndex: {TabIndex}, forceRegistGuide: {ForceRegistGuide}, " +
            $"useNavi: {UseNavigation}";
    }

    [XmlType]
    public class QuestNotify
    {
        [XmlElement(Order = 1)]
        public string CompleteUiEffect;
        [XmlElement(Order = 2)]
        public string AcceptSoundKey;
        [XmlElement(Order = 3)]
        public string CompleteSoundKey;

        public QuestNotify() { }

        public override string ToString() =>
            $"\r\ncompleteUiEffect: {CompleteUiEffect}, acceptSoundKey: {AcceptSoundKey}, completeSoundKey: {CompleteSoundKey}";
    }

    [XmlType]
    public class QuestRequire
    {
        [XmlElement(Order = 1)]
        public short Level;
        [XmlElement(Order = 2)]
        public short MaxLevel;
        [XmlElement(Order = 3)]
        public List<short> Job = new List<short>();
        [XmlElement(Order = 4)]
        public List<int> RequiredQuests = new List<int>();
        [XmlElement(Order = 5)]
        public List<int> SelectableQuest = new List<int>();
        [XmlElement(Order = 6)]
        public List<int> Unrequire = new List<int>();
        [XmlElement(Order = 7)]
        public int Field;
        [XmlElement(Order = 8)]
        public int Achievement;
        [XmlElement(Order = 9)]
        public List<int> UnreqAchievement = new List<int>();
        [XmlElement(Order = 10)]
        public int GroupID;
        [XmlElement(Order = 11)]
        public string DayOfWeek;
        [XmlElement(Order = 12)]
        public int GearScore;

        public QuestRequire() { }

        public override string ToString() =>
            $"\r\nLevel: {Level}, maxLevel: {MaxLevel}, job: {string.Join(", ", Job)}, quest: {string.Join(", ", RequiredQuests)}, " +
            $"selectableQuest: {string.Join(", ", SelectableQuest)}, unrequire: {string.Join(", ", Unrequire)}, field: {Field}, achievement: {Achievement}, " +
            $"unreqAchievement: {string.Join(", ", UnreqAchievement)}, groupID: {GroupID}, dayOfWeek: {DayOfWeek}, gearScore: {GearScore}";
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

        public QuestReward() { }

        public override string ToString() => $"\r\nexp: {Exp}, relativeExp: {RelativeExp}, money: {Money}, karma: {Karma}, lu: {Lu}";
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

        public override string ToString() => $"\r\nItemId: {Code}, rank: {Rank}, count: {Count}";
    }

    [XmlType]
    public class QuestGuide
    {
        [XmlElement(Order = 1)]
        public string Type;
        [XmlElement(Order = 2)]
        public string Icon;
        [XmlElement(Order = 3)]
        public short MinLevel;
        [XmlElement(Order = 4)]
        public short MaxLevel;
        [XmlElement(Order = 5)]
        public string Group;

        public QuestGuide() { }

        public override string ToString() => $"\r\nguideType: {Type}, guideIcon: {Icon}, guideMinLevel: {MinLevel}, guideMaxLevel: {MaxLevel}, guideGroup: {Group}";
    }

    [XmlType]
    public class QuestNpc
    {
        [XmlElement(Order = 1)]
        public byte Enable;
        [XmlElement(Order = 2)]
        public int GoToField;
        [XmlElement(Order = 3)]
        public int GoToPortal;

        public QuestNpc() { }

        public override string ToString() => $"\r\nenable: {Enable}, gotoField: {GoToField}, gotoPortal: {GoToPortal}";
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

        public QuestDungeon() { }

        public override string ToString() => $"\r\nstate: {State}, gotoDungeon: {GoToDungeon}, gotoInstanceID: {GoToInstanceID}";
    }

    [XmlType]
    public class QuestRemoteAccept
    {
        [XmlElement(Order = 1)]
        public string UseRemote;
        [XmlElement(Order = 2)]
        public int RequireField;

        public QuestRemoteAccept() { }

        public override string ToString() => $"\r\nuseRemote: {UseRemote}, requireField: {RequireField}";
    }

    [XmlType]
    public class QuestRemoteComplete
    {
        [XmlElement(Order = 1)]
        public string UseRemote;
        [XmlElement(Order = 2)]
        public int RequireField;
        [XmlElement(Order = 3)]
        public int RequireDungeonClear;

        public QuestRemoteComplete() { }

        public override string ToString() => $"\r\nuseRemote: {UseRemote}, requireField: {RequireField}, requireDungeonClear: {RequireDungeonClear}";
    }

    [XmlType]
    public class QuestSummonPortal
    {
        [XmlElement(Order = 1)]
        public int FieldID;
        [XmlElement(Order = 2)]
        public int PortalID;
        public QuestSummonPortal() { }

        public override string ToString() => $"\r\nfieldID: {FieldID}, portalID: {PortalID}";
    }

    [XmlType]
    public class QuestCondition
    {
        [XmlElement(Order = 1)]
        public string Type;
        [XmlElement(Order = 2)]
        public string[] Codes;
        [XmlElement(Order = 3)]
        public int Goal;
        [XmlElement(Order = 4)]
        public List<string> Target = new List<string>();

        public QuestCondition() { }

        public QuestCondition(string type, string[] codes, int goal, List<string> target)
        {
            Type = type;
            Codes = codes;
            Goal = goal;
            Target = target;
        }

        public override string ToString() => $"\r\ntype: {Type}, codes: {Codes}, Goal: {Goal}, Targets: {string.Join(",", Target)}";
    }

    [XmlType]
    public class QuestNavigation
    {
        [XmlElement(Order = 1)]
        public string Type;
        [XmlElement(Order = 2)]
        public string Code;
        [XmlElement(Order = 3)]
        public int Map;

        public QuestNavigation() { }

        public QuestNavigation(string type, string code, int map)
        {
            Type = type;
            Code = code;
            Map = map;
        }

        public override string ToString() => $"\r\ntype: {Type}, code: {Code}, value: {Map}";
    }
}
