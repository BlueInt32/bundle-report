using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Collecte.DAL;
using Collecte.DTO;
using Tools;

namespace Collecte.MorningService
{
	public class MainProcess
	{
		public List<User> RetrieveNewUsers()
		{
			using (DataContext context = new DataContext())
			{
				DateTime hierMemeHeure = DateTime.Now.AddDays(-1d);

				var nouveauxInscrits = from u in context.Users.Include("ShowTypes").Include("ConnexionTypes")
									   where u.ParticipationDate > hierMemeHeure
									   select u;
				return nouveauxInscrits.ToList();
			}
		}

		/// <summary>
		/// Create a csv file with user data from the list input
		/// </summary>
		/// <param name="list">User list from DB</param>
		/// <returns>Complete Filesystem path of the csv created (to pass to ftp)</returns>
		public string CreateCsvFileFromList(List<User> list)
		{
			//string mydocpath = Environment.GetFolderPath();
			
			string path = ConfigurationManager.AppSettings["localCsvFilesDirectory"];
			string completeFilePath = Path.Combine(path, string.Format("InscritCineProfiler_{0}", DateTime.Now.ToString("yyyyMMdd")));
			if (File.Exists(completeFilePath + ".csv"))
			{
				completeFilePath = string.Format("{0}-{1}h{2}m{3}", completeFilePath, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
			}
			completeFilePath += ".csv";
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("\"Date de participation\";\"Civilité\";\"Nom\";\"Prénom\";\"Email\";\"Adresse\";\"Code postal\";\"Abonné\";\"Optin\";\"Type Programme\";\"Type Connexion\";\"FilleulEmail1\";\"FilleulEmail2\";\"FilleulEmail3\";\"Chances\"\n");
			foreach (User user in list)
			{
				sb.AppendFormat("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\";\"{7}\";\"{8}\";\"{9}\";\"{10}\";\"{11}\";\"{12}\";\"{13}\";\"{14}\"\n",
					user.ParticipationDate,
					user.Civilite,
					user.LastName,
					user.FirstName,
					user.Email,
					user.Adress,
					user.Zipcode,
					user.IsCanal,
					user.IsNewsLetter,
					user.ShowType.Label,
					user.ConnexionType.Label,
					user.FriendEmail1,
					user.FriendEmail2,	
					user.FriendEmail3,
					user.ChancesAmount
					);
			}
			Encoding enc = Encoding.UTF8;
			using (StreamWriter outfile = new StreamWriter(completeFilePath, true, enc))
			{
				outfile.Write(sb.ToString());
			}

			return completeFilePath;

		}

		public void PushFileFTP(string localPath)
		{
			// Get the object used to communicate with the server.

			string distantPath = string.Format("ftp://{0}{1}",
												ConfigurationManager.AppSettings["ftpServer"],
												ConfigurationManager.AppSettings["ftpFilePath"].Replace("#Date#", DateTime.Now.ToString("yyyyMMdd")));
			Program.log("Distant Path : " + distantPath);
			try
			{
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(distantPath);
				request.Method = WebRequestMethods.Ftp.UploadFile;

				// This example assumes the FTP site uses anonymous logon.
				request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ftpLogin"],
															ConfigurationManager.AppSettings["ftpPass"]);

				// Copy the contents of the file to the request stream.

				StreamReader sourceStream = new StreamReader(localPath, true);
				byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
				fileContents = Encoding.UTF8.GetPreamble().Concat(fileContents).ToArray();
				//byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
				sourceStream.Close();
				request.ContentLength = fileContents.Length;

				Stream requestStream = request.GetRequestStream();
				//requestStream.
				requestStream.Write(fileContents, 0, fileContents.Length);
				requestStream.Close();

				FtpWebResponse response = (FtpWebResponse)request.GetResponse();
				Program.log("response : " + response.StatusCode.ToString() + " " + response.StatusDescription);
				//Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

				response.Close();



				MailHelper mh = new MailHelper(MailType.BasicText);
				mh.MailSubject = "Morning Service Collecte Canal";
				mh.MailType = MailType.BasicText;
				mh.Recipients = new List<MailAddress>{new MailAddress("simon.budin@gmail.com")};
				mh.Sender = new MailAddress("canal.collecte@rappfrance.com");
				mh.ReplyTo = new MailAddress("canal.collecte@rappfrance.com");
				mh.SetContentDirect("Ok pour ce matin.");
				mh.Send();
			}
			catch (Exception e)
			{
				Program.log("Exception envoi FTP : " + e.Message + " /// " + e.StackTrace);
			}
		}
	}
}
