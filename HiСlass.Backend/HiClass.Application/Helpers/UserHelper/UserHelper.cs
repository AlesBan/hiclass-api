using System.Text;
using AutoMapper;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Common.Exceptions.User.Forbidden;
using HiClass.Application.Common.Exceptions.User.ResettingPassword;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByClass;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByEmail;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserById;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Institution;
using HiClass.Application.Models.Invitations.Feedbacks;
using HiClass.Application.Models.Invitations.Invitations;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Communication;
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

    public void CheckUserCreateAccountAbility(User user)
    {
        if (user.IsCreatedAccount)
        {
            throw new UserAlreadyHasAccountException(user.UserId);
        }
    }

    public Task<UserProfileDto> MapUserToUserProfileDto(User user)
    {
        var userProfileDto = _mapper.Map<UserProfileDto>(user);
        userProfileDto.LanguageTitles = user.UserLanguages.Select(ul => ul.Language.Title).ToList();
        userProfileDto.DisciplineTitles = user.UserDisciplines.Select(ud => ud.Discipline.Title).ToList();
        userProfileDto.Institution = _mapper.Map<InstitutionDto>(user.Institution);
        userProfileDto.GradeNumbers = user.UserGrades.Select(ug => ug.Grade.GradeNumber).ToList();
        userProfileDto.ClassDtos = MapClassProfileDtos(user.Classes.ToList());
        userProfileDto.FeedbackDtos = MapFeedbackDtos(user.ReceivedFeedbacks.ToList());
        return Task.FromResult(userProfileDto);
    }

    public CreateAccountUserProfileDto MapUserToCreateAccountUserProfileDto(User user)
    {
        var userProfileDto = _mapper.Map<CreateAccountUserProfileDto>(user);
        userProfileDto.LanguageTitles = user.UserLanguages.Select(ul => ul.Language.Title).ToList();
        userProfileDto.DisciplineTitles = user.UserDisciplines.Select(ud => ud.Discipline.Title).ToList();
        userProfileDto.Institution = _mapper.Map<InstitutionDto>(user.Institution);
        userProfileDto.GradeNumbers = user.UserGrades.Select(ug => ug.Grade.GradeNumber).ToList();
        userProfileDto.ClassDtos = MapClassProfileDtos(user.Classes.ToList());
        return userProfileDto;
    }


    public async Task<FullUserProfileDto> MapUserToFullUserProfileDto(User user)
    {
        var userProfileDto = _mapper.Map<FullUserProfileDto>(user);
        userProfileDto.LanguageTitles = user.UserLanguages.Select(ul => ul.Language.Title).ToList();
        userProfileDto.DisciplineTitles = user.UserDisciplines.Select(ud => ud.Discipline.Title).ToList();
        userProfileDto.Institution = _mapper.Map<InstitutionDto>(user.Institution);
        userProfileDto.GradeNumbers = user.UserGrades.Select(ug => ug.Grade.GradeNumber).ToList();
        userProfileDto.ClassDtos = MapClassProfileDtos(user.Classes.ToList());
        userProfileDto.ReceivedInvitationDtos = MapInvitationDtos(user.ReceivedInvitations.ToList());
        userProfileDto.ReceivedFeedbackDtos = MapFeedbackDtos(user.ReceivedFeedbacks.ToList());
        return userProfileDto;
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

    public void CheckResetTokenValidation(User user)
    {
        if (user.PasswordResetToken == null)
        {
            throw new InvalidResetTokenProvidedException();
        }

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

    private static List<ClassProfileDto> MapClassProfileDtos(IEnumerable<Class> classes)
    {
        return classes.Select(c => new ClassProfileDto
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
            .ToList();
    }

    private static List<FeedbackDto> MapFeedbackDtos(IEnumerable<Feedback> feedbacks)
    {
        var feedbackDtos = feedbacks.Select(feedback => new FeedbackDto
            {
                FeedbackId = feedback.FeedbackId,
                InvitationId = feedback.InvitationId,
                UserSenderId = feedback.UserSenderId,
                UserSenderFullName = feedback.UserSender.FullName,
                UserSenderImageUrl = feedback.UserSender.ImageUrl!,
                UserSenderFullLocation = feedback.UserSender.FullLocation,
                WasTheJointLesson = feedback.WasTheJointLesson,
                FeedbackText = feedback.FeedbackText,
                Rating = feedback.Rating,
                CreatedAt = feedback.CreatedAt
            })
            .ToList();

        return feedbackDtos;
    }
    
    private IEnumerable<InvitationDto> MapInvitationDtos(IEnumerable<Invitation> toList)
    {
        var invitations = toList.Select(invitation => _mapper.Map<InvitationDto>(invitation));
        return invitations;
    }
}