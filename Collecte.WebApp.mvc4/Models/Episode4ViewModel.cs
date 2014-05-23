using System.ComponentModel.DataAnnotations;

namespace Collecte.WebApp.Models
{
	public class Episode4ViewModel : BaseModel
	{
		[Required(ErrorMessage = "Merci de compléter le n° de voie."), StringLength(6, ErrorMessage = "Le n° de voie n'est pas valide.")]
		public string NumeroVoie { get; set; }
		[Required(ErrorMessage = "Merci de compléter le type de voie."), StringLength(20, ErrorMessage = "Le type de voie n'est pas valide.")]
		public string TypeVoie { get; set; }
		[Required(ErrorMessage = "Merci de compléter le nom de la voie."), StringLength(150, ErrorMessage = "Le nom de la voie n'est pas valide.")]
		public string NomVoie { get; set; }

		[Required(ErrorMessage = "Merci de compléter le nom de la ville."), StringLength(150, ErrorMessage = "Le nom de la ville n'est pas valide.")]
		public string City { get; set; }
		[Required(ErrorMessage = "Merci de compléter le code postal."), Range(1000, 99999, ErrorMessage = "Le n° de voie n'est pas valide.")] 
		public int Zipcode { get; set; }
	}
}