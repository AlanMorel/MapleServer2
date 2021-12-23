using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class GuildPropertyMetadata
{
    [XmlElement(Order = 1)]
    public int Level;
    [XmlElement(Order = 2)]
    public int AccumExp;
    [XmlElement(Order = 3)]
    public int Capacity;
    [XmlElement(Order = 4)]
    public int FundMax;
    [XmlElement(Order = 5)]
    public int DonationMax;
    [XmlElement(Order = 6)]
    public int AttendExp;
    [XmlElement(Order = 7)]
    public int WinMiniGameExp;
    [XmlElement(Order = 8)]
    public int LoseMiniGameExp;
    [XmlElement(Order = 9)]
    public int RaidGuildExp;
    [XmlElement(Order = 10)]
    public int AttendFunds;
    [XmlElement(Order = 11)]
    public int WinMiniGameFunds;
    [XmlElement(Order = 12)]
    public int LoseMiniGameFunds;
    [XmlElement(Order = 13)]
    public int RaidGuildFunds;
    [XmlElement(Order = 14)]
    public int AttendUserExpFactor;
    [XmlElement(Order = 15)]
    public float DonateUserExpFactor;
    [XmlElement(Order = 16)]
    public int AttendGuildCoin;
    [XmlElement(Order = 17)]
    public int DonateGuildCoin;
    [XmlElement(Order = 18)]
    public int WinMiniGameGuildCoin;
    [XmlElement(Order = 19)]
    public int LoseMiniGameGuildCoin;

    public override string ToString()
    {
        return
            $"GuildBuff(Level:{Level},AccumExp:{AccumExp},Capacity:{Capacity},FundMax:{FundMax},DonationMax:{DonationMax},AttendExp:{AttendExp},WinMiniGameExp:{WinMiniGameExp},LoseMiniGameExp:{LoseMiniGameExp}" +
            $",RaidGuildExp:{RaidGuildExp},AttendFunds:{AttendFunds},WinMiniGameFunds:{WinMiniGameFunds},LoseMiniGameFunds:{LoseMiniGameFunds},RaidGuildFunds:{RaidGuildFunds}," +
            $"AttendUserExpFactor:{AttendUserExpFactor},DonateUserExpFactor:{DonateUserExpFactor},AttendGuildCoin:{AttendGuildCoin},DonateGuildCoin:{DonateGuildCoin}" +
            $",WinMiniGameGuildCoin:{WinMiniGameGuildCoin},LoseMiniGameGuildCoin:{LoseMiniGameGuildCoin})";
    }
}
