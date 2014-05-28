using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;
using Tools;
using System.Configuration;

namespace Collecte.Logic
{
	public class FTP
	{
		public string Host { get; set; }
		public string Login { get; set; }
		public string Pwd { get; set; }
		public Mode Mode { get; set; }
		public Action<string> LogDelegate { get; set; }

		string _distantPath;
		public string DistantPath
		{
			get
			{
				return _distantPath;
			}
		}

		private static void SetMethodRequiresCWD()
		{
			Type requestType = typeof(FtpWebRequest);
			FieldInfo methodInfoField = requestType.GetField("m_MethodInfo", BindingFlags.NonPublic | BindingFlags.Instance);
			Type methodInfoType = methodInfoField.FieldType;


			FieldInfo knownMethodsField = methodInfoType.GetField("KnownMethodInfo", BindingFlags.Static | BindingFlags.NonPublic);
			Array knownMethodsArray = (Array)knownMethodsField.GetValue(null);

			FieldInfo flagsField = methodInfoType.GetField("Flags", BindingFlags.NonPublic | BindingFlags.Instance);

			int MustChangeWorkingDirectoryToPath = 0x100;
			foreach (object knownMethod in knownMethodsArray)
			{
				int flags = (int)flagsField.GetValue(knownMethod);
				flags |= MustChangeWorkingDirectoryToPath;
				flagsField.SetValue(knownMethod, flags);
			}
		}

		public OperationResult<NoType> PushFile(string localFilePath, string distantDirectory)
		{
			return Mode == Mode.Sftp ? PushFileSFTP(localFilePath, distantDirectory) : PushFileFTP(localFilePath, distantDirectory);
		}

		private OperationResult<NoType> PushFileSFTP(string localFilePath, string distantDirectory)
		{
			if (!distantDirectory.StartsWith("/"))
				distantDirectory = string.Concat("/", distantDirectory);
			string distantPath = string.Format("ftp://{0}{1}", Host, distantDirectory);
			LogDelegate(string.Format("[FTP] Distant path: {0}", distantPath));
			try
			{
				//new SftpClient(Host, 22, Login, Pwd)

				using (var sftp = new SftpClient(new PasswordConnectionInfo(Host, 22, Login, Pwd)))
				{

					sftp.HostKeyReceived += sftp_HostKeyReceived;
					sftp.Connect();
					sftp.ChangeDirectory(distantDirectory);
					FileInfo fi = new FileInfo(localFilePath);
					string distantFullPath = string.Format("{0}{1}", distantDirectory, fi.Name);
					LogDelegate(string.Format("[FTP] ConnectionInfo : sftp.ConnectionInfo.IsAuthenticated:{0}, distant directory: {1}, username:{2}, host:{3}, port:{4}, distantPath:{5}", 
						sftp.ConnectionInfo.IsAuthenticated, 
						distantDirectory,
						sftp.ConnectionInfo.Username,
						sftp.ConnectionInfo.Host,
						sftp.ConnectionInfo.Port, 
						distantFullPath));
					//var sftpFiles = sftp.ListDirectory(distantDirectory);
					//FileStream local = File.OpenRead(localFilePath);
					//sftp.UploadFile(sr.BaseStream, distantDirectory, null);
					using (StreamReader sr = new StreamReader(localFilePath))
					{
						LogDelegate(string.Format("[FTP] File being sent : {0} bytes.", sr.BaseStream.Length));
						sftp.UploadFile(sr.BaseStream, distantFullPath);
					}
				}
				LogDelegate(string.Format("[FTP] File Sent successfully."));
				return OperationResult<NoType>.OkResult;

			}
			catch (Exception e)
			{
				if (LogDelegate != null)
				{
					Mailer mailer = new Mailer();
					mailer.LogDelegate = LogDelegate;
					Exception exception = e;
					while (exception != null)
					{
						LogDelegate("[FTP] Exception envoi : " + exception.Message + " " + exception.StackTrace);
						exception = e.InnerException;
					}
					string emailConf = ConfigurationManager.AppSettings["NotificationEmail"];
					mailer.SendMail(emailConf, "[Canal Collecte] Erreur FTP!", exception.Message + "<br/>" + e.StackTrace, null, ConfigurationManager.AppSettings["NotificationEmail_CC"]);
					
				}
				return OperationResult<NoType>.BadResultFormat("[FTP] Exception envoi: {0} /// {1}", e.Message);
			}
		}

		private void sftp_HostKeyReceived(object sender, HostKeyEventArgs e)
		{
			LogDelegate("[SFTP] HostKeyReceived : " + e.HostKeyName + " " + e.HostKey);
		}


		private OperationResult<NoType> PushFileFTP(string localFilePath, string distantDirectory)
		{
			if (!distantDirectory.StartsWith("/"))
				distantDirectory = string.Concat("/", distantDirectory);

			FileInfo fi = new FileInfo(localFilePath);
			string distantPath = string.Format("ftp://{0}{1}{2}", Host, distantDirectory, fi.Name);
			LogDelegate(string.Format("[FTP] Distant path: {0}", distantPath));
			try
			{
				SetMethodRequiresCWD();
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(distantPath);
				request.Method = WebRequestMethods.Ftp.UploadFile;
				request.KeepAlive = false;
				request.UsePassive = true;
				//request.

				// This example assumes the FTP site uses anonymous logon.
				request.Credentials = new NetworkCredential(Login, Pwd);

				// Copy the contents of the file to the request stream.
				StreamReader sourceStream = new StreamReader(localFilePath, true);
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
				LogDelegate("response : " + response.StatusCode + " " + response.StatusDescription);
				//Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

				response.Close();
				return OperationResult<NoType>.OkResult;

			}
			catch (Exception e)
			{
				WebException we = (WebException)e;
				String status = ((FtpWebResponse)we.Response).StatusDescription;
				if (LogDelegate != null)
				{
					LogDelegate("[FTP] Exception envoi : " + we.Message + " /// " + status);
					Mailer mailer = new Mailer();
					mailer.LogDelegate = LogDelegate;

					string emailConf = ConfigurationManager.AppSettings["NotificationEmail"];
					mailer.SendMail(emailConf, "[Canal Collecte] Erreur FTP!", e.Message + " " + e.StackTrace, null, ConfigurationManager.AppSettings["NotificationEmail_CC"]);
				}
				return OperationResult<NoType>.BadResultFormat("[FTP] Exception envoi: {0} /// {1}", we.Message, we.Status);
			}
		}
	}

	public enum Mode
	{
		Ftp,
		Sftp
	}
}
