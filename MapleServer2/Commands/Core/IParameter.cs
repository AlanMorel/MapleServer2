using System;

namespace MapleServer2.Commands.Core
{
    public interface IParameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        object DefaultValue { get; }
        Type ValueType { get; }

        object ConvertString(string value);

        void SetValue(string str);

        void SetValue(object obj);

        void SetDefaultValue();
    }

    public interface IParameter<out T> : IParameter
    {
        T Value { get; }
    }
}
