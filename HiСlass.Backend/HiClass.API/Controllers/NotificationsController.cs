using HiClass.API.Helpers;
using HiClass.Application.Models.Notifications;
using HiClass.Infrastructure.InternalServices.NotificationHandlerService;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

public class NotificationsController : BaseController
{
    private readonly INotificationHandlerService _notificationHandlerService;

    public NotificationsController(INotificationHandlerService notificationHandlerService)
    {
        _notificationHandlerService = notificationHandlerService;
    }

    [HttpGet("all-notifications")]
    public async Task<IActionResult> GetAllNotifications()
    {
        var notifications = await _notificationHandlerService.GetUserNotificationsByUserId(UserId, Mediator);
        return ResponseHelper.GetOkResult(notifications);
    }

    [HttpPost("update-notification-status")]
    public async Task<IActionResult> UpdateNotificationStatus([FromBody] UpdateNotificationStatusRequestDto updateNotificationStatusRequestDto)
    {
       await _notificationHandlerService.UpdateNotificationStatus(updateNotificationStatusRequestDto, Mediator);
       return ResponseHelper.GetOkResult();
    }
}