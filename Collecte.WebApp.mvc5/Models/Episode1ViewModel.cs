using System;
using Tools.Asp.net_MVC.Attributes;

namespace Collecte.WebApp.Models
{
	public class GodsonsMailsModel : BaseModel
	{
		/// <summary>
		/// will be used for TRADE DOUBLER tracking
		/// </summary>
		public Guid UserId { get; set; }
		[EmailValidation(isRequired: false, malformedErrorMessage:"Merci d’entrer une adresse e-mail valide.<br/>Format requis : xxxx@xxx.x", requiredErrorMessage :null)]
		public string Email1 { get; set; }
		[EmailValidation(isRequired: false, malformedErrorMessage: "Merci d’entrer une adresse e-mail valide.<br/>Format requis : xxxx@xxx.x", requiredErrorMessage: null)]
		public string Email2 { get; set; }
		[EmailValidation(isRequired: false, malformedErrorMessage: "Merci d’entrer une adresse e-mail valide.<br/>Format requis : xxxx@xxx.x", requiredErrorMessage: null)]
		public string Email3 { get; set; }

		public bool IsUserOptin { get; set; }
	}
}