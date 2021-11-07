namespace MapleServer2.Enums;

public enum PartyNotice : byte
{
    AcceptedInvite = 0x1, //needs confirmation?
    NotLeader = 0x4,
    PartyAlreadyMade = 0x5,
    RefusedInvite = 0x9,
    InviteSelf = 0xB,
    NoResponseInvite = 0xC,
    UnableToInvite = 0xD,
    CannotAcceptInvite = 0xE,
    UserAlreadyReceivedRequest = 0xF,
    EntryRequirementsNotMet = 0x10,
    MinimumLevelNotMet = 0x11,
    MinimumGearScoreNotMet = 0x12,
    FullParty = 0x13,
    RecruitmentListingDeleted = 0x16,
    OutdatedRecruitmentListing = 0x17,
    InsufficientMerets = 0x1B,
    InviteAlreadyReceived = 0x1C,
    UnableToResetDungeon = 0x1D,
    UnableToInviteInDungeonBoss = 0x1E,
    PartyNotFound = 0x1F,
    RequestToJoin = 0x20,
    AnotherRequestInProgress = 0x21,
    InsufficientMemberCountForKickVote = 0x22,
    KickVoteCooldown = 0x23,
    UnableToKickInDungeonBoss = 0x26,
    UnableToKickUserInMushkingRoyale = 0x27,
    LeaderOnlyRequest = 0x28,
    MemberDisconnected = 0x29, // pop up notice
    MemberInDungeon = 0x2A,
    CurrentlyMatching = 0x2B,
    MemberOffline = 0x2D,
    MemberInMushkingRoyale = 0x30,
    MushkingRoyaleMaxSquad = 0x31
}
