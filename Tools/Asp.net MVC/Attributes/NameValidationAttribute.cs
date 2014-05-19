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
	public class NameValidationAttribute : ValidationAttribute
	{
		private readonly bool _isRequired;
		private readonly string _requiredErrorMessage;
		private readonly string _malformedErrorMessage;
		private readonly int _minLength;
		private readonly int _maxlength;

		public NameValidationAttribute(bool isRequired, string requiredErrorMessage, string malformedErrorMessage, int minLength, int maxLength)
		{
			_isRequired = isRequired;
			_requiredErrorMessage = requiredErrorMessage;
			_malformedErrorMessage = malformedErrorMessage;
			_minLength = minLength;
			_maxlength = maxLength;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			//validationContext.


			if (string.IsNullOrWhiteSpace((string)value) && !_isRequired)
					return ValidationResult.Success;
			if (string.IsNullOrWhiteSpace((string)value) && _isRequired)
				return new ValidationResult(_requiredErrorMessage);
			string regPattern = string.Format(@"^[a-zA-Záàâäãéèêëíìîïĩóòôöõúüîôûúùýñçÿ\s.\-_' \+\&]{{{0},{1}}}$", _minLength, _maxlength);
			Regex reg = new Regex(regPattern);
			if(!reg.IsMatch((string)value))
				return new ValidationResult(_malformedErrorMessage);
			return ValidationResult.Success;
		}
	}
}
