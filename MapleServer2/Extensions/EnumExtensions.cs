using System.Reflection;

namespace MapleServer2.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the value with the type of the enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dynamic GetValue(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            Type type = Enum.GetUnderlyingType(value.GetType());

            return Convert.ChangeType(field.GetValue(value), type);
        }
    }
}
