using System.Collections.Generic;
using System.IO;

namespace MapleServer2.Config
{
    public class ConfigHandler
    {
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        private static ConfigHandler instance = null;
        public static ConfigHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigHandler("config.ini");
                }
                return instance;
            }
        }

        public string commentDelimiter { get; set; }
        private string file;

        public string this[string section, string key] => GetValue(section, key);

        public ConfigHandler(string file, string commentDelimiter = ";")
        {
            this.commentDelimiter = commentDelimiter;
            this.file = file;
        }

        public void Init(string value)
        {
            file = null;
            dictionary.Clear();
            if (File.Exists(value))
            {
                file = value;
                using (StreamReader sr = new StreamReader(file))
                {
                    string line, section = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length == 0)
                        {
                            continue;  // empty line
                        }

                        if (!string.IsNullOrEmpty(commentDelimiter) && line.StartsWith(commentDelimiter))
                        {
                            continue;  // comment
                        }

                        if (line.StartsWith("[") && line.Contains("]"))  // [section]
                        {
                            int index = line.IndexOf(']');
                            section = line[1..index].Trim();
                            continue;
                        }

                        if (line.Contains("="))  // key=value
                        {
                            int index = line.IndexOf('=');
                            string key = line.Substring(0, index).Trim();
                            string val = line.Substring(index + 1).Trim();
                            string key2 = string.Format("[{0}]{1}", section, key).ToLower();

                            if (val.StartsWith("\"") && val.EndsWith("\""))  // strip quotes
                            {
                                val = val.Substring(1, val.Length - 2);
                            }

                            if (dictionary.ContainsKey(key2))  // multiple values can share the same key
                            {
                                index = 1;
                                string key3;
                                while (true)
                                {
                                    key3 = string.Format("{0}~{1}", key2, ++index);
                                    if (!dictionary.ContainsKey(key3))
                                    {
                                        dictionary.Add(key3, val);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                dictionary.Add(key2, val);
                            }
                        }
                    }
                }
            }
        }

        private bool TryGetValue(string section, string key, out string value)
        {
            string key2 = section.StartsWith("[") ? string.Format("{0}{1}", section, key) : string.Format("[{0}]{1}", section, key);

            return dictionary.TryGetValue(key2.ToLower(), out value);
        }

        public string GetValue(string section, string key, string defaultValue = "")
        {
            return !TryGetValue(section, key, out string value) ? defaultValue : value;
        }

        public int GetInteger(string section, string key, int defaultValue = 0, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            if (!TryGetValue(section, key, out string stringValue))
            {
                return defaultValue;
            }

            if (!int.TryParse(stringValue, out int value))
            {
                if (!double.TryParse(stringValue, out double dvalue))
                {
                    return defaultValue;
                }

                value = (int) dvalue;
            }

            if (value < minValue)
            {
                value = minValue;
            }

            if (value > maxValue)
            {
                value = maxValue;
            }

            return value;
        }

        public double GetDouble(string section, string key, double defaultValue = 0, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            if (!TryGetValue(section, key, out string stringValue))
            {
                return defaultValue;
            }

            if (!double.TryParse(stringValue, out double value))
            {
                return defaultValue;
            }

            if (value < minValue)
            {
                value = minValue;
            }

            if (value > maxValue)
            {
                value = maxValue;
            }

            return value;
        }

        public bool GetBoolean(string section, string key, bool defaultValue = false)
        {
            return !TryGetValue(section, key, out string stringValue) ? defaultValue : stringValue != "0" && !stringValue.StartsWith("f", true, null);
        }

        public string[] GetAllValues(string section, string key)
        {
            string key2, key3, value;
            key2 = section.StartsWith("[") ? string.Format("{0}{1}", section, key).ToLower() : string.Format("[{0}]{1}", section, key).ToLower();

            if (!dictionary.TryGetValue(key2, out value))
            {
                return null;
            }

            List<string> values = new List<string>();
            values.Add(value);
            int index = 1;
            while (true)
            {
                key3 = $"{key2}~{++index}";
                if (!dictionary.TryGetValue(key3, out value))
                {
                    break;
                }

                values.Add(value);
            }

            return values.ToArray();
        }
    }
}
