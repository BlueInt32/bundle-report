using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools.Asp.net_MVC.Attributes
{
	/// <summary>
	/// Allows you to server_side validate a firstname or a lastname.
	/// Lets you decide if field is required (i.e. string.empty is allowed)
	/// </summary>
	public class ZipCodeValidationAttribute : ValidationAttribute
	{
		private readonly bool _isRequired;
		private readonly string _requiredErrorMessage;
		private readonly string _malformedErrorMessage;

		public ZipCodeValidationAttribute(bool isRequired, string requiredErrorMessage, string malformedErrorMessage)
		{
			_isRequired = isRequired;
			_requiredErrorMessage = requiredErrorMessage;
			_malformedErrorMessage = malformedErrorMessage;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (string.IsNullOrWhiteSpace((string)value) && !_isRequired)
					return ValidationResult.Success;
			if (string.IsNullOrWhiteSpace((string)value) && _isRequired)
				return new ValidationResult(_requiredErrorMessage);
			string regPattern = @"[0-9]{5}$";
			Regex reg = new Regex(regPattern);
			if(!reg.IsMatch((string)value))
				return new ValidationResult(_malformedErrorMessage);
			return ValidationResult.Success;
		}
	}
}
