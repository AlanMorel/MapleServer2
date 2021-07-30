using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameDataParser.Files;
using GameDataParser.Parsers;

namespace GameDataParser
{
    internal static class Program
    {
        static async Task Main()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Create Resources folders if they don't exist
            Directory.CreateDirectory(Paths.INPUT);
            Directory.CreateDirectory(Paths.OUTPUT);

            Spinner spinner = new Spinner();
            spinner.Start();

            MetadataResources resources = new MetadataResources();

            IEnumerable<MetadataExporter> exporters = new List<MetadataExporter>()
            {
                new AnimationParser(resources),
                new DungeonParser(resources),
                new ItemParser(resources),
                new ItemOptionConstantParser(resources),
                new ItemOptionStaticParser(resources),
                new ItemOptionRandomParser(resources),
                new ItemOptionRangeParser(resources),
                new ItemSocketParser(resources),
                new ItemGemstoneUpgradeParser(resources),
                new MapEntityParser(resources),
                new MapParser(resources),
                new SkillParser(resources),
                new InsigniaParser(resources),
                new ExpParser(resources),
                new QuestParser(resources),
                new ScriptParser(resources),
                new GuildContributionParser(resources),
                new GuildBuffParser(resources),
                new GuildPropertyParser(resources),
                new GuildServiceParser(resources),
                new GuildHouseParser(resources),
                new PrestigeParser(resources),
                new TrophyParser(resources),
                new RecipeParser(resources),
                new MasteryParser(resources),
                new NpcParser(resources),
                new ChatStickerParser(resources),
                new ItemExchangeScrollParser(resources),
                new MasteryFactorParser(resources),
                new PremiumClubPackageParser(resources),
                new PremiumClubDailyBenefitParser(resources),
                new InstrumentInfoParser(resources),
                new InstrumentCategoryInfoParser(resources),
                new BeautyParser(),
                new ColorPaletteParser(resources),
                new GachaParser(resources),
                new ItemExtractionParser(resources),
                new FishParser(resources),
                new FishingSpotParser(resources),
                new FishingRodParser(resources),
                new UGCMapParser(resources),
                new FurnishingShopParser(resources),
                new HomeTemplateParser(resources)
            };

            IEnumerable<Task> tasks = exporters.Select(exporter => Task.Run(() => exporter.Export()));

            await Task.WhenAll(tasks);

            spinner.Stop();
            TimeSpan runtime = spinner.GetRuntime();

            Console.WriteLine($"\rExporting finished in {runtime.Minutes} minutes and {runtime.Seconds} seconds");
        }
    }
}
