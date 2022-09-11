using DijkstraAlgorithm.Graphing;
using DijkstraAlgorithm.Pathing;
using Path = DijkstraAlgorithm.Pathing.Path;

namespace MapleServer2.Data.Static;

public static class WorldMapGraphStorage
{
    private static Graph WorldMapGraph { get; set; } = null!;

    public static void Init()
    {
        GraphBuilder builder = new();

        builder.AddNode("63000011").AddNode("63000006").AddNode("63000012").AddNode("2000487").AddNode("2000496").AddNode("2000062")
            .AddNode("2000114").AddNode("2000115").AddNode("2000116").AddNode("2000117").AddNode("2000118").AddNode("2000001").AddNode("2000107")
            .AddNode("2000119").AddNode("2000083").AddNode("2000092").AddNode("2000084").AddNode("2000134").AddNode("2000136").AddNode("2000043")
            .AddNode("2000017").AddNode("2000014").AddNode("2000064").AddNode("2000137").AddNode("2000041").AddNode("2000135").AddNode("2000145")
            .AddNode("2000146").AddNode("2000402").AddNode("2000266").AddNode("2000265").AddNode("2000279").AddNode("2000162").AddNode("2000269")
            .AddNode("2000271").AddNode("2000272").AddNode("2000270").AddNode("2000273").AddNode("2000274").AddNode("2000275").AddNode("2000276")
            .AddNode("2000187").AddNode("2000277").AddNode("2000163").AddNode("2000167").AddNode("2000148").AddNode("2000216").AddNode("2000100")
            .AddNode("2000164").AddNode("2000147").AddNode("2000139").AddNode("2000110").AddNode("2000086").AddNode("2000111").AddNode("2000085")
            .AddNode("2000059").AddNode("2000003").AddNode("2000130").AddNode("2000002").AddNode("2000087").AddNode("2000088").AddNode("2000006")
            .AddNode("2000089").AddNode("2000101").AddNode("2000023").AddNode("2000005").AddNode("2000112").AddNode("2000129").AddNode("2000113")
            .AddNode("2000042").AddNode("2000182").AddNode("2000183").AddNode("2000063").AddNode("2000067").AddNode("2000102").AddNode("2000181")
            .AddNode("2000180").AddNode("2000131").AddNode("2000132").AddNode("2000108").AddNode("2000278").AddNode("2000214").AddNode("2000172")
            .AddNode("2000053").AddNode("2000090").AddNode("2000103").AddNode("2000060").AddNode("2000004").AddNode("2000024").AddNode("2000215")
            .AddNode("2000022").AddNode("2000048").AddNode("2000010").AddNode("2000267").AddNode("2000051").AddNode("2000050").AddNode("2000046")
            .AddNode("2000095").AddNode("2000096").AddNode("2000128").AddNode("2000045").AddNode("2000039").AddNode("2000056").AddNode("2000054")
            .AddNode("2000217").AddNode("2000168").AddNode("2000049").AddNode("2000169").AddNode("2000170").AddNode("2000094").AddNode("2000133")
            .AddNode("2000061").AddNode("2000097").AddNode("2000093").AddNode("2000044").AddNode("2000076").AddNode("2000104").AddNode("2000058")
            .AddNode("2000173").AddNode("2000174").AddNode("2000256").AddNode("2000175").AddNode("2000040").AddNode("2000011").AddNode("2000268")
            .AddNode("2000200").AddNode("2000192").AddNode("2000105").AddNode("2000203").AddNode("2000143").AddNode("2000057").AddNode("2000091")
            .AddNode("2000186").AddNode("2000185").AddNode("2000007").AddNode("2000009").AddNode("2000021").AddNode("2000038").AddNode("2000015")
            .AddNode("2000013").AddNode("2000166").AddNode("2000184").AddNode("2000231").AddNode("2000018").AddNode("2000255").AddNode("2000280")
            .AddNode("2000262").AddNode("2000259").AddNode("2000258").AddNode("2000257").AddNode("2000142").AddNode("2000165").AddNode("2000008")
            .AddNode("2000124").AddNode("2000138").AddNode("2000036").AddNode("2000377").AddNode("3000134").AddNode("3000146").AddNode("3000014")
            .AddNode("3000135").AddNode("3000136").AddNode("3000145").AddNode("3000059").AddNode("3000022").AddNode("3000130").AddNode("3000003")
            .AddNode("3000057").AddNode("3000143").AddNode("3000118").AddNode("3000117").AddNode("3000115").AddNode("3000165").AddNode("3000185")
            .AddNode("3000007").AddNode("3000009").AddNode("3000089").AddNode("3000006").AddNode("3000101").AddNode("3000024").AddNode("3000060")
            .AddNode("3000103").AddNode("3000090").AddNode("3000004").AddNode("3000043").AddNode("3000064").AddNode("3000092").AddNode("2010002")
            .AddNode("2010084").AddNode("2010043").AddNode("2010083").AddNode("2010020").AddNode("2010016").AddNode("2010009").AddNode("2010039")
            .AddNode("2010038").AddNode("2010036").AddNode("2010063").AddNode("2010023").AddNode("2010033").AddNode("2010047").AddNode("2010027")
            .AddNode("2010028").AddNode("2010030").AddNode("2010034").AddNode("2010029").AddNode("2010026").AddNode("2010062").AddNode("2010010")
            .AddNode("2010014").AddNode("2010019").AddNode("2010012").AddNode("2010022").AddNode("2010011").AddNode("2000176").AddNode("2000263")
            .AddNode("2000264").AddNode("2000261").AddNode("2000260").AddNode("2000411").AddNode("2000412").AddNode("2000413").AddNode("2000414")
            .AddNode("2000415").AddNode("2000171").AddNode("2000235").AddNode("2000236").AddNode("2000416").AddNode("2000417").AddNode("2000418")
            .AddNode("2020029").AddNode("2020041").AddNode("2020030").AddNode("2020031").AddNode("2020006").AddNode("2020004").AddNode("2020001")
            .AddNode("2020013").AddNode("2020032").AddNode("2020010").AddNode("2020034").AddNode("2020051").AddNode("2020016").AddNode("2020017")
            .AddNode("2020033").AddNode("2020003").AddNode("2020014").AddNode("2020009").AddNode("2020008").AddNode("2020018").AddNode("2020035")
            .AddNode("2020036").AddNode("2020037").AddNode("2000424").AddNode("2000425").AddNode("2000423");

        // Victoria island
        builder.AddBidirectionalLink("2000062", "2000114", 1);
        builder.AddBidirectionalLink("2000114", "2000115", 1);
        builder.AddBidirectionalLink("2000115", "2000116", 1);
        builder.AddBidirectionalLink("2000116", "2000117", 1);
        builder.AddBidirectionalLink("2000117", "2000118", 1);
        builder.AddBidirectionalLink("2000118", "2000001", 1);

        builder.AddBidirectionalLink("2000001", "2000487", 1);
        builder.AddBidirectionalLink("2000001", "2000002", 1);
        builder.AddBidirectionalLink("2000001", "2000064", 1);
        builder.AddBidirectionalLink("2000001", "2000119", 1);
        builder.AddBidirectionalLink("2000001", "2000107", 1);
        builder.AddBidirectionalLink("2020004", "2020001", 1);

        builder.AddBidirectionalLink("2000107", "2000119", 1);

        builder.AddBidirectionalLink("2000014", "2000119", 1);
        builder.AddBidirectionalLink("2000014", "2000134", 1);
        builder.AddBidirectionalLink("2000014", "2000092", 1);

        builder.AddBidirectionalLink("2000134", "2000092", 1);

        builder.AddBidirectionalLink("2000092", "2000135", 1);
        builder.AddBidirectionalLink("2000092", "2000084", 1);
        builder.AddBidirectionalLink("2000092", "2000083", 1);
        builder.AddBidirectionalLink("2000092", "2000119", 1);

        builder.AddBidirectionalLink("2000135", "2000136", 1);

        builder.AddBidirectionalLink("2000136", "2000145", 1);
        builder.AddBidirectionalLink("2000136", "2000266", 1);
        builder.AddBidirectionalLink("2000136", "2000137", 1);

        builder.AddBidirectionalLink("2000146", "2000145", 1);
        builder.AddBidirectionalLink("2000146", "2000136", 1);
        builder.AddBidirectionalLink("2000146", "2000402", 1);

        builder.AddBidirectionalLink("2000402", "2000496", 1);

        builder.AddBidirectionalLink("2000266", "2000137", 1);
        builder.AddBidirectionalLink("2000266", "2000265", 1);
        builder.AddBidirectionalLink("2000266", "2000279", 1);

        builder.AddBidirectionalLink("2000279", "2000265", 1);
        builder.AddBidirectionalLink("2000279", "2000162", 1);

        builder.AddBidirectionalLink("2000162", "2000269", 1);
        builder.AddBidirectionalLink("2000162", "2000271", 1);
        builder.AddBidirectionalLink("2000162", "2000265", 1);

        builder.AddBidirectionalLink("2000271", "2000272", 1);
        builder.AddBidirectionalLink("2000271", "2000269", 1);
        builder.AddBidirectionalLink("2000271", "2000270", 1);
        builder.AddBidirectionalLink("2000271", "2000163", 1);

        builder.AddBidirectionalLink("2000270", "2000273", 1);

        builder.AddBidirectionalLink("2000273", "2000274", 1);

        builder.AddBidirectionalLink("2000274", "2000275", 1);
        builder.AddBidirectionalLink("2000274", "2000276", 1);

        builder.AddBidirectionalLink("2000276", "2000187", 1);
        builder.AddBidirectionalLink("2000276", "2000163", 1);

        builder.AddBidirectionalLink("2000187", "2000277", 1);
        builder.AddBidirectionalLink("2000187", "2000163", 1);
        builder.AddBidirectionalLink("2000277", "2000163", 1);

        builder.AddBidirectionalLink("2000269", "2000163", 1);

        builder.AddBidirectionalLink("2000265", "2000148", 1);
        builder.AddBidirectionalLink("2000265", "2000167", 1);

        builder.AddBidirectionalLink("2000138", "2000137", 1);
        builder.AddBidirectionalLink("2000138", "2000139", 1);

        builder.AddBidirectionalLink("2000139", "2000148", 1);
        builder.AddBidirectionalLink("2000139", "2000147", 1);
        builder.AddBidirectionalLink("2000139", "2000110", 1);

        builder.AddBidirectionalLink("2000110", "2000084", 1);
        builder.AddBidirectionalLink("2000110", "2000086", 1);

        builder.AddBidirectionalLink("2000083", "2000041", 1);
        builder.AddBidirectionalLink("2000083", "2000084", 1);

        builder.AddBidirectionalLink("2000017", "2000064", 1);
        builder.AddBidirectionalLink("2000017", "2000041", 1);
        builder.AddBidirectionalLink("2000017", "2000043", 1);

        builder.AddBidirectionalLink("2000111", "2000086", 1);
        builder.AddBidirectionalLink("2000111", "2000085", 1);

        builder.AddBidirectionalLink("2000085", "2000043", 1);
        builder.AddBidirectionalLink("2000085", "2000059", 1);
        builder.AddBidirectionalLink("2000085", "2000054", 1);

        builder.AddBidirectionalLink("2000059", "2000043", 1);
        builder.AddBidirectionalLink("2000059", "2000054", 1);
        builder.AddBidirectionalLink("2000059", "2000003", 1);

        builder.AddBidirectionalLink("2000100", "2000148", 1);
        builder.AddBidirectionalLink("2000100", "2000167", 1);
        builder.AddBidirectionalLink("2000100", "2000164", 1);
        builder.AddBidirectionalLink("2000100", "2000216", 1);
        builder.AddBidirectionalLink("2000100", "2000166", 1);
        builder.AddBidirectionalLink("2000100", "2000147", 1);
        builder.AddBidirectionalLink("2000100", "2000165", 1);

        builder.AddBidirectionalLink("2000216", "2000164", 1);
        builder.AddBidirectionalLink("2000216", "2000167", 1);

        builder.AddBidirectionalLink("2000147", "2000091", 1);
        builder.AddBidirectionalLink("2000147", "2000165", 1);

        builder.AddBidirectionalLink("2000166", "2000165", 1);
        builder.AddBidirectionalLink("2000166", "2000142", 1);
        builder.AddBidirectionalLink("2000166", "2000184", 1);
        builder.AddBidirectionalLink("2000166", "2000231", 1);

        builder.AddBidirectionalLink("2000231", "2000184", 1);
        builder.AddBidirectionalLink("2000231", "2000255", 1);
        builder.AddBidirectionalLink("2000231", "2000280", 1);
        builder.AddBidirectionalLink("2000231", "2000018", 1);

        builder.AddBidirectionalLink("2000262", "2000018", 1);
        builder.AddBidirectionalLink("2000262", "2000280", 1);

        builder.AddBidirectionalLink("2000280", "2000255", 1);
        builder.AddBidirectionalLink("2000280", "2000257", 1);

        builder.AddBidirectionalLink("2000258", "2000259", 1);
        builder.AddBidirectionalLink("2000258", "2000257", 1);

        builder.AddBidirectionalLink("2000255", "2000184", 1);
        builder.AddBidirectionalLink("2000255", "2000257", 1);
        builder.AddBidirectionalLink("2000255", "2000142", 1);

        builder.AddBidirectionalLink("2000142", "2000184", 1);
        builder.AddBidirectionalLink("2000142", "2000165", 1);
        builder.AddBidirectionalLink("2000142", "2000008", 1);

        builder.AddBidirectionalLink("2000185", "2000165", 1);
        builder.AddBidirectionalLink("2000185", "2000186", 1);
        builder.AddBidirectionalLink("2000185", "2000007", 1);

        builder.AddBidirectionalLink("2000007", "2000008", 1);
        builder.AddBidirectionalLink("2000007", "2000009", 1);
        builder.AddBidirectionalLink("2000007", "2000021", 1);

        builder.AddBidirectionalLink("2000038", "2000021", 1);
        builder.AddBidirectionalLink("2000038", "2000009", 1);
        builder.AddBidirectionalLink("2000009", "2000021", 1);

        builder.AddBidirectionalLink("2000015", "2000021", 1);
        builder.AddBidirectionalLink("2000015", "2000013", 1);

        builder.AddBidirectionalLink("2000105", "2000013", 1);

        builder.AddBidirectionalLink("2000076", "2000054", 1);
        builder.AddBidirectionalLink("2000076", "2000104", 1);
        builder.AddBidirectionalLink("2000076", "2000105", 1);
        builder.AddBidirectionalLink("2000076", "2000203", 1);
        builder.AddBidirectionalLink("2000076", "2000143", 1);

        builder.AddBidirectionalLink("2000143", "2000203", 1);
        builder.AddBidirectionalLink("2000143", "2000057", 1);

        builder.AddBidirectionalLink("2000057", "2000186", 1);
        builder.AddBidirectionalLink("2000057", "2000091", 1);

        builder.AddBidirectionalLink("2000002", "2000087", 1);
        builder.AddBidirectionalLink("2000002", "2000088", 1);

        builder.AddBidirectionalLink("2000088", "2000087", 1);
        builder.AddBidirectionalLink("2000088", "2000006", 1);

        builder.AddBidirectionalLink("2000006", "2000089", 1);
        builder.AddBidirectionalLink("2000006", "2000036", 1);
        builder.AddBidirectionalLink("2000006", "2000101", 1);

        builder.AddBidirectionalLink("2000023", "2000129", 1);
        builder.AddBidirectionalLink("2000023", "2000005", 1);
        builder.AddBidirectionalLink("2000023", "2000036", 1);
        builder.AddBidirectionalLink("2000023", "2000101", 1);
        builder.AddBidirectionalLink("2000023", "2000102", 1);
        builder.AddBidirectionalLink("2000023", "2000112", 1);

        builder.AddBidirectionalLink("2000036", "2000101", 1);
        builder.AddBidirectionalLink("2000036", "2000005", 1);

        builder.AddBidirectionalLink("2000005", "2000129", 1);
        builder.AddBidirectionalLink("2000005", "2000113", 1);
        builder.AddBidirectionalLink("2000005", "2000042", 1);

        builder.AddBidirectionalLink("2000042", "2000113", 1);
        builder.AddBidirectionalLink("2000042", "2000063", 1);
        builder.AddBidirectionalLink("2000042", "2000182", 1);
        builder.AddBidirectionalLink("2000042", "2000183", 1);
        builder.AddBidirectionalLink("2000182", "2000183", 1);

        builder.AddBidirectionalLink("2000113", "2000129", 1);
        builder.AddBidirectionalLink("2000113", "2000063", 1);

        builder.AddBidirectionalLink("2000067", "2000063", 1);
        builder.AddBidirectionalLink("2000067", "2000377", 1);

        builder.AddBidirectionalLink("2000129", "2000112", 1);
        builder.AddBidirectionalLink("2000181", "2000180", 1);
        builder.AddBidirectionalLink("2000181", "2000129", 1);

        builder.AddBidirectionalLink("2000131", "2000180", 1);
        builder.AddBidirectionalLink("2000131", "2000132", 1);

        builder.AddBidirectionalLink("2000132", "2000108", 1);
        builder.AddBidirectionalLink("2000132", "2000278", 1);

        builder.AddBidirectionalLink("2000214", "2000172", 1);
        builder.AddBidirectionalLink("2000214", "2000278", 1);

        builder.AddBidirectionalLink("2000172", "2000053", 1);
        builder.AddBidirectionalLink("2000172", "2000102", 1);
        builder.AddBidirectionalLink("2000172", "2000090", 1);

        builder.AddBidirectionalLink("2000053", "2000090", 1);
        builder.AddBidirectionalLink("2000053", "2000102", 1);
        builder.AddBidirectionalLink("2000102", "2000112", 1);

        builder.AddBidirectionalLink("2000103", "2000090", 1);
        builder.AddBidirectionalLink("2000103", "2000060", 1);
        builder.AddBidirectionalLink("2000103", "2000048", 1);

        builder.AddBidirectionalLink("2000060", "2000048", 1);
        builder.AddBidirectionalLink("2000060", "2000004", 1);

        builder.AddBidirectionalLink("2000024", "2000004", 1);
        builder.AddBidirectionalLink("2000024", "2000101", 1);
        builder.AddBidirectionalLink("2000024", "2000215", 1);

        builder.AddBidirectionalLink("2000022", "2000215", 1);
        builder.AddBidirectionalLink("2000022", "2000130", 1);
        builder.AddBidirectionalLink("2000022", "2000128", 1);

        builder.AddBidirectionalLink("2000130", "2000003", 1);

        builder.AddBidirectionalLink("2000128", "2000096", 1);

        builder.AddBidirectionalLink("2000095", "2000096", 1);
        builder.AddBidirectionalLink("2000095", "2000046", 1);

        builder.AddBidirectionalLink("2000046", "2000050", 1);
        builder.AddBidirectionalLink("2000046", "2000045", 1);

        builder.AddBidirectionalLink("2000039", "2000045", 1);
        builder.AddBidirectionalLink("2000039", "2000056", 1);

        builder.AddBidirectionalLink("2000056", "2000054", 1);
        builder.AddBidirectionalLink("2000056", "2000104", 1);

        builder.AddBidirectionalLink("2000104", "2000058", 1);

        builder.AddBidirectionalLink("2000173", "2000058", 1);
        builder.AddBidirectionalLink("2000173", "2000174", 1);
        builder.AddBidirectionalLink("2000174", "2000256", 1);

        builder.AddBidirectionalLink("2000175", "2000256", 1);
        builder.AddBidirectionalLink("2000175", "2000040", 1);
        builder.AddBidirectionalLink("2000175", "2000192", 1);

        builder.AddBidirectionalLink("2000011", "2000040", 1);
        builder.AddBidirectionalLink("2000011", "2000268", 1);

        builder.AddBidirectionalLink("2000268", "2000200", 1);
        builder.AddBidirectionalLink("2000268", "2000235", 1);

        builder.AddBidirectionalLink("2000236", "2000235", 1);
        builder.AddBidirectionalLink("2000236", "2000416", 1);

        builder.AddBidirectionalLink("2000417", "2000416", 1);
        builder.AddBidirectionalLink("2000417", "2000418", 1);


        builder.AddBidirectionalLink("2000176", "2000192", 1);
        builder.AddBidirectionalLink("2000176", "2000261", 1);

        builder.AddBidirectionalLink("2000260", "2000261", 1);
        builder.AddBidirectionalLink("2000260", "2000263", 1);
        builder.AddBidirectionalLink("2000260", "2000264", 1);

        builder.AddBidirectionalLink("2000264", "2000263", 1);
        builder.AddBidirectionalLink("2000264", "2000415", 1);

        builder.AddBidirectionalLink("2000414", "2000415", 1);
        builder.AddBidirectionalLink("2000414", "2000413", 1);

        builder.AddBidirectionalLink("2000412", "2000413", 1);
        builder.AddBidirectionalLink("2000412", "2000411", 1);

        builder.AddBidirectionalLink("2000044", "2000411", 1);
        builder.AddBidirectionalLink("2000044", "2000171", 1);
        builder.AddBidirectionalLink("2000044", "2000093", 1);

        builder.AddBidirectionalLink("2000170", "2000171", 1);
        builder.AddBidirectionalLink("2000170", "2000169", 1);

        builder.AddBidirectionalLink("2000097", "2000093", 1);
        builder.AddBidirectionalLink("2000097", "2000061", 1);

        builder.AddBidirectionalLink("2000061", "2000133", 1);
        builder.AddBidirectionalLink("2000061", "2000094", 1);

        builder.AddBidirectionalLink("2000094", "2000168", 1);
        builder.AddBidirectionalLink("2000094", "2000049", 1);
        builder.AddBidirectionalLink("2000049", "2000169", 1);

        builder.AddBidirectionalLink("2000051", "2000049", 1);
        builder.AddBidirectionalLink("2000051", "2000168", 1);
        builder.AddBidirectionalLink("2000051", "2000217", 1);
        builder.AddBidirectionalLink("2000051", "2000267", 1);
        builder.AddBidirectionalLink("2000051", "2000010", 1);
        builder.AddBidirectionalLink("2000051", "2000050", 1);

        builder.AddBidirectionalLink("2000010", "2000050", 1);
        builder.AddBidirectionalLink("2000010", "2000048", 1);
        builder.AddBidirectionalLink("2000048", "2000267", 1);

        // Karkar island
        builder.AddBidirectionalLink("2010002", "2010010", 1);
        builder.AddBidirectionalLink("2010002", "2010084", 1);
        builder.AddBidirectionalLink("2010002", "2010009", 1);

        builder.AddBidirectionalLink("2010014", "2010019", 1);
        builder.AddBidirectionalLink("2010014", "2010010", 1);

        builder.AddBidirectionalLink("2010043", "2010084", 1);

        builder.AddBidirectionalLink("2010012", "2010009", 1);
        builder.AddBidirectionalLink("2010012", "2010039", 1);

        builder.AddBidirectionalLink("2010038", "2010039", 1);
        builder.AddBidirectionalLink("2010038", "2010036", 1);
        builder.AddBidirectionalLink("2010063", "2010036", 1);
        builder.AddBidirectionalLink("2010063", "2010023", 1);
        builder.AddBidirectionalLink("2010023", "2010033", 1);
        builder.AddBidirectionalLink("2010023", "2010036", 1);

        // Kritias
        builder.AddBidirectionalLink("2020041", "2020004", 1);
        builder.AddBidirectionalLink("2020041", "2020031", 1);
        builder.AddBidirectionalLink("2020041", "2020032", 1);
        builder.AddBidirectionalLink("2020041", "2020013", 1);

        builder.AddBidirectionalLink("2020031", "2020030", 1);
        builder.AddBidirectionalLink("2020030", "2020029", 1);
        builder.AddBidirectionalLink("2020032", "2020010", 1);

        builder.AddBidirectionalLink("2020051", "2020013", 1);
        builder.AddBidirectionalLink("2020051", "2020032", 1);
        builder.AddBidirectionalLink("2020051", "2020003", 1);

        builder.AddBidirectionalLink("2020008", "2020003", 1);
        builder.AddBidirectionalLink("2020008", "2020018", 1);
        builder.AddBidirectionalLink("2020008", "2020035", 1);

        builder.AddBidirectionalLink("2020033", "2020018", 1);
        builder.AddBidirectionalLink("2020033", "2020017", 1);
        builder.AddBidirectionalLink("2020016", "2020017", 1);
        builder.AddBidirectionalLink("2020016", "2020013", 1);

        WorldMapGraph = builder.Build();
    }

    /// <summary>
    /// Returns if the pathfinder is able to find a path to the given destination.
    /// </summary>
    /// <returns>The count of maps between the origin and destination.</returns>
    public static bool CanPathFind(string mapOrigin, string mapDestination, out int mapCount)
    {
        mapCount = 0;
        PathFinder pathFinder = new(WorldMapGraph);
        Node? originNode = WorldMapGraph.Nodes.FirstOrDefault(x => x.Id == mapOrigin);
        Node? destinationNode = WorldMapGraph.Nodes.FirstOrDefault(x => x.Id == mapDestination);

        if (originNode == default && destinationNode == default)
        {
            return false;
        }

        Path path = pathFinder.FindShortestPath(originNode, destinationNode);
        if (path == null)
        {
            return false;
        }

        mapCount = path.Segments.Count;
        return true;
    }
}
