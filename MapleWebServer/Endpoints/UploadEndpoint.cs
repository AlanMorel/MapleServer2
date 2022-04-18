using Maple2Storage.Enums;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Types;
using Serilog;

namespace MapleWebServer.Endpoints;

public static class UploadEndpoint
{
    public static async Task<IResult> Post(HttpRequest request)
    {
        Stream bodyStream = request.Body;

        MemoryStream memoryStream = await CopyStream(bodyStream);
        if (memoryStream.Length == 0)
        {
            return Results.BadRequest();
        }

        PacketReader pReader = new(memoryStream.ToArray());

        int flagA = pReader.ReadInt();
        UGCType mode = (UGCType) pReader.ReadInt();
        long accountId = pReader.ReadLong();
        long characterId = pReader.ReadLong();
        long ugcUid = pReader.ReadLong();
        int id = pReader.ReadInt(); // item id, guild id, others?
        int flagB = pReader.ReadInt();
        long unk = pReader.ReadLong();

        byte[]? fileBytes = pReader.ReadBytes(pReader.Available);

        return mode switch
        {
            UGCType.ProfileAvatar => HandleProfileAvatar(fileBytes, characterId),
            UGCType.Item or UGCType.Furniture or UGCType.Mount => HandleItem(fileBytes, id, ugcUid),
            UGCType.ItemIcon => HandleItemIcon(fileBytes, id, ugcUid),
            UGCType.GuildEmblem => HandleGuildEmblem(fileBytes, ugcUid, id),
            UGCType.Banner => HandleBanner(fileBytes, ugcUid, id),
            UGCType.GuildBanner => HandleGuildBanner(fileBytes, ugcUid, id),
            _ => HandleUnknownMode(mode)
        };
    }

    private static IResult HandleItemIcon(byte[] fileBytes, int itemId, long ugcUid)
    {
        string filePath = $"{Paths.DATA_DIR}/itemicon/{itemId}/";
        Directory.CreateDirectory(filePath);

        UGC ugc = DatabaseManager.UGC.FindByUid(ugcUid);
        if (ugc is null)
        {
            Log.Logger.Error($"Could not find ugc with uid {ugcUid}");
            return Results.BadRequest();
        }

        File.WriteAllBytes($"{filePath}/{ugc.Guid}-{ugcUid}.png", fileBytes);
        return Results.Text($"0,itemicon/ms2/01/{itemId}/{ugc.Guid}-{ugcUid}.png");
    }

    private static IResult HandleItem(byte[] fileBytes, int itemId, long ugcUid)
    {
        string filePath = $"{Paths.DATA_DIR}/item/{itemId}/";
        Directory.CreateDirectory(filePath);

        UGC ugc = DatabaseManager.UGC.FindByUid(ugcUid);
        if (ugc is null)
        {
            Log.Logger.Error($"Could not find ugc with uid {ugcUid}");
            return Results.BadRequest();
        }

        string url = $"item/ms2/01/{itemId}/{ugc.Guid}-{ugcUid}.m2u";
        ugc.Url = url;
        DatabaseManager.UGC.Update(ugc);

        File.WriteAllBytes($"{filePath}/{ugc.Guid}-{ugcUid}.m2u", fileBytes);
        return Results.Text($"0,{url}");
    }

    private static IResult HandleProfileAvatar(byte[] fileBytes, long characterId)
    {
        string filePath = $"{Paths.DATA_DIR}/profiles/{characterId}/";
        Directory.CreateDirectory(filePath);

        string uniqueFileName = Guid.NewGuid().ToString();

        // Deleting old files in the character folder
        DirectoryInfo di = new(filePath);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        File.WriteAllBytes($"{filePath}/{uniqueFileName}.png", fileBytes);
        return Results.Text($"0,data/profiles/avatar/{characterId}/{uniqueFileName}.png");
    }

    private static IResult HandleGuildEmblem(byte[] fileBytes, long ugcUid, long guildId)
    {
        string filePath = $"{Paths.DATA_DIR}/guildmark/{guildId}/";
        Directory.CreateDirectory(filePath);

        UGC ugc = DatabaseManager.UGC.FindByUid(ugcUid);
        if (ugc is null)
        {
            Log.Logger.Error($"Could not find ugc with uid {ugcUid}");
            return Results.NotFound();
        }

        // Deleting old files in the guild folder
        DirectoryInfo di = new(filePath);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        string url = $"guildmark/ms2/01/{guildId}/{ugc.Guid}.png";
        ugc.Url = url;

        DatabaseManager.UGC.Update(ugc);

        File.WriteAllBytes($"{filePath}/{ugc.Guid}.png", fileBytes);
        return Results.Text($"0,{url}");
    }

    private static IResult HandleGuildBanner(byte[] fileBytes, long ugcUid, long guildId)
    {
        string filePath = $"{Paths.DATA_DIR}/guildmark/{guildId}/banner";
        Directory.CreateDirectory(filePath);

        UGC ugc = DatabaseManager.UGC.FindByUid(ugcUid);
        if (ugc is null)
        {
            Log.Logger.Error($"Could not find ugc with uid {ugcUid}");
            return Results.NotFound();
        }

        string url = $"guildmark/ms2/01/{guildId}/banner/{ugc.Guid}.png";
        ugc.Url = url;

        DatabaseManager.UGC.Update(ugc);

        File.WriteAllBytes($"{filePath}/{ugc.Guid}.png", fileBytes);
        return Results.Text($"0,{url}");
    }

    private static IResult HandleBanner(byte[] fileBytes, long ugcUid, int bannerId)
    {
        string filePath = $"{Paths.DATA_DIR}/banner/{bannerId}/";
        Directory.CreateDirectory(filePath);

        UGC ugc = DatabaseManager.UGC.FindByUid(ugcUid);
        if (ugc is null)
        {
            Log.Logger.Error($"Could not find ugc with uid {ugcUid}");
            return Results.BadRequest();
        }

        string url = $"banner/ms2/01/{bannerId}/{ugc.Guid}-{ugcUid}.m2u";
        ugc.Url = url;

        DatabaseManager.UGC.Update(ugc);

        File.WriteAllBytes($"{filePath}/{ugc.Guid}-{ugcUid}.m2u", fileBytes);
        return Results.Text($"0,{url}");
    }

    private static IResult HandleUnknownMode(UGCType mode)
    {
        Log.Logger.Warning("Unknown upload mode: {mode}", mode);
        return Results.BadRequest();
    }

    private static async Task<MemoryStream> CopyStream(Stream input)
    {
        MemoryStream output = new();
        byte[] buffer = new byte[16 * 1024];
        int read;
        while ((read = await input.ReadAsync(buffer)) > 0)
        {
            output.Write(buffer, 0, read);
        }

        return output;
    }
}
