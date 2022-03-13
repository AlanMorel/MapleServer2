namespace MapleServer2.Enums;

[Flags]
public enum NoticeType : short
{
    Chat = 1,
    FastText = 4,
    Mint = 16,
    Popup = 64,
    KickPopup = 128,
    RedBanner = 1024
}
