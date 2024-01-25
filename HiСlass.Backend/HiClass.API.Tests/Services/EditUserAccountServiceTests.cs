using AutoMapper;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Dtos.InstitutionDtos;
using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Dtos.UserDtos.Update;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.CreateUserDisciplines;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.UpdateUserDisciplines;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.CreateUserGrade;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.UpdateUserGrades;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.CreateUserLanguages;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.UpdateUserLanguages;
using HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplinesByTitles;
using HiClass.Application.Handlers.EntityHandlers.GradeHandlers.Queries.GetGrades;
using HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Commands.CreateInstitution;
using HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Queries.GetInstitution;
using HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetLanguagesByTitles;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdatePersonalInfo;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateProfessionalInfo;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserEmail;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserInstitution;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserById;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Tests.Common;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Location;
using HiClass.Domain.Entities.Main;
using HiClass.Domain.Enums.EntityTypes;
using HiClass.Infrastructure.Services.EditUserServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HiClass.API.Tests.Services;

public class EditUserAccountServiceTests : TestCommonBase
{
    [Fact]
    public async Task EditUserPersonalInfoAsync_ReturnsOkObjectResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var userHelperMock = new Mock<IUserHelper>();
        var tokenHelperMock = new Mock<ITokenHelper>();

        var userId = SharedLessonDbContextFactory.UserAId;
        var user = Context.Users.FirstOrDefault(u =>
            u.UserId == userId);

        mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(user!);
        user!.IsATeacher = true;
        user.IsAnExpert = true;
        user.FirstName = "NewName";
        user.LastName = "NewLastName";
        user.Description = "NewDescription";
        user.City = new City
        {
            Title = "NewCity",
            CountryId = SharedLessonDbContextFactory.CityAId,
        };
        user.Country = new Country()
        {
            Title = "NewCountry",
            CountryId = SharedLessonDbContextFactory.CountryAId,
        };
        user.Description = "NewDescription";

        mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePersonalInfoCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var requestUserDto = new UpdatePersonalInfoRequestDto
        {
            IsATeacher = true,
            IsAnExpert = true,
            FirstName = "NewName",
            LastName = "NewLastName",
            CityTitle = "NewCity",
            CountryTitle = "NewCountry",
            Description = "NewDescription"
        };

