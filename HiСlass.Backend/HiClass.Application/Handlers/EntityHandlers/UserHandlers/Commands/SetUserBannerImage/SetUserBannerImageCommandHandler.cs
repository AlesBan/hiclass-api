using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.SetUserImage;
using HiClass.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.SetUserBannerImage;

public class SetUserBannerImageCommandHandler: IRequestHandler<SetUserBannerImageCommand, string>
{
    
    private readonly ISharedLessonDbContext _context;

    public SetUserBannerImageCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(SetUserBannerImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        user.BannerImageUrl = request.ImageUrl;

        _context.Users.Attach(user).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        
        return user.BannerImageUrl;
    }
}