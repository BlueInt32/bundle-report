using System.Net;
using System.Web.Mvc;

namespace Collecte.WebApp.Controllers
{
	public class ErrorController : Controller
	{
		public ViewResult Page404()
		{
			Response.StatusCode = (int)HttpStatusCode.NotFound;
			Response.TrySkipIisCustomErrors = true;
			return View("Page404");
		}
		//public ViewResult GenericError()
		//{
		//    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		//    Response.TrySkipIisCustomErrors = true;
		//    return View("Error");
		//}

	}
}
