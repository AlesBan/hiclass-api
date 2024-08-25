using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Queries.GetInstitution;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserInstitution;

public class EditUserInstitutionCommandHandler : IRequestHandler<EditUserInstitutionCommand, User>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public EditUserInstitutionCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<User> Handle(EditUserInstitutionCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users
            .AsNoTracking()
            .FirstOrDefault(u =>
                u.UserId == request.UserId);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        var institution = await _mediator.Send(new GetInstitutionQuery()
        {
            InstitutionTitle = request.InstitutionTitle,
            Address = request.Address,
            Types = request.Types
        }, cancellationToken);

        user.InstitutionId = institution.InstitutionId;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        user = _context.Users
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .Include(u => u.Classes)
            .ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes)
            .ThenInclude(c => c.Grade)
            .Include(u => u.UserDisciplines)
            .ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages)
            .ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades)
            .ThenInclude(ug => ug.Grade)
            .FirstOrDefault(u =>
                u.UserId == request.UserId);
        return user;
    }
}