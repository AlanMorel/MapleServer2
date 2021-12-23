using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ConstantsParser : Exporter<List<ConstantsMetadata>>
{
    public ConstantsParser(MetadataResources resources) : base(resources, "constants") { }

    protected override List<ConstantsMetadata> Parse()
    {
        List<ConstantsMetadata> constants = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/constants.xml"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                string locale = string.IsNullOrEmpty(node.Attributes["locale"]?.Value) ? "" : node.Attributes["locale"].Value;
                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                if (node.Name != "v")
                {
                    continue;
                }

                constants.Add(new(node.Attributes["key"].Value, node.Attributes["value"].Value));
            }
        }

        return constants;
    }
}
