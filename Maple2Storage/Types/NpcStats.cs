using System.Xml;
using System.Xml.Serialization;

namespace Maple2Storage.Types;

[XmlType]
public struct NpcStats
{
    [XmlElement(Order = 1)]
    public NpcStat<int> Str;
    [XmlElement(Order = 2)]
    public NpcStat<int> Dex;
    [XmlElement(Order = 3)]
    public NpcStat<int> Int;
    [XmlElement(Order = 4)]
    public NpcStat<int> Luk;
    [XmlElement(Order = 5)]
    public NpcStat<long> Hp;
    [XmlElement(Order = 6)]
    public NpcStat<int> HpRegen;
    [XmlElement(Order = 7)]
    public NpcStat<int> HpInv;
    [XmlElement(Order = 8)]
    public NpcStat<int> Sp;
    [XmlElement(Order = 9)]
    public NpcStat<int> SpRegen;
    [XmlElement(Order = 10)]
    public NpcStat<int> SpInv;
    [XmlElement(Order = 11)]
    public NpcStat<int> Ep;
    [XmlElement(Order = 12)]
    public NpcStat<int> EpRegen;
    [XmlElement(Order = 13)]
    public NpcStat<int> EpInv;
    [XmlElement(Order = 14)]
    public NpcStat<int> AtkSpd;
    [XmlElement(Order = 15)]
    public NpcStat<int> MoveSpd;
    [XmlElement(Order = 16)]
    public NpcStat<int> Accuracy;
    [XmlElement(Order = 17)]
    public NpcStat<int> Evasion;
    [XmlElement(Order = 18)]
    public NpcStat<int> Cap;
    [XmlElement(Order = 19)]
    public NpcStat<int> Cad;
    [XmlElement(Order = 20)]
    public NpcStat<int> Car;
    [XmlElement(Order = 21)]
    public NpcStat<int> Ndd;
    [XmlElement(Order = 22)]
    public NpcStat<int> Abp;
    [XmlElement(Order = 23)]
    public NpcStat<int> JumpHeight;
    [XmlElement(Order = 24)]
    public NpcStat<int> PhysAtk;
    [XmlElement(Order = 25)]
    public NpcStat<int> MagAtk;
    [XmlElement(Order = 26)]
    public NpcStat<int> PhysRes;
    [XmlElement(Order = 27)]
    public NpcStat<int> MagRes;
    [XmlElement(Order = 28)]
    public NpcStat<int> MinAtk;
    [XmlElement(Order = 29)]
    public NpcStat<int> MaxAtk;
    [XmlElement(Order = 30)]
    public NpcStat<int> Damage;
    [XmlElement(Order = 31)]
    public NpcStat<int> Pierce;
    [XmlElement(Order = 32)]
    public NpcStat<int> MountSpeed;
    [XmlElement(Order = 33)]
    public NpcStat<int> BonusAtk;
    [XmlElement(Order = 34)]
    public NpcStat<int> BonusAtkPet;
    // There's more on some other NPC but until this point, all of them have it
}
[XmlType]
public struct NpcStat<T>
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

    public NpcStat(T totalStat)
    {
        Bonus = totalStat;
        Base = totalStat;
        Total = totalStat;
    }

    public NpcStat(T bonusStat, T baseStat, T totalStat)
    {
        Bonus = bonusStat;
        Base = baseStat;
        Total = totalStat;
    }
}
