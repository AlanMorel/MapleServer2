using System.Reflection;
using Maple2Storage.Tools;
using Maple2Storage.Types;
using Serilog;

namespace MapleServer2.Tools;

public static class MetadataHelper
{
    private static readonly ILogger Logger = Log.Logger.ForContext(typeof(MetadataHelper));

    public static async Task InitializeAll()
    {
        Logger.Information("Initializing Data...Please Wait");

        List<Task> tasks = new();
        List<Type> listStaticClass = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsAbstract && t.IsClass && t.Namespace == "MapleServer2.Data.Static").ToList();

        int count = 1;
        foreach (Type staticClass in listStaticClass)
        {
            tasks.Add(Task.Run(() => staticClass.GetMethod("Init")?.Invoke(null, null))
                .ContinueWith(_ =>
                {
                    ConsoleUtility.WriteProgressBar((float) count++ / listStaticClass.Count * 100);
                }));
        }

        await Task.WhenAll(tasks);
        Logger.Information("Initializing Data...Complete!");
    }

    public static FileStream GetFileStream(string metadataName)
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{metadataName}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        return File.OpenRead(path);
    }
}
