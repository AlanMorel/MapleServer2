using Maple2Storage.Types;
using MapleServer2.Types;
using Newtonsoft.Json;

namespace MapleServer2.Data.Static;

public static class GlobalEventsMetadataStorage
{
    private static readonly Dictionary<int, GlobalEvent> GlobalEvents = new();

    public static void Init()
    {
        string json = File.ReadAllText($"{Paths.JSON_DIR}/GlobalEvents.json");
        List<GlobalEvent> items = JsonConvert.DeserializeObject<List<GlobalEvent>>(json);
        foreach (GlobalEvent item in items)
        {
            GlobalEvents[item.Id] = item;
        }
    }

    public static List<GlobalEvent> GetAllAutoGlobalEvents()
    {
        return GlobalEvents.Values.ToList();
    }
}
