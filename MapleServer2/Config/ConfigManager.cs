using System;
using System.IO;
using System.Reflection;
using IniParser;
using IniParser.Model;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Config
{
    public class ConfigHandler
    {
        public static string ConfigIniPath = Path.Combine(Directory.GetCurrentDirectory(), "config.cfg");
        private static readonly ILogger Logger;

        public static bool LoadSettings()
        {
            try
            {
                if (File.Exists(ConfigIniPath))
                {
                    Configuration.Current = LoadFromIni(ConfigIniPath);
                }
                else
                {
                    Logger.LogError($"Configuration not found in path {ConfigIniPath}.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Could not load config file: {ex}");
                return false;
            }

            return true;
        }

        public static Configuration LoadFromIni(string filename)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData configdata = parser.ReadFile(filename);

            Configuration conf = new Configuration();
            foreach (PropertyInfo prop in typeof(Configuration).GetProperties())
            {
                string keyName = prop.Name;
                MethodInfo method = prop.PropertyType.GetMethod("LoadIni", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                if (method != null)
                {
                    object result = method.Invoke(null, new object[] { configdata, keyName });
                    prop.SetValue(conf, result, null);
                }
            }

            return conf;
        }

        public static Configuration LoadFromIni(Stream iniStream)
        {
            using (StreamReader iniReader = new StreamReader(iniStream))
            {
                FileIniDataParser parser = new FileIniDataParser();
                IniData configdata = parser.ReadData(iniReader);

                Configuration conf = new Configuration();
                foreach (PropertyInfo prop in typeof(Configuration).GetProperties())
                {
                    string keyName = prop.Name;
                    MethodInfo method = prop.PropertyType.GetMethod("LoadIni", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                    if (method != null)
                    {
                        object result = method.Invoke(null, new object[] { configdata, keyName });
                        prop.SetValue(conf, result, null);
                    }
                }
                return conf;
            }
        }
    }
}
