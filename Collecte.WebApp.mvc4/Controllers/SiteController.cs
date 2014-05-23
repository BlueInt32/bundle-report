using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using Collecte.DAL;
using Collecte.DTO;
using Collecte.WebApp.App_Code;
using Collecte.WebApp.Filters;
using Collecte.WebApp.Models;
using Tools;
using Tools.CustomActionResults;
using Tools.Mail;
using Collecte.Logic;

namespace Collecte.WebApp.Controllers
{
	[HandleError, ActionLog]
	public class SiteController : Controller
	{
		public SiteController()
		{
			UserDal = new UserDal();
		}
		UserDal UserDal { get; set; }


		[AffiliationFilter]
		public ActionResult Index()
		{
			if (WebConfigurationManager.AppSettings["opeTerminee"] == "true")
			{
				return RedirectToActionPermanent("JeuFini");
			}
			BasicUserInfoViewModel model = Session["fbModel"] != null ? Session["fbModel"] as BasicUserInfoViewModel : new BasicUserInfoViewModel();

			model.IsCanal = false;
#if DEBUG
			model.FirstName = "Simon";
			model.LastName = "Budin";
			model.Email = "simon.budin@gmail.com";
			model.Zipcode = "92300";
			model.Civi = Civi.Mr;
			model.IsCanal = false;
			model.IsOptinRules = true;

#else
            var param = Request.Params;
            if(param != null)
            {
                model.FirstName = param["firstname"] ?? "";
                model.LastName = param["lastname"] ?? "";
                model.Email = param["email"] ?? "";
            }
#endif
            
			return View(model);
		}
		[HttpPost]
		public ActionResult Index(BasicUserInfoViewModel model)
		{
			if (ModelState.IsValid)
			{
				User alreadyInDbUser = UserDal.GetUserByEmail(model.Email);
				if (alreadyInDbUser != null)
				{

					Session["UserId"] = alreadyInDbUser.Id;
					return RedirectToAction("DejaParticipe");
				}

				// We're sure it's a new user now

				// create user
				User newUser = new User
				{
					Civilite = model.Civi.ToString(),
					FirstName = model.FirstName,
					LastName = model.LastName,
					Email = model.Email,
					IsCanal = model.IsCanal,
					IsOffreGroupCanal = model.IsOffreGroupCanal,
					Zipcode = Convert.ToInt32(model.Zipcode),
					CreationDate = DateTime.Now,
					Provenance = Session["utm_campaign"] == null ? "" : Session["utm_campaign"].ToString()
				};
				newUser.InitStepChances();

				OperationResult<User> saveNewUserResult = UserDal.Create(newUser);
				if (!saveNewUserResult.Result)
					throw new CollecteException(saveNewUserResult.Message);
				if (saveNewUserResult.ReturnObject.Id == Guid.Empty)
					throw new CollecteException("Guid user vide");
				Session["UserOptin"] = newUser.IsOffreGroupCanal;
				Session["UserId"] = saveNewUserResult.ReturnObject.Id;
				newUser = saveNewUserResult.ReturnObject;
				newUser.InstantsGagnantWon = new List<InstantGagnant>();



				// Play 'instant gagnant'
				OperationResult<InstantGagnant> currentIgResult = (new InstantGagnantLogic()).PlayInstantGagnant(newUser);
				if (currentIgResult.Result)
				{
					InstantGagnant ig = currentIgResult.ReturnObject;
					Session["WhatYouWonString"] = ig.Label;
					Session["WhatYouWonDivId"] = ig.FrontHtmlId;

					return RedirectToAction("Gagne");
				}
				else
				{
					return RedirectToAction("Perdu");
				}
			}
			return View(model);
		}
		
        [CheckSession]
		public ActionResult Gagne()
		{
			ViewBag.WhatYouWonString = Session["WhatYouWonString"];
			ViewBag.WhatYouWonDivId = Session["WhatYouWonDivId"];
			return View();
		}
		
        [CheckSession]
		public ActionResult Perdu()
		{
			return View();
		}

