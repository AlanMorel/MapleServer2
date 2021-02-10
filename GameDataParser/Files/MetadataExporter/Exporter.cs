namespace GameDataParser.Files
{
    public abstract class Exporter<Metadata> : MetadataExporter
    {
        protected MetadataResources Resources;

        public Exporter(MetadataResources resources, string slug) : base(slug)
        {
            Resources = resources;
        }

        protected override void Serialize()
        {
            Metadata entities = Parse();
            Write<Metadata>(entities);
        }

        protected abstract Metadata Parse();
    }
}
