using System;
using System.ComponentModel.DataAnnotations;

namespace Tools.Asp.net_MVC.Attributes
{
    public class DateValidationAttribute : ValidationAttribute
    {
        private readonly bool _isRequired;
        private readonly string _requiredErrorMessage;
        private readonly string _malformedErrorMessage;

        public DateValidationAttribute(bool isRequired, string requiredErrorMessage, string malformedErrorMessage)
		{
			_isRequired = isRequired;
			_requiredErrorMessage = requiredErrorMessage;
			_malformedErrorMessage = malformedErrorMessage;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			try
			{
				if (string.IsNullOrWhiteSpace((string)value) && !_isRequired)
					return ValidationResult.Success;
				if (string.IsNullOrWhiteSpace((string)value) && _isRequired)
					return new ValidationResult(_requiredErrorMessage);
			    
                DateTime.Parse((string)value);
                return ValidationResult.Success;
				
			}
			catch (FormatException)
			{
				return new ValidationResult(_malformedErrorMessage);
			}
		}
    }
}
