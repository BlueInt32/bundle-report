using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeneralTools;
using HttpServiceLibrary;
using OpeningAct.DTO;

namespace Tools.Filters
{
	public class GetCanalPostData : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{

			OperationResult<User> getCanalPostDataResult = InitUserFromPost(filterContext.HttpContext.Request.Params);
			if (getCanalPostDataResult.Result)
			{
				HttpSessionService.CurrentUser = getCanalPostDataResult.ReturnObject;
				Log.Info("GetCanalPostData", string.Format("User retrieved [{0}]", HttpSessionService.CurrentUser.ToLogString()));
			}
			else
			{
				/** If Canal Post aren't retrieved correctly, what to do ? **/
				Log.InfoFormat("GetCanalPostData", "No User retrieved from post in session :'{0}'", getCanalPostDataResult.SingleMessage);
			}
			base.OnActionExecuting(filterContext);
		}
		private OperationResult<User> InitUserFromPost(NameValueCollection postParamsCollection)
		{
			foreach (string s in postParamsCollection)
			{
				//Log.Info("getpost", string.Format("post[{0}]={1}", s, postParamsCollection[s]));
			}
			if (postParamsCollection["webindice"] != null)
			{
				Log.Info("GetCanalPostData", string.Format("User webindice:{0}", postParamsCollection["webindice"]));
				User userPost = new User();

				if (postParamsCollection["email"] != null)
				{
					userPost.Email = postParamsCollection["email"];
				}
				if (postParamsCollection["nom"] != null)
				{
					userPost.Nom = postParamsCollection["nom"];
				}
				if (postParamsCollection["prenom"] != null)
					userPost.Prenom = postParamsCollection["prenom"];

				if (postParamsCollection["webindice"] != null)
					userPost.UserId = postParamsCollection["webindice"];

				if (postParamsCollection["type_abo"] != null)
				{
					userPost.TypeAbo = postParamsCollection["type_abo"];
				}
				int zip;
				if (!string.IsNullOrWhiteSpace(postParamsCollection["zip"]) && int.TryParse(postParamsCollection["zip"], out zip))
				{
					userPost.Zip = postParamsCollection["zip"];
				}

				if (postParamsCollection["ville"] != null)
				{
					userPost.Ville = postParamsCollection["ville"];
				}
				if (postParamsCollection["adresse"] != null)
				{
					userPost.Adresse = postParamsCollection["adresse"];
				}
				if (postParamsCollection["civilite"] != null)
				{
					userPost.Sexe = postParamsCollection["civilite"] == "1" ? "H" : "F";
				}

				return OperationResult<User>.OkResultInstance(userPost);
			}
			else
			{
				return OperationResult<User>.BadResult("Webindice non fournis");
			}
		}
	}
}