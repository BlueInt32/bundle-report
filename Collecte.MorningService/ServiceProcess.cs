using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using Collecte.DAL;
using Collecte.DTO;
using Collecte.Logic;
using Tools;
using Tools.Mail;

namespace Collecte.MorningService
{
	public class ServiceProcess
	{
		public List<User> RetrieveNewUsers()
		{
			using (DataContext context = new DataContext())
			{
				try
				{
					DateTime hierMemeHeure = DateTime.Now.AddDays(-1d);

					var nouveauxInscrits = from u in context.Users
										   where u.CreationDate > hierMemeHeure
											   //&& !u.IsCanal // [12/05/2014] temps 4 : on récupère désormais tous les inscrits optins
										   && u.IsOffreGroupCanal
										   orderby u.CreationDate descending
										   select u;

					return nouveauxInscrits.ToList();

				}
				catch (Exception e)
				{
					Program.Log("Probleme base de données : " + e.Message + " (" + e.StackTrace + ")");
					return new List<User>();
				}
			}
		}

		public void MailPerformancePushFileFTP(string localPath, string brand)
		{
			string distantDirectory = (brand == "cplus" ? ConfigurationManager.AppSettings["ftpFilePathCplus"] : ConfigurationManager.AppSettings["ftpFilePathCsat"])
												.Replace("#Date#", DateTime.Now.ToString("yyyyMMdd"));

			FTP ftp = new FTP
						{
							Host = ConfigurationManager.AppSettings["ftpServer"],
							Login = ConfigurationManager.AppSettings["ftpLogin"],
							Pwd = ConfigurationManager.AppSettings["ftpPass"],
							LogDelegate = Program.Log
						};

			OperationResult<NoType> ftpResult = ftp.PushFile(localPath, distantDirectory);

			if (ftpResult.Result)
			{
				Mailer mailer = new Mailer();
				mailer.LogDelegate = Program.Log;
				string emailConf = ConfigurationManager.AppSettings["NotificationEmail"];
				mailer.SendMail(emailConf, "[Morning Service Collecte] Envoi à MailPerf", "Tout est ok.", new Attachment(localPath), ConfigurationManager.AppSettings["NotificationEmail_CC"]);
			}
		}
	}
}
