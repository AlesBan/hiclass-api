using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByEmail;

public class GetBlankUserByEmailQueryHandler : IRequestHandler<GetBlankUserByEmailQuery, User>
{
    private readonly ISharedLessonDbContext _context;

    public GetBlankUserByEmailQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetBlankUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Email == request.Email, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByEmailException(request.Email);
        }

        return user;
    }
}