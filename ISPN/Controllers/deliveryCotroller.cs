using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Engines;

namespace EcommerceApiBusinessLayer.Controllers;

[ApiController]
[Route("api/DeliveryApi")]
public class DeliveryController : ControllerBase
{
    [HttpGet("GetAllAvailableOrders")]

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllAvailableOrders()
    {
        var ordersList = Delivery.getAllOrdersAvailable();
        if (ordersList == null || !ordersList.Any())
            return NotFound(new { status = false, message = "no orders found" });
        return Ok(new { status = true, data = ordersList });
    }

    [HttpGet("getAllAcceptedOrders")]

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult getAllAcceptedOrders([FromQuery] int deliveryId)
    {
        if (deliveryId < 1)
            return BadRequest(new { status = "fail", message = "invalid parameter" });
        var ordersList = Delivery.getAllOrdersAccepted(deliveryId);
        if (ordersList == null || !ordersList.Any())
            return NotFound(new { status = false, message = "no orders found" });
        return Ok(new { status = true, data = ordersList });
    }

    // [HttpPost("acceptOrder")]

    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status200OK)]

    // public IActionResult acceptOrder([FromBody] AcceptedOrderDTO acceptedDto)
    // {
    //     if (acceptedDto == null || acceptedDto.DeliveryId < 1 || acceptedDto.OrderId < 1)
    //         return BadRequest(new { status = "fail", message = "invalid parameter" });
    //     bool isSuccess = Delivery.AcceptedOrder(acceptedDto.DeliveryId, acceptedDto.OrderId);
    //     return Ok(isSuccess);
    // }

    public class AcceptedOrderDTO
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
    }

    [HttpPost("AcceptedOrder")]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AcceptedOrder(AcceptedOrderDTO acceptedDto)
    {
        if (acceptedDto == null || acceptedDto.DeliveryId < 1 || acceptedDto.OrderId < 1)
            return BadRequest(new { status = "fail", message = "invalid parameter !!" });

        bool isAccepted = Delivery.AcceptedOrder(acceptedDto.DeliveryId, acceptedDto.OrderId);

        if (isAccepted)
        {
            var data = new Dictionary<string, string>
                {
                    {"AcceptedOrder", $"{acceptedDto.OrderId}"}
                };
            await NotificationServices.SendNotificationToTopicAsync(
                           $"delivery",
                           $"order accepted ",
                           $"order accepted by delivery id : {acceptedDto.DeliveryId}", data);

            var data2 = new Dictionary<string, string>
                {
                    {"AcceptedOrder", $"{acceptedDto.OrderId}"},
                    {"deliveryId",$"{acceptedDto.DeliveryId}"}
                };

            await NotificationServices.SendNotificationToTopicAsync(
                         $"admin",
                         $"order accepted by delivery ",
                         $"order accepted by delivery id : {acceptedDto.DeliveryId}", data);
        }




        return Ok(isAccepted);
    }


    [HttpPost("delete_AcceptedOrder")]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> delete_AcceptedOrder(AcceptedOrderDTO acceptedDto)
    {
        if (acceptedDto == null || acceptedDto.DeliveryId < 1 || acceptedDto.OrderId < 1)
            return BadRequest(new { status = "fail", message = "invalid parameter !!" });

        bool isDeleted_Accepted = Delivery.deleteAcceptedOrder(acceptedDto.DeliveryId, acceptedDto.OrderId);

        if (isDeleted_Accepted)
        {
            var data = new Dictionary<string, string>
                {
                    {"deleteOrder", "refresh"}
                };
            await NotificationServices.SendNotificationToTopicAsync(
                           $"delivery{acceptedDto.DeliveryId}",
                           $"deleted",
                           $"Order number : {acceptedDto.OrderId} has been deleted from you", data);

            await NotificationServices.SendNotificationToTopicAsync(
          $"delivery",
          $"new order",
          $"Order number : {acceptedDto.OrderId} has been available", data);


        }




        return Ok(isDeleted_Accepted);
    }




}