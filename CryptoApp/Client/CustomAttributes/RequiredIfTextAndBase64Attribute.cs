using System.ComponentModel.DataAnnotations;
using CryptoApp.Client.Extensions;

namespace CryptoApp.Client.CustomAttributes;

internal class RequiredIfTextAndBase64Attribute : ValidationAttribute
{
    private readonly string _propertyName;
    private readonly bool _condition;

    public RequiredIfTextAndBase64Attribute(string propertyName, bool condition)
    {
        _propertyName = propertyName;
        _condition = condition;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_propertyName);
        if (property != null)
        {
            var propertyValue = property.GetValue(validationContext.ObjectInstance);
            if (propertyValue is bool boolValue && boolValue == _condition)
            {
                if (!value.ToString().IsBase64String())
                {
                    return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                }
            }
        }

        return ValidationResult.Success;
    }
}
