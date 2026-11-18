using EcommerceApiBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Shared.ItemDTOs;

namespace ISPN.Controllers
{
    [ApiController]
    [Route("api/AddressApi")]
    public class AddressController : ControllerBase
    {
        [HttpPost("AddNewAddress", Name = "AddNewAddress")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<AddressDTO>> AddNewAddress([FromBody] AddressDTO request)
        {
            if (request == null || request.Address_id < 0 || request.Address_userId < 0 || string.IsNullOrWhiteSpace(request.Address_name) ||
             string.IsNullOrWhiteSpace(request.Address_city) || string.IsNullOrWhiteSpace(request.Address_street) || request.Address_lat < 0 || request.Address_long < 0)
                return BadRequest("invalid user data.");

            var address = new Address(request);
            if (address.Save())
            {
                if (address.Address_id == -2)
                    return Conflict(new { status = false, message = "new address for user failed to save" });

                request.Address_id = address.Address_id;
                Console.WriteLine(request.Address_id);
                return CreatedAtRoute("GetUserById", new { id = request.Address_id }, request);
            }
            else
                return StatusCode(500, new { message = "Error: Data not saved. Failed to add new address." });
        }

        [HttpGet("GetAllUserAdresses")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AddressDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<AddressDTO>> GetAllUserAdresses(int userId)
        {
            try
            {
                if (userId < 1)
                    return BadRequest(new { status = "error", message = "invalid parameter" });
                List<AddressDTO> addressesList = new List<AddressDTO>();
                addressesList = Address.getAllAddressesByUserId(userId);

                if (addressesList == null)
                    return Ok(new List<AddressDTO>());

                return Ok(addressesList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        [HttpDelete("DeleteAddressByAddressId")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteAddressByAddressId([FromQuery]int addressId,[FromQuery] int userId)
        {
            if (addressId < 1 || userId < 1)
                return BadRequest(new { status = false, message = "invalid parameters" });

            bool isDelete = Address.deleteAddressByAddressId(addressId, userId);

            if (isDelete)
                return Ok(new { status = true, message = "item deleted. successfully" });

            return StatusCode(500, new { status = false, message = "unauthorized action or item not found" });
        }

    }





}