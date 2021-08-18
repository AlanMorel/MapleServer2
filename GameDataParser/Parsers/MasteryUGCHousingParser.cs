using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class MasteryUGCHousingParser : Exporter<List<MasteryUGCHousingMetadata>>
    {
        public MasteryUGCHousingParser(MetadataResources resources) : base(resources, "mastery-ugc-housing") { }

        protected override List<MasteryUGCHousingMetadata> Parse()
        {
            List<MasteryUGCHousingMetadata> metadataList = new List<MasteryUGCHousingMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/masteryugchousing"))
                {
                    continue;
                }

                XmlNodeList document = Resources.XmlReader.GetXmlDocument(entry).GetElementsByTagName("v");
                foreach (XmlNode node in document)
                {
                    MasteryUGCHousingMetadata metadata = new MasteryUGCHousingMetadata();

                    metadata.Grade = byte.Parse(node.Attributes["grade"].Value);
                    metadata.MasteryRequired = short.Parse(node.Attributes["value"].Value);
                    metadata.ItemId = string.IsNullOrEmpty(node.Attributes["rewardJobItemID"].Value) ? 0 : int.Parse(node.Attributes["rewardJobItemID"].Value);

                    metadataList.Add(metadata);
                }
            }

            return metadataList;
        }
    }
}
