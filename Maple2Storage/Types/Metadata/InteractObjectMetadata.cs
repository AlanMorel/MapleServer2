using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class InteractObjectMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public InteractObjectType Type;
    [XmlElement(Order = 3)]
    public InteractObjectRewardMetadata Reward;
    [XmlElement(Order = 4)]
    public InteractObjectDropMetadata Drop;
    [XmlElement(Order = 5)]
    public List<(int QuestId, QuestState State)> Quests = new();
    [XmlElement(Order = 6)]
    public InteractObjectGatheringMetadata Gathering;
    [XmlElement(Order = 7)]
    public InteractObjectWebMetadata Web;
}

[XmlType]
public class InteractObjectRewardMetadata
{
    [XmlElement(Order = 1)]
    public int Exp;
    [XmlElement(Order = 2)]
    public string ExpType;
    [XmlElement(Order = 3)]
    public float ExpRate;
    [XmlElement(Order = 4)]
    public string FirstExpType;
    [XmlElement(Order = 5)]
    public float FirstExpRate;
}

[XmlType]
public class InteractObjectDropMetadata
{
    [XmlElement(Order = 1)]
    public string ObjectLevel;
    [XmlElement(Order = 2)]
    public int DropRank;
    [XmlElement(Order = 3)]
    public List<int> GlobalDropBoxId = new();
    [XmlElement(Order = 4)]
    public List<int> IndividualDropBoxId = new();
}

[XmlType]
public class InteractObjectGatheringMetadata
{
    [XmlElement(Order = 1)]
    public int RecipeId;
}

[XmlType]
public class InteractObjectWebMetadata
{
    [XmlElement(Order = 1)]
    public string Url;
}
