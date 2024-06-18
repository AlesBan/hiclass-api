using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Server;

public class DatabaseConnectionException: Exception, IServerException
{
    public DatabaseConnectionException() : base("Database connection error.")
    {
        
    }
}