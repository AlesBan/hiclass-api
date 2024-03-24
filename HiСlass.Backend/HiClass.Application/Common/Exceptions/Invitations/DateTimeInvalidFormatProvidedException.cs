using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class DateTimeInvalidFormatProvidedException : Exception, IUiException
{
    public DateTimeInvalidFormatProvidedException() : base("Invalid date-time format provided.")
    {
    }
}