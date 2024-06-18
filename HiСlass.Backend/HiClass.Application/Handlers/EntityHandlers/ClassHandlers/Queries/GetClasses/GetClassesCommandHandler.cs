using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClasses;

public class GetClassesCommandHandler : IRequestHandler<GetClassesCommand, IEnumerable<Class>>
{
    private readonly ISharedLessonDbContext _context;

    public GetClassesCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Class>> Handle(GetClassesCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.ReceivedFeedbacks)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassDisciplines)
            .ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .Include(u => u.Classes)
            .ThenInclude(c => c.Grade)
            .FirstOrDefaultAsync(u =>
                u.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        return user.Classes;
    }
}