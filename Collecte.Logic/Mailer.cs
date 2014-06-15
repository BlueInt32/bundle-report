using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Tools;
using Tools.Mail;

namespace Collecte.Logic
{
	public class Mailer
	{
		public Action<string> LogDelegate { get; set; }
		public StdResult<NoType> SendMail(string mailTo, string mailSubject, string mailContent, Attachment attachedFile, string ccs)
		{
			try
			{
				MailHelper mh = new MailHelper(MailType.BasicText);
				mh.MailSubject = mailSubject;
				mh.MailType = MailType.BasicText;
				string emailConf = mailTo;

				mh.Recipients = emailConf.Split(',').ToList().ConvertAll(emailAddress => new MailAddress(emailAddress));
				mh.Sender = new MailAddress("canal.collecte@rappfrance.com");
				mh.ReplyTo = new MailAddress("canal.collecte@rappfrance.com");
				mh.Content = mailContent;
				if(!string.IsNullOrWhiteSpace(ccs))
					mh.CCs = ccs.Split(',').ToList().ConvertAll(emailAddress => new MailAddress(emailAddress));
				if (attachedFile != null)
				{
					mh.Attachments = new List<Attachment> { attachedFile };
				}
				LogDelegate(string.Format("Mailer is gonna send a mail to {0}", mailTo));
				//mh.SetContentDirect(text);

				mh.Send();
				return StdResult<NoType>.OkResult;
			}
			catch (Exception e)
			{
				LogDelegate(string.Format("Exception while sending mail to {0}. E = {1} [{2}]", mailTo, e.Message, e.StackTrace));
				return StdResult<NoType>.BadResult(e.Message);
			}

		}
	}
}
