namespace HiClass.Application.Helpers.CommandHandlerHelper;

public interface ICommandHandlerHelper
{
    public Task<T> GetObjectFromDatabaseById<T>(Guid id) where T : class;
}