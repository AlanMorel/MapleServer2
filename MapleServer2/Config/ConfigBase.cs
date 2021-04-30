using System.Reflection;
using IniParser.Model;

namespace MapleServer2.Config
{
    public abstract class ConfigBase<T> : IConfig where T : IConfig, new()
    {
        public static IniData iniUpdated = null;

        public static T LoadIni(IniData data, string section)
        {
            T n = new T();

            n.LoadIniData(data[section]);

            return n;
        }

        public void LoadIniData(KeyDataCollection data)
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                string keyName = prop.Name;
                if (keyName != string.Empty && char.IsUpper(keyName[0]))
                {
                    keyName = char.ToLower(keyName[0]) + keyName.Substring(1);
                }

                if (!data.ContainsKey(keyName))
                {
                    continue;
                }

                object existingValue = prop.GetValue(this, null);

                if (prop.PropertyType == typeof(float))
                {
                    prop.SetValue(this, data.GetFloat(keyName, (float) existingValue), null);
                    continue;
                }

                if (prop.PropertyType == typeof(int))
                {
                    prop.SetValue(this, data.GetInt(keyName, (int) existingValue), null);
                    continue;
                }

                if (prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(this, data.GetBool(keyName), null);
                    continue;
                }

                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(this, data.GetString(keyName, ""), null);
                    continue;
                }
            }
        }
    }
}
