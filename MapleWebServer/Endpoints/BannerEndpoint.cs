using Maple2Storage.Types;

namespace MapleWebServer.Endpoints;

public static class BannerEndpoint
{
    public static IResult Get(int bannerId, string fileHash)
    {
        string fullPath = $"{Paths.DATA_DIR}/banner/{bannerId}/{fileHash}.m2u";
        if (!File.Exists(fullPath))
        {
            return Results.BadRequest();
        }

        FileStream guildEmblem = File.OpenRead(fullPath);
        return Results.File(guildEmblem, contentType: "application/octet-stream");
    }
}
