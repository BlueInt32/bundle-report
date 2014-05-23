using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Collecte.DAL;
using Collecte.DTO;
using Tools;

namespace Collecte.WebApp.App_Code
{
	public class QuestionRouteConstraint : IRouteConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
			if (routeDirection == RouteDirection.UrlGeneration)
				return true;

			string DictionaryName = string.Format("question{0}", values["questionNum"]);
			Dictionary<string, AnswerToken> dico = httpContext.Application[DictionaryName] as Dictionary<string, AnswerToken>;
			//OperationResult<ShowType> testUrl = qualifDal.GetShowTypeByUrl(values[parameterName].ToString());
			bool question1AnswerExists = dico.ContainsKey(values[parameterName].ToString());
			if (httpContext.Items.Contains("ChosenAnswer"))
				return true;
			if (question1AnswerExists)
			{
				httpContext.Items.Add("ChosenAnswer", dico[values[parameterName].ToString()]);
				return true;
			}
			return false;
		}
	}

	//public class ShowTypeRouteConstraint : IRouteConstraint
	//{
	//	public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
	//	{
	//		if (routeDirection == RouteDirection.UrlGeneration)
	//			return true;
	//		Dictionary<string, ShowType> dico = httpContext.Application["dicoShowTypes"] as Dictionary<string, ShowType>;
	//		//OperationResult<ShowType> testUrl = qualifDal.GetShowTypeByUrl(values[parameterName].ToString());
	//		bool showTypeExists = dico.ContainsKey(values[parameterName].ToString());
	//		if (showTypeExists)
	//		{
	//			httpContext.Items.Add("ChosenShowType", dico[values[parameterName].ToString()]);
	//			return true;
	//		}
	//		return false;
	//	}
	//}
	//public class ConnexionTypeRouteConstraint : IRouteConstraint
	//{
	//	public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
	//	{
	//		if (routeDirection == RouteDirection.UrlGeneration)
	//			return true;
	//		//OperationResult<ConnexionType> testUrl = qualifDal.GetConnexionTypeByUrl(values[parameterName].ToString());
	//		Dictionary<string, ConnexionType> dico = httpContext.Application["dicoConnexionTypes"] as Dictionary<string, ConnexionType>;
	//		bool connexionTypeExists = dico.ContainsKey(values[parameterName].ToString());
	//		if (connexionTypeExists)
	//		{
	//			//httpContext.Session["ChosenConnexionType"] = testUrl.ReturnObject;
	//			httpContext.Items.Add("ChosenConnexionType", dico[values[parameterName].ToString()]);
	//			return true;
	//		}
	//		return false;
	//	}
	//}
}