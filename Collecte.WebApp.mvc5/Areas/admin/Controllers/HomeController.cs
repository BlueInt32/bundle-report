using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using Collecte.DAL;
using Collecte.DTO;
using Collecte.WebApp.Areas.admin.Models;
using Tools.CustomActionResults;
using System.Linq;

namespace Collecte.WebApp.Areas.admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Index";

            return View(new HomeModel());
        }
        [HttpPost]
        public ActionResult Index(HomeModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                #region checkParams
                var dd = DateTime.Parse(model.DateDebut);
                var df = DateTime.Parse(model.DateFin);
                if (dd > df)
                {
                    ModelState.AddModelError("", "La date de fin doit être supérieur à la date de début");
                    return View(model);
                }
                #endregion

                //---
                var uDal = new UserDal();
                var extractResult = uDal.ExtractUsersWithDateLapsTime(dd, df);
                if (!extractResult.Result)
                    throw new Exception("Erreur d'extraction : " + extractResult.Message);

                var users = extractResult.ReturnObject;

                //---
                var po = users.Where(u => u.IsOffreGroupCanal).ToList();
                var nbpo = po.Count;
                var nbpoa = po.Where(u => u.IsCanal).Count();
                var total = new StatsTotal
                                {
                                    NbParticipations = users.Count,
                                    NbParticipationsOptin = nbpo,
                                    NbParticipationsOptinAbonnés = nbpoa,
                                    NbParticipationsOptinNonAbonnés = nbpo - nbpoa
                                };

                //---
				var usersa = users.Where(u => u.Provenance == "tradedoubler").ToList();
                var pa = usersa.Where(u => u.IsOffreGroupCanal).ToList();
                var nbpa = pa.Count;
                var nbpaa = pa.Where(u => u.IsCanal).Count();
				var tradedoubler = new StatsTradedoubler
                {
                    NbParticipants = usersa.Count,
                    NbParticipantsOptin = nbpa,
                    NbParticipantsOptinAbonnés = nbpaa,
                    NbParticipantsOptinNonAbonnés = nbpa - nbpaa
                };

                //---
                var oa = (int) (((Double) nbpoa/(Double) nbpo)*100);
                var oapa = (int) (((Double) nbpaa/(Double) usersa.Count)*100);
                var ona = (int) (((Double) (nbpo - nbpoa)/(Double) nbpo)*100);
                var onapa = (int) (((Double) (nbpa - nbpaa)/(Double) usersa.Count)*100);
                var op = (int) (((Double) nbpo/(Double) users.Count)*100);
                var ppa = (int) (((Double) usersa.Count/(Double) users.Count)*100);
                var analyse = new StatsAnalyse
                {
                    TauxOptinAbonnes = oa > 0 ? oa : 0,
					TauxOptinAbonnesParticipantsTradedoubler = oapa > 0 ? oapa : 0,
                    TauxOptinNonAbonnes = ona > 0 ? ona : 0,
					TauxOptinNonAbonnesParticipantsTradedoubler = onapa > 0 ? onapa : 0,
                    TauxOptinParticipants = op > 0 ? op : 0,
					TauxParticipationsParticipantsTradedoubler = ppa > 0 ? ppa : 0
                };

                //---
                var episodes = new StatsEpisodes
                                   {
                                       NbJoueursEpisode1 = users.Where(u=>u.FriendEmail1!=null || u.FriendEmail2!=null || u.FriendEmail3!=null).Count(),
                                       //NbJoueursEpisode2 = users.Where(u => u.ShowType != null).Count(),
                                       //NbJoueursEpisode3 = users.Where(u => u.ConnexionType != null).Count(),
                                       NbJoueursEpisode4 = users.Where(u => u.Address != null).Count(),
                                       NbJoueursEpisode5 = users.Where(u => u.Phone != null).Count()
                                   };

                //---
                Session[ConfigurationManager.AppSettings["sessionId"] + "_users"] = users;
                model.Stats = new Stats
                                  {
                                      Total = total,
                                      Analyse = analyse,
									  Tradedoubler = tradedoubler,
                                      Episodes = episodes
                                  };


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Extract()
        {
            var users = Session[ConfigurationManager.AppSettings["sessionId"] + "_users"] as List<User>;

            var path = UserDal.CreateCsvFileExtractFromList(Server.MapPath("~/CsvFiles/"), users);

            return new CsvActionResult(path, Encoding.UTF8);
        }

        public ActionResult ExtractChances()
        {
            var users = Session[ConfigurationManager.AppSettings["sessionId"] + "_users"] as List<User>;

            var path = UserDal.CreateCsvFileExtractFromList(Server.MapPath("~/CsvFiles/"), GetUsersWithChances(users));

            return new CsvActionResult(path, Encoding.UTF8);
        }

        private static List<User> GetUsersWithChances(List<User> lu)
        {
            var nlu = new List<User>();

            var l = lu.Count();
            for (var i = 0; i < l; i++)
            {
                int nc = lu[i].ChancesAmount;
                for(var j=0;j<nc;j++)
                {
                    nlu.Add(lu[i]);
                }
            }

            return nlu;
        }
    }
}
