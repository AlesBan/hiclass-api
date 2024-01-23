using System.Diagnostics;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPhoto;

public class UpdateUserPhotoCommandHandler : IRequestHandler<UpdateUserPhotoCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public UpdateUserPhotoCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserPhotoCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        user.PhotoUrl = request.NewPhotoUrl;

        _context.Users.Attach(user).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}