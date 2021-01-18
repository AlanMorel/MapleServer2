using System;
using System.Threading;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Xml;
using System.Collections.Generic;
using GameDataParser.Crypto;
using GameDataParser.Crypto.Stream;
using GameDataParser.Files.Export;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;

namespace GameDataParser
{
    internal static class Program
    {
        private static void Main()
        {
            // Force Globalization to en-US because we use periods instead of commas for decimals
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Resources
            string xmlHeaderFile = VariableDefines.XML_PATH.Replace(".m2d", ".m2h");
            string exportedHeaderFile = VariableDefines.EXPORTED_PATH.Replace(".m2d", ".m2h");

            List<PackFileEntry> xmlFiles = FileList.ReadFile(File.OpenRead(xmlHeaderFile));
            List<PackFileEntry> exportedFiles = FileList.ReadFile(File.OpenRead(exportedHeaderFile));

            MemoryMappedFile xmlMemFile = MemoryMappedFile.CreateFromFile(VariableDefines.XML_PATH);
            MemoryMappedFile exportedMemFile = MemoryMappedFile.CreateFromFile(VariableDefines.EXPORTED_PATH);

            // Threads
            Thread itemThread = new Thread(() => ItemMetadataExport.Export(xmlFiles, xmlMemFile));
            Thread mapEntityThread = new Thread(() => MapMetadataExport.Export(exportedFiles, exportedMemFile));
            Thread skillThread = new Thread(() => SkillMetadataExport.Export(xmlFiles, xmlMemFile));
            Thread insigniaThread = new Thread(() => InsigniaMetadataExport.Export(xmlFiles, xmlMemFile));
            Thread prestigeThread = new Thread(() => PrestigeMetadataExport.Export(xmlFiles, xmlMemFile));
            Thread expThread = new Thread(() => ExpMetadataExport.Export(xmlFiles, xmlMemFile));
            Thread questThread = new Thread(() => QuestMetadataExport.Export(xmlFiles, xmlMemFile));

            Spinner spinner = new Spinner();
            spinner.Start();

            itemThread.Start();
            mapEntityThread.Start();
            skillThread.Start();
            insigniaThread.Start();
            prestigeThread.Start();
            expThread.Start();
            questThread.Start();

            itemThread.Join();
            mapEntityThread.Join();
            skillThread.Join();
            insigniaThread.Join();
            prestigeThread.Join();
            expThread.Join();
            questThread.Join();

            spinner.Stop();
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
