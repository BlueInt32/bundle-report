using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using Collecte.DTO;
using Tools;
using Tools.EntityFramework;
using Tools.Orm;
using System.Data.Entity;

namespace Collecte.DAL
{
	public class UserDataService : Crud<User, Guid>
	{
		#region Implementation of IRepository<User,in string>

		public StdResult<User> Create(User inputObject)
		{
			using (CollectContext context = new CollectContext())
			{
				context.Users.Add(inputObject);
				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();
				if (errors.Count() > 0)
					return StdResult<User>.BadResultWithList("Erreur(s) de validation pour la création, voir ErrorList", errors.FormatValidationErrorList());

				int nbSaved = context.SaveChanges();
				return nbSaved != 1 ? StdResult<User>.BadResult("User non créé", inputObject) : StdResult<User>.OkResultInstance(inputObject);
			}
		}

		public StdResult<User> Get(Guid id)
		{
			using (CollectContext context = new CollectContext())
			{
				User UserFromDb = context.Users.Include("AnswerChoices").Include("InstantsGagnantWon").Where(u => u.Id == id).FirstOrDefault();
				if (UserFromDb == null)
					return StdResult<User>.BadResult("User introuvable");
				return StdResult<User>.OkResultInstance(UserFromDb);
			}
		}

		public StdResult<User> Update(User inputObject)
		{
			using (CollectContext context = new CollectContext())
			{
					context.Users.Attach(inputObject);
					var entry = context.Entry(inputObject);
					entry.State = EntityState.Modified;

					IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();
					if (errors.Count() > 0)
						return StdResult<User>.BadResultWithList("Erreur(s) de validation pour la création, voir ErrorList", errors.FormatValidationErrorList());
				
					context.SaveChanges();
					return StdResult<User>.OkResultInstance(inputObject);
			}
		}

		public StdResult<User> Delete(Guid id)
		{
			using (CollectContext context = new CollectContext())
			{
				User UserFromDb = context.Users.Where(u => u.Id == id).FirstOrDefault();
				if (UserFromDb == null)
					return StdResult<User>.BadResult("User introuvable");
				context.Users.Remove(UserFromDb);
				if (context.SaveChanges() != 1)
					return StdResult<User>.BadResult("User non supprimé", UserFromDb);
				else
					return StdResult<User>.OkResult;
			}
		}
		#endregion

		public User GetUserByEmail(string email)
		{
			using (CollectContext context = new CollectContext())
			{
				User userFromDb = context.Users.Include("InstantsGagnantWon").Where(u => u.Email == email).FirstOrDefault();
				return userFromDb;
			}
		}

		public List<User> GetAllUsers()
		{
			using (CollectContext context = new CollectContext())
			{
				var userFromDb = context.Users.ToList();
				return userFromDb;
			}
		}

		public List<UserItem> GetAllUserItems()
		{
			using (CollectContext context = new CollectContext())
			{

				var userFromDb = (from u in context.Users select new UserItem { Email = u.Email.ToLower(), Id = u.Id }).ToList();
				
				return userFromDb;
			}
		}

		public StdResult<List<User>> ExtractUsers()
		{
			using (CollectContext context = new CollectContext())
			{
				try
				{
					List<User> participationsFromDb = context.Users.Include("ShowType").Include("ConnexionType").OrderByDescending(u => u.CreationDate).ToList();
					return StdResult<List<User>>.OkResultInstance(participationsFromDb);
				}
				catch (Exception e)
				{
					return StdResult<List<User>>.BadResult("Echec de la récupération des utilisateurs : " + e.Message);
				}
			}
		}

        /// <summary>
        /// Methode pour le back
        /// </summary>
        /// <param name="debut"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        public StdResult<List<User>> ExtractUsersWithDateLapsTime(DateTime debut, DateTime fin)
        {
            using (var context = new CollectContext())
            {
                try
                {
                    List<User> participationsFromDb = context.Users.Where(u=> u.CreationDate>= debut && u.CreationDate<=fin).OrderByDescending(u => u.CreationDate).ToList();
                    return StdResult<List<User>>.OkResultInstance(participationsFromDb);
                }
                catch (Exception e)
                {
                    return StdResult<List<User>>.BadResult("Echec de la récupération des utilisateurs : " + e.Message);
                }
            }
        }

