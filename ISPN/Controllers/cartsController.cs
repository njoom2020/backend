
using EcommerceApiBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Shared.UserCartDTO;

namespace ISPN.Controllers
{
    [ApiController]
    [Route("api/CartsApi")]
    public class CartsController : ControllerBase
    {
        [HttpGet("GetUserCarts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserCarts([FromQuery] int userId)
        {
            try
            {
                if (userId < 1)
                    return BadRequest(new { status = false, message = "invalid parameters" });

                string baseUrl = $"{Request.Scheme}://{Request.Host}";

                var list = Cart.getUserCarts(userId, baseUrl);

                if (list == null || list.Count == 0)
                    return Ok(new { status = true, message = "no carts found", data = new List<UserCartDTO>() });

                return Ok(new { status = true, data = list });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = ex.Message });

            }
        }

        [HttpPost("ManageUserCart")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ManageUserCart([FromBody] CartDataRequestDto requestDto)
        {
            if (requestDto == null || requestDto.UserId < 1 || requestDto.ItemId < 1 || requestDto.QuantityChange == 0 || requestDto.QuantityChange > 100 || requestDto.QuantityChange < -100)
                return BadRequest(new { status = false, message = "invalid parameter values." });
            try
            {
                var result = Cart.addSubtractQuantityAndAddNewOrUpdateOrDelete(requestDto);

                if (result.IsComplete)
                    return Ok(new
                    {
                        status = true,
                        message = "cart updated successfully.",
                        data = new
                        {
                            isComplete = result.IsComplete,
                            finalQuantity = result.FinalQuantity
                        }
                    });
                else
                    return BadRequest(new
                    {
                        status = false,
                        message = "failed to update cart item.",
                        data = new
                        {
                            isComplete = result.IsComplete,
                            finalQuantity = result.FinalQuantity
                        }
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
            }


        }
[HttpPost("AddUpdateCart")]

[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddUpdateCart([FromBody] CartDataRequestDto requestDto)
        {
            if (requestDto == null || requestDto.UserId < 1 || requestDto.ItemId < 1)
                return BadRequest(new {status = false, message = "invalid parameter values."});

            try
            {
                bool isSuccess = Cart.addNewUpateCarts(requestDto.UserId, requestDto.ItemId, requestDto.QuantityChange);

                if(isSuccess)
                    return Ok(new{status = true, message = "cart updated successfully."});
                else
                    return BadRequest(new{status = false, message = "failed to update cart item"});

            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message});
            }
        }

    }



}