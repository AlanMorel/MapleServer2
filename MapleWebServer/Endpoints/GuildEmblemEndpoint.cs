﻿using Maple2Storage.Types;

namespace MapleWebServer.Endpoints;

public class GuildEmblemEndpoint
{
    public static IResult Get(int guildId, string guid)
    {
        string fullPath = $"{Paths.DATA_DIR}/guildmark/{guildId}/{guid}.png";
        if (!File.Exists(fullPath))
        {
            return Results.BadRequest();
        }

        FileStream profileImage = File.OpenRead(fullPath);
        return Results.File(profileImage, contentType: "image/png");
    }
}
