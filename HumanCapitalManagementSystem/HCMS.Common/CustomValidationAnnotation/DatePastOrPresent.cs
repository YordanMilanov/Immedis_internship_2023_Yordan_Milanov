namespace HCMS.Common.CustomValidationAnnotation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DatePastOrPresent : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date <= DateTime.UtcNow;
            }
            return false;
        }
    }
}
