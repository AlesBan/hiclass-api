using System.Text;
using AutoMapper;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Common.Exceptions.User.Forbidden;
using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByClass;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByEmail;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserById;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Institution;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Helpers.UserHelper;

public class UserHelper : IUserHelper
{
    private readonly IMapper _mapper;

    public UserHelper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<User> GetUserById(Guid userId, IMediator mediator)
    {
        var user = await mediator.Send(new GetUserByIdQuery(userId));

        return user;
    }

    public async Task<User> GetUserByEmail(string email, IMediator mediator)
    {
        try
        {
            var user = await mediator.Send(new GetUserByEmailQuery(email));
            return user;
        }
        catch (UserNotFoundException)
        {
            throw new InvalidInputCredentialsException("User with this email does not exist");
        }
    }

    public async Task<Guid> GetUserIdByClassId(Guid classId, IMediator mediator)
    {
        var command = new GetUserIdByClassIdQuery(classId);

        var userId = await mediator.Send(command);

        return userId;
    }

    public void CheckUserVerification(User user)
    {
        if (!user.IsVerified)
        {
            throw new UserNotVerifiedException(user.UserId);
        }
    }



    public async Task<UserProfileDto> MapUserToUserProfileDto(User user)
    {
        var userProfileDto = _mapper.Map<UserProfileDto>(user);
        userProfileDto.LanguageTitles = user.UserLanguages.Select(ul => ul.Language.Title).ToList();
        userProfileDto.DisciplineTitles = user.UserDisciplines.Select(ud => ud.Discipline.Title).ToList();
        userProfileDto.Institution = _mapper.Map<InstitutionDto>(user.Institution);
        userProfileDto.GradeNumbers = user.UserGrades.Select(ug => ug.Grade.GradeNumber).ToList();
        userProfileDto.ClasseDtos = await MapClassProfileDtos(user.Classes.ToList());
        return userProfileDto;
    }

    public string GenerateVerificationCode()
    {
        var random = new Random();
        var verificationCodeBuilder = new StringBuilder();

        for (var i = 0; i < 6; i++)
        {
            verificationCodeBuilder.Append(random.Next(0, 10));
        }

        return verificationCodeBuilder.ToString();
    }

    public void CheckResetTokenExpiration(User user)
    {
        if (user.ResetTokenExpires < DateTime.Now)
        {
            throw new ResetTokenHasExpiredException(user.UserId, user.PasswordResetToken ?? "");
        }
    }

    public string GeneratePasswordResetCode()
    {
        var random = new Random();
        var verificationCodeBuilder = new StringBuilder();

        for (var i = 0; i < 6; i++)
        {
            verificationCodeBuilder.Append(random.Next(0, 10));
        }

        return verificationCodeBuilder.ToString();
    }

    public void CheckResetPasswordCode(User user, string code)
    {
        if (user.PasswordResetCode != code)
        {
            throw new InvalidResetPasswordCodeException();
        }
    }

    private static Task<List<ClassProfileDto>> MapClassProfileDtos(IEnumerable<Class> classes)
    {
        return Task.FromResult(classes.Select(c => new ClassProfileDto
            {
                ClassId = c.ClassId,
                Title = c.Title,
                UserFullName = c.User.FullName,
                UserRating = c.User.Rating,
                Grade = c.Grade.GradeNumber,
                Languages = c.ClassLanguages.Select(cl => cl.Language.Title).ToList(),
                Disciplines = c.ClassDisciplines.Select(cd => cd.Discipline.Title).ToList(),
                ImageUrl = c.ImageUrl!
            })
            .ToList());
    }
}