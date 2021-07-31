using System;
using NLog;

namespace MapleServer2.Commands.Core
{
    public class Parameter<T> : IParameter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string Name { get; set; }
        public string Description { get; set; }
        public T Value { get; private set; }
        public Type ValueType => typeof(T);
        object IParameter.DefaultValue => Value;

        public Parameter(string name, string description = "", T defaultValue = default)
        {
            Name = name;
            Description = description;
            Value = defaultValue;
        }

        public void SetValue(string str) => Value = ConvertString(str) != null ? (T) ConvertString(str) : default;

        public void SetValue(object obj) => Value = obj != null ? (T) obj : default;

        public void SetDefaultValue() => Value = (T) ConvertString(string.Empty);

        public object ConvertString(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return Value;
                }
                if (ValueType == typeof(string))
                {
                    return value;
                }
                return Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                Logger.Error($"Error converting arg: {value} => {typeof(T)}.");
                return default;
            }
        }
    }
}
