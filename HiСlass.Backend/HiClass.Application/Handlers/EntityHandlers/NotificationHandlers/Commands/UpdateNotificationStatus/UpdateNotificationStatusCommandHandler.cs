using HiClass.Application.Common.Exceptions.Notifications;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Domain.Enums;

namespace HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands.UpdateNotificationStatus
{
    public class UpdateNotificationStatusCommandHandler : IRequestHandler<UpdateNotificationStatusCommand>
    {
        private readonly ISharedLessonDbContext _context;

        public UpdateNotificationStatusCommandHandler(ISharedLessonDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == request.NotificationId && !n.IsDeleted,
                    cancellationToken: cancellationToken);

            if (notification == null)
            {
                throw new NotFoundException(nameof(Notification), request.NotificationId);
            }

            if (notification.Status == request.Status)
            {
                throw new SameStatusUpdateException(notification.NotificationId, notification.Status);
            }

            if (request.Status == NotificationStatus.Unread)
            {
                throw new CannotSetUnreadStatusException(notification.NotificationId, notification.Status);
            }

            if (notification.Status == NotificationStatus.Deleted)
            {
                throw new CannotModifyDeletedNotificationException(notification.NotificationId);
            }

            notification.Status = request.Status;

            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}