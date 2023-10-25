using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Client.CustomAttributes;

internal class RequiredIfSelectedPropertyAttribute : ValidationAttribute
{
    private readonly string _propertyName;
    private readonly string[] _requiredValues;

    public RequiredIfSelectedPropertyAttribute(string propertyName, params string[] requiredValues)
    {
        _propertyName = propertyName;
        _requiredValues = requiredValues;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_propertyName);
        if (property != null)
        {
            var propertyValue = property.GetValue(validationContext.ObjectInstance);
            if (propertyValue is string selectedProperty && _requiredValues.Contains(selectedProperty))
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
