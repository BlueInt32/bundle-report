#region Usings

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Tools;

#endregion

namespace Tools.Mail
{
	public enum MailType
	{
		BasicText,
		WithTemplateHtml
	}

	public class ReplaceField
	{
		public string Replace { get; set; }
		public string With { get; set; }
	}

	/// <summary>
	/// 
	/// Mail Helper lets you manage Emails by too possible ways : 
	/// - BasicText (direct string input)
	/// - WithTemplateHtml (fill in TemplateHtml property and ReplacementFields list). TemplateHtml directory must contain 'email.html' and 'images' directory
	/// 
	/// 
	/// </summary>
	public class MailHelper
	{
		#region Private Fields
		private string _content;
		private string _templatePath;
		#endregion
		#region Constructor

		public MailHelper(MailType mailType)
		{
			MailType = mailType;
			Recipients = new List<MailAddress>();
		}
		#endregion

		public MailType MailType { get; set; }
		public MailAddress ReplyTo { get; set; }
		public MailAddress Sender { get; set; }
		public List<MailAddress> Recipients { get; set; }
		public string MailSubject { get; set; }
		public List<Attachment> Attachments { get; set; }
		public List<MailAddress> CCs { get; set; }


		/// <summary>
		/// Lets you set ReplaceField items that are to be replaced within the html template. You have to call ProcessReplacements() method setting them.
		/// </summary>
		public List<ReplaceField> ReplacementFields { get; set; }
		public string Content
		{
			get { return _content; }
			set
			{
				if (MailType == MailType.WithTemplateHtml)
				{
					throw new Exception("You cannot fill in Content when mailtype is 'WithTemplateHtml', it's automagically done under the hood when you fill in TemplatePath.");
				}
				_content = value;
			}
		}

		public string TemplatePath
		{
			get
			{
				return _templatePath;
			}
			set
			{
				if (MailType == MailType.BasicText)
				{
					throw new Exception("You cannot fill in TemplatePath when mailtype is 'BasicText'. You use Content property for that.");
				}
				_templatePath = value;
				_content = CreateHtmlEmail(_templatePath);
			}
		}
		/// <summary>
		/// Launch fields replacements provided within ReplacementFields and set Content. This must be called after setting Content or TemplatePath.
		/// </summary>
		public void ProcessReplacements()
		{
			if (ReplacementFields == null)
				return;
			foreach (ReplaceField replaceField in ReplacementFields)
			{
				_content = _content.Replace(replaceField.Replace, replaceField.With);
			}
		}

		/// <summary>
		/// - Replaces replaceFields that need to be.
		/// - Send emails to each recipients.
		/// </summary>
		/// <returns></returns>
		public OperationResult<MailHelper> Send()
		{
			try
			{
				ProcessReplacements();
				foreach (MailAddress destinataire in Recipients)
				{
					MailMessage mailMessage = new MailMessage
					{
						BodyEncoding = Encoding.UTF8,
						IsBodyHtml = false,
						From = Sender
					};
					if(CCs  != null)
						foreach (MailAddress cC in CCs)
						{
							mailMessage.CC.Add(cC);
						}
					mailMessage.To.Add(destinataire);
					mailMessage.Body = Content;
					mailMessage.SubjectEncoding = Encoding.UTF8;
					mailMessage.Subject = MailSubject;
					SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SmtpPath"], Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]));
					mailMessage.BodyEncoding = Encoding.UTF8;
					mailMessage.ReplyTo = ReplyTo;

					AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Content, new ContentType("text/html; charset=" + Encoding.UTF8.BodyName));
					htmlView.TransferEncoding = TransferEncoding.SevenBit;

					mailMessage.AlternateViews.Add(htmlView);

					if (Attachments != null)
					{
						foreach (Attachment attachment in Attachments)
						{
							mailMessage.Attachments.Add(attachment);
						}
					}


					smtp.Send(mailMessage);
				}

				return OperationResult<MailHelper>.OkResultInstance(this);
			}
			catch (Exception e)
			{
				Log.Error("MailHelper", e.Message + e.StackTrace);
				return OperationResult<MailHelper>.BadResult(e.Message);
			}
		}

		[Obsolete]
		public static string StripHtml(string inputString)
		{
			return Regex.Replace(inputString, @"<(.|\n)*?>", string.Empty);
		}

		private string CreateHtmlEmail(string templatePath)
		{
			FileHelper fileUtils = new FileHelper();
			string html = fileUtils.ReadFileFromDisk(Path.Combine(templatePath, "email.html"));
			return html;
		}
	}
}