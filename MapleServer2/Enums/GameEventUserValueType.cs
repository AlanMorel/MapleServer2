namespace MapleServer2.Enums;

public enum GameEventUserValueType
{
    // Attendance Event
    AttendanceActive = 100, //?? maybe. String is "True"
    AttendanceCompletedTimestamp = 101,
    AttendanceRewardsClaimed = 102,
    AttendanceEarlyParticipationRemaining = 103,
    AttendanceAccumulatedTime = 106,

    // Rock Paper Scissors Event
    RPSDailyMatches = 1800,
    RPSRewardsClaimed = 1801,

}
