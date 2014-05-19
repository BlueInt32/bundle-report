using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Tools.Asp.net_MVC.Attributes
{
	public class EmailValidationAttribute : ValidationAttribute
	{
		private readonly bool _isRequired;
		private readonly string _requiredErrorMessage;
		private readonly string _malformedErrorMessage;


		public EmailValidationAttribute(bool isRequired, string requiredErrorMessage, string malformedErrorMessage)
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

				if (IsValidEmail((string)value))
				{
					new MailAddress((string)value);
					return ValidationResult.Success;
				}
				return new ValidationResult(_malformedErrorMessage);
			}
			catch (FormatException)
			{
				return new ValidationResult(_malformedErrorMessage);
			}
		}

		bool IsValidEmail(string strIn)
		{
			// Return true if strIn is in valid e-mail format.
			return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
		}
	}
}
