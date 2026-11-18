using Microsoft.AspNetCore.Mvc;
using Shared.Helper;

namespace EcommerceApiBusinessLayer.Controllers;

[ApiController]
[Route("api/SettingApi")]
public class SettingController : ControllerBase
{
    [HttpGet("getLastSetting")]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult getLastSetting(byte appType = 1)
    {

        string baseUrl = $"{Request.Scheme}://{Request.Host}";
        var settingDto = Setting.getLastSetting(baseUrl, appType);

        if (settingDto == null)
        {
            return NotFound(new { status = false, message = "No setting found" });
        }

        return Ok(new { status = "success", data = settingDto });
    }

    












    [HttpGet("image/{filename}")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetItemImage(string fileName)
    {
        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string filePath = Path.Combine(homePath, "AppImages", "Setting", fileName);


        if (!System.IO.File.Exists(filePath))
            return NotFound(new { message = "image not found" });

        string contentType = MimeTypeHelper.GetContentType(filePath);
        return File(System.IO.File.ReadAllBytes(filePath), contentType);
    }

}
