using Maple2Storage.Types;

namespace MapleWebServer.Endpoints;

public class GuildEndpoint
{
    public static IResult GetEmblem(int guildId, string guid)
    {
        string fullPath = $"{Paths.DATA_DIR}/guildmark/{guildId}/{guid}.png";
        if (!File.Exists(fullPath))
        {
            return Results.BadRequest();
        }

        FileStream guildEmblem = File.OpenRead(fullPath);
        return Results.File(guildEmblem, contentType: "image/png");
    }

    public static IResult GetBanner(int guildId, string guid)
    {
        string fullPath = $"{Paths.DATA_DIR}/guildmark/{guildId}/banner/{guid}.png";
        if (!File.Exists(fullPath))
        {
            return Results.BadRequest();
        }

        FileStream guildEmblem = File.OpenRead(fullPath);
        return Results.File(guildEmblem, contentType: "image/png");
    }
}
