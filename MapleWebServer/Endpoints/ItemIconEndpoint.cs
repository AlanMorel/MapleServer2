using Maple2Storage.Types;

namespace MapleWebServer.Endpoints;

public static class ItemIconEndpoint
{
    public static IResult Get(int itemId, string uuid)
    {
        string fullPath = $"{Paths.DATA_DIR}/itemicon/{itemId}/{uuid}.png";
        if (!File.Exists(fullPath))
        {
            return Results.BadRequest();
        }

        FileStream profileImage = File.OpenRead(fullPath);
        return Results.File(profileImage, contentType: "image/png");
    }
}
