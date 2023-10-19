using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMS.Infrastructure.CustomValidationAnnotation
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
