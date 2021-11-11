using Maple2Storage.Types;
using Microsoft.AspNetCore.Mvc;

namespace MapleWebServer.Controllers;

[ApiController]
public class ItemController : ControllerBase
{
    [Route("itemicon/ms2/01/{itemId}/{uuid}.png")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStream))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetItemIcon(string itemId, string uuid)
    {
        string requestImagePath = HttpContext.Request.Path;
        string fullPath = $"{Paths.DATA_DIR}/itemicon/{itemId}/{uuid}.png".Replace("$", "");
        if (!System.IO.File.Exists(fullPath))
        {
            return BadRequest();
        }
        Response.Headers.Add("content-type", "image/png");
        FileStream profileImage = System.IO.File.OpenRead(fullPath);
        return Ok(profileImage);
    }

    [Route("item/ms2/01/{itemId}/{uuid}.m2u")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStream))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetItem(string itemId, string uuid)
    {
        string requestImagePath = HttpContext.Request.Path;
        string fullPath = $"{Paths.DATA_DIR}/item/{itemId}/{uuid}.m2u".Replace("$", "");
        if (!System.IO.File.Exists(fullPath))
        {
            return BadRequest();
        }
        Response.Headers.Add("content-type", "image/png");
        FileStream profileImage = System.IO.File.OpenRead(fullPath);
        return Ok(profileImage);
    }
}
