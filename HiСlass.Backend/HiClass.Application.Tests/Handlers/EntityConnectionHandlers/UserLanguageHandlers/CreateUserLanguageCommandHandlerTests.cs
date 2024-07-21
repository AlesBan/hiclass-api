using HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguageHandlers.Commands.CreateUserLanguages;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.CreateUserLanguages;
using HiClass.Application.Tests.Common;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityConnectionHandlers.UserLanguageHandlers;

public class CreateUserLanguageCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task CreateUserLanguagesCommandHandler_Handle_ShouldCreateUserLanguages()
    {
        // Arrange
        var user = Context.Users.SingleOrDefault(x =>
            x.UserId == SharedLessonDbContextFactory.UserRegisteredId);
        var language = Context.Languages.SingleOrDefault(x => x.Title == "English")!;

        var handler = new CreateUserLanguagesCommandHandler(Context);

        // Act
        await handler.Handle(new CreateUserLanguagesCommand()
        {
            UserId = user!.UserId,
            LanguageIds = new[] { language.LanguageId }
        }, CancellationToken.None);

        // Assert
        Assert.NotEmpty(Context.UserLanguages
            .Where(ud => ud.UserId == user!.UserId &&
                         ud.Language.Title == language.Title));
    }
}