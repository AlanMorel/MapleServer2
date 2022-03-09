namespace GameDataParser.Files;

public abstract class Exporter<Metadata> : MetadataExporter
{
    protected readonly MetadataResources Resources;

    protected Exporter(MetadataResources resources, string slug) : base(slug)
    {
        Resources = resources;
    }

    protected override void Serialize()
    {
        Metadata entities = Parse();
        Write(entities);
    }

    protected abstract Metadata Parse();
}
