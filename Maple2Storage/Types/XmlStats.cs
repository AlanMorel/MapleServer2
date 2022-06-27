using System.Xml.Serialization;

namespace Maple2Storage.Types;

[XmlType]
public class XmlStats
{
    [XmlElement(Order = 1)]
    public XmlStat<int> Str;
    [XmlElement(Order = 2)]
    public XmlStat<int> Dex;
    [XmlElement(Order = 3)]
    public XmlStat<int> Int;
    [XmlElement(Order = 4)]
    public XmlStat<int> Luk;
    [XmlElement(Order = 5)]
    public XmlStat<long> Hp;
    [XmlElement(Order = 6)]
    public XmlStat<int> HpRegen;
    [XmlElement(Order = 7)]
    public XmlStat<int> HpInterval;
    [XmlElement(Order = 8)]
    public XmlStat<int> Sp;
    [XmlElement(Order = 9)]
    public XmlStat<int> SpRegen;
    [XmlElement(Order = 10)]
    public XmlStat<int> SpInterval;
    [XmlElement(Order = 11)]
    public XmlStat<int> Ep;
    [XmlElement(Order = 12)]
    public XmlStat<int> EpRegen;
    [XmlElement(Order = 13)]
    public XmlStat<int> EpInterval;
    [XmlElement(Order = 14)]
    public XmlStat<int> AtkSpd;
    [XmlElement(Order = 15)]
    public XmlStat<int> MoveSpd;
    [XmlElement(Order = 16)]
    public XmlStat<int> Accuracy;
    [XmlElement(Order = 17)]
    public XmlStat<int> Evasion;
    [XmlElement(Order = 18)]
    public XmlStat<int> CritRate;
    [XmlElement(Order = 19)]
    public XmlStat<int> CritDamage;
    [XmlElement(Order = 20)]
    public XmlStat<int> CritResist;
    [XmlElement(Order = 21)]
    public XmlStat<int> Defense;
    [XmlElement(Order = 22)]
    public XmlStat<int> Guard;
    [XmlElement(Order = 23)]
    public XmlStat<int> JumpHeight;
    [XmlElement(Order = 24)]
    public XmlStat<int> PhysAtk;
    [XmlElement(Order = 25)]
    public XmlStat<int> MagAtk;
    [XmlElement(Order = 26)]
    public XmlStat<int> PhysRes;
    [XmlElement(Order = 27)]
    public XmlStat<int> MagRes;
    [XmlElement(Order = 28)]
    public XmlStat<int> MinAtk;
    [XmlElement(Order = 29)]
    public XmlStat<int> MaxAtk;
    [XmlElement(Order = 30)]
    public XmlStat<int> Damage;
    [XmlElement(Order = 31)]
    public XmlStat<int> Pierce;
    [XmlElement(Order = 32)]
    public XmlStat<int> MountSpeed;
    [XmlElement(Order = 33)]
    public XmlStat<int> BonusAtk;
    [XmlElement(Order = 34)]
    public XmlStat<int> BonusAtkPet;
    // There's more on some other NPC but until this point, all of them have it
}
[XmlType]
public struct XmlStat<T> where T : struct
{
    [XmlElement(Order = 1)]
    public T Bonus;
    [XmlElement(Order = 2)]
    public T Base;
    [XmlElement(Order = 3)]
    public T Total;

    public T this[int i] => i switch
    {
        1 => Base,
        2 => Total,
        _ => Bonus
    };

    public XmlStat(T totalStat)
    {
        Bonus = totalStat;
        Base = totalStat;
        Total = totalStat;
    }

    public XmlStat(T bonusStat, T baseStat, T totalStat)
    {
        Bonus = bonusStat;
        Base = baseStat;
        Total = totalStat;
    }
}
