using AutoMapper;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.CreateUserDisciplines;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.CreateUserGrade;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguageHandlers.Commands.CreateUserLanguages;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.CreateUserLanguages;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.CreateUserAccount;

public class CreateUserAccountCommandHandler : IRequestHandler<CreateUserAccountCommand, TokenModelResponseDto>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITokenHelper _tokenHelper;
    private readonly IMediator _mediator;

    public CreateUserAccountCommandHandler(ISharedLessonDbContext serviceDbContext,
        ITokenHelper tokenHelper, IMapper mapper, IMediator mediator)
    {
        _context = serviceDbContext;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<TokenModelResponseDto> Handle(CreateUserAccountCommand request,
        CancellationToken cancellationToken)
    {
        var user = _context
            .Users
            .Include(u => u.Country)
            .Include(u => u.City)
            .Include(u => u.Institution)
            .Include(u => u.UserDevices)
            .ThenInclude(ud => ud.Device)
            .FirstOrDefault(u => u.UserId == request.UserId);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.IsATeacher = request.IsATeacher;
        user.IsAnExpert = request.IsAnExpert;
        user.CityId = request.CityId;
        user.CountryId = request.CountryId;
        user.InstitutionId = request.InstitutionId;

        user.IsCreatedAccount = true;
        user.CreatedAt = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        var tokenUserDto = _mapper.Map<CreateTokenDto>(user);
        var newAccessToken = _tokenHelper.CreateAccessToken(tokenUserDto);
        var newRefreshToken = _tokenHelper.CreateRefreshToken(tokenUserDto);

        var userDevice = user.UserDevices
            .FirstOrDefault(ud => ud.Device.DeviceToken == request.DeviceToken);

        if (userDevice != null)
        {
            userDevice.IsActive = true;
            userDevice.LastActive = DateTime.UtcNow;
            userDevice.RefreshToken = newRefreshToken;
        }
        else
        {
            var command = new CreateUserDeviceCommand
            {
                DeviceToken = request.DeviceToken,
                UserId = user.UserId,
                RefreshToken = newRefreshToken,
            };
            await _mediator.Send(command, cancellationToken);
        }

        await SeedUserLanguages(user.UserId, request, cancellationToken);
        await SeedUserDisciplines(user.UserId, request, cancellationToken);
        await SeedUserGrades(user.UserId, request, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new TokenModelResponseDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    private async Task SeedUserLanguages(Guid userId, CreateUserAccountCommand request,
        CancellationToken cancellationToken = default)
    {
        var handler = new CreateUserLanguagesCommandHandler(_context);
        await handler.Handle(new CreateUserLanguagesCommand
        {
            UserId = userId,
            LanguageIds = request.LanguageIds
        }, cancellationToken);
    }

    private async Task SeedUserDisciplines(Guid userId, CreateUserAccountCommand request,
        CancellationToken cancellationToken = default)
    {
        var handler = new CreateUserDisciplinesCommandHandler(_context);
        await handler.Handle(new CreateUserDisciplinesCommand()
        {
            UserId = userId,
            DisciplineIds = request.DisciplineIds
        }, cancellationToken);
    }

    private async Task SeedUserGrades(Guid userId, CreateUserAccountCommand request,
        CancellationToken cancellationToken = default)
    {
        var handler = new CreateUserGradesCommandHandler(_context);
        await handler.Handle(new CreateUserGradesCommand()
        {
            UserId = userId,
            GradeIds = request.GradeIds
        }, cancellationToken);
    }
}