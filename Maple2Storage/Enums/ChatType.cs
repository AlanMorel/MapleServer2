namespace Maple2Storage.Enums {
    public enum ChatType {
        All = 0,
        WhisperFrom = 3,
        WhisperTo = 4,
        WhisperFail = 5,
        WhisperReject = 6,
        Party = 7,
        Guild = 8,
        Notice = 9,
        World = 11,
        Channel = 12,
        NoticeAlert = 13,
        NoticeAlert2 = 14,
        ItemEnchant = 15,
        Super = 16,
        NoticeAlert3 = 17,
        GuildNotice = 18,
        Unknown = 19, // Guild chat color without [Guild] prefix
        Club = 20,
        UnknownPurple,
    }
}