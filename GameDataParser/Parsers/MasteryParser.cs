using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

internal class MasteryParser : Exporter<List<MasteryMetadata>>
{
    public MasteryParser(MetadataResources resources) : base(resources, MetadataName.Mastery) { }

    protected override List<MasteryMetadata> Parse()
    {
        List<MasteryMetadata> masteryList = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/mastery") || entry.Name.StartsWith("table/masteryrecipe"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? masteries = document.SelectNodes("/ms2/mastery");
            if (masteries is null)
            {
                continue;
            }

            foreach (XmlNode mastery in masteries)
            {
                if (ParserHelper.CheckForNull(mastery, "type"))
                {
                    continue;
                }

                MasteryMetadata newMastery = new()
                {
                    Type = int.Parse(mastery.Attributes!["type"]!.Value)
                };

                XmlNodeList? grades = mastery.SelectNodes("v");
                if (grades is null)
                {
                    continue;
                }

                foreach (XmlNode grade in grades)
                {
                    if (grade?.Attributes is null)
                    {
                        continue;
                    }

                    MasteryGrade newGrade = new();
                    _ = int.TryParse(grade.Attributes["grade"]?.Value ?? "0", out newGrade.Grade);
                    _ = long.TryParse(grade.Attributes["value"]?.Value ?? "0", out newGrade.Value);
                    _ = int.TryParse(grade.Attributes["rewardJobItemID"]?.Value ?? "0", out newGrade.RewardJobItemID);
                    _ = int.TryParse(grade.Attributes["rewardJobItemRank"]?.Value ?? "0", out newGrade.RewardJobItemRank);
                    _ = int.TryParse(grade.Attributes["rewardJobItemCount"]?.Value ?? "0", out newGrade.RewardJobItemCount);
                    newGrade.Feature = grade.Attributes["feature"]?.Value ?? "";

                    newMastery.Grades.Add(newGrade);
                }

                masteryList.Add(newMastery);
            }
        }

        return masteryList;
    }
}
