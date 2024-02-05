using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Database;

public class DatabaseConnectionException: Exception, IDbException
{
    public DatabaseConnectionException() : base()
    {
        
    }
}