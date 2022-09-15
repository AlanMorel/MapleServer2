using System.Xml;

namespace GameDataParser.Parsers.Helpers;

public static class ParserHelper
{
    public static bool CheckForNull(XmlNode node, params string[] attributes)
    {
        foreach (string attribute in attributes)
        {
            if (node.Attributes?[attribute] is null)
            {
                return true;
            }
        }

        return false;
    }
}
