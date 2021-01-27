using System;
using System.IO;
using ProtoBuf;

namespace GameDataParser.Files
{
    public abstract class MetadataExporter
    {
        protected string filename;

        public MetadataExporter(string slug)
        {
            this.filename = $"ms2-{slug}-metadata";
        }

        public void Export()
        {
            if (this.CheckHash())
            {
                Console.WriteLine($"\rSkipping {this.filename}");
                return;
            }

            this.Serialize();
            this.WriteHash();

            Console.WriteLine($"\rSuccessfully exported {this.filename}");
        }

        private bool CheckHash()
        {
            return Hash.HasValidHash(this.filename);
        }

        private void WriteHash()
        {
            Hash.WriteHash(this.filename);
        }

        public void Write<Entities>(Entities entities)
        {
            using (FileStream writeStream = File.Create($"{Paths.OUTPUT}/{this.filename}"))
            {
                Serializer.Serialize(writeStream, entities);
            }
        }

        protected abstract void Serialize();
    }
}
