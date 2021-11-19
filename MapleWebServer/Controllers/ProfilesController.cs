using Maple2Storage.Types;
using Microsoft.AspNetCore.Mvc;

namespace MapleWebServer.Controllers;

[ApiController]
public class ProfilesController : ControllerBase
{
    private readonly ILogger<ProfilesController> Logger;

    public ProfilesController(ILogger<ProfilesController> logger)
    {
        Logger = logger;
    }

    [Route("data/profiles/avatar/{characterId}/{hash}.png")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStream))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(string characterId, string hash)
    {
        string requestImagePath = HttpContext.Request.Path;
        string fullPath = $"{Paths.DATA_DIR}/profiles/{characterId}/{hash}.png".Replace("$", "");
        if (!System.IO.File.Exists(fullPath))
        {
            return BadRequest();
        }
        Response.Headers.Add("content-type", "image/png");
        FileStream profileImage = System.IO.File.OpenRead(fullPath);
        return Ok(profileImage);
    }
}
