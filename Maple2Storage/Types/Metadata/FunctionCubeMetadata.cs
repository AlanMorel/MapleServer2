using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class FunctionCubeMetadata
{
    [XmlElement(Order = 1)]
    public int CubeId;
    [XmlElement(Order = 2)]
    public int RecipeId;

    public FunctionCubeMetadata() { }

    public FunctionCubeMetadata(int cubeId, int recipeId)
    {
        CubeId = cubeId;
        RecipeId = recipeId;
    }
}
