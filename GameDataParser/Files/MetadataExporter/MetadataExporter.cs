using System.Xml.Serialization;
using Maple2Storage.Types;
using ProtoBuf;

namespace GameDataParser.Files;

public abstract class MetadataExporter
{
    private readonly string Filename;

    protected MetadataExporter(string slug)
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

        if (!Serialize())
        {
            return;
        }

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

    protected bool Write<Entities>(Entities entities)
    {
        if (entities is null)
        {
            return false;
        }

        using (FileStream writeStream = File.Create($"{Paths.RESOURCES_DIR}/{Filename}"))
        {
            Serializer.Serialize(writeStream, entities);
        }

#if DEBUG
        using (FileStream debugWriteStream = File.Create($"{Paths.RESOURCES_DIR}/{Filename}.xml"))
        {
            try
            {
                XmlSerializer xmlSerializer = new(typeof(Entities));
                xmlSerializer.Serialize(debugWriteStream, entities);
            }
            catch
            {
                Console.WriteLine($"Unable to serialize {debugWriteStream.Name}");
            }
        }
#endif
        return true;
    }

    protected abstract bool Serialize();
}
