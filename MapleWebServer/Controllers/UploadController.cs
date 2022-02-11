using System.Security.Cryptography;
using System.Text;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Types;
using MapleWebServer.Enums;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace MapleWebServer.Controllers;

public class UploadController : ControllerBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    [HttpPost("urq.aspx")]
    public async Task<IActionResult> PostAsync()
    {
        Stream bodyStream = Request.Body;

        MemoryStream memoryStream = await CopyStream(bodyStream);
        if (memoryStream.Length == 0)
        {
            return BadRequest();
        }

        PacketReader pReader = new(memoryStream.ToArray());

        int flagA = pReader.ReadInt();
        PostUgcMode mode = (PostUgcMode) pReader.ReadInt();
        long accountId = pReader.ReadLong();
        long characterId = pReader.ReadLong();
        long itemUid = pReader.ReadLong();
        int itemId = pReader.ReadInt();
        int flagB = pReader.ReadInt();
        pReader.Skip(8);

        byte[]? fileBytes = pReader.ReadBytes(pReader.Available);

        return mode switch
        {
            PostUgcMode.ProfileAvatar => HandleProfileAvatar(fileBytes, characterId),
            PostUgcMode.Item or PostUgcMode.Furnishing => HandleItem(fileBytes, itemId, itemUid),
            PostUgcMode.ItemIcon => HandleItemIcon(fileBytes, itemId, itemUid),
            _ => HandleUnknownMode(mode)
        };
    }

    private IActionResult HandleItemIcon(byte[] fileBytes, int itemId, long itemUid)
    {
        string filePath = $"{Paths.DATA_DIR}/itemicon/{itemId}/";
        Directory.CreateDirectory(filePath);

        Item item = DatabaseManager.Items.FindByUgcUid(itemUid);
        if (item is null)
        {
            return BadRequest();
        }

        System.IO.File.WriteAllBytes($"{filePath}/{item.Ugc.Guid}-{itemUid}.png", fileBytes);
        return Ok($"0,itemicon/ms2/01/{itemId}/{item.Ugc.Guid}-{itemUid}.png");
    }

    private IActionResult HandleItem(byte[] fileBytes, int itemId, long itemUid)
    {
        string filePath = $"{Paths.DATA_DIR}/item/{itemId}/";
        Directory.CreateDirectory(filePath);

        Item item = DatabaseManager.Items.FindByUgcUid(itemUid);
        if (item is null)
        {
            return BadRequest();
        }

        string url = $"item/ms2/01/{itemId}/{item.Ugc.Guid}-{itemUid}.m2u";
        item.Ugc.Url = url;
        DatabaseManager.Ugc.Update(item.Ugc);

        System.IO.File.WriteAllBytes($"{filePath}/{item.Ugc.Guid}-{itemUid}.m2u", fileBytes);
        return Ok($"0,{url}");
    }

    private IActionResult HandleProfileAvatar(byte[] fileBytes, long characterId)
    {
        string filePath = $"{Paths.DATA_DIR}/profiles/{characterId}/";
        Directory.CreateDirectory(filePath);

        // Adding timestamp to the file name to prevent caching, client doesn't refresh the image if the url is already cached
        string fileHash = CreateMd5(Encoding.UTF8.GetString(fileBytes) + DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        // Deleting old files in the character folder
        DirectoryInfo di = new(filePath);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        System.IO.File.WriteAllBytes($"{filePath}/{fileHash}.png", fileBytes);
        return Ok($"0,data/profiles/avatar/{characterId}/{fileHash}.png");
    }

    private IActionResult HandleUnknownMode(PostUgcMode mode)
    {
        Logger.Info($"Unknown UGC post mode: {mode}");
        return BadRequest();
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

    private static string CreateMd5(string input)
    {
        // Use input string to calculate MD5 hash
        using MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Convert the byte array to hexadecimal string
        StringBuilder sb = new();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }
}
