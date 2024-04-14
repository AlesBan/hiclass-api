using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User.Forbidden;

public class UserUnableToCreateClassException : Exception, IUiForbiddenException
{
    public UserUnableToCreateClassException() : base("Expert unable to create class.")
    {
        
    }
}