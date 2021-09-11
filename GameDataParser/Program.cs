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
            Spinner spinner = new Spinner();
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Create Resources folders if they don't exist
            Directory.CreateDirectory(Paths.INPUT);
            Directory.CreateDirectory(Paths.OUTPUT);

            object resources = Activator.CreateInstance(typeof(MetadataResources));
            List<Task> tasks = new();
            List<Type> callingTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsNested && t.IsClass && t.Namespace == "GameDataParser.Parsers").ToList();

            foreach (Type type in callingTypes)
            {
                // Initialize the constructor of the class.
                ConstructorInfo ctor = type.GetConstructor(new Type[] { typeof(MetadataResources) });

                // Initialize and get all params for the constructor.
                object parser = ctor != null ? ctor.Invoke(ctor.GetParameters().Select(param => param.HasDefaultValue ? param.DefaultValue : param.ParameterType.IsValueType
                    && Nullable.GetUnderlyingType(param.ParameterType) == null ? Activator.CreateInstance(param.ParameterType) : null).ToArray()) : Activator.CreateInstance(type);

                // Initialize the instances of the type with the constructor and parameter.
                MetadataExporter exporter = (MetadataExporter) (ctor != null ? Activator.CreateInstance(parser.GetType(), resources) : Activator.CreateInstance(parser.GetType()));

                // Add the exporter in a task list for later be called in async.
                tasks.Add(Task.Run(() => exporter.Export()));
            }

            int count = 0;
            foreach (Task task in tasks)
            {
                task.Wait();
                count++;
                ConsoleUtility.WriteProgressBar((float) count / tasks.Count * 100f);
            }

            await Task.WhenAll(tasks);
            spinner.Stop();
            TimeSpan runtime = spinner.GetRuntime();

            Console.WriteLine("\nExporting Data Successfully!".ColorGreen());
            Console.WriteLine($"\nIt finished exporting in {runtime.Minutes} minutes and {runtime.Seconds} seconds");
        }
    }
}
