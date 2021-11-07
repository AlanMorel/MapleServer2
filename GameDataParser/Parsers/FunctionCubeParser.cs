using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class FunctionCubeParser : Exporter<List<FunctionCubeMetadata>>
{
    public FunctionCubeParser(MetadataResources resources) : base(resources, "function-cube") { }

    protected override List<FunctionCubeMetadata> Parse()
    {
        List<FunctionCubeMetadata> metadatas = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("object"))
            {
                continue;
            }

            int cubeId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);

            string recipeString = document.DocumentElement.SelectSingleNode("FunctionCube")?.Attributes["receipeID"]?.Value;
            int recipeId = string.IsNullOrEmpty(recipeString) ? 0 : int.Parse(recipeString);

            metadatas.Add(new(cubeId, recipeId));
        }

        return metadatas;
    }
}