		/// <summary>
		/// Create a csv file with user data from the list input
		/// </summary>
		/// <param name="list">User list from DB</param>
		/// <returns>Complete Filesystem path of the csv created (to pass to ftp)</returns>
		public static string CreateCsvFileExtractFromList(string outputFileDirectory, List<User> list)
		{
			//string mydocpath = Environment.GetFolderPath();

			string completeFilePath = Path.Combine(outputFileDirectory, string.Format("InscritCollecte_{0}", DateTime.Now.ToString("yyyyMMdd")));
			if (File.Exists(completeFilePath + ".csv"))
			{
				completeFilePath = string.Format("{0}-{1}h{2}m{3}", completeFilePath, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
			}
			completeFilePath += ".csv";
			var sb = new StringBuilder();

			sb.AppendFormat("\"Date de participation\";\"Civilité\";\"Nom\";\"Prénom\";\"Email\";\"Adresse\";\"Ville\";\"Code postal\";\"Téléphone\";\"Abonné\";\"Optin\";\"FilleulEmail1\";\"FilleulEmail2\";\"FilleulEmail3\";\"Chances\";\"Détail chances\";\"Provenance\";\"Scellé\"\n");
			foreach (var user in list)
			{
				sb.AppendFormat("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\";\"{7}\";\"{8}\";\"{9}\";\"{10}\";\"{11}\";\"{12}\";\"{13}\";\"{14}\";\"{15}\";\"{16}\"\n",
					user.CreationDate,
					user.Civilite,
					user.LastName,
					user.FirstName,
					user.Email,
					user.Address,
					user.City,
					user.Zipcode,
					user.Phone,
					BoolToOuiNon(user.IsCanal),
					BoolToOuiNon(user.IsOffreGroupCanal),
					//user.ShowType == null ? string.Empty : user.ShowType.Id.ToString(),
					//user.ConnexionType == null ? string.Empty : user.ConnexionType.Id.ToString(),
					
					user.FriendEmail1,
					user.FriendEmail2,
					user.FriendEmail3,
					user.ChancesAmount,
					user.ChancesByStep,
					user.Provenance ?? string.Empty
					);
			}
            using (var outfile = new StreamWriter(completeFilePath, true, Encoding.UTF8))
			{
				outfile.Write(sb.ToString());
			}

			return completeFilePath;

		}




		/// <summary>
		/// Create a csv file with user data from the list input
		/// </summary>
		/// <param name="list">User list from DB</param>
		/// <returns>Complete Filesystem path of the csv created (to pass to ftp)</returns>
		public string CreateCsvFileFtpFromList(string outputFileDirectory, List<User> list, string brand)
		{
			//string mydocpath = Environment.GetFolderPath();

			string completeFilePath = Path.Combine(outputFileDirectory, string.Format("InscritCollecte_{1}_{0}", DateTime.Now.ToString("yyyyMMdd"), brand));
			if (File.Exists(completeFilePath + ".csv"))
			{
				completeFilePath = string.Format("{0}-{1}h{2}m{3}", completeFilePath, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
			}
			completeFilePath += ".csv";
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("\"Date de participation\";\"Civilité\";\"Nom\";\"Prénom\";\"Email\";\"Abonné\";\"Optin\";\"Type Heros\";\"Provenance\";\"Envoi différé\"\n");

			// tous les 5 mecs, on flag à true la colonne "envoi différé"
			int mod5count = 0;
			foreach (User user in list)
			{
				sb.AppendFormat("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\";\"{7}\";\"{8}\";\"{9}\"\n",
					user.CreationDate,
					user.Civilite,
					user.LastName,
					user.FirstName,
					user.Email,
					BoolToOuiNon(user.IsCanal),
					BoolToOuiNon(user.IsOffreGroupCanal),
					user.HeroicStatus,
					user.Provenance,
					(mod5count++)%5 == 0 ? 1 : 0
					);
				
			}
			Encoding enc = Encoding.UTF8;
			using (StreamWriter outfile = new StreamWriter(completeFilePath, true, enc))
			{
				outfile.Write(sb.ToString());
			}

			return completeFilePath;

		}

		public string CreateCsvContentForCanal(string outputFileDirectory, List<User> list, int timeOfDayCode)
		{
			string FilePathSuffix = Path.Combine(outputFileDirectory, string.Format("FC_RAP_IN_CIBLE_{0}_", DateTime.Now.ToString("yyyyMMdd")));
			// typiquement : 
			// "E:\DDB\CANAL\Collecte\Collecte.MorningService\CsvFiles\FC_RAP_IN_CIBLE_20131120_"

			string completeFilePath = string.Format("{0}{1}.csv", FilePathSuffix, timeOfDayCode);
			// typiquement : 
			// "E:\DDB\CANAL\Collecte\Collecte.MorningService\CsvFiles\FC_RAP_IN_CIBLE_20131120_1.csv"

			StringBuilder sb = new StringBuilder();
			if (File.Exists(completeFilePath))
			{
				File.Move(completeFilePath, completeFilePath.Replace(".csv", string.Format(".replaced_{0}.csv", DateTime.Now.ToString("yyyyMMdd_HH-mm-ss"))));
			}
			//sb.AppendFormat("\"Date de participation\";\"Email\";\"Nom\";\"Prénom\";\"Email\";\"Abonné\";\"Optin\";\"Type Programme\";\"Provenance\"\n");
			const string escapeQuote = "";
			foreach (User user in list)
			{
				sb.AppendFormat("{0}{1}{0};{0}{2}{0};{0}{3}{0};{0}{4}{0};{0}{5}{0};{0}{6}{0};{0}{7}{0};{0}{8}{0};{0}{9}{0};{0}{10}{0}\n",
					escapeQuote,
					user.CreationDate.ToString("yyyy-MM-dd"),
					user.Email,
					user.LastName,
					user.FirstName,
					user.Address,
					user.Zipcode,
					user.City,
					user.Phone,
					"T01",
					System.Configuration.ConfigurationManager.AppSettings["CanalCampaignName"]
					);
			}
			Encoding enc = Encoding.GetEncoding(28591);
			using (StreamWriter outfile = new StreamWriter(completeFilePath, true, enc))
			{
				outfile.Write(sb.ToString());
			}

			return completeFilePath;
		}

	    static string BoolToOuiNon(bool input)
		{
			return input ? "oui" : "non";
		}
	}
}