		[CheckSession]
		public ActionResult Question1()
		{
			ViewBag.IsUserOptin = bool.Parse(Session["UserOptin"].ToString());
			ViewBag.UserId = Session["UserId"].ToString();
			return View();
		}
		[CheckSession]
		public ActionResult Question2()
		{
			return View();
		}
		[CheckSession]
		public ActionResult Question3()
		{
			return View();
		}
		[CheckSession]
		public ActionResult Question4()
		{
			return View();
		}
		[CheckSession]
		public ActionResult Question5()
		{
			return View();
		}
		[CheckSession]
		public ActionResult QuestionChoice(int questionNum, string urlToken)
		{
			if (!HandleQuestionAnswer(questionNum))
				return RedirectToAction("DejaParticipe");
			return RedirectToAction(questionNum < 5 ? string.Format("Question{0}", questionNum+1) : "Resultat");
		}


		[CheckSession]
		public ActionResult Resultat()
		{
			ViewBag.OkMessage = "";
			ViewBag.NotifState = "ok";
			// TODO compute results

			QualifDal qDal = new QualifDal();
			User userFromDb = RetrieveUserFromDb(null);
			ViewBag.chances = string.Format("{0} chance{1} supplémentaire{1}", userFromDb.ChancesAmount, userFromDb.ChancesAmount == 1 ? "" : "s");
			var quizRetrieveResult = qDal.GetUsersAnswers(userFromDb);
			if (quizRetrieveResult.Result)
			{
				int[][] scoreReference = 
				{
					new int[]{1, 2, 3}, // question 1 scores for resp answer 1, 2 and 3
					new int[]{3, 1, 2}, // question 2 answers for blabla
					new int[]{1, 3, 2}, 
					new int[]{1, 3, 2}, 
					new int[]{1, 2, 3} 
				};


				List<AnswerChoice> answers = quizRetrieveResult.ReturnObject;
				int totalScore = 0;

				answers.ForEach(choice => totalScore += scoreReference[choice.QuestionNumber - 1][choice.AnswerChosen - 1]);

				//TODO remplir les profils et les mettre en base
				if (totalScore >= 5 && totalScore < 8.5)
				{
					// profil 1: 
					ViewBag.ResultId = "resultat-heroslegendaire";
					ViewBag.LabelResult = "H&Eacute;ROS L&Eacute;GENDAIRE";
					ViewBag.TextResult = "Le mot « <span class='demiitalic'>SUPER</span> » n'a plus de secret pour vous. On vous a vu décrocher la lune, déplacer des montagnes, faire des miracles. Tous vos proches vous admirent et le confirment : <span class='demiitalic'>vous n'avez plus de preuve à faire</span>, vous avez votre place aux côtés des plus grands super héros… Respect !";
					userFromDb.HeroicStatus = 1;
					
				}
				else if (totalScore >= 8.3 && totalScore < 11.6)
				{
					// profil 2: 
					ViewBag.ResultId = "resultat-antiheros";
					ViewBag.LabelResult = "ANTI-H&Eacute;ROS !";
					ViewBag.TextResult = "C'est simple, vous dégagez un <span class='demiitalic'>charme irrésistible</span>.<br /> Une peau de banane, un pot de fleur, une gaffe… Ils seront immanquablement pour vous. Et pourtant, malgré votre propension à attirer le mauvais sort, <span class='demiitalic'>on vous aime comme vous êtes</span> : vos qualités (bien sûr, vous les multipliez aussi) mais surtout, on adore vos défauts, plus que tout.";
					userFromDb.HeroicStatus = 2;
				}
				else
				{
					//profil 3: 
					ViewBag.ResultId = "resultat-herosdujour";
					ViewBag.LabelResult = "H&Eacute;ROS DU JOUR !";
					ViewBag.TextResult = "Une opportunité qui passe… <span class='demiitalic'>Et hop, vous voilà.</span> Vous avez l'intuition pour dénicher la bonne affaire, celle qui va tout faire basculer. Un coup d'éclat au bon moment, au bon endroit dont <span class='demiitalic'>vous êtes le maître</span> et qui ne passe pas inaperçu ! <span class='demiitalic'>Bravo.</span>";
					userFromDb.HeroicStatus = 3;
				}

				Session["ResultId"] = ViewBag.ResultId;
				Session["LabelResult"] = ViewBag.LabelResult;
				Session["TextResult"] = ViewBag.TextResult;

				userFromDb.HeroicScore = totalScore;
				OperationResult<User> updateResult = UserDal.Update(userFromDb);
			}
			else
			{
				throw new CollecteException(quizRetrieveResult.Message);
			}

			return View();
		}


#if !DEBUG
		[CheckSession]

#endif
		[HttpPost]
		public ActionResult Resultat(GodsonsMailsModel model)
		{

			ViewBag.OkMessage = "";
			ViewBag.ResultId = Session["ResultId"];
			ViewBag.LabelResult = Session["LabelResult"];
			ViewBag.TextResult = Session["TextResult"];

			if (ModelState.IsValid)
			{
				// Retrieve user from db
				User userFromDb = RetrieveUserFromDb(null);

				if (string.IsNullOrWhiteSpace(model.Email1) && string.IsNullOrWhiteSpace(model.Email2) && string.IsNullOrWhiteSpace(model.Email3))
				{
					ModelState.AddModelError("Email1", "Merci d’entrer une adresse e-mail valide.<br />Format requis : xxxx@xxx.xx");
					ViewBag.NotifState = "erreur";
					return View(model);
				}
				if (model.Email1 == userFromDb.Email || model.Email2 == userFromDb.Email || model.Email3 == userFromDb.Email)
				{
					ModelState.AddModelError("Email1", "Merci d'entrer une adresse e-mail différente de la vôtre.");
					ViewBag.NotifState = "erreur";
					return View(model);
				}
				if ((model.Email1 == model.Email2 && !string.IsNullOrWhiteSpace(model.Email1))
					|| (model.Email1 == model.Email3 && !string.IsNullOrWhiteSpace(model.Email1))
					|| (model.Email2 == model.Email3 && !string.IsNullOrWhiteSpace(model.Email2)))
				{
					ModelState.AddModelError("Email1", "Merci de rentrer des adresses e-mails distinctes.");
					ModelState.AddModelError("Email2", "Merci de rentrer des adresses e-mails distinctes.");
					ModelState.AddModelError("Email3", "Merci de rentrer des adresses e-mails distinctes.");
					ViewBag.NotifState = "erreur";

					return View(model);
				}

				// Save to Db
				userFromDb.FriendEmail1 = model.Email1;
				userFromDb.FriendEmail2 = model.Email2;
				userFromDb.FriendEmail3 = model.Email3;
				OperationResult<User> updateResult = UserDal.Update(userFromDb);
				if (!updateResult.Result)
					throw new CollecteException(updateResult.Message);



				// Actually send mails
				if (!userFromDb.HasSentEmailsToFriends)
				{
					MailHelper mailManager = CreateFriendsEmailInstance(userFromDb);
					OperationResult<MailHelper> sendResult = mailManager.Send();
					if (!sendResult.Result)
					{
						ModelState.AddModelError("Email1", "Echec d'envoi de l'email. Veuillez rééssayer ultérieurement.");
						ViewBag.NotifState = "erreur";
						Log.ErrorFormat("Mails filleuls envoi : erreur. User id :{0}, email1 : {1}, email2 : {2}, email3 : {3}. Erreur : {4}", userFromDb.Id.ToString(), userFromDb.FriendEmail1, userFromDb.FriendEmail2, userFromDb.FriendEmail3, sendResult.Message);

						return View(model);
					}
					userFromDb.HasSentEmailsToFriends = true;
				}
				ViewBag.NotifState = "ok";
				ViewBag.OkMessage = "Merci !";
			}
			else
			{
				ViewBag.NotifState = "erreur";
			}

			return View(model);
		}

