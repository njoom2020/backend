using EcommerceApiBusinessLayer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.FavoriteDTO;
using Shared.ItemDTOs;
using Shared.MyFavoriteDTO;

namespace ISPN.Controllers
{
    [ApiController]
    [Route("api/FavoriteApi")]

    public class FavoriteController : ControllerBase
    {
        [HttpPost("AddFavorite")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddFavorite([FromBody] FavoriteDTO fTdto)
        {
            if (fTdto == null || fTdto.UserId <= 0 || fTdto.ItemId <= 0)
                return BadRequest(new { status = false, message = "invalid parameters" });

            Favorite newFavorite = new Favorite();

            newFavorite.UserId = fTdto.UserId;
            newFavorite.ItemId = fTdto.ItemId;

            if (newFavorite.save() && newFavorite.Id > 0)
            {
                fTdto.Id = newFavorite.Id;
                return Ok(new { status = true, favoriteId = fTdto.Id });
            }

            return StatusCode(500, new { status = false, message = "could not add item to favorites" });

        }

        [HttpDelete("RemoveFavorite")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFavorite(int userId, int itemId)
        {
            if (userId <= 0 || itemId <= 0)
                return BadRequest(new { status = false, message = "invalid parameters" });

            bool isDelete = Favorite.deleteFavorite(userId, itemId);

            if (isDelete)
                return Ok(new { status = true, message = "item deleted. successfully" });

            return StatusCode(403, new { status = false, message = "unauthorized action or item not found" });
        }

        [HttpGet("GetUserFavorites")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserFavorites(int userId, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                if (userId < 1 || pageNumber < 1 || pageSize < 1)
                    return BadRequest(new { status = false, message = "invalid parameters" });
                string baseUrl = $"{Request.Scheme}://{Request.Host}";

                var list = Favorite.getUserFavoriteItems(userId, baseUrl, pageNumber, pageSize);

                if (list == null || list.Count == 0)
                    return Ok(new { status = false, message = "no favorites found", data = new List<MyFavoriteDTO>() });

                return Ok(new { status = true, data = list });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }


        [HttpGet("SearchUserFavoriteItems")]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ItemDTO>))]
        public ActionResult<IEnumerable<ItemDTO>> SearchUserFavoriteItems(int userId, string searchText)
        {

            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                List<ItemDTO> itemsList = new List<ItemDTO>();
                itemsList = Favorite.SearchInUserFavoriteItems(userId, searchText, baseUrl);

                if (itemsList == null)
                    return Ok(new List<ItemDTO>());

                return Ok(itemsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }





    }

}