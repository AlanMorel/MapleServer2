using System.Text;
using MapleWebServer.Constants;
using Microsoft.AspNetCore.Mvc;

namespace MapleWebServer.Controllers
{
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ILogger<ProfilesController> Logger;

        public ProfilesController(ILogger<ProfilesController> logger) => Logger = logger;

        [Route("profiles/ms2/emu/{characterId}/{hash}.png")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStream))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string characterId, string hash)
        {
            string requestImagePath = HttpContext.Request.Path;
            string fullPath = $"{Paths.IMAGES}/profiles/ms2/emu/{characterId}/{hash}.png".Replace("$", "");
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
        public IActionResult Post()
        {
            Stream bodyStream = Request.Body;
            if (bodyStream == null)
            {
                return BadRequest();
            }

            MemoryStream memoryStream = new MemoryStream();
            CopyStream(bodyStream, memoryStream);
            byte[] array = memoryStream.ToArray();
            long accountId = (long) BitConverter.ToUInt64(array.Skip(8).Take(8).ToArray(), 0);
            long characterId = (long) BitConverter.ToUInt64(array.Skip(16).Take(8).ToArray(), 0);

            string filePath = $"{Paths.IMAGES}/profiles/ms2/emu/{characterId}/";
            Directory.CreateDirectory(filePath);

            byte[] fileBytes = array.Skip(48).ToArray();

            string md5Hash = CreateMD5(Encoding.UTF8.GetString(fileBytes));
            if (System.IO.File.Exists($"{filePath}/{md5Hash}.png"))
            {
                return Ok($"0,profiles/ms2/emu/${characterId}/${md5Hash}.png");
            }

            System.IO.File.WriteAllBytes($"{filePath}/{md5Hash}.png", fileBytes);

            return Ok($"0,profiles/ms2/emu/${characterId}/${md5Hash}.png");
        }

        public static async void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = await input.ReadAsync(buffer)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        public static string CreateMD5(string input)
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
