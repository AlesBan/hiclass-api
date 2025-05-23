using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Handlers.EntityHandlers.CityHandlers.Queries.GetCity;
using HiClass.Application.Handlers.EntityHandlers.CountryHandlers.Queries.GetCountryByTitle;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditPersonalInfo;

public class EditPersonalInfoCommandHandler : IRequestHandler<EditPersonalInfoCommand, User>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public EditPersonalInfoCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<User> Handle(EditPersonalInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        var country = await _mediator.Send(new GetCountryByTitleQuery(request.CountryTitle), cancellationToken);

        var city = await _mediator.Send(new GetCityQuery()
        {
            Title = request.CityTitle,
            CountryId = country.CountryId
        }, cancellationToken);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.IsATeacher = request.IsATeacher;
        user.IsAnExpert = request.IsAnExpert;
        user.Country = country;
        user.City = city;
        user.Description = request.Description;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        user = await _context.Users
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
            .FirstOrDefaultAsync(u =>
                u.UserId == request.UserId, cancellationToken: cancellationToken);
        
        return user!;
    }
}