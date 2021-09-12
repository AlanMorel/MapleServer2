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

            Spinner spinner = new Spinner();

            object resources = Activator.CreateInstance(typeof(MetadataResources));
            List<Task> tasks = new();
            List<Type> parserClassList = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && !t.IsNested && t.IsClass && t.Namespace == "GameDataParser.Parsers").ToList();

            int count = 0;
            foreach (Type parserClass in parserClassList)
            {
                ConstructorInfo newConstructor = parserClass.GetConstructor(new Type[] { typeof(MetadataResources) });

                object currentParser = new object();

                // Verify if the new constructor doesn't need a parameter so can create an instances without a parameter.
                if (newConstructor != null)
                {
                    ParameterInfo[] newParameters = newConstructor.GetParameters();

                    foreach (ParameterInfo currentParameter in newParameters)
                    {
                        currentParser = Activator.CreateInstance(parserClass, resources);
                    }
                }
                else
                {
                    currentParser = Activator.CreateInstance(parserClass);
                }

                MetadataExporter exporter = (MetadataExporter) (newConstructor != null ? Activator.CreateInstance(currentParser.GetType(), resources) : Activator.CreateInstance(currentParser.GetType()));
                tasks.Add(Task.Run(() => exporter.Export()).ContinueWith(t => ConsoleUtility.WriteProgressBar((float) count++ / parserClassList.Count * 100f)));
            }

            await Task.WhenAll(tasks);
            spinner.Stop();
            TimeSpan runtime = spinner.GetRuntime();

            Console.WriteLine("\nExporting Data Successfully!".ColorGreen());
            Console.WriteLine($"\nIt finished exporting in {runtime.Minutes} minutes and {runtime.Seconds} seconds");
        }
    }
}
