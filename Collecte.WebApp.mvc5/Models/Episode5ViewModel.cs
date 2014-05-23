using System.ComponentModel.DataAnnotations;

namespace Collecte.WebApp.Models
{
	public class Episode5ViewModel : BaseModel
	{


		[Required(ErrorMessage = "Merci d’indiquer votre numéro de téléphone."), RegularExpression(@"(01|02|03|04|05|06|07|09)[0-9]{8}", ErrorMessage = "Merci d’indiquer un numéro de téléphone valide comportant 10 chiffres.<br />Votre numéro doit commencer par : 01 - 02 - 03 - 04 - 05 - 06 - 07 - 09")]
		public string Phone { get; set; }


	}
}