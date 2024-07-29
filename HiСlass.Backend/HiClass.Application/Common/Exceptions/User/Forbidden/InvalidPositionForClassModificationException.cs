using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User.Forbidden;

public class InvalidPositionForClassModificationException : Exception, IUiForbiddenException
{
    public InvalidPositionForClassModificationException() : base("Expert unable to create or edit class.")
    {
        
    }
}