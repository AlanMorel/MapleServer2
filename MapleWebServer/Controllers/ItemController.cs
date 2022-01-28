using Maple2Storage.Types;
using Microsoft.AspNetCore.Mvc;

namespace MapleWebServer.Controllers;

public class ItemController : ControllerBase
{
    [HttpGet("itemicon/ms2/01/{itemId}/{uuid}.png")]
    public IActionResult GetItemIcon(string itemId, string uuid)
    {
        string fullPath = $"{Paths.DATA_DIR}/itemicon/{itemId}/{uuid}.png";
        if (!System.IO.File.Exists(fullPath))
        {
            return BadRequest();
        }

        Response.Headers.Add("content-type", "image/png");
        FileStream profileImage = System.IO.File.OpenRead(fullPath);
        return Ok(profileImage);
    }

    [HttpGet("item/ms2/01/{itemId}/{uuid}.m2u")]
    public IActionResult GetItem(string itemId, string uuid)
    {
        string fullPath = $"{Paths.DATA_DIR}/item/{itemId}/{uuid}.m2u";
        if (!System.IO.File.Exists(fullPath))
        {
            return BadRequest();
        }

        Response.Headers.Add("content-type", "image/png");
        FileStream profileImage = System.IO.File.OpenRead(fullPath);
        return Ok(profileImage);
    }
}
