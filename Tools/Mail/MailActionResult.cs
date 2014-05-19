using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tools.Mail;

namespace Tools.CustomActionResults
{
	/// <summary>
	/// Encapsulates a custom action result for sending a mailhelper instance as a result.
	/// </summary>
	public class MailHelperActionResult : ActionResult
	{
		private readonly string _emailTemplatePath;
		//private readonly string _pureFileName;
		private readonly Encoding _fileEncoding;
		MailHelper _mailHelperInstance;

		public MailHelperActionResult(MailHelper mailHelperInstance, Encoding fileEncoding)
		{
			_mailHelperInstance = mailHelperInstance;
			//_pureFileName = completefilePath.Substring(completefilePath.LastIndexOf('\\'));
			_fileEncoding = fileEncoding;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "text/html";
			context.HttpContext.Response.ContentEncoding = _fileEncoding;
			//byte[] fileContents = System.IO.File.ReadAllBytes(_completefilePath);

			_mailHelperInstance.ProcessReplacements();
			context.HttpContext.Response.Write(_mailHelperInstance.Content);
			//context.HttpContext.Response.BinaryWrite(fileContents);
			//context.HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + _pureFileName + "\"");
		}
	}
}