        public ActionResult DejaParticipe()
		{
			return View();
		}

		public ActionResult JeuFini()
		{
			return View();
		}

		public ActionResult Maintenance()
		{
			if (WebConfigurationManager.AppSettings["maintenance"] != "true")
				return RedirectToAction("Index");
			return View();
		}

		public JsonResult PostAddress([ModelBinder(typeof(AddressModelBinder))] AddressModel model)
		{
			if (Session["UserId"] == null)
				return Json(new { result = false, message = "Session expired." });
			if(!model.IsValid)
				return Json(new { result = false, message = model.Message, badFieldId=model.HtmlId });
			User userFromDb = RetrieveUserFromDb(Guid.Parse(Session["UserId"].ToString()));
			userFromDb.NomVoie = model.NomVoie;
			userFromDb.TypeVoie = model.TypeVoie;
			userFromDb.NumeroVoie = model.NumVoie;
			userFromDb.City = model.City;
			userFromDb.Zipcode = model.ZipCode;
			userFromDb.Address = string.Concat(model.NumVoie, ", ", model.TypeVoie, " ", model.NomVoie);
			userFromDb.SetStepChance(1, true);
			OperationResult<User> updateResult = UserDal.Update(userFromDb);
			if (!updateResult.Result)
			{
				return Json(new { result = false, message = updateResult.Message });
			}
			else
			{

				return Json(new { result = true });
			}
		}

