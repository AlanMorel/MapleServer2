namespace MapleServer2.Enums
{
    public enum NoticeType : short
    {
        Chat = 0x1,
        ChatAndFastText = 0x5,
        Mint = 0x10,
        ChatAndMint = 0x11,
        FastText = 0x14,
        Popup = 0x40,
        PopupAndChat = 0x41,
        PopupAndFastText = 0x44,
        MintAndPopup = 0x50,
        PopupAndKick = 0x80,
        PopupMintAndKick = 0x90
    }
}
