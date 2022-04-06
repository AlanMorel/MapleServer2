using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using Newtonsoft.Json;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ConstantsMetadataStorage
{
    private static readonly Dictionary<string, ConstantsMetadata> Constants = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Constants}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<ConstantsMetadata> constantsList = Serializer.Deserialize<List<ConstantsMetadata>>(stream);
        foreach (ConstantsMetadata constant in constantsList)
        {
            Constants[constant.Key] = constant;
        }

        // add/override
        string json = File.ReadAllText($"{Paths.JSON_DIR}/Constants.json");
        List<ConstantsMetadata> addedConstants = JsonConvert.DeserializeObject<List<ConstantsMetadata>>(json);
        foreach (ConstantsMetadata constant in addedConstants)
        {
            Constants[constant.Key] = constant;
        }
    }

    public static string GetConstant(string key)
    {
        return Constants.GetValueOrDefault(key).Value;
    }
}
