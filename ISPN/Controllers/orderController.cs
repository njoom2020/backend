using Microsoft.AspNetCore.Mvc;
using EcommerceApiBusinessLayer;
using Shared.OrderDTOs;
using FirebaseAdmin.Messaging;
using Shared.OrdersDTOs;
using EcommerceApiDataLayer;

namespace EcommerceApiBusinessLayer.Controllers
{
    [ApiController]
    [Route("api/OrderApi")]
    public class OrderController : ControllerBase
    {
        [HttpPost("AddOrder", Name = "AddOrder")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewOrder([FromBody] NewOrderDTO request)
        {
            if (request == null || request.Order_userId < 1 || request.Order_type < 0 || request.Order_priceDelivery < 0 ||
                     request.Order_price < 0 || request.Order_paymentMethode < 0)
                return BadRequest(new { status = false, message = "invalid parameters" });

            Order newOrder = new Order(request);

            if (newOrder.save())
            {
                request.Order_id = newOrder.Order_id;
                return Ok(new { status = true, message = "success" });
            }

            if (newOrder.Order_id == -1)
                return BadRequest(new { status = false, message = "the cart is empty can't create order!." });

            return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "error: data not saved. failed to add new order." });
        }
        [HttpGet("GetAllUserOrders/{userId:int}")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserOrders(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new { status = "fail", message = "Invalid User ID" });
            }

            var orders = Order.getAllUserOrders(userId);

            if (orders == null || !orders.Any())
            {
                return NotFound(new { status = "fail", message = "No orders found for this user" });
            }

