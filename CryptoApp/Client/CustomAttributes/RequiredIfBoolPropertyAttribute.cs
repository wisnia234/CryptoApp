using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Client.CustomAttributes;

internal class RequiredIfBoolPropertyAttribute : ValidationAttribute
{
    private readonly string _propertyName;
    private readonly bool _condition;

    public RequiredIfBoolPropertyAttribute(string propertyName, bool condition)
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
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                }
            }
        }

        return ValidationResult.Success;
    }

}
