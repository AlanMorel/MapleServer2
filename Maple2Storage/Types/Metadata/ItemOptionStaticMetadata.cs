﻿using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemOptionStaticMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public List<ItemOptionsStatic> ItemOptions = new();
}
[XmlType]
public class ItemOptionsStatic
{
    [XmlElement(Order = 1)]
    public byte Rarity;
    [XmlElement(Order = 2)]
    public float DefenseCalibrationFactor;
    [XmlElement(Order = 3)]
    public int HiddenDefenseAdd;
    [XmlElement(Order = 4)]
    public float WeaponAtkCalibrationFactor;
    [XmlElement(Order = 5)]
    public int HiddenWeaponAtkAdd;
    [XmlElement(Order = 6)]
    public int HiddenBonusAtkAdd;
    [XmlElement(Order = 7)]
    public byte[] Slots = Array.Empty<byte>();
    [XmlElement(Order = 8)]
    public List<ParserStat> Stats = new();
    [XmlElement(Order = 9)]
    public List<ParserSpecialStat> SpecialStats = new();
}