            return Ok(new { status = "success", data = orders });
        }

        [HttpGet("GetAllUserArchiveOrders/{userId:int}")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetAllUserArchiveOrders(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new { status = "fail", message = "Invalid User ID" });
            }

            var orders = Order.getAllUserArchiveOrders(userId);

            if (orders == null || !orders.Any())
            {
                return NotFound(new { status = "fail", message = "No orders found for this user" });
            }

            return Ok(new { status = "success", data = orders });
        }


        [HttpGet("GetOrderDetails/{orderId:int}")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetOrderDetails(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest(new { status = "fail", message = "Invalid User ID" });
            }

            var orders = Order.GetOrderDetails(orderId);

            if (orders == null || !orders.Any())
            {
                return NotFound(new { status = false, message = "No orders found for this user" });
            }

            return Ok(new { status = "success", data = orders });
        }

        [HttpDelete("DeleteOrder/{orderId:int}")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteOrder(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest(new { status = false, message = "Invalid order ID" });
            }

            bool isDeleted = Order.DeleteOrder(orderId);

            if (!isDeleted)
                return NotFound(new { status = false, message = $"No orders delete for this orderId: {orderId}" });


            return Ok(new { status = true, data = true });
        }


        [HttpPost("UpdateOrderRating")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateOrderRating([FromBody] UpdateOrderRatingDto requestDto)
        {
            if (requestDto.orderId < 1 || requestDto.orderRating < 0 || requestDto.orderRating > 10)
                return BadRequest(new { status = false, message = "invalid parameter" });
            bool isRating = Order.updateOrderRatingsSetSumbit(requestDto.orderId, requestDto.orderRating, requestDto.orderRatingNote);
            if (!isRating)
                return NotFound(new { status = false, message = $"fail no order update rating for this order id : {requestDto.orderId}" });

            return Ok(new { status = true, data = true });
        }


        // ---------------------------------------------- admin -----------------------------

        [HttpGet("UpdateStatusOrder")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateStatusOrder(int userId, int orderId, byte orderStatus, string title, string body)
        {
            if (userId < 1 || orderId < 1 || orderStatus < 0 || orderStatus > 5)
                return BadRequest(new { status = "false", message = "failed to send message !!" });

            bool isUpdateOrderStatus = Order.updateStatusOrder(orderId, orderStatus, userId, title, body);
            bool result = false;
            if (isUpdateOrderStatus)
            {
                var data = new Dictionary<string, string>
                {
                    {"pageName", "orders"}
                };
                result = await NotificationServices.SendNotificationToTopicAsync(
                               $"users{userId}",
                               $"{title}",
                               $"{body}", data);
            }


            if (result)
                return Ok(new { status = "Success", message = "تم إرسال إشعار الـ Topic بنجاح" });

            return BadRequest(new { status = "Failed", message = "فشل إرسال إشعار الـ Topic" });
        }

        [HttpPost("acceptOrder")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> acceptOrder([FromQuery] int orderId)
        {
            try
            {
                if (orderId < 1)
                    return BadRequest(new { status = "false", message = "failed to send message !!" });

                Order? order = Order.findOrderById(orderId);

                if (order == null)
                    return NotFound(new { status = "fail", message = "order not found." });

                if (order.Order_status != 0)
                    return BadRequest(new { status = "fail", message = "the order status != 0" });

                order.Order_status = 1;

                bool isUpdate = order.save();

                if (!isUpdate)
                    return BadRequest(new { status = "fail", message = "the order not update" });

                try
                {

                    var data = new Dictionary<string, string>
                {
                    {"pageName", "orders"}
                };
                    await NotificationServices.SendNotificationToTopicAsync(
                                  $"users{order.Order_userId}",
                                  "success",
                                  $"order id : {order.Order_id} acceppted", data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"notification error: {ex.Message}");
                }

                return Ok(new { status = "success", message = "the order accepted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }
        }

        [HttpPost("prepareOrder")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> prepareOrder([FromQuery] int orderId)
        {
            try
            {
                if (orderId < 1)
                    return BadRequest(new { status = "false", message = "failed to send message !!" });

                Order? order = Order.findOrderById(orderId);

                if (order == null)
                    return NotFound(new { status = "fail", message = "order not found." });
                if (order.Order_status != 1)
                    return BadRequest(new { status = "fail", message = "the order status != 1" });
                if (order.Order_type == 1)
                    order.Order_status = 4;
                else
                    order.Order_status = 2;

                bool isUpdate = order.save();

                if (!isUpdate)
                    return BadRequest(new { status = "fail", message = "the order not update" });

                try
                {
                    if (order.Order_status == 2)
                    {

                        var data = new Dictionary<string, string>
                {
                    {"pageName", "orders"}
                };
                        await NotificationServices.SendNotificationToTopicAsync(
                                      $"delivery",
                                      "new order",
                                      $"new order id : {order.Order_id} available", data);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"notification error: {ex.Message}");
                }

                return Ok(new { status = "success", message = "the order accepted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }
        }

        [HttpPost("receiptOrder")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> receiptOrder([FromQuery] int orderId)
        {
            try
            {
                if (orderId < 1)
                    return BadRequest(new { status = "false", message = "failed to send message !!" });

                Order? order = Order.findOrderById(orderId);

                if (order == null)
                    return NotFound(new { status = "fail", message = "order not found." });

                if (order.Order_status == 3)
                    order.Order_status = 4;
                else if (order.Order_status == 4 && order.Order_type == 1)
                    order.Order_status = 5;
                else
                    return BadRequest(new { status = "fail", message = "order status not equil 3 or 4" });

                bool isUpdate = order.save();

                if (!isUpdate)
                    return BadRequest(new { status = "fail", message = "the order not update" });

                try
                {
                    if (order.Order_status == 4)
                    {

                        var data = new Dictionary<string, string>
                {
                    {"pageName", "orders"}
                };
                        await NotificationServices.SendNotificationToTopicAsync(
                                      $"delivery{order.deliveryId}",
                                      "receipt order",
                                      $"receipt order id : {order.Order_id} to you", data);
                    }
                    else if (order.Order_status == 5)
                    {
                        var data = new Dictionary<string, string>
                {
                    {"pageName", "orders"}
                };
                        await NotificationServices.SendNotificationToTopicAsync(
                                      $"users{order.Order_userId}",
                                      "receipt order",
                                      $"receipt order id : {order.Order_id} to you", data);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"notification error: {ex.Message}");
                }

                return Ok(new { status = "success", message = "the order receipt updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", message = ex.Message });
            }
        }











        [HttpGet("GetAllNewOrders")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllNewOrders()
        {
            try
            {
                var orders = Order.getAllNewOrders();
                return Ok(new { status = "success", data = orders });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", Message = ex.Message });
            }
        }

        [HttpGet("GetAllOrdersInPrepare")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllOrdersInPrepare()
        {
            try
            {
                var orders = Order.getAllOrdersInPrepare();
                return Ok(new { status = "success", data = orders });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", Message = ex.Message });
            }
        }

        [HttpGet("GetAllOrdersInProcess")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllOrdersInProcess()
        {
            try
            {
                var orders = Order.getAllOrdersInProcess();
                return Ok(new { status = "success", data = orders });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", Message = ex.Message });
            }
        }

        [HttpGet("GetAllOrdersArchive")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllOrdersArchive()
        {
            try
            {
                var orders = Order.getAllOrdersArchive();
                return Ok(new { status = "success", data = orders });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", Message = ex.Message });
            }
        }

        [HttpGet("GetOrderInfoByOrderId")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetOrderInfoByOrderId([FromQuery] int orderId)
        {
            try
            {
                if (orderId < 1)
                    return BadRequest(new { status = "fail", message = "invalid parameter" });

                var orderInfo = Order.getOrderInfoById(orderId);

                if (orderInfo == null)
                    return NotFound(new { status = "fail", message = $"the order id: {orderId} not found." });

                return Ok(new { status = "success", data = orderInfo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "fail", Message = ex.Message });
            }
        }








        [HttpGet("test-push-notification")]
        public async Task<IActionResult> TestPushNotification()
        {
            // الـ Token الحقيقي الذي تم استخراجه من المحاكي لديك
            string deviceToken = "fXZvb0CyR0mwlqyx854LXd:APA91bFPbl5OyRW1VAYBjEdQY5qxaRL_vuUdlKEFIdWKpO0QUNbGSxVNOsX7lJcgjpdPuS45EkNM56BtwtAoNLcu1M84erNApYkdogUlzgU6monY67MoVbM";

            bool result = await NotificationServices.SendNotificationAsync(
                deviceToken,
                "تطبيق المتجر 🛒",
                "تم الربط بين ASP.NET Core و Flutter بنجاح! 🎉"
            );

            if (result)
                return Ok(new { status = "Success", message = "تم إرسال الإشعار بنجاح" });

            return BadRequest(new { status = "Failed", message = "فشل إرسال الإشعار، تأكد من ملف firebase-key.json" });
        }















    }

}

public class UpdateOrderRatingDto
{
    public int orderId { get; set; }
    public byte orderRating { get; set; }
    public string orderRatingNote { get; set; } = string.Empty;
}