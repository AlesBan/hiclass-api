using System.Net.WebSockets;
using System.Text;
using HiClass.API.Helpers;
using HiClass.Infrastructure.Services.NotificationHandlerService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
}