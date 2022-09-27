using System.Xml;
using GameDataParser.Files;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class SetItemInfoParser : Exporter<List<SetItemInfoMetadata>>
{
    public SetItemInfoParser(MetadataResources resources) : base(resources, MetadataName.SetItemInfo) { }

    protected override List<SetItemInfoMetadata> Parse()
    {
        List<SetItemInfoMetadata> sets = new();

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/setiteminfo"))
            {
                continue;
            }

            XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/set");
            foreach (XmlNode node in nodeList)
            {
                int id = int.Parse(node.Attributes["id"].Value);
                int[] itemIds = node.Attributes["itemIDs"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? new int[0];
                int optionId = int.Parse(node.Attributes["optionID"].Value);
                string feature = node.Attributes["feature"]?.Value ?? "";
                bool showEffectIfItsSetItemMotion = int.Parse(node.Attributes["showEffectIfItsSetItemMotion"]?.Value ?? "0") == 1;
                bool isDisableTooltip = node.Attributes["isDisableTooltip"]?.Value == "true";

                sets.Add(new()
                {
                    Id = id,
                    ItemIds = itemIds,
                    OptionId = optionId,
                    Feature = feature,
                    ShowEffectIfItsSetItemMotion = showEffectIfItsSetItemMotion,
                    IsDisableTooltip = isDisableTooltip
                });
            }
        }

        return sets;
    }
}
