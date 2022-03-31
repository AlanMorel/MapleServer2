namespace MapleServer2.Commands.Core;

public class Parameter<T> : IParameter<T>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public T Value { get; private set; }
    public Type ValueType => typeof(T);
    dynamic IParameter.DefaultValue
    {
        get => Value;
        set => Value = default;
    }

    public Parameter(string name, string description = "", T defaultValue = default)
    {
        Name = name;
        Description = description;
        Value = defaultValue;
    }

    public void SetValue(string str)
    {
        Value = ConvertString(str);
    }

    public void SetValue(dynamic obj)
    {
        Value = obj;
    }

    public void SetDefaultValue()
    {
        Value = ConvertString(string.Empty);
    }

    public dynamic ConvertString(string value)
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
            return Activator.CreateInstance(typeof(T));
        }
    }
}
