using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Asp.net_MVC.Models
{
	public abstract class IMessageViewModel
	{
		public bool ShowMessage { get; set; }
		public bool InError { get; set; }
		public string Message { get; set; }
	}
}
