using System;
using System.IO;
using ProtoBuf;

namespace GameDataParser.Files
{
    public abstract class MetadataExporter
    {
        protected string Filename;

        public MetadataExporter(string slug)
        {
            Filename = $"ms2-{slug}-metadata";
        }

        public void Export()
        {
            if (CheckHash())
            {
                Console.WriteLine($"\rSkipping {Filename}");
                return;
            }

            Serialize();
            WriteHash();

            Console.WriteLine($"\rSuccessfully exported {Filename}");
        }

        private bool CheckHash()
        {
            return Hash.HasValidHash(Filename);
        }

        private void WriteHash()
        {
            Hash.WriteHash(Filename);
        }

        public void Write<Entities>(Entities entities)
        {
            using (FileStream writeStream = File.Create($"{Paths.OUTPUT}/{Filename}"))
            {
                Serializer.Serialize(writeStream, entities);
            }
        }

        protected abstract void Serialize();
    }
}
