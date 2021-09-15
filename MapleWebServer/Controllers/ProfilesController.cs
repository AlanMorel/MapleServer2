using System.Text;
using MapleWebServer.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MapleWebServer.Controllers
{
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ILogger<ProfilesController> Logger;

        public ProfilesController(ILogger<ProfilesController> logger) => Logger = logger;

        [Route("data/profiles/avatar/{characterId}/{hash}.png")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStream))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string characterId, string hash)
        {
            string requestImagePath = HttpContext.Request.Path;
            string fullPath = $"{Paths.DATA}/profiles/{characterId}/{hash}.png".Replace("$", "");
            if (!System.IO.File.Exists(fullPath))
            {
                return BadRequest();
            }
            Response.Headers.Add("content-type", "image/png");
            FileStream profileImage = System.IO.File.OpenRead(fullPath);
            return Ok(profileImage);
        }

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
            long accountId = (long) BitConverter.ToUInt64(array.Skip(8).Take(8).ToArray(), 0);
            long characterId = (long) BitConverter.ToUInt64(array.Skip(16).Take(8).ToArray(), 0);

            string filePath = $"{Paths.DATA}/profiles/{characterId}/";
            Directory.CreateDirectory(filePath);

            byte[] fileBytes = array.Skip(48).ToArray();

            string md5Hash = CreateMD5(Encoding.UTF8.GetString(fileBytes));
            if (System.IO.File.Exists($"{filePath}/{md5Hash}.png"))
            {
                return Ok($"0,data/profiles/avatar/{characterId}/{md5Hash}.png");
            }

            System.IO.File.WriteAllBytes($"{filePath}/{md5Hash}.png", fileBytes);

            return Ok($"0,data/profiles/avatar/{characterId}/{md5Hash}.png");
        }

        private static async Task<MemoryStream> CopyStream(Stream input)
        {
            MemoryStream output = new MemoryStream();
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
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
