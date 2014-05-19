using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Safe_Web.Models
{
	public class ExceptionTagEvent
	{
		public string OmnitureEventName { get; set; }
		public string Var25 { get; set; }
	}

	public class TaggageViewModel
	{
		public string OmnitureCanalUrl { get; set; }
		public string PageName { get; set; }
		public ExceptionTagEvent ExceptionTagEvent { get; set; }
	}
}