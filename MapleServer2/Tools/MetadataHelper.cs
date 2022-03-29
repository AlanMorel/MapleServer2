using System.Reflection;
using Maple2Storage.Tools;
using Serilog;

namespace MapleServer2.Tools;

public static class MetadataHelper
{
    private static readonly ILogger _logger = Log.Logger.ForContext(typeof(MetadataHelper));

    public static async Task InitializeAll()
    {
        _logger.Information("Initializing Data...Please Wait");

        List<Task> tasks = new();
        List<Type> listStaticClass = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsAbstract && t.IsClass && t.Namespace == "MapleServer2.Data.Static").ToList();

        int count = 1;
        foreach (Type staticClass in listStaticClass)
        {
            tasks.Add(Task.Run(() => staticClass.GetMethod("Init")?.Invoke(null, null))
                .ContinueWith(t =>
                {
                    ConsoleUtility.WriteProgressBar((float) count++ / listStaticClass.Count * 100);
                }));
        }

        await Task.WhenAll(tasks);
        _logger.Information("Initializing Data...Complete!");
    }
}
