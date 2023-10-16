using System.ComponentModel.DataAnnotations;
namespace HCMS.Infrastructure.CustomValidationAnnotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NoWhitespaceAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string input)
            {
                // Check if the string contains whitespace characters
                return !input.Contains(" ");
            }
            return false; // Return false for non-string values
        }
    }
}
