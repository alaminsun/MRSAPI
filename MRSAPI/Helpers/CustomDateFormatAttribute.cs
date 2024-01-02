namespace MRSAPI.Helpers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomDateFormatAttribute : ValidationAttribute
    {
        private readonly string _dateFormat;

        public CustomDateFormatAttribute(string dateFormat)
        {
            _dateFormat = dateFormat;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (DateOnly.TryParseExact(value.ToString(), _dateFormat, null, System.Globalization.DateTimeStyles.None, out _))
                {
                    return ValidationResult.Success;
                }

                //if (value, _dateFormat)
                //{
                //    return ValidationResult.Success;
                //}
                else
                {
                    return new ValidationResult($"The field {validationContext.DisplayName} must be in the format {_dateFormat}.");
                }
            }

            return ValidationResult.Success;
        }
    }

}
