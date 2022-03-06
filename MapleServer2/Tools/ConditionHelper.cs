namespace MapleServer2.Tools;

public static class ConditionHelper
{
    /// <summary>
    /// Checks if the given condition code or target match the given string value.
    /// </summary>
    /// <param name="conditionType">Condition code or target</param>
    /// <param name="value">Value to try match</param>
    public static bool IsMatching(string conditionType, string value)
    {
        return conditionType.Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if the given condition codes or targets match the given long value.
    /// </summary>
    /// <param name="conditionType">Conditions codes or targets</param>
    /// <param name="value">Value to try match</param>
    public static bool IsMatching(string conditionType, long value)
    {
        // Conditions can have ranges and lists, so we need to check both
        if (conditionType.Contains('-') && conditionType.Contains(','))
        {
            string[] conditionsValues = conditionType.Split(',');
            foreach (string conditionValue in conditionsValues)
            {
                if (conditionValue.Contains('-') && IsInRange(conditionValue, value))
                {
                    return true;
                }

                if (IsMatching(conditionValue, value))
                {
                    return true;
                }
            }
        }

        // Check if the value is in the range
        if (conditionType.Contains('-') && IsInRange(conditionType, value))
        {
            return true;
        }

        // Check if the value is in the list
        if (conditionType.Contains(',') && IsInList(conditionType, value))
        {
            return true;
        }

        return long.TryParse(conditionType, out long parsedCondition) && parsedCondition == value;
    }

    public static bool IsInList(string conditionList, long value)
    {
        string[] conditions = conditionList.Split(',');
        foreach (string c in conditions)
        {
            if (long.TryParse(c, out long listItem) && value == listItem)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsInRange(string conditionRange, long value)
    {
        string[] parts = conditionRange.Split('-');
        if (!long.TryParse(parts[0], out long lowerBound))
        {
            return false;
        }

        if (!long.TryParse(parts[1], out long upperBound))
        {
            return false;
        }

        return value >= lowerBound && value <= upperBound;
    }
}
