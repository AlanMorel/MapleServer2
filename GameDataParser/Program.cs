using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Xml;
using GameDataParser.Crypto;
using GameDataParser.Crypto.Common;
using GameDataParser.Crypto.Stream;
using GameDataParser.Files;
using GameDataParser.Files.Export;

namespace GameDataParser
{
    internal static class Program
    {
        private static void Main()
        {
            // Force Globalization to en-US because we use periods instead of commas for decimals
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Create Resources folders if they don't exist
            Directory.CreateDirectory(Paths.INPUT);
            Directory.CreateDirectory(Paths.OUTPUT);

            // Resources
            try
            {
                string xmlPath = $"{Paths.INPUT}/Xml.m2d";
                string exportedPath = $"{Paths.INPUT}/Exported.m2d";
                string xmlHeaderPath = xmlPath.Replace("m2d", "m2h");
                string exportedHeaderPath = exportedPath.Replace("m2d", "m2h");

                List<PackFileEntry> xmlFiles = FileList.ReadFile(File.OpenRead(xmlHeaderPath));
                List<PackFileEntry> exportedFiles = FileList.ReadFile(File.OpenRead(exportedHeaderPath));

                MemoryMappedFile xmlMemFile = MemoryMappedFile.CreateFromFile(xmlPath);
                MemoryMappedFile exportedMemFile = MemoryMappedFile.CreateFromFile(exportedPath);

                // Threads
                Thread itemThread = new Thread(() => ItemMetadataExport.Export(xmlFiles, xmlMemFile));
                Thread mapEntityThread = new Thread(() => MapMetadataExport.Export(exportedFiles, exportedMemFile));
                Thread skillThread = new Thread(() => SkillMetadataExport.Export(xmlFiles, xmlMemFile));
                Thread insigniaThread = new Thread(() => InsigniaMetadataExport.Export(xmlFiles, xmlMemFile));
                Thread prestigeThread = new Thread(() => PrestigeMetadataExport.Export(xmlFiles, xmlMemFile));
                Thread expThread = new Thread(() => ExpMetadataExport.Export(xmlFiles, xmlMemFile));
                Thread questThread = new Thread(() => QuestMetadataExport.Export(xmlFiles, xmlMemFile));
                Thread scriptThread = new Thread(() => ScriptMetadataExport.Export(xmlFiles, xmlMemFile));
                Thread guildThread = new Thread(() => GuildMetadataExport.Export(xmlFiles, xmlMemFile));

                Spinner spinner = new Spinner();
                spinner.Start();

                itemThread.Start();
                mapEntityThread.Start();
                skillThread.Start();
                insigniaThread.Start();
                prestigeThread.Start();
                expThread.Start();
                questThread.Start();
                scriptThread.Start();
                guildThread.Start();

                itemThread.Join();
                mapEntityThread.Join();
                skillThread.Join();
                insigniaThread.Join();
                prestigeThread.Join();
                expThread.Join();
                questThread.Join();
                scriptThread.Join();
                guildThread.Join();

                spinner.Stop();

                Console.WriteLine("\r\nFinished parsing.");
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Xml.m2h") || ex.ToString().Contains("Xml.m2d") || ex.ToString().Contains("Exported.m2h") || ex.ToString().Contains("Exported.m2d"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Missing pack files.\nPlease copy all pack files (Xml.m2d, Xml.m2h, Exported.m2d, Exported.m2h) from game client into GameDataParser/Resources.");
                }
            }
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

        private class Spinner
        {
            private readonly string[] sequence = new string[] { "/", "-", "\\", "|" };
            private int counter = 0;
            private readonly int delay;
            private bool active;
            private readonly Thread thread;

            public Spinner(int delay = 500)
            {
                this.delay = delay;
                thread = new Thread(Spin);
            }

            public void Start()
            {
                active = true;
                if (!thread.IsAlive)
                {
                    thread.Start();
                }
            }

            public void Stop()
            {
                active = false;
                Draw(" ");
            }

            private void Spin()
            {
                while (active)
                {
                    Turn();
                    Thread.Sleep(delay);
                }
            }

            private void Draw(string c)
            {
                Console.Write($"\r{c}");
            }

            private void Turn()
            {
                Draw(sequence[++counter % sequence.Length]);
            }
        }
    }
}
