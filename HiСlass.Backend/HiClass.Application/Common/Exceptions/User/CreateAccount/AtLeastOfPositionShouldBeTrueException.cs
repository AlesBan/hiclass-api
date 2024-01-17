using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User.CreateAccount;

public class AtLeastOfPositionShouldBeTrueException : Exception, IUiException
{
    public AtLeastOfPositionShouldBeTrueException() : base("At least one of position should be true")
    {
        
    }
}