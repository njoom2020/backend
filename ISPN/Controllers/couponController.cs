
using EcommerceApiBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Shared.CouponDTOs;

namespace ISPN.Controllers;

[ApiController]
[Route("api/CouponApi")]
public class CouponController : ControllerBase
{

[HttpGet("CheckCoupon")]

[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public ActionResult<CouponDTO> CheckCoupon([FromQuery] string couponName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(couponName))
                return BadRequest(new { status = "error", message = "invalid parameter" });

            CouponDTO couponDto = Coupon.checkCoupon(couponName);

            if(couponDto == null || couponDto.coupon_id == -1)
                return NotFound(new {status = "error", message = "no coupon found."});
            
            return Ok(couponDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { status = "error", message = ex.Message });

        }
    }

}