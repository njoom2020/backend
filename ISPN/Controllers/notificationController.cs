using Microsoft.AspNetCore.Mvc;

namespace EcommerceApiBusinessLayer.Controllers;

[ApiController]
[Route("api/NotificationApi")]
public class NotificationController : ControllerBase
{
    [HttpGet("GetAllUserNotifications/{userId:int}")]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllUserNotifications(int userId)
    {
        if (userId < 1)
            return BadRequest(new { status = "fail", message = "Invalid User ID" });
        var NotificationsList = Notification.getAllNotificationsUser(userId);
        if (NotificationsList == null || !NotificationsList.Any())
            return NotFound(new { status = false, message = "No orders found for this user" });
        return Ok(new { status = "success", data = NotificationsList });
    }
}
