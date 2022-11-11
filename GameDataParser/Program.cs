using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using Maple2Storage.Extensions;
using Maple2Storage.Tools;
using Maple2Storage.Types;

namespace GameDataParser;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        CultureInfo.CurrentCulture = new("en-US");

        // Create Resources folders if they don't exist
        Directory.CreateDirectory(Paths.RESOURCES_INPUT_DIR);
        Directory.CreateDirectory(Paths.RESOURCES_DIR);
        Directory.CreateDirectory(Paths.NAVMESH_DIR);

        // If parameter --no-cache, delete all files inside resources folder
        if (args.Contains("--no-cache"))
        {
            foreach (string path in Directory.GetFiles(Paths.RESOURCES_DIR))
            {
                File.Delete(path);
            }

            foreach (string path in Directory.GetFiles(Paths.NAVMESH_DIR))
            {
                File.Delete(path);
            }
        }

        Stopwatch runtime = Stopwatch.StartNew();

        MetadataResources resources = new();
        List<Task> tasks = new();
        List<Type> parserClassList = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => !t.IsAbstract && !t.IsNested && t.IsClass && t.Namespace == "GameDataParser.Parsers").ToList();

        int count = 1;
        foreach (Type parserClass in parserClassList)
        {
            ConstructorInfo? newConstructor = parserClass.GetConstructor(new[]
            {
                typeof(MetadataResources)
            });
            MetadataExporter? exporter;

            // Verify if the new constructor doesn't need a parameter so can create an instances without a parameter.
            if (newConstructor != null)
            {
                exporter = (MetadataExporter) Activator.CreateInstance(parserClass, resources)!;
            }
            else
            {
                exporter = (MetadataExporter) Activator.CreateInstance(parserClass)!;
            }

            tasks.Add(Task.Run(() => exporter.Export()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    throw new($"Error: {t.Exception}");
                }

                ConsoleUtility.WriteProgressBar((float) count++ / parserClassList.Count * 100f);
            }));
        }

        await Task.WhenAll(tasks);
        runtime.Stop();
        Console.WriteLine("\nExporting Data Successfully!".ColorGreen());
        Console.WriteLine($"\nIt finished exporting in {runtime.Elapsed.Minutes} minutes and {runtime.Elapsed.Seconds} seconds");
    }
}
