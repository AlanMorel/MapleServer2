using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class PremiumClubEffectParser : Exporter<List<PremiumClubEffectMetadata>>
{
    public PremiumClubEffectParser(MetadataResources resources) : base(resources, MetadataName.PremiumClubEffect) { }

    protected override List<PremiumClubEffectMetadata> Parse()
    {
        List<PremiumClubEffectMetadata> metadataList = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/vipbenefiteffecttable"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodes = document.SelectNodes("/ms2/benefit");

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["locale"].Value != "NA")
                {
                    continue;
                }

                PremiumClubEffectMetadata metadata = new()
                {
                    EffectId = int.Parse(node.Attributes["effectID"].Value),
                    EffectLevel = int.Parse(node.Attributes["effectLevel"].Value),
                };

                metadataList.Add(metadata);
            }
        }

        return metadataList;
    }
}
