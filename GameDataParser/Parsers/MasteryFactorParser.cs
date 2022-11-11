using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

internal class MasteryFactorParser : Exporter<List<MasteryFactorMetadata>>
{
    public MasteryFactorParser(MetadataResources resources) : base(resources, MetadataName.MasteryFactor) { }

    protected override List<MasteryFactorMetadata> Parse()
    {
        List<MasteryFactorMetadata> masteryFactorList = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/masterydifferentialfactor"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? factors = document.SelectNodes("/ms2/v");
            if (factors is null)
            {
                continue;
            }

            foreach (XmlNode factor in factors)
            {
                if (ParserHelper.CheckForNull(factor, "differential", "factor"))
                {
                    continue;
                }

                MasteryFactorMetadata newFactor = new()
                {
                    Differential = int.Parse(factor.Attributes!["differential"]!.Value),
                    Factor = int.Parse(factor.Attributes["factor"]!.Value)
                };
                masteryFactorList.Add(newFactor);
            }
        }

        return masteryFactorList;
    }
}
