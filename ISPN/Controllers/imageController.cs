using EcommerceApiBusinessLayer;
using EcommerceApiDataLayer;
using Microsoft.AspNetCore.Mvc;
using Shared.Helper;
using Shared.ItemImageDTOs;
using Shared.SettingImageDTOs;

namespace ISPN.Controllers;

[ApiController]
[Route("api/ImageApi")]
public class ImageController : ControllerBase
{
    [HttpGet("image/{folderName}/{fileName}")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetImage(string folderName, string fileName)
    {
        string cleanFolderName = Path.GetFileName(folderName);
        string cleanFileName = Path.GetFileName(fileName);

        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string filePath = Path.Combine(homePath, "AppImages", cleanFolderName, cleanFileName);


        if (!System.IO.File.Exists(filePath))
            return NotFound(new { message = "image not found" });
        string contentType = MimeTypeHelper.GetContentType(filePath);
        return File(System.IO.File.ReadAllBytes(filePath), contentType);
    }

    [HttpGet("GetSettingImages/{settingId}")]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<SettingImageDTO>), StatusCodes.Status200OK)]

    public IActionResult GetSettingImages(int settingId)
    {
        if(settingId < 1)
            return BadRequest(new { message = "invalid parameter"});
        
        string baseUrl = $"{Request.Scheme}://{Request.Host}";

        List<SettingImageDTO> imagesList = SettingImage.getAllImageForSetting(settingId, baseUrl);

        if(imagesList == null ||imagesList.Count == 0)
            return NotFound(new { message = $"no images found for setting id {settingId}"});

        return Ok(imagesList);
    }

    [HttpGet("GetItemsImages/{itemId}")]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<ItemImageDTO>), StatusCodes.Status200OK)]

    public IActionResult GetItemsImages(int itemId)
    {
        if(itemId < 1)
            return BadRequest(new { message = "invalid parameter"});
        
        string baseUrl = $"{Request.Scheme}://{Request.Host}";

        List<ItemImageDTO> imagesList = ItemImageData.getAllItemImages(itemId, baseUrl);

        if(imagesList == null ||imagesList.Count == 0)
            return NotFound(new { message = $"no images found for item id {itemId}"});

        return Ok(imagesList);
    }

    


}