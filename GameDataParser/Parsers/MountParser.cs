using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
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
        Dictionary<int, MountMetadata> mounts = new();

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("riding/"))
            {
                continue;
            }

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

                if (node.Attributes["locale"]?.Value == "NA")
                {
                    mounts[id] = new()
                    {
                        Id = id,
                        RunConsumeEp = runXConsumeEp,
                        MountStats = mountStats
                    };

                    // If there is a NA locale, use it and skip the other locales.
                    break;
                }

                mounts[id] = new()
                {
                    Id = id,
                    RunConsumeEp = runXConsumeEp,
                    MountStats = mountStats
                };
            }
        }

        return mounts.Values.ToList();
    }
}
