// using System.ComponentModel.DataAnnotations;
// using HiClass.Domain.EntityConnections;
// using MediatR;
//
// namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;
//
// public class CreateUserDeviceCommand : IRequest<UserDevice>
// {
//     [Required] public string DeviceToken { get; set; } = string.Empty;
//     [Required] public Guid UserId { get; set; }
// }