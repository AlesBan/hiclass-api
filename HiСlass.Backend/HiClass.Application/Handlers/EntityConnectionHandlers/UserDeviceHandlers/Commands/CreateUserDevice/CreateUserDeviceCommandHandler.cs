// using HiClass.Application.Common.Exceptions.Device;
// using HiClass.Application.Common.Exceptions.User;
// using HiClass.Application.Interfaces;
// using HiClass.Domain.EntityConnections;
// using MediatR;
//
// namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;
//
// public class CreateUserDeviceCommandHandler : IRequestHandler<CreateUserDeviceCommand, UserDevice>
// {
//     private readonly ISharedLessonDbContext _context;
//
//     public CreateUserDeviceCommandHandler(ISharedLessonDbContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<UserDevice> Handle(CreateUserDeviceCommand request, CancellationToken cancellationToken)
//     {
//         var userId = request.UserId;
//         _ = await _context.Users.FindAsync(new object?[] { userId },
//                 cancellationToken: cancellationToken) ??
//             throw new UserNotFoundByIdException(userId);
//
//         var deviceToken = request.DeviceToken;
//         _ = await _context.Devices
//                 .FindAsync(new object[] { userId, deviceToken }, cancellationToken: cancellationToken) ??
//             throw new DeviceByTokenNotFoundException(deviceToken);
//
//         var userDevice = new UserDevice
//         {
//             UserId = userId,
//             DeviceToken = deviceToken
//         };
//
//         _context.UserDevices.Add(userDevice);
//
//         await _context.SaveChangesAsync(cancellationToken);
//
//         return userDevice;
//     }
// }