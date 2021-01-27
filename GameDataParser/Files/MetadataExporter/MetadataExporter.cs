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
            this.filename = "ms2-" + slug + "-metadata";
        }

        public void export()
        {
            if (this.checkHash())
            {
                Console.WriteLine("\rSkipping " + this.filename);
                return;
            }

            this.serialize();
            this.writeHash();

            Console.WriteLine("\rSuccessfully exported " + this.filename);
        }

        private bool checkHash()
        {
            return Hash.HasValidHash(this.filename);
        }

        private void writeHash()
        {
            Hash.WriteHash(this.filename);
        }

        public void write<Entities>(Entities entities)
        {
            using (FileStream writeStream = File.Create(Paths.OUTPUT + "/" + this.filename))
            {
                Serializer.Serialize(writeStream, entities);
            }
        }

        protected abstract void serialize();
    }
}
