using System.Net.WebSockets;
using System.Text;
using HiClass.API.Helpers;
using HiClass.Infrastructure.InternalServices.DeviceHandlerService;
using HiClass.Infrastructure.InternalServices.NotificationHandlerService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

public class NotificationsController : BaseController
{
    private readonly INotificationHandlerService _notificationHandlerService;
    private readonly IDeviceHandlerService _deviceHandlerService;

    public NotificationsController(INotificationHandlerService notificationHandlerService, IDeviceHandlerService deviceHandlerService)
    {
        _notificationHandlerService = notificationHandlerService;
        _deviceHandlerService = deviceHandlerService;
    }

    [HttpGet("all-notifications")]
    public async Task<IActionResult> GetAllNotifications()
    {
        var notifications = await _notificationHandlerService.GetUserNotificationsByUserId(UserId, Mediator);
        return ResponseHelper.GetOkResult(notifications);
    }
}