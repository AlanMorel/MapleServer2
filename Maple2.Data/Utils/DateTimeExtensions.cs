using System;

namespace Maple2.Data.Utils {
    public static class DateTimeExtensions {
        public static long ToEpochSeconds(this DateTime dateTime) {
            return (long) (dateTime - DateTime.UnixEpoch).TotalSeconds;
        }

        public static DateTime FromEpochSeconds(this long epochSeconds) {
            return DateTime.UnixEpoch + TimeSpan.FromSeconds(epochSeconds);
        }
    }
}