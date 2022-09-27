namespace GameDataParser.Files.MetadataExporter;

public abstract class Exporter<Metadata> : MetadataExporter
{
    protected readonly MetadataResources Resources;

    protected Exporter(MetadataResources resources, string slug) : base(slug)
    {
        Resources = resources;
    }

    protected override bool Serialize()
    {
        Metadata entities = Parse();
        return Write(entities);
    }

    protected abstract Metadata Parse();
}
