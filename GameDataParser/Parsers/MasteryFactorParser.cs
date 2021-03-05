using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    class MasteryFactorParser : Exporter<List<MasteryFactorMetadata>>
    {
        public MasteryFactorParser(MetadataResources resources) : base(resources, "mastery-factor") { }

        protected override List<MasteryFactorMetadata> Parse()
        {
            List<MasteryFactorMetadata> masteryFactorList = new();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/masterydifferentialfactor"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
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
