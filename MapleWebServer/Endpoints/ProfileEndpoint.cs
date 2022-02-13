using Maple2Storage.Types;

namespace MapleWebServer.Endpoints;

public static class ProfileEndpoint
{
    public static IResult Get(long characterId, string hash)
    {
        string fullPath = $"{Paths.DATA_DIR}/profiles/{characterId}/{hash}.png";
        if (!File.Exists(fullPath))
        {
            return Results.BadRequest();
        }

        FileStream profileImage = File.OpenRead(fullPath);
        return Results.File(profileImage, contentType: "image/png");
    }
}
