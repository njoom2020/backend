using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using EcommerceApiBusinessLayer;
using Org.BouncyCastle.Bcpg.Attr;
using MimeKit;
using Org.BouncyCastle.Tls;
using Shared.Helper;
using Shared.CategoryDTOs;
using Shared.ImageDTOs;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace ISPN.Controllers
{
    [ApiController]
    [Route("api/CategoryApi")]
    public class CategoriesController : ControllerBase
    {
        [HttpGet("AllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            List<CategoryDTO> categories = Category.GetAllCategories(baseUrl);

            if (categories.Count == 0)
                return NotFound(new { message = "no category found" });

            return Ok(categories);
        }

        [HttpGet("image/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoryImage(string fileName)
        {
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = Path.Combine(homePath, "AppImages", "Categories", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "image not found" });

            string contentType = MimeTypeHelper.GetContentType(filePath);
            return File(System.IO.File.ReadAllBytes(filePath), contentType);

        }

        [HttpPost("AddCategory")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCategory([FromForm] AddNewCategoryRequest addCategoryRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(addCategoryRequest.name) || string.IsNullOrWhiteSpace(addCategoryRequest.nameAr))
                    return BadRequest(new { status = "fail", message = "invalid parameter" });
                string imageName = await SaveImage(addCategoryRequest.imageFile, "Categories", true);
                CategoryDTO CDTO = new CategoryDTO(-1, addCategoryRequest.name, addCategoryRequest.nameAr, imageName);
                Category newCategory = new Category(CDTO, null);
                if (newCategory.Save())
                {
                    CDTO.Id = newCategory.Id;
                    return Ok(new { status = "success", data = CDTO });
                }
                else
                    return BadRequest(new { status = "fail", message = "new category not saved" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }

        }

        [HttpPost("EditCategoryPicture")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditCategoryPicture([FromForm] EditCategoryPicRequest editPicDto)
        {
            try
            {
                if (editPicDto == null || editPicDto.id < 1 || editPicDto.imageFile == null)
                    return BadRequest(new { status = "fail", message = "invalid parameter" });

                Category? cat = Category.FindCategoryById(editPicDto.id);

                if (cat == null)
                    return NotFound(new { status = "fail", message = $"category with id:{editPicDto.id} not found " });

                if (cat.image != "noImage.svg")
                    Image.DeleteOldImageFromStoreg(cat.image, "Categories");

                string imageName = await SaveImage(editPicDto.imageFile, "Categories", true);

                cat.image = imageName;

                bool isUpdate = cat.Save();

                if (isUpdate)
                    return Ok(new { status = "success", message = "update picture done successfully." });

                return BadRequest(new { status = "fail", message = "fail to update picture." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }

        }

        [HttpPost("editCategory")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> editCategory([FromBody] CategoryDTO catDto)
        {
            try
            {
                if (catDto == null || catDto.Id < 1 || string.IsNullOrWhiteSpace(catDto.Name) || string.IsNullOrWhiteSpace(catDto.Name_ar))
                    return BadRequest(new { status = "fail", message = "invalid parameter" });

                Category? cat = Category.FindCategoryById(catDto.Id);

                if (cat == null)
                    return NotFound(new { status = "fail", message = $"the category with id: {catDto.Id} not found" });

                cat.Name = catDto.Name;
                cat.Name_ar = catDto.Name_ar;


                bool isUpdate = cat.Save();

                if (isUpdate)
                    return Ok(new { status = "success", data = cat });

                return BadRequest(new { status = "fail", message = "fail to update this category." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }


        }

        [HttpDelete("DeleteCategory")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory([FromBody] int categoryId)
        {
            try
            {
                if (categoryId < 1)
                    return BadRequest(new { status = "fail", message = "invalid parameter" });
                Category? cat = Category.FindCategoryById(categoryId);
                if (cat == null)
                    return NotFound(new { status = "fail", message = "category not found" });
                string imageName = cat.image ?? "";
                if (Category.DeleteCategoryById(categoryId))
                {
                    if (!string.IsNullOrWhiteSpace(imageName) && imageName != "noImage.svg")
                        Image.DeleteOldImageFromStoreg(imageName, "Categories");
                    return Ok(new { status = "success", message = "category deleted successfully" });
                }
                return BadRequest(new { status = "fail", message = "failed to delete category." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }
        }

        //------------------------------------------------------------------------------------------------------
        public class AddNewCategoryRequest
        {
            public int? id { get; set; } = -1;
            public string name { get; set; } = "";
            public string nameAr { get; set; } = "";
            public IFormFile? imageFile { get; set; }
        }

        public class EditCategoryPicRequest
        {
            public int id { get; set; } = -1;
            public IFormFile? imageFile { get; set; }
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


