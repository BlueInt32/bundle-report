using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Collecte.DAL;
using Collecte.DTO;

namespace Collecte.WebApp.App_Code
{
	public class InitQualifData
	{
		public static void Go()
		{
			// Set show types to application

			QualifDataService qDal = new QualifDataService();

			Dictionary<string, AnswerToken> dicoQuestion1Answers = new Dictionary<string, AnswerToken>
			{
				{"repere", new AnswerToken{ Id=1, UrlToken="repere" }},
				{"arbre", new AnswerToken{ Id=2, UrlToken="arbre" }},
				{"cordeau", new AnswerToken{ Id=3, UrlToken="cordeau" }}
			};
			HttpContext.Current.Application["question1"] = dicoQuestion1Answers;

			Dictionary<string, AnswerToken> dicoQuestion2Answers = new Dictionary<string, AnswerToken>
			{
				{"epingles", new AnswerToken{ Id=1, UrlToken="epingles" }},
				{"discret", new AnswerToken{ Id=2, UrlToken="discret" }},
				{"fourrure", new AnswerToken{ Id=3, UrlToken="fourrure" }}
			};
			HttpContext.Current.Application["question2"] = dicoQuestion2Answers;

			Dictionary<string, AnswerToken> dicoQuestion3Answers = new Dictionary<string, AnswerToken>
			{
				{"secret", new AnswerToken{ Id=1, UrlToken="secret" }},
				{"grassemat", new AnswerToken{ Id=2, UrlToken="grassemat" }},
				{"sain", new AnswerToken{ Id=3, UrlToken="sain" }}
			};
			HttpContext.Current.Application["question3"] = dicoQuestion3Answers;

			Dictionary<string, AnswerToken> dicoQuestion4Answers = new Dictionary<string, AnswerToken>
			{
				{"police", new AnswerToken{ Id=1, UrlToken="police" }},
				{"chauffeur", new AnswerToken{ Id=2, UrlToken="chauffeur" }},
				{"quatrepattes", new AnswerToken{ Id=3, UrlToken="quatrepattes" }}
			};
			HttpContext.Current.Application["question4"] = dicoQuestion4Answers;

			Dictionary<string, AnswerToken> dicoQuestion5Answers = new Dictionary<string, AnswerToken>
			{
				{"nimporte", new AnswerToken{ Id=1, UrlToken="nimporte" }},
				{"chezvous", new AnswerToken{ Id=2, UrlToken="chezvous" }},
				{"message", new AnswerToken{ Id=3, UrlToken="message" }}
			};
			HttpContext.Current.Application["question5"] = dicoQuestion5Answers;

		}
	}
}