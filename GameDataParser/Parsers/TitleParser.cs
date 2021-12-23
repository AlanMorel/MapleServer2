using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class TitleParser : Exporter<List<TitleMetadata>>
{
    public TitleParser(MetadataResources resources) : base(resources, "title") { }

    protected override List<TitleMetadata> Parse()
    {
        List<TitleMetadata> metadatas = new();

        PackFileEntry file = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.Contains("string/en/titlename.xml"));
        if (file is null)
        {
            throw new FileNotFoundException("File not found: string/en/titlename.xml");
        }

        XmlDocument document = Resources.XmlReader.GetXmlDocument(file);
        XmlNodeList nodes = document.SelectNodes("/ms2/key");
        foreach (XmlNode node in nodes)
        {
            int id = int.Parse(node.Attributes["id"].Value);
            if (id < 4)
            {
                continue;
            }

            string name = node.Attributes["name"].Value;
            string feature = node.Attributes["feature"]?.Value ?? string.Empty;
            metadatas.Add(new(id, name, feature));
        }

        return metadatas;
    }
}
