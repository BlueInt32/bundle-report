using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Tools.Asp.net_MVC.Attributes
{
	public class RequiredToBeTrueAttribute : RequiredAttribute
	{
		public override bool IsValid(object value)
		{
			return value != null && (bool)value;
		}
	}
}
