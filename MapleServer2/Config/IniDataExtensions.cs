using System;
using System.Globalization;
using System.Linq;
using IniParser.Model;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Config
{
    public static class IniDataExtensions
    {
        private static ILogger Logger;

        public static float GetFloat(this KeyDataCollection data, string key, float defaultVal)
        {
            if (float.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                return result;
            }

            Logger.LogWarning($" [Float] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static bool GetBool(this KeyDataCollection data, string key)
        {
            string[] truevals = new[] { "y", "yes", "true" };
            return truevals.Contains($"{data[key]}".ToLower());
        }

        public static int GetInt(this KeyDataCollection data, string key, int defaultVal)
        {
            if (int.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out int result))
            {
                return result;
            }

            Logger.LogWarning($" [Int] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static string GetString(this KeyDataCollection data, string key, string defaultVal)
        {
            if (data[key].ToString() != null)
            {
                return data[key].ToString();
            }

            Logger.LogWarning($" [String] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static T LoadConfiguration<T>(this IniData data, string key) where T : ConfigBase<T>, new()
        {
            KeyDataCollection idata = data[key];
            return (T) typeof(T).GetMethod("LoadIni").Invoke(null, new[] { idata });
        }
    }
}
