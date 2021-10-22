using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class InstrumentInfoMetadataStorage
    {
        private static readonly Dictionary<int, InsturmentInfoMetadata> package = new Dictionary<int, InsturmentInfoMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-instrument-info-metadata");
            List<InsturmentInfoMetadata> items = Serializer.Deserialize<List<InsturmentInfoMetadata>>(stream);
            foreach (InsturmentInfoMetadata item in items)
            {
                package[item.InstrumentId] = item;
            }
        }

        public static bool IsValid(int instrumentId)
        {
            return package.ContainsKey(instrumentId);
        }

        public static InsturmentInfoMetadata GetMetadata(int instrumentId)
        {
            return package.GetValueOrDefault(instrumentId);
        }

        public static int GetId(int instrumentId)
        {
            return package.GetValueOrDefault(instrumentId).InstrumentId;
        }
    }
}
