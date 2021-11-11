using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class UGCDesignParser : Exporter<List<UGCDesignMetadata>>
{
    public UGCDesignParser(MetadataResources resources) : base(resources, "ugc-design") { }
    protected override List<UGCDesignMetadata> Parse()
    {
        List<UGCDesignMetadata> metadatas = new();

        PackFileEntry file = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.Contains("table/na/ugcdesign.xml"));
        if (file is null)
        {
            throw new FileNotFoundException("File not found: table/na/ugcdesign.xml");
        }

        XmlDocument document = Resources.XmlReader.GetXmlDocument(file);
        XmlNodeList nodes = document.SelectNodes($"/ms2/list");
        foreach (XmlNode node in nodes)
        {
            int itemId = int.Parse(node.Attributes["itemID"].Value);
            bool visible = byte.Parse(node.Attributes["visible"].Value) == 1;
            byte rarity = byte.Parse(node.Attributes["itemGrade"].Value);
            byte priceType = byte.Parse(node.Attributes["priceType"].Value);
            CurrencyType currencyType = priceType switch
            {
                0 => CurrencyType.Meso,
                1 => CurrencyType.Meret,
                _ => throw new NotImplementedException(),
            };
            long price = long.Parse(node.Attributes["price"].Value);
            long salePrice = long.Parse(node.Attributes["salePrice"].Value);
            long marketMinPrice = long.Parse(node.Attributes["marketMinPrice"].Value);
            long marketMaxPrice = long.Parse(node.Attributes["marketMaxPrice"].Value);
            metadatas.Add(new UGCDesignMetadata(itemId, visible, rarity, currencyType, price, salePrice, marketMinPrice, marketMaxPrice));
        }

        return metadatas;
    }
}
