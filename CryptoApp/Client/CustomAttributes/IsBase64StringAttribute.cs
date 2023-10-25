using CryptoApp.Client.Extensions;
using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Client.CustomAttributes;

internal class IsBase64StringAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return false;
        }

        string s = (string)value;

        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        return s.IsBase64String();
    }
}
