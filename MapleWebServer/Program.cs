using Maple2Storage.Tools;
using Maple2Storage.Types;
using MapleWebServer.Endpoints;

string dotenv = Path.Combine(Paths.SOLUTION_DIR, ".env");
if (!File.Exists(dotenv))
{
    throw new FileNotFoundException(".env file not found!");
}

DotEnv.Load(dotenv);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

app.MapGet("/item/ms2/01/{itemId}/{uuid}.m2u", ItemEndpoint.Get);
app.MapGet("/itemicon/ms2/01/{itemId}/{uuid}.png", ItemIconEndpoint.Get);
app.MapGet("/data/profiles/avatar/{characterId}/{hash}.png", ProfileEndpoint.Get);

app.MapPost("/urq.aspx", UploadEndpoint.Post);

app.Run($"http://*:{Environment.GetEnvironmentVariable("WEB_PORT")}");
