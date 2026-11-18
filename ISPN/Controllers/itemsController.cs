
using System.Data.SqlTypes;
using System.Security.Cryptography;
using EcommerceApiBusinessLayer;
using EcommerceApiDataLayer;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Shared.CategoryDTOs;
using Shared.Helper;
using Shared.ItemAdminDTOs;
using Shared.ItemDTOs;

namespace ISPN.Controllers
{
    [ApiController]
    [Route("api/ItemApi")]
    public class ItemsController : ControllerBase
    {

        [HttpGet("GetHomeData")]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomeDataDTO))]
        public ActionResult<HomeDataDTO> GetHomeData(byte appType = 1)
        {
            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}";

                //var allItems = Item.GetAllItems(baseUrl) ?? new List<ItemDTO>();
                var discountItems = Item.GetAllItems(baseUrl).Where(i => i.Discount > 0).ToList();
                var cattegoriesList = Category.GetAllCategories(baseUrl) ?? new List<CategoryDTO>();
                var topSellingItems = Item.GetTopSelling(baseUrl);
                var topLastSetting = Setting.getLastSetting(baseUrl, appType);

                var response = new HomeDataDTO
                {
                    //items = allItems,
                    offerItems = discountItems,
                    categories = cattegoriesList,
                    topSelling = topSellingItems,
                    lastSetting = topLastSetting
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("GetAllItems")]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ItemDTO>))]
        public ActionResult<List<ItemDTO>> GetAllItems([FromQuery] bool isOffers = false)
        {
            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}";

                var allItems = Item.GetAllItems(baseUrl, isOffers) ?? new List<ItemDTO>();

                return Ok(allItems);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("GetItemsByCatId")]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ItemDTO>))]
        public ActionResult<IEnumerable<ItemDTO>> GetItemsByCatId(int categoryId, int userId, int pageNumber = 1, int pageSize = 30)
        {

            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                List<ItemDTO> itemsList = new List<ItemDTO>();
                itemsList = Item.getItemsByCategoryId(categoryId, userId, baseUrl, pageNumber, pageSize);

                if (itemsList == null)
                    return Ok(new List<ItemDTO>());

                return Ok(itemsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("SearchItems")]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ItemDTO>))]
        public ActionResult<IEnumerable<ItemDTO>> SearchItems(int userId, string searchText, int categoryId = 0)
        {

            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                List<ItemDTO> itemsList = new List<ItemDTO>();
                itemsList = Item.searchItems(userId, searchText, baseUrl, categoryId);

                if (itemsList == null)
                    return Ok(new List<ItemDTO>());

                return Ok(itemsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("image/{filename}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetItemImage(string fileName)
        {
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = Path.Combine(homePath, "AppImages", "Items", fileName);


            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "image not found" });
            string contentType = MimeTypeHelper.GetContentType(filePath);
            return File(System.IO.File.ReadAllBytes(filePath), contentType);
        }
        ///
        /// 
        /// 
        //
        //----------------- admin -----------------
        ///
        /// 
        /// 


        [HttpGet("GetAllItemsForAdmin")]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllItemsForAdmin()
        {

            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                List<ItemAdminDTO> itemsList = [];
                itemsList = Item.getAllItemsForAdmin(baseUrl);

                if (itemsList == null || itemsList.Count == 0)
                    return NotFound(new { status = "fail", message = "no item found." });

                return Ok(itemsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }
        }

        [HttpPost("AddNewItem")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewItem([FromForm] AdimItemDTO IDTORequest)
        {
            try
            {
                if (IDTORequest == null || string.IsNullOrWhiteSpace(IDTORequest.Name) || string.IsNullOrWhiteSpace(IDTORequest.NameAr) ||
                 IDTORequest.Count < 1 || IDTORequest.Discount < 0 || IDTORequest.Discount > 100 || IDTORequest.Price < 0 || IDTORequest.CategoryId < 1)
                    return BadRequest(new { status = "fail", message = "invalid parameter." });

                string imageName = await SaveImage(IDTORequest.Image, "Items");


                Item item = new Item();

                item.Name = IDTORequest.Name;
                item.NameAr = IDTORequest.NameAr;
                item.Desc = IDTORequest.Desc == "string" ? " " : IDTORequest.Desc;
                item.DescAr = IDTORequest.DescAr == "string" ? " " : IDTORequest.DescAr;
                item.Image = imageName;
                item.Count = IDTORequest.Count;
                item.Active = IDTORequest.IsActive;
                item.Price = IDTORequest.Price;
                item.Discount = IDTORequest.Discount;
                item.CategoryId = IDTORequest.CategoryId;

                if (item.Save())
                {
                    IDTORequest.Id = item.Id;
                    return Ok(new { status = "success", message = "add new item done successfully.", imageName = imageName });
                }

                return BadRequest(new { status = "fail", message = "fail add new item." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }
        }

        [HttpGet("GetItemInfoForAdmin")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetItemInfoForAdmin([FromQuery] int itemId)
        {

            if (itemId < 1)
                return BadRequest(new { status = "fail", message = "invalid parameter" });

            try
            {

                var itmeInfo = ItemData.getItemInfo(itemId);

                if (itmeInfo == null)
                    return NotFound(new { status = "fail", message = "the item not found." });

                return Ok(new { status = "success", data = itmeInfo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }
        }

        [HttpPost("UpdateItem")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItem([FromForm] ItemDTOrequest itemDtoReq)
        {
            try
            {
                if (itemDtoReq == null || itemDtoReq.Id < 1 || string.IsNullOrWhiteSpace(itemDtoReq.Name) || string.IsNullOrWhiteSpace(itemDtoReq.NameAr) ||
                itemDtoReq.Count < 0 || itemDtoReq.Price <= 0 || itemDtoReq.Discount < 0 || itemDtoReq.CategoryId < 1)
                    return BadRequest(new { status = "fail", message = "invalid parameter." });

                Item? item = Item.findItemByitemId(itemDtoReq.Id);
                if (item == null)
                    return NotFound(new { status = "fail", message = "item not found." });

                item.Name = itemDtoReq.Name;
                item.NameAr = itemDtoReq.NameAr;
                item.Desc = itemDtoReq.Desc;
                item.DescAr = itemDtoReq.DescAr;
                item.Price = itemDtoReq.Price;
                item.Count = itemDtoReq.Count;
                item.Discount = itemDtoReq.Discount;
                item.Active = itemDtoReq.IsActive;
                item.CategoryId = itemDtoReq.CategoryId;
                string oldImageName = item.Image;
                if (itemDtoReq.ImageFile != null)
                {
                    item.Image = await SaveImage(itemDtoReq.ImageFile, "Items");

                }

                bool isUpdate = item.Save();

                if (isUpdate)
                {
                    if (!string.IsNullOrWhiteSpace(oldImageName) && oldImageName != "noImage.png")
                        Image.DeleteOldImageFromStoreg(oldImageName, "Items");

                    return Ok(new { status = "success", data = item.IDTO });

                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(item.Image) && item.Image != "noImage.png")
                        Image.DeleteOldImageFromStoreg(item.Image, "Items");
                }

                return BadRequest(new { status = "fail", message = "the item not updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }
        }

        [HttpDelete("DeleteItem")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteItem(int itemId)
        {
            try
            {
                if (itemId < 1)
                    return BadRequest(new { status = "fail", message = "invalid parameter" });
                bool isDeleted = ItemData.deleteItem(itemId);
                if (isDeleted)
                    return Ok(new { status = "success", message = "Item deleted successfully." });
                return NotFound(new { status = "fail", message = "item not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }
        }


        //--------------------------  

        public class AdimItemDTO
        {


            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string NameAr { get; set; } = "";
            public string Desc { get; set; } = "";
            public string DescAr { get; set; } = "";
            public IFormFile? Image { get; set; }
            public int Count { get; set; } = 1;
            public decimal Price { get; set; }
            public int Discount { get; set; } = 0;
            public int CategoryId { get; set; }
            public bool IsActive { get; set; } = true;

        }


        public static async Task<string> SaveImage(IFormFile? imageFile, string subFolder, bool isSVG = false)
        {
            if (imageFile == null || imageFile.Length == 0)
                if (isSVG)
                    return "noImage.svg";
                else
                    return "noImage.png";

            try
            {
                string basePath = "/Users/mbp/AppImages";
                string targetFolderPath = Path.Combine(basePath, subFolder);

                if (!Directory.Exists(targetFolderPath))
                    Directory.CreateDirectory(targetFolderPath);

                string extension = Path.GetExtension(imageFile.FileName);

                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string fullPath = Path.Combine(targetFolderPath, uniqueFileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                if (isSVG)
                    return "noImage.svg";
                else
                    return "noImage.png";
            }





        }







    }
}


public class ItemDTOrequest
{
    public ItemDTOrequest()
    {

        this.Id = -1;
        this.Name = "";
        this.NameAr = "";
        this.Desc = "";
        this.DescAr = "";
        this.Image = "";
        this.Count = -1;
        this.Price = 0;
        this.Discount = 0;
        this.CategoryId = -1;
        this.IsFavorite = false;
        this.IsActive = false;
        this.ImageFile = null;


    }

    public ItemDTOrequest(int id, string name, string nameAr, string desc,
         string descAr, string image, int count, decimal price, int discount,
         int categoryId = 0,
         bool isFavorite = false,
         bool isActive = true,
         IFormFile? imageFile = null
         )
    {
        this.Id = id;
        this.Name = name;
        this.NameAr = nameAr;
        this.Desc = desc;
        this.DescAr = descAr;
        this.Image = image;
        this.Count = count;
        this.Price = price;
        this.Discount = discount;
        this.CategoryId = categoryId;
        this.IsFavorite = isFavorite;
        this.IsActive = isActive;
        this.ImageFile = imageFile;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string NameAr { get; set; }
    public string Desc { get; set; } = "";
    public string DescAr { get; set; } = "";
    public string Image { get; set; } = "noImage.png";
    public int Count { get; set; }
    public decimal Price { get; set; }
    public int Discount { get; set; } = 0;
    public int CategoryId { get; set; }
    public bool IsFavorite { get; set; } = false;
    public bool IsActive { get; set; } = true;

    public IFormFile? ImageFile { get; set; }


}



