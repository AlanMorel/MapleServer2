using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class UGCMapParser : Exporter<List<UGCMapMetadata>>
    {
        public UGCMapParser(MetadataResources resources) : base(resources, "ugc-map") { }

        protected override List<UGCMapMetadata> Parse()
        {
            List<UGCMapMetadata> ugcmap = new List<UGCMapMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("ugcmap"))
                {
                    continue;
                }

                UGCMapMetadata metadata = new UGCMapMetadata();
                string filename = Path.GetFileNameWithoutExtension(entry.Name);
                metadata.MapId = int.Parse(filename);

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "group")
                    {
                        UGCMapGroup group = new UGCMapGroup();

                        group.Id = byte.Parse(node.Attributes["no"].Value);
                        group.Price = int.Parse(node.Attributes["contractPrice"].Value);
                        group.PriceItemCode = int.Parse(node.Attributes["contractPriceItemCode"].Value);
                        group.ExtensionPrice = int.Parse(node.Attributes["extensionPrice"].Value);
                        group.ExtensionPriceItemCode = int.Parse(node.Attributes["extensionPriceItemCode"].Value);
                        group.ContractDate = short.Parse(node.Attributes["ugcHomeContractDate"].Value);
                        group.ExtensionDate = short.Parse(node.Attributes["ugcHomeExtensionDate"].Value);
                        group.HeightLimit = byte.Parse(node.Attributes["heightLimit"].Value);
                        group.BuildingCount = short.Parse(node.Attributes["installableBuildingCount"].Value);
                        if (node.Attributes["returnPlaceID"] != null)
                        {
                            group.ReturnPlaceId = byte.Parse(node.Attributes["returnPlaceID"].Value);
                        }
                        group.Area = short.Parse(node.Attributes["area"].Value);
                        group.SellType = byte.Parse(node.Attributes["sellType"].Value);
                        group.BlockCode = byte.Parse(node.Attributes["blockCode"].Value);
                        group.HouseNumber = short.Parse(node.Attributes["houseNumber"].Value);

                        metadata.Groups.Add(group);
                    }
                }

                ugcmap.Add(metadata);
            }
            return ugcmap;
        }

        public static CurrencyType GetCurrencyType(int itemId)
        {
            switch (itemId)
            {
                case 30000145: // Apartment Permits. This is obsolete content
                case 30000253:
                case 30000254:
                case 30000255:
                case 90000001:
                case 90000002:
                case 90000003:
                    return CurrencyType.Meso;
                case 90000004:
                    return CurrencyType.Meret;
                case 90000006:
                    return CurrencyType.ValorToken;
                case 90000013:
                    return CurrencyType.Rue;
                case 90000014:
                    return CurrencyType.HaviFruit;
                case 90000017:
                    return CurrencyType.Treva;
                case 90000027:
                    return CurrencyType.MesoToken;
                default:
                    break;
            }
            throw new ArgumentException($"Unknown Currency Type for: {itemId}");
        }
    }
}