        var confMapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<User, UserProfileDto>();
                cfg.CreateMap<Institution, InstitutionDto>();
            });

        var mapper = confMapper.CreateMapper();

        var service = new EditUserAccountService(userHelperMock.Object, tokenHelperMock.Object);

        // Act
        var result = await service.EditUserPersonalInfoAsync(userId, requestUserDto, mediatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var userProfileDto = Assert.IsType<UserProfileDto>(okResult.Value);

        Assert.Equal("NewName", userProfileDto.FirstName);
        Assert.Equal("NewLastName", userProfileDto.LastName);
        Assert.Equal("NewCity", userProfileDto.CityTitle);
        Assert.Equal("NewCountry", userProfileDto.CountryTitle);
        Assert.Equal("NewDescription", userProfileDto.Description);
    }

    [Fact]
    public async Task EditUserPersonalInfoAsync_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var userHelperMock = new Mock<IUserHelper>();
        var tokenHelperMock = new Mock<ITokenHelper>();


        var userId = Guid.NewGuid();
        var user = new User()
        {
            UserId = userId,
            Email = "Email",
            // Password = "Password"
        };

        mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
            It.IsAny<CancellationToken>())).ThrowsAsync(new UserNotFoundException(userId));
        user.IsATeacher = true;
        user.IsAnExpert = true;
        user.FirstName = "NewName";
        user.LastName = "NewLastName";
        user.Description = "NewDescription";
        user.City = new City
        {
            Title = "NewCity",
            CountryId = SharedLessonDbContextFactory.CityAId,
        };
        user.Country = new Country()
        {
            Title = "NewCountry",
            CountryId = SharedLessonDbContextFactory.CountryAId,
        };
        user.Description = "NewDescription";

        mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePersonalInfoCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var requestUserDto = new UpdatePersonalInfoRequestDto
        {
            IsATeacher = true,
            IsAnExpert = true,
            FirstName = "NewName",
            LastName = "NewLastName",
            CityTitle = "NewCity",
            CountryTitle = "NewCountry",
            Description = "NewDescription"
        };

        var confMapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<User, UserProfileDto>();
                cfg.CreateMap<Institution, InstitutionDto>();
            });

        var mapper = confMapper.CreateMapper();

        var service = new EditUserAccountService(userHelperMock.Object, tokenHelperMock.Object);

        // Act
        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            await service.EditUserPersonalInfoAsync(userId, requestUserDto, mediatorMock.Object));
    }

    [Fact]
    public async Task EditUserInstitutionAsync_ReturnsOkObjectResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var userHelperMock = new Mock<IUserHelper>();
        var tokenHelperMock = new Mock<ITokenHelper>();

        const string newInstitutionTitle = "NewInstitutionTitle";
        const string newAddress = "NewAddress";
        var types = new[] { "School" };

        var userId = SharedLessonDbContextFactory.UserAId;
        var user = Context.Users.FirstOrDefault(u =>
            u.UserId == userId);

        var updateUserCommand = new UpdateUserInstitutionCommand
        {
            UserId = user!.UserId,
            InstitutionTitle = newInstitutionTitle,
            Address = newAddress,
            Types = types,
        };
        var updateUserHandler = new UpdateUserInstitutionCommandHandler(Context, mediatorMock.Object);

        var getInstitutionQuery = new GetInstitutionQuery()
        {
            InstitutionTitle = newInstitutionTitle,
            Address = newAddress,
            Types = types,
        };
        var getInstitutionHandler = new GetInstitutionQueryHandler(Context, mediatorMock.Object);

        var createInstitutionCommand = new CreateInstitutionCommand()
        {
            InstitutionTitle = newInstitutionTitle,
            Address = newAddress,
            Types = types,
        };
        var createInstitutionQueryHandler = new CreateInstitutionCommandHandler(Context);
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateInstitutionCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await createInstitutionQueryHandler.Handle(createInstitutionCommand, CancellationToken.None));
        mediatorMock.Setup(m => m.Send(It.IsAny<GetInstitutionQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await getInstitutionHandler.Handle(getInstitutionQuery, CancellationToken.None));
        mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(user);
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserInstitutionCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await updateUserHandler.Handle(updateUserCommand, CancellationToken.None));

        var requestUserDto = new UpdateInstitutionRequestDto
        {
            InstitutionTitle = "NewInstitutionTitle",
            Address = "NewAddress",
            Types = new[] { "School" }
        };

        var confMapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<User, UserProfileDto>();
                cfg.CreateMap<Institution, InstitutionDto>();
            });

        var mapper = confMapper.CreateMapper();

        var service = new EditUserAccountService(userHelperMock.Object, tokenHelperMock.Object);
        // Act
        var result = await service.EditUserInstitutionAsync(userId, requestUserDto, mediatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var userProfileDto = Assert.IsType<UserProfileDto>(okResult.Value);

        Assert.Equal("NewInstitutionTitle", userProfileDto.Institution.Title);
        Assert.Equal("NewAddress", userProfileDto.Institution.Address);
    }

    [Fact]
    public async Task EditUserEmailAsync_ReturnsOkObjectResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var userHelperMock = new Mock<IUserHelper>();
        var tokenHelperMock = new Mock<ITokenHelper>();

        const string newEmail = "NewEmail@gmail.com";

        var userId = SharedLessonDbContextFactory.UserAId;
        var user = Context.Users.FirstOrDefault(u =>
            u.UserId == userId);
        var updateUserEmailCommand = new UpdateUserEmailAndRemoveVerificationCommand()
        {
            UserId = user!.UserId,
            Email = newEmail
        };
        var updateUserEmailHandler = new UpdateUserEmailAndRemoveVerificationCommandHandler(Context);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(user);
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserEmailAndRemoveVerificationCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await updateUserEmailHandler
                .Handle(updateUserEmailCommand, CancellationToken.None));

        var requestUserDto = new UpdateUserEmailRequestDto
        {
            Email = newEmail
        };

        var confMapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<User, UserProfileDto>();
                cfg.CreateMap<Institution, InstitutionDto>();
            });

        var mapper = confMapper.CreateMapper();

        var service = new EditUserAccountService(userHelperMock.Object, tokenHelperMock.Object);

        // Act
        var result = await service.EditUserEmailAsync(userId, requestUserDto, mediatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var userProfileDto = Assert.IsType<UserProfileDto>(okResult.Value);

        Assert.NotNull(userProfileDto);
        Assert.Equal(newEmail, userProfileDto.Email);
    }

    [Fact]
    public async Task EditUserPasswordAsync_ReturnsOkObjectResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var userHelperMock = new Mock<IUserHelper>();
        var tokenHelperMock = new Mock<ITokenHelper>();

        const string newPassword = "NewPassword";

        var userId = SharedLessonDbContextFactory.UserAId;
        var user = Context.Users.FirstOrDefault(u =>
            u.UserId == userId);
        // var updateUserPasswordCommand = new UpdateUserPasswordCommand()
        // {
        //     UserId = user!.UserId,
        //     Password = newPassword
        // };
        // var updateUserPasswordHandler = new UpdateUserPasswordCommandHandler(Context);
        // mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
        //     It.IsAny<CancellationToken>())).ReturnsAsync(user);
        // mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserPasswordCommand>(),
        //         It.IsAny<CancellationToken>()))
        //     .ReturnsAsync(await updateUserPasswordHandler
        //         .Handle(updateUserPasswordCommand, CancellationToken.None));

        var requestUserDto = new UpdateUserPasswordHashRequestDto()
        {
            Password = newPassword
        };

        var confMapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<User, UserProfileDto>();
                cfg.CreateMap<Institution, InstitutionDto>();
            });

        var mapper = confMapper.CreateMapper();

        var service = new EditUserAccountService(userHelperMock.Object, tokenHelperMock.Object);

        // Act
        var result = await service.EditUserPasswordAsync(userId, requestUserDto, mediatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var userProfileDto = Assert.IsType<UserProfileDto>(okResult.Value);

        Assert.NotNull(userProfileDto);
    }

    [Fact]
    public async Task EditUserProfessionalInfoAsync_ReturnsOkObjectResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var userHelperMock = new Mock<IUserHelper>();
        var tokenHelperMock = new Mock<ITokenHelper>();

        var languageTitles = new List<string>()
        {
            "English",
            "Russian"
        };
        var disciplineTitles = new List<string>()
        {
            "Chemistry",
            "Biology",
            "Physics"
        };
        var gradeNList = new List<int>()
        {
            7, 8, 9
        };

        var userId = SharedLessonDbContextFactory.UserAId;
        var user = Context.Users.FirstOrDefault(u =>
            u.UserId == userId);

        var updateProfessionalInfoCommand = new UpdateProfessionalInfoCommand()
        {
            UserId = user!.UserId,
            LanguageTitles = languageTitles,
            DisciplineTitles = disciplineTitles,
            GradeNumbers = gradeNList
        };
        var updateProfessionalInfoHandler = new UpdateProfessionalInfoCommandHandler(Context, mediatorMock.Object);


        var getLanguagesQuery = new GetLanguagesByTitlesQuery(languageTitles);
        var getLanguagesHandler = new GetLanguagesByTitlesQueryHandler(Context);

        var getDisciplinesQuery = new GetDisciplinesByTitlesQuery(disciplineTitles);
        var getDisciplinesHandler = new GetDisciplinesByTitlesQueryHandler(Context);

        var getGradesQuery = new GetGradesQuery(gradeNList);
        var getGradesHandler = new GetGradesQueryHandler(Context);


        var languages = await getLanguagesHandler
            .Handle(getLanguagesQuery, CancellationToken.None);

        var disciplines = await getDisciplinesHandler
            .Handle(getDisciplinesQuery, CancellationToken.None);

        var grades = await getGradesHandler
            .Handle(getGradesQuery, CancellationToken.None);

        var createUserLanguagesCommand = new CreateUserLanguagesCommand()
        {
            UserId = user.UserId,
            LanguageIds = languages.Select(l => l.LanguageId).ToList()
        };
        var createUserLanguagesHandler = new CreateUserLanguagesCommandHandler(Context);

        var createUserDisciplinesCommand = new CreateUserDisciplinesCommand()
        {
            UserId = user.UserId,
            DisciplineIds = disciplines.Select(d => d.DisciplineId).ToList()
        };
        var createUserDisciplinesHandler = new CreateUserDisciplinesCommandHandler(Context);

        var createUserGradesCommand = new CreateUserGradesCommand()
        {
            UserId = user.UserId,
            GradeIds = grades.Select(g => g.GradeId).ToList()
        };
        var createUserGradesHandler = new CreateUserGradesCommandHandler(Context);

        mediatorMock.Setup(m => m.Send(It.IsAny<CreateUserLanguagesCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await createUserLanguagesHandler.Handle(createUserLanguagesCommand, CancellationToken.None));
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateUserDisciplinesCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                await createUserDisciplinesHandler.Handle(createUserDisciplinesCommand, CancellationToken.None));
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateUserGradesCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await createUserGradesHandler.Handle(createUserGradesCommand, CancellationToken.None));

        var updateUserLanguagesCommand = new UpdateUserLanguagesCommand
        {
            UserId = user.UserId,
            NewLanguageIds = languages.Select(l => l.LanguageId).ToList()
        };
        var updateUserLanguagesHandler = new UpdateUserLanguagesCommandHandler(Context, mediatorMock.Object);

        var updateUserDisciplinesCommand = new UpdateUserDisciplinesCommand
        {
            UserId = user.UserId,
            NewDisciplineIds = disciplines.Select(d => d.DisciplineId).ToList()
        };
        var updateUserDisciplinesHandler = new UpdateUserDisciplinesCommandHandler(Context, mediatorMock.Object);

        var updateUserGradesCommand = new UpdateUserGradesCommand
        {
            UserId = user.UserId,
            NewGradeIds = grades.Select(g => g.GradeId).ToList()
        };
        var updateUserGradesHandler = new UpdateUserGradesCommandHandler(Context, mediatorMock.Object);

        mediatorMock.Setup(m => m.Send(It.IsAny<GetLanguagesByTitlesQuery>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(languages);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetDisciplinesByTitlesQuery>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(disciplines);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetGradesQuery>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(grades);

        mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(user);
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProfessionalInfoCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await updateProfessionalInfoHandler
                .Handle(updateProfessionalInfoCommand, CancellationToken.None));

        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserLanguagesCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await updateUserLanguagesHandler
                .Handle(updateUserLanguagesCommand, CancellationToken.None));

        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserDisciplinesCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await updateUserDisciplinesHandler
                .Handle(updateUserDisciplinesCommand, CancellationToken.None));

        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserGradesCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(await updateUserGradesHandler
                .Handle(updateUserGradesCommand, CancellationToken.None));

        var requestUserDto = new UpdateProfessionalInfoRequestDto
        {
            Languages = languageTitles,
            Disciplines = disciplineTitles,
            Grades = gradeNList
        };

        var confMapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<User, UserProfileDto>();
                cfg.CreateMap<Institution, InstitutionDto>();
            });

        var mapper = confMapper.CreateMapper();

        var service = new EditUserAccountService(userHelperMock.Object, tokenHelperMock.Object);

        // Act
        var result = await service.EditUserProfessionalInfoAsync(userId, requestUserDto, mediatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var userProfileDto = Assert.IsType<UserProfileDto>(okResult.Value);

        Assert.NotNull(userProfileDto);

        Assert.Equal(languageTitles.Count, userProfileDto.LanguageTitles.Count);
        Assert.Equal(disciplineTitles.Count, userProfileDto.DisciplineTitles.Count);

        Assert.True(languageTitles.OrderBy(x => x)
            .SequenceEqual(userProfileDto.LanguageTitles.OrderBy(x => x)));
        Assert.True(disciplineTitles.OrderBy(x => x)
            .SequenceEqual(userProfileDto.DisciplineTitles.OrderBy(x => x)));
    }
}