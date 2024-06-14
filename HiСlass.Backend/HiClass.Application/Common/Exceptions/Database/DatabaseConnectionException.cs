using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Database;

public class DatabaseConnectionException: Exception, IServerException
{
    public DatabaseConnectionException() : base("Database connection error.")
    {
        
    }
}