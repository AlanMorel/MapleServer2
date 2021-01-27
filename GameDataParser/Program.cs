using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using GameDataParser.Crypto;
using GameDataParser.Crypto.Stream;
using GameDataParser.Files;
using GameDataParser.Parsers;

namespace GameDataParser
{
    internal static class Program
    {
        static async Task Main()
        {
            // Create Resources folders if they don't exist
            Directory.CreateDirectory(Paths.INPUT);
            Directory.CreateDirectory(Paths.OUTPUT);

            Spinner spinner = new Spinner();
            spinner.Start();

            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            MetadataResources resources = new MetadataResources();

            IEnumerable<MetadataExporter> exports = new List<MetadataExporter>()
            {
                new ItemParser(resources),
                new MapEntityParser(resources),
                new SkillParser(resources),
                new InsigniaParser(resources),
                new ExpTableParser(resources),
                new QuestParser(resources),
                new ScriptParser(resources),
                new GuildParser(resources),
                new PrestigeParser(resources)
            };

            IEnumerable<Task> tasks = exports.Select(exporter => Task.Run(() => exporter.export()));

            await Task.WhenAll(tasks);

            spinner.Stop();
            TimeSpan runtime = spinner.getRuntime();

            Console.WriteLine("\rExporting finished in " + runtime.Minutes + " minutes and " + runtime.Seconds + " seconds");
        }

        public static XmlReader GetReader(this MemoryMappedFile m2dFile, IPackFileHeader header)
        {
            return XmlReader.Create(new MemoryStream(CryptoManager.DecryptData(header, m2dFile)));
        }

        public static XmlDocument GetDocument(this MemoryMappedFile m2dFile, IPackFileHeader header)
        {
            XmlDocument document = new XmlDocument();
            document.Load(new MemoryStream(CryptoManager.DecryptData(header, m2dFile)));
            return document;
        }
    }
}
