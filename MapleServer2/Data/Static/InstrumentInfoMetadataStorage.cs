using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class InstrumentInfoMetadataStorage
{
    private static readonly Dictionary<int, InstrumentInfoMetadata> Instruments = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.InstrumentInfo}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<InstrumentInfoMetadata> items = Serializer.Deserialize<List<InstrumentInfoMetadata>>(stream);
        foreach (InstrumentInfoMetadata item in items)
        {
            Instruments[item.InstrumentId] = item;
        }
    }

    public static bool IsValid(int instrumentId)
    {
        return Instruments.ContainsKey(instrumentId);
    }

    public static InstrumentInfoMetadata GetMetadata(int instrumentId)
    {
        return Instruments.GetValueOrDefault(instrumentId);
    }

    public static int GetId(int instrumentId)
    {
        return Instruments.GetValueOrDefault(instrumentId).InstrumentId;
    }
}
