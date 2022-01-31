using Maple2Storage.Types;
using Microsoft.AspNetCore.Mvc;

namespace MapleWebServer.Controllers;

public class ProfilesController : ControllerBase
{
    [HttpGet("data/profiles/avatar/{characterId}/{hash}.png")]
    public IActionResult Get(string characterId, string hash)
    {
        string fullPath = $"{Paths.DATA_DIR}/profiles/{characterId}/{hash}.png";
        if (!System.IO.File.Exists(fullPath))
        {
            return BadRequest();
        }

        Response.Headers.Add("content-type", "image/png");
        FileStream profileImage = System.IO.File.OpenRead(fullPath);
        return Ok(profileImage);
    }
}
