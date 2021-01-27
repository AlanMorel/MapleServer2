namespace GameDataParser.Files
{
    public abstract class Exporter<Metadata> : MetadataExporter
    {
        protected MetadataResources resources;

        public Exporter(MetadataResources resources, string slug) : base(slug)
        {
            this.resources = resources;
        }

        protected override void serialize()
        {
            Metadata entities = this.parse();
            this.write<Metadata>(entities);
        }

        protected abstract Metadata parse();
    }
}
