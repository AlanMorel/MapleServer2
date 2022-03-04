using System.Reflection;
using Maple2Storage.Extensions;
using Maple2Storage.Tools;
using NLog;

namespace MapleServer2.Tools;

public static class MetadataHelper
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static async Task InitializeAll()
    {
        Logger.Info("Initializing Data...Please Wait".ColorYellow());

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
        Logger.Info("Initializing Data...Complete!".ColorGreen());
    }
}
