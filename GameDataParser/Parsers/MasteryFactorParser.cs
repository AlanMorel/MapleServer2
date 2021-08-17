using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    class MasteryFactorParser : Exporter<List<MasteryFactorMetadata>>
    {
        public MasteryFactorParser(MetadataResources resources) : base(resources, "mastery-factor") { }

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
                XmlNodeList factors = document.SelectNodes("/ms2/v");

                foreach (XmlNode factor in factors)
                {
                    MasteryFactorMetadata newFactor = new MasteryFactorMetadata();
                    newFactor.Differential = int.Parse(factor.Attributes["differential"].Value);
                    newFactor.Factor = int.Parse(factor.Attributes["factor"].Value);
                    masteryFactorList.Add(newFactor);
                }
            }

            return masteryFactorList;
        }
    }
}
