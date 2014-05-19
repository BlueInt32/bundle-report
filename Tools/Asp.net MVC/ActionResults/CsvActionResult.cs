using System.Text;
using System.Web.Mvc;

namespace Tools.CustomActionResults
{
	/// <summary>
	/// Encapsulates a custom action result for sending a csv file directly as a response.
	/// </summary>
	public class CsvActionResult : ActionResult
	{
		private readonly string _completefilePath;
		private readonly string _pureFileName;
		private readonly Encoding _fileEncoding;


		/// <summary>
		/// Encapsulates a custom action result for sending a csv file directly as a response.
		/// </summary>
		/// <param name="completefilePath">The Server File to send. Must use \\ as path delimiters to extract the filename. </param>
		/// <param name="fileEncoding"></param>
		public CsvActionResult(string completefilePath, Encoding fileEncoding )
		{
			_completefilePath = completefilePath;
			_pureFileName = completefilePath.Substring(completefilePath.LastIndexOf('\\'));
			_fileEncoding = fileEncoding;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "text/plain";
			context.HttpContext.Response.ContentEncoding = _fileEncoding;
			byte[] fileContents = System.IO.File.ReadAllBytes(_completefilePath);
			context.HttpContext.Response.BinaryWrite(fileContents);
			context.HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + _pureFileName + "\"");
		}
	}
}
