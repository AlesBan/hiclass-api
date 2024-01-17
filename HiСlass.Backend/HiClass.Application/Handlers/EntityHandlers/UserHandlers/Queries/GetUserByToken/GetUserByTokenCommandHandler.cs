using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByToken;

public class GetUserByTokenCommandHandler : IRequestHandler<GetUserByTokenCommand, User>
{
    private readonly ISharedLessonDbContext _context;

    public GetUserByTokenCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetUserByTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.AccessToken == request.Token, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(request.Token);
        }

        return user;
    }
}