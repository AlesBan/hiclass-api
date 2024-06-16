using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.SetUserImage;

public class SetUserImageCommandHandler : IRequestHandler<SetUserImageCommand, string>
{
    private readonly ISharedLessonDbContext _context;

    public SetUserImageCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(SetUserImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        user.ImageUrl = request.ImageUrl;

        _context.Users.Attach(user).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        
        return user.ImageUrl;
    }
}