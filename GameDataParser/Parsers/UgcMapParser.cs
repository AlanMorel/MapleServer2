using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class UgcMapParser : Exporter<List<UgcMapMetadata>>
{
    public UgcMapParser(MetadataResources resources) : base(resources, MetadataName.UGCMap) { }

    protected override List<UgcMapMetadata> Parse()
    {
        List<UgcMapMetadata> ugcmap = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("ugcmap"))
            {
                continue;
            }

            UgcMapMetadata metadata = new();
            string filename = Path.GetFileNameWithoutExtension(entry.Name);
            metadata.MapId = int.Parse(filename);

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodes = document.SelectNodes("/ugcmap/group");

            foreach (XmlNode node in nodes)
            {
                UgcMapGroup group = new()
                {
                    Id = byte.Parse(node.Attributes["no"].Value),
                    Price = int.Parse(node.Attributes["contractPrice"].Value),
                    PriceItemCode = int.Parse(node.Attributes["contractPriceItemCode"].Value),
                    ExtensionPrice = int.Parse(node.Attributes["extensionPrice"].Value),
                    ExtensionPriceItemCode = int.Parse(node.Attributes["extensionPriceItemCode"].Value),
                    ContractDate = short.Parse(node.Attributes["ugcHomeContractDate"].Value),
                    ExtensionDate = short.Parse(node.Attributes["ugcHomeExtensionDate"].Value),
                    HeightLimit = byte.Parse(node.Attributes["heightLimit"].Value),
                    BuildingCount = short.Parse(node.Attributes["installableBuildingCount"].Value),
                    ReturnPlaceId = byte.Parse(node.Attributes["returnPlaceID"]?.Value ?? "0"),
                    Area = short.Parse(node.Attributes["area"].Value),
                    SellType = byte.Parse(node.Attributes["sellType"].Value),
                    BlockCode = byte.Parse(node.Attributes["blockCode"].Value),
                    HouseNumber = short.Parse(node.Attributes["houseNumber"].Value)
                };

                metadata.Groups.Add(group);
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
