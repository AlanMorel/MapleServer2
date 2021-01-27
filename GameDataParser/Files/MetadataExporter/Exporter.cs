namespace GameDataParser.Files
{
    public abstract class Exporter<Metadata> : MetadataExporter
    {
        protected MetadataResources resources;

        public Exporter(MetadataResources resources, string slug) : base(slug)
        {
            this.resources = resources;
        }

        protected override void Serialize()
        {
            Metadata entities = this.Parse();
            this.Write<Metadata>(entities);
        }

        protected abstract Metadata Parse();
    }
}
