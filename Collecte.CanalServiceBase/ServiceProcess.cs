﻿using System;
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

namespace Collecte.CanalServiceBase
{
	public class ServiceProcess
	{
		public List<User> RetrieveNewUsersSince(DateTime since)
		{
			using (CollectContext context = new CollectContext())
			{
				var nouveauxInscrits = from u in context.Users
									   where u.CreationDate > since
									   //&& !u.IsCanal
									   && u.IsOffreGroupCanal
									   orderby u.CreationDate descending
									   select u;
				
				return nouveauxInscrits.ToList();
			}
		}

		public StdResult<NoType> CanalPushFileFTP(string localPath)
		{ 
			string distantDirectory = "/";

			FTP ftp = new FTP
			          	{
			          		Host = ConfigurationManager.AppSettings["ftpServer_canal"],
							Login = ConfigurationManager.AppSettings["ftpLogin_canal"],
							Pwd = ConfigurationManager.AppSettings["ftpPass_canal"],
			          		LogDelegate = Program.log,
							Mode = Mode.Sftp
			          	};
			StdResult<NoType> ftpResult = ftp.PushFile(localPath, distantDirectory);
			if (ftpResult.Result)
			{
				Mailer mailer = new Mailer();
				mailer.LogDelegate = Program.log;
				string emailConf = ConfigurationManager.AppSettings["NotificationEmail"];
				mailer.SendMail(emailConf, "[Moulinette Canal Collecte] Envoi Ftp chez Canal", "Tout est ok. <br/><a href='http://monitoring.collecte.canalplus.clients.rappfrance.com'>Monitoring</a>", null, ConfigurationManager.AppSettings["NotificationEmail_CC"]);
			}
			return ftpResult;
		}

		
	}
}
