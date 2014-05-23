using System.ComponentModel.DataAnnotations;
using Tools.Asp.net_MVC.Attributes;

namespace Collecte.WebApp.Models
{
	public class BasicUserInfoViewModel : BaseModel
	{
		public Civi Civi { get; set; }

		[NameValidation(isRequired: true, requiredErrorMessage: "Merci de renseigner votre nom.", malformedErrorMessage: "Votre nom n'est pas valide.", minLength: 1, maxLength: 100)]
		public string LastName { get; set; }
		[NameValidation(isRequired:true, requiredErrorMessage:"Merci de renseigner votre prénom.", malformedErrorMessage:"Votre prénom n'est pas valide.", minLength:1, maxLength:100)]
		public string FirstName { get; set; }
		[EmailValidation(isRequired: true, requiredErrorMessage:"Merci d'entrer une adresse e-mail.", malformedErrorMessage:"Merci d’entrer une adresse e-mail valide.")]
		public string Email { get; set; }
		[ZipCodeValidation(isRequired:true, requiredErrorMessage:"Merci de renseigner votre code postal.", malformedErrorMessage:"Code postal non valide.")]
		public string Zipcode { get; set; }

		[Required(ErrorMessage="Merci de préciser si vous êtes abonné.")]
		public bool? IsCanal { get; set; }
		public bool IsOffreGroupCanal { get; set; }
		[RequiredToBeTrue(ErrorMessage = "Vous devez accepter les conditions du règlement.")]
		public bool IsOptinRules { get; set; }

	}

	public enum Civi
	{
		Mme,
		Mr
	}
}