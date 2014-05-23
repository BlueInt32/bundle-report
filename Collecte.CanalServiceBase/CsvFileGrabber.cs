using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using Collecte.DAL;
using Collecte.DTO;
using Collecte.Logic;
using Collecte.CanalServiceBase.tradedoublerStructure;
using CsvHelper;
using CsvHelper.Configuration;
using Tools;

namespace Collecte.CanalServiceBase
{
	public class CsvFileGrabber
	{
		FileSystemWatcher VideoFilesWatcher { get; set; }
		public List<FromCanalUser> FromCanalList { get; set; }
		DateTime ScannedDate { get; set; }
		BundleLogic BundleLogic { get; set; }

		private Dictionary<string, Timer> Timers = new Dictionary<string, Timer>();


		public CsvFileGrabber()
		{
			VideoFilesWatcher = new FileSystemWatcher
			{
				Path = ConfigurationManager.AppSettings["fromCanalFtpPath"],
				NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
				//NotifyFilter = NotifyFilters.
			};
			VideoFilesWatcher.Filter = "*.*";
			VideoFilesWatcher.Created += new FileSystemEventHandler(CSvFilesWatcher_Event);
			VideoFilesWatcher.Changed += new FileSystemEventHandler(CSvFilesWatcher_Event);
			VideoFilesWatcher.EnableRaisingEvents = true;
		}


		void CSvFilesWatcher_Event(object sender, FileSystemEventArgs e)
		{

			if (!Timers.ContainsKey(e.FullPath))
			{
				Program.log("Creation " + e.FullPath);
				Timer timer = new Timer();
				timer.Elapsed += new ElapsedEventHandler(processFileUploaded);
				timer.Interval = 1000;
				timer.Start();
				Timers[e.FullPath] = timer;
			}
			else
			{
				Program.log("Still being pushed " + e.FullPath);
				Timers[e.FullPath].Stop();
				Timers[e.FullPath].Start();
			}
		}

		void processFileUploaded(object sender, ElapsedEventArgs e)
		{
			Timer currentTimer = (Timer)sender;

			string fullPath = Timers.FirstOrDefault(x => x.Value == currentTimer).Key;
			currentTimer.Dispose();
			Timers.Remove(fullPath);

			Program.log(string.Format("File uploaded : {0}", fullPath));

			// start Copy to monitoring path
			FileInfo fi = new FileInfo(fullPath);
			string destPath = string.Format("{0}/{1}", ConfigurationManager.AppSettings["fromCanalCsvFilesDirectory"], fi.Name);
			Program.log(string.Format("Copying from {0} to {1}", fullPath, destPath));
			File.Copy(fullPath, destPath,true);

			if (fullPath.Substring(fullPath.LastIndexOf('.') + 1).ToLower() != "csv")
			{
				try
				{
					using (StreamReader reader = File.OpenText(fullPath))
					{
						reader.ToString();
					}
				}
				catch (Exception ex)
				{
					Program.log("Fichier non accessible : " + ex.Message);
				}
				return;
			}
			Program.log("It's a CSV !");

			// Retrieve corresponding Bundle
			Regex dateResolve = new Regex(@"((19|20)\d\d(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01]))");
			Match m = dateResolve.Match(fullPath);
			if (!string.IsNullOrWhiteSpace(m.Value))
			{
				ScannedDate = new DateTime(Convert.ToInt16(m.Value.Substring(0, 4)), Convert.ToInt16(m.Value.Substring(4, 2)), Convert.ToInt16(m.Value.Substring(6, 2)));
				BundleLogic = new BundleLogic();
				var bResult = BundleLogic.GetBundleByDate(ScannedDate);

				if (bResult.Result)
				{
					BundleLogic.SetBundleStatus(ScannedDate, BundleStatus.CsvOutReceived);
					OperationResult<List<FromCanalUser>> parseResult = ParseFile(fullPath);
					if (parseResult.Result)
					{

						FromCanalList = parseResult.ReturnObject;


						Program.log(string.Format("XML genéré, résultat final :{0}", parseResult.Result));
					}
				}

			}
		}


