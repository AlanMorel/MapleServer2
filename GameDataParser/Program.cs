using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using GameDataParser.Files;
using Maple2Storage.Extensions;
using Maple2Storage.Tools;

namespace GameDataParser
{
    internal static class Program
    {
        private static async Task Main()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Create Resources folders if they don't exist
            Directory.CreateDirectory(Paths.INPUT);
            Directory.CreateDirectory(Paths.OUTPUT);
            Stopwatch runtime = Stopwatch.StartNew();

            object resources = Activator.CreateInstance(typeof(MetadataResources));
            List<Task> tasks = new();
            List<Type> parserClassList = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && !t.IsNested && t.IsClass && t.Namespace == "GameDataParser.Parsers").ToList();

            int count = 1;
            foreach (Type parserClass in parserClassList)
            {
                ConstructorInfo newConstructor = parserClass.GetConstructor(new Type[] { typeof(MetadataResources) });
                MetadataExporter exporter;

                // Verify if the new constructor doesn't need a parameter so can create an instances without a parameter.
                if (newConstructor != null)
                {
                    exporter = (MetadataExporter) Activator.CreateInstance(parserClass, resources);
                }
                else
                {
                    exporter = (MetadataExporter) Activator.CreateInstance(parserClass);
                }

                tasks.Add(Task.Run(() => exporter.Export()).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        throw new Exception($"Error: {t.Exception}");
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
}
