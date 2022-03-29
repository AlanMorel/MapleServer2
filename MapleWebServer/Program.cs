using Maple2Storage.Tools;
using Maple2Storage.Types;
using MapleWebServer.Endpoints;
using Serilog;

string dotenv = Path.Combine(Paths.SOLUTION_DIR, ".env");
if (!File.Exists(dotenv))
{
    throw new FileNotFoundException(".env file not found!");
}

DotEnv.Load(dotenv);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/WebServerLogs.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

// Register Serilog
builder.Logging.AddSerilog(Log.Logger);

WebApplication app = builder.Build();

app.MapGet("/item/ms2/01/{itemId}/{uuid}.m2u", ItemEndpoint.Get);
app.MapGet("/itemicon/ms2/01/{itemId}/{uuid}.png", ItemIconEndpoint.Get);
app.MapGet("/data/profiles/avatar/{characterId}/{hash}.png", ProfileEndpoint.Get);

app.MapPost("/urq.aspx", UploadEndpoint.Post);

app.Run($"http://*:{Environment.GetEnvironmentVariable("WEB_PORT")}");
