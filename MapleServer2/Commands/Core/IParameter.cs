using System;

namespace MapleServer2.Commands.Core
{
    public interface IParameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        dynamic DefaultValue { get; }
        Type ValueType { get; }

        dynamic ConvertString(string value);

        void SetValue(string str);

        void SetValue(dynamic obj);

        void SetDefaultValue();
    }

    public interface IParameter<out T> : IParameter
    {
        T Value { get; }
    }
}