		public OperationResult<List<FromCanalUser>> ParseFile(string filePath)
		{
			try
			{
				using (StreamReader reader = File.OpenText(filePath))
				{
					CsvReader csvReader = new CsvReader(reader);
					csvReader.Configuration.RegisterClassMap<FromCanalUser>();
					csvReader.Configuration.Delimiter = ";";
					csvReader.Configuration.HasHeaderRecord = false;
					var resultList = csvReader.GetRecords<FromCanalUser>().ToList();


					int fromCanalProfiles = resultList.Count;
					int statusOkScanned = resultList.Where(cu => cu.Status == "OK").Count();
					int statusKoScanned = resultList.Where(cu => cu.Status == "KO").Count();

					BundleLogic.AttachFileToBundle(ScannedDate, filePath, BundleFileType.CsvOut);
					BundleLogic.SetBundleNbReturnsCanal(ScannedDate, fromCanalProfiles, statusOkScanned, statusKoScanned);
					BundleLogic.SetBundleStatus(ScannedDate, BundleStatus.CsvOutParsed);


					Program.log(String.Format("Csv reader detected {0} entries", resultList.Count));

					UserDal uDal = new UserDal();

					List<UserItem> subListBase = uDal.GetAllUserItems();
					Program.log(String.Format("Base has {0} entries", subListBase.Count));
					var subListCanal = from u in resultList where u.Status == "OK" select new UserItem { Email = u.Email.ToLower(), Id = Guid.Empty };

					List<TradeDoublerTransaction> tradedoublerUsersList = new List<TradeDoublerTransaction>();
					var equalityComparer = new UserItemEqualityComparer();
					subListBase = subListBase.Intersect<UserItem>(subListCanal, equalityComparer).ToList();


					Program.log(String.Format("Intersect has {0} entries", subListBase.Count()));

					foreach (UserItem userItem in subListBase)
					{
						tradedoublerUsersList.Add(new TradeDoublerTransaction
						{
							orderNumber = userItem.Id,
							eventId = ConfigurationManager.AppSettings["tradeDoublerEventId"],
							status = "A"
						});
					}

					int sequenceNumber = TradeDoublerProvider.GetTradeDoublerSequenceNumber() + 1;

					TradeDoublerContainer tdc = new TradeDoublerContainer
					{
						OrganizationId = ConfigurationManager.AppSettings["tradeDoublerOrgId"],
						SequenceNumber = sequenceNumber,
						NumberOfTransactions = resultList.Count,
						Transactions = tradedoublerUsersList
					};
					Program.log("TradeDoublerContainer created");

					int number = 1;
					string filePathTradedoublerXmlNumberLess = string.Format("{0}\\pending_{1}_", ConfigurationManager.AppSettings["tradeDoublerLocalXmlPath"], ConfigurationManager.AppSettings["tradeDoublerOrgId"]);
					while (File.Exists(string.Format("{0}{1}.xml", filePathTradedoublerXmlNumberLess, number)))
					{
						number++;
					}

					string filePathTradedoublerXml = string.Format("{0}{1}.xml", filePathTradedoublerXmlNumberLess, number);
					tdc.SaveToFile(filePathTradedoublerXml);
					Program.log("Saved an XML file: " + filePathTradedoublerXml);
					BundleLogic.SetBundleStatus(ScannedDate, BundleStatus.XmlCreated);
					BundleLogic.AttachFileToBundle(ScannedDate, filePathTradedoublerXml, BundleFileType.XmlTrade);

					//string emailConf = ConfigurationManager.AppSettings["TradeDoublerMailTo"];

					/*
					// Send mail to tradeDoubler
					if (ConfigurationManager.AppSettings["sendXmlToTrade"] == "true")
					{
						Mailer mailer = new Mailer();
						mailer.LogDelegate = Program.log;
						OperationResult<NoType> mailerResult = mailer.SendMail(emailConf, "", "", new Attachment(filePathTradedoublerXml, MediaTypeNames.Text.Plain), ConfigurationManager.AppSettings["TradeDoublerCCTo"]);

						emailConf = ConfigurationManager.AppSettings["NotifEmail"];

						if (!mailerResult.Result)
						{
							mailer.SendMail(emailConf, "[Moulinette Canal Collecte] Exchec de l'envoi à Tradedoubler", mailerResult.Message, new Attachment(filePathTradedoublerXml, MediaTypeNames.Text.Plain), null);

						}
						// Send mail to rapp for information 
						mailer.SendMail(emailConf, "[Moulinette Canal Collecte] Xml envoyé à TradeDoubler", "Tout est ok (voir piece jointe)", new Attachment(filePathTradedoublerXml, MediaTypeNames.Text.Plain), null);

						TradeDoublerProvider.SetTradeDoublerSequenceNumber(sequenceNumber);
					}
					*/

					if (ConfigurationManager.AppSettings["sendXmlToTrade"] == "true")
					{
						FTP ftp = new FTP()
						{
							Host = ConfigurationManager.AppSettings["TradeDoublerFtpHost"],
							Login = ConfigurationManager.AppSettings["TradeDoublerFtpLogin"],
							Mode = Logic.Mode.Ftp,
							Pwd = ConfigurationManager.AppSettings["TradeDoublerFtpPass"],
							LogDelegate = Program.log
						};
						Program.log("Initialisation envoi ftp to Trade. Host:" + ftp.Host + ", Login:"+ftp.Login+", Mode:"+ftp.Mode+", pwd:"+ftp.Pwd);

						OperationResult<NoType> ftpResult = ftp.PushFile(filePathTradedoublerXml, ConfigurationManager.AppSettings["TradeDoublerFtpDistantPath"]);
						if (ftpResult.Result)
						{
							Program.log("File sent to TradeDoubler !");
							TradeDoublerProvider.SetTradeDoublerSequenceNumber(sequenceNumber);
							BundleLogic.SetBundleStatus(ScannedDate, BundleStatus.XmlSentToTrade);
						}
						else
							Program.log("Erreur envoi fichier TradeDoubler !" + ftpResult.Message);
					}
					return OperationResult<List<FromCanalUser>>.OkResultInstance(resultList);
				}
			}
			catch (Exception e)
			{

				Program.log("FAILED trade process. " + e.Message + " " + e.StackTrace);
				throw;
			}
		}
	}
	public class FromCanalUser : CsvClassMap<FromCanalUser>
	{
		public string DateCreation { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string Address { get; set; }
		public string Zip { get; set; }
		public string City { get; set; }
		public string Phone { get; set; }
		public string Status { get; set; }
		public string CommunicationType { get; set; }
		public override void CreateMap()
		{
			Map(m => m.DateCreation).Index(0);
			Map(m => m.Email).Index(1);
			Map(m => m.Name).Index(2);
			Map(m => m.FirstName).Index(3);
			Map(m => m.Address).Index(4);
			Map(m => m.Zip).Index(5);
			Map(m => m.City).Index(6);
			Map(m => m.Phone).Index(7);
			Map(m => m.Status).Index(8);
			Map(m => m.CommunicationType).Index(9);
		}
	}



	public class UserItemEqualityComparer : IEqualityComparer<UserItem>
	{
		#region Implementation of IEqualityComparer<in UserItem>

		/// <summary>
		/// Détermine si les objets spécifiés sont égaux.
		/// </summary>
		/// <returns>
		/// true si les objets spécifiés sont égaux ; sinon, false.
		/// </returns>
		/// <param name="x">Premier objet de type <paramref name="T"/> à comparer.</param><param name="y">Deuxième objet de type <paramref name="T"/> à comparer.</param>
		public bool Equals(UserItem x, UserItem y)
		{
			return x.Email.Trim().ToLower() == y.Email.Trim().ToLower();
		}

		/// <summary>
		/// Retourne un code de hachage pour l'objet spécifié.
		/// </summary>
		/// <returns>
		/// Code de hachage pour l'objet spécifié.
		/// </returns>
		/// <param name="obj"><see cref="T:System.Object"/> pour lequel un code de hachage doit être retourné.</param><exception cref="T:System.ArgumentNullException">Le type de <paramref name="obj"/> est un type référence et <paramref name="obj"/> est null.</exception>
		public int GetHashCode(UserItem obj)
		{
			return obj.Email.ToLower().GetHashCode();
		}

		#endregion
	}
}
