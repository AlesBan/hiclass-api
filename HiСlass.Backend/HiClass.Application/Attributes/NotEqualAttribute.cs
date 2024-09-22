using System.ComponentModel.DataAnnotations;
using HiClass.Application.Common.Exceptions.Client;
using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Common.Exceptions.Properties;

namespace HiClass.Application.Attributes;

public class NotEqualAttribute : ValidationAttribute
{
    private readonly string _propertyName;
    private readonly string _otherPropertyTitle;

    public NotEqualAttribute(string otherPropertyTitle, string propertyName)
    {
        _otherPropertyTitle = otherPropertyTitle;
        _propertyName = propertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyTitle);

        if (otherPropertyInfo == null)
            throw new NullPropertyException(_otherPropertyTitle);

        var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

        if (otherPropertyValue == null)
            throw new NullPropertyException(_propertyName);

        if (!Equals(value, otherPropertyValue))
        {
            return ValidationResult.Success;
        }

        var errorMessage = _propertyName switch
        {
            _ when _propertyName.Contains("Class") =>
                "ClassSenderId and ClassReceiverId must not be the same.",

            _ when _propertyName.Contains("User") =>
                "UserSenderId and UserReceiverId must not be the same.",

            _ when _propertyName.Contains("String") && _otherPropertyTitle.Contains("Password") =>
                "NewPassword cannot be equal to OldPassword.",

            _ => "Fields must not be equal."
        };

        throw new EqualityPropertiesException(errorMessage);
    }
}