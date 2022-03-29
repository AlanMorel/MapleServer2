using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class FieldWarParser : Exporter<List<FieldWarMetadata>>
{
    public FieldWarParser(MetadataResources resources) : base(resources, "fieldwar") { }

    protected override List<FieldWarMetadata> Parse()
    {
        List<FieldWarMetadata> fieldWar = new();
        PackFileEntry fieldWarData = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/fieldwardata"));

        if (fieldWarData is null)
        {
            throw new FileNotFoundException("Could not find table fieldwardata.xml");
        }

        XmlDocument document = Resources.XmlReader.GetXmlDocument(fieldWarData);
        XmlNodeList nodes = document.SelectNodes("/ms2/fieldWar");

        foreach (XmlNode node in nodes)
        {
            FieldWarMetadata metadata = new()
            {
                FieldWarId = int.Parse(node.Attributes["fieldWarID"].Value),
                RewardId = int.Parse(node.Attributes["rewardID"].Value),
                MapId = int.Parse(node.Attributes["fieldID"].Value),
                GroupId = byte.Parse(node.Attributes["fieldWarGroupID"].Value)
            };

            fieldWar.Add(metadata);
        }

        return fieldWar;
    }
}