		public JsonResult PostPhone([ModelBinder(typeof(PhoneModelBinder))] PhoneModel model)
		{
			if (Session["UserId"] == null)
				return Json(new { result = false, message = "Session expired." });
			if (!model.IsValid)
				return Json(new { result = false, message = model.Message });
			User userFromDb = RetrieveUserFromDb(Guid.Parse(Session["UserId"].ToString()));
			userFromDb.Phone = model.Phone; 
			userFromDb.SetStepChance(2, true);

			OperationResult<User> updateResult = UserDal.Update(userFromDb);
			if (!updateResult.Result)
			{
				return Json(new { result = false, message = updateResult.Message });
			}
			else
			{
				return Json(new { result = true });
			}
		}


		public MailHelperActionResult FriendMailDisplay(Guid id)
		{
			User userFromDb = RetrieveUserFromDb(id);
			MailHelper mailManager = CreateFriendsEmailInstance(userFromDb);

			return new MailHelperActionResult(mailManager, Encoding.UTF8);
		}


		[NonAction]
		private bool HandleQuestionAnswer(int questionNumber)
		{
			// Retrieve user from db
			User userFromDb = RetrieveUserFromDb(null);

			QualifDal qDal = new QualifDal();
			OperationResult<AnswerChoice> updateResult = qDal.SetAnswer(userFromDb, questionNumber, (HttpContext.Items["ChosenAnswer"] as AnswerToken).Id);

			if (!updateResult.Result)
				throw new CollecteException(updateResult.Message);
			return true;
		}

		/// <summary>
		/// Get a user from database. If input param is null, uses Session["UserId"].
		/// In a session-less context like from emails users, you have to provide the id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[NonAction]
		private User RetrieveUserFromDb(Guid? id)
		{
			if(Session["UserId"] == null)
				throw new CollecteException("Session expirée.");
			Guid idUser = id.HasValue ? id.Value : Guid.Parse(Session["UserId"].ToString());
			OperationResult<User> retrieveUserResult = UserDal.Retrieve(idUser);
			if (!retrieveUserResult.Result)
				throw new CollecteException(string.Format("id:{0} error : {1}", Session["UserId"], retrieveUserResult.Message));
			return retrieveUserResult.ReturnObject;
		}

		[NonAction]
		private MailHelper CreateFriendsEmailInstance(User userFromDb)
		{
			MailHelper mh = new MailHelper(MailType.WithTemplateHtml);
			mh.Recipients = new List<string> { userFromDb.FriendEmail1, userFromDb.FriendEmail2, userFromDb.FriendEmail3 }
				.Where(item => !string.IsNullOrWhiteSpace(item))
				.ToList()
				.ConvertAll(item => new MailAddress(item));

			string parrainName = string.Format("{0} {1}", userFromDb.FirstName, userFromDb.LastName);

            mh.MailSubject = string.Format("{0} vous propose de gagner un voyage à Hollywood", parrainName);
			mh.ReplyTo = new MailAddress("CANAL+ <no-reply@vivelejeudhivercanalsat.fr>");
			mh.Sender = new MailAddress("CANAL+ <no-reply@vivelejeudhivercanalsat.fr>");

			mh.TemplatePath = Server.MapPath("~/Content/mail/");
			mh.ReplacementFields = new List<ReplaceField> { 
				new ReplaceField { Replace = "#imagesPath#", With = "~/Content/mail/images/".ContentAbsolute() },
				new ReplaceField { Replace = "#senderFullName#", With =  parrainName},
				new ReplaceField { Replace = "#staticMailViewLink#", With = string.Format("{0}/Site/FriendMailDisplay/{1}", UrlHelperSb.GetApplicationBaseUrl(false) , userFromDb.Id)},
				new ReplaceField { Replace = "#opeLink#", With = string.Format("{0}", UrlHelperSb.GetApplicationBaseUrl(false))},
			};
			return mh;
		}

		public ActionResult FlushMoiLappDomainUzul()
		{
			//HttpRuntime.UnloadAppDomain();
			return RedirectToAction("Index");
		}
	}
}
