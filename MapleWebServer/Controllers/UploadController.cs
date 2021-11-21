using System.Text;
using Maple2Storage.Types;
using MapleServer2.Database;
using MapleServer2.Types;
using MapleWebServer.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MapleWebServer.Controllers;

[ApiController]
public class UploadController : ControllerBase
{
    [Route("urq.aspx")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostAsync()
    {
        Stream bodyStream = Request.Body;
        if (bodyStream == null)
        {
            return BadRequest();
        }

        MemoryStream memoryStream = await CopyStream(bodyStream);
        byte[] array = memoryStream.ToArray();

        byte[] flagA = array.Take(4).ToArray();
        byte[] modeBytes = array.Skip(4).Take(4).ToArray();

        byte[] accountIdBytes = array.Skip(8).Take(8).ToArray();
        byte[] characterIdBytes = array.Skip(16).Take(8).ToArray();
        byte[] itemUidBytes = array.Skip(24).Take(8).ToArray();
        byte[] itemIdBytes = array.Skip(32).Take(4).ToArray();

        byte[] unkFlagBytes = array.Skip(40).Take(4).ToArray();

        byte[] fileBytes = array.Skip(48).ToArray();

        long accountId = (long) BitConverter.ToUInt64(accountIdBytes, 0);
        long characterId = (long) BitConverter.ToUInt64(characterIdBytes, 0);
        int itemId = (int) BitConverter.ToUInt32(itemIdBytes, 0);
        long itemUid = (long) BitConverter.ToUInt64(itemUidBytes, 0);
        PostUGCMode mode = (PostUGCMode) BitConverter.ToUInt32(modeBytes, 0);

        return mode switch
        {
            PostUGCMode.ProfileAvatar => HandleProfileAvatar(fileBytes, characterId),
            PostUGCMode.Item => HandleItem(fileBytes, itemId, itemUid),
            PostUGCMode.ItemIcon => HandleItemIcon(fileBytes, itemId, itemUid),
            _ => BadRequest(),
        };
    }

    private IActionResult HandleItemIcon(byte[] fileBytes, int itemId, long itemUid)
    {
        string filePath = $"{Paths.DATA_DIR}/itemicon/{itemId}/";
        Directory.CreateDirectory(filePath);

        Item item = DatabaseManager.Items.FindByUGCUid(itemUid);
        if (item is null)
        {
            return BadRequest();
        }

        System.IO.File.WriteAllBytes($"{filePath}/{item.UGC.Guid}-{itemUid}.png", fileBytes);
        return Ok($"0,itemicon/ms2/01/{itemId}/{item.UGC.Guid}-{itemUid}.png");
    }

    private IActionResult HandleItem(byte[] fileBytes, int itemId, long itemUid)
    {
        string filePath = $"{Paths.DATA_DIR}/item/{itemId}/";
        Directory.CreateDirectory(filePath);

        Item item = DatabaseManager.Items.FindByUGCUid(itemUid);
        if (item is null)
        {
            return BadRequest();
        }

        string url = $"item/ms2/01/{itemId}/{item.UGC.Guid}-{itemUid}.m2u";
        item.UGC.Url = url;
        DatabaseManager.UGC.Update(item.UGC);

        System.IO.File.WriteAllBytes($"{filePath}/{item.UGC.Guid}-{itemUid}.m2u", fileBytes);
        return Ok($"0,{url}");
    }

    private IActionResult HandleProfileAvatar(byte[] fileBytes, long characterId)
    {
        string filePath = $"{Paths.DATA_DIR}/profiles/{characterId}/";
        Directory.CreateDirectory(filePath);

        // Adding timestamp to the file name to prevent caching, client doesn't refresh the image if the url is already cached
        string fileHash = CreateMD5(Encoding.UTF8.GetString(fileBytes) + DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        // Deleting old files in the character folder
        DirectoryInfo di = new(filePath);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        System.IO.File.WriteAllBytes($"{filePath}/{fileHash}.png", fileBytes);
        return Ok($"0,data/profiles/avatar/{characterId}/{fileHash}.png");
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

    private static string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
