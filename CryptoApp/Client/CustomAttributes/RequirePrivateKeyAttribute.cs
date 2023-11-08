using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Client.CustomAttributes;

public class RequirePrivateKeyAttribute : ValidationAttribute
{
    private readonly string _boolPropertyName;
    private readonly bool _requiredBool;

    private readonly string _propertyName;
    private readonly string[] _requiredValues;

    public RequirePrivateKeyAttribute(string boolPropertyName, bool requiredBool, 
        string stringPropertyName, params string[] requiredValues)
    {
        _boolPropertyName = boolPropertyName;
        _requiredBool = requiredBool;
        _propertyName = stringPropertyName;
        _requiredValues = requiredValues;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var stringProperty = validationContext.ObjectType.GetProperty(_propertyName);
        var boolProperty = validationContext.ObjectType.GetProperty(_boolPropertyName);

        if (stringProperty is not null 
            && boolProperty is not null)
        {
            var propertyStringValue = stringProperty.GetValue(validationContext.ObjectInstance);
            var propertyBoolValue = boolProperty.GetValue(validationContext.ObjectInstance);

            if ((propertyStringValue is string selectedProperty && _requiredValues.Contains(selectedProperty))
                && propertyBoolValue is bool boolSelectedProperty && _requiredBool == boolSelectedProperty)
            {
                if (value is null /*|| string.IsNullOrWhiteSpace(value.ToString())*/)
                {
                    return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                }
            }
        }

        return ValidationResult.Success;
    }
}
