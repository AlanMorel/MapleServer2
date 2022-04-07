using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class InstrumentInfoMetadataStorage
{
    private static readonly Dictionary<int, InstrumentInfoMetadata> Instruments = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.InstrumentInfo);
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
