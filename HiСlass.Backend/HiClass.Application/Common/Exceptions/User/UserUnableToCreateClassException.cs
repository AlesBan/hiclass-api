using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User;

public class UserUnableToCreateClassException : Exception, IUiException
{
    public UserUnableToCreateClassException() : base("Expert unable to create class.")
    {
        
    }
}