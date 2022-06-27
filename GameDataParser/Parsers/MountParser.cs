using System.Xml;
using GameDataParser.Files;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class RideParser : Exporter<List<MountMetadata>>
{
    public RideParser(MetadataResources resources) : base(resources, MetadataName.Mount) { }

    protected override List<MountMetadata> Parse()
    {
        List<MountMetadata> rewards = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("riding/"))
            {
                continue;
            }

            // TRY FILTER NA HERE
            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList tableNodes = document.GetElementsByTagName("riding");
            foreach (XmlNode node in tableNodes)
            {
                XmlNode basic = node.SelectSingleNode("basic");

                if (!int.TryParse(basic?.Attributes?["id"]?.Value, out int id))
                {
                    continue;
                }

                if (!int.TryParse(basic.Attributes?["runXConsumeEp"]?.Value, out int runXConsumeEp))
                {
                    continue;
                }

                XmlStats mountStats = StatParser.ParseStats(node.SelectSingleNode("stat").Attributes);

                rewards.Add(new()
                {
                    Id = id,
                    RunConsumeEp = runXConsumeEp,
                    MountStats = mountStats
                });
            }
        }

        return rewards;
    }
}
