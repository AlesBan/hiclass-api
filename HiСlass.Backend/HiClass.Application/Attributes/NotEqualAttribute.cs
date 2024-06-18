using System.ComponentModel.DataAnnotations;
using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Common.Exceptions.Properties;

namespace HiClass.Application.Attributes;

public class NotEqualAttribute : ValidationAttribute
{
    private readonly string _propertyName;
    private readonly string _otherProperty;

    public NotEqualAttribute(string otherProperty, string propertyName)
    {
        _propertyName = propertyName;
        _otherProperty = otherProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);

        if (otherPropertyInfo == null)
            throw new NullPropertyException(_otherProperty);

        var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

        if (otherPropertyValue == null)
            throw new NullPropertyException(_propertyName);

        if (!Equals(value, otherPropertyValue))
        {
            return ValidationResult.Success;
        }

        if (_propertyName.Contains("Class"))
        {
            throw new EqualityInvitationClassIdPropertiesException();
        }

        if (_propertyName.Contains("User"))
        {
            throw new EqualityInvitationUserIdPropertiesException();
        }
        
        return ValidationResult.Success;
    }
}