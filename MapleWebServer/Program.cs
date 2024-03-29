﻿using Maple2Storage.Tools;
using Maple2Storage.Types;
using MapleWebServer.Endpoints;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

string dotenv = Path.Combine(Paths.SOLUTION_DIR, ".env");
if (!File.Exists(dotenv))
{
    throw new FileNotFoundException(".env file not found!");
}

DotEnv.Load(dotenv);

const string ConsoleOutputTemplate = "[{@t:HH:mm:ss}] [{@l:u3}]" +
                                     "{#if SourceContext is not null} {Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)}:{#end} {@m}\n{@x}";
const string FileOutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] {SourceContext:l}: {Message:lj}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(new ExpressionTemplate(ConsoleOutputTemplate, theme: TemplateTheme.Literate))
    .WriteTo.File($"{Paths.SOLUTION_DIR}/Logs/MapleWebServer/LOG-.txt",
        rollingInterval: RollingInterval.Day, outputTemplate: FileOutputTemplate)
    .CreateLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

// Register Serilog
builder.Logging.AddSerilog(Log.Logger);

WebApplication app = builder.Build();

app.MapGet("/item/ms2/01/{itemId}/{uuid}.m2u", ItemEndpoint.Get);
app.MapGet("/itemicon/ms2/01/{itemId}/{uuid}.png", ItemIconEndpoint.Get);
app.MapGet("/guildmark/ms2/01/{guildId}/{guid}.png", GuildEndpoint.GetEmblem);
app.MapGet("/guildmark/ms2/01/{guildId}/banner/{guid}.png", GuildEndpoint.GetBanner);
app.MapGet("/data/profiles/avatar/{characterId}/{hash}.png", ProfileEndpoint.Get);
app.MapGet("/banner/ms2/01/{bannerId}/{fileHash}.m2u", BannerEndpoint.Get);

app.MapPost("/urq.aspx", UploadEndpoint.Post);

app.Run($"http://*:{Environment.GetEnvironmentVariable("WEB_PORT")}");
