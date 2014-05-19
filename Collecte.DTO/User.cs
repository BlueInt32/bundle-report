using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Tools.Asp.net_MVC.Attributes;

namespace Collecte.DTO
{
	public class User
	{
		public User()
		{
			Id = Guid.NewGuid();
		}

		[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }
		[StringLength(100), Required, RegularExpression(@"[a-zA-Z0-9áàâäãéèêëíìîïĩóòôöõúüîôûúùýñçÿ\s.\-_' \+\&]+", ErrorMessage="Mauvais format.")]
		public string LastName { get; set; }
		[StringLength(100), Required, RegularExpression(@"[a-zA-Z0-9áàâäãéèêëíìîïĩóòôöõúüîôûúùýñçÿ\s.\-_' \+\&]+", ErrorMessage="Mauvais format.")]
		public string FirstName { get; set; }
		[StringLength(200), Required, EmailValidation(true, "Merci d'entrer une adresse e-mail." , "Merci d’entrer une adresse e-mail valide.")]
		public string Email { get; set; }
		[StringLength(3)]
		public string Civilite { get; set; }
		[StringLength(300)]
		public string Address { get; set; }
		[StringLength(10)]
		public string NumeroVoie { get; set; }
		[StringLength(20)]
		public string TypeVoie { get; set; }
		[StringLength(200)]
		public string NomVoie { get; set; }


		[Required]
		public int Zipcode { get; set; }
		[StringLength(200)]
		public string City { get; set; }
		[StringLength(10), RegularExpression(@"^(01|02|03|04|05|06|07|09)[0-9]{8}$", ErrorMessage = "Mauvais format.")]
		public string Phone { get; set; }
		public bool IsCanal { get; set; }
		public bool IsOffreGroupCanal { get; set; }

		[StringLength(200)]
		public string FriendEmail1 { get; set; }
		[StringLength(200)]
		public string FriendEmail2 { get; set; }
		[StringLength(200)]
		public string FriendEmail3 { get; set; }

		public bool HasSentEmailsToFriends { get; set; }

		public int HeroicScore { get; set; }
		public int HeroicStatus { get; set; }

		public virtual List<AnswerChoice> AnswerChoices { get; set; }

		public virtual List<InstantGagnant> InstantsGagnantWon { get; set; }

		/// <summary>
		/// This array stores chances bits by step: first step validated "10000000"
		/// all steps validated : "11111111"
		/// only first and last step validated : "10000001"
		/// </summary>
		[StringLength(8)]
		public string ChancesByStep {get;set;}
		public short ChancesAmount { get; set; }

		public void SetStepChance(int index, bool validated)
		{
			char[] array = ChancesByStep.ToCharArray();
			array[index] = validated ? '1' : '0';
			ChancesByStep = array.Aggregate(string.Empty, (current, c) => string.Concat(current, c));
			ChancesAmount = (short)ChancesByStep.ToCharArray().Count(c => c == '1');
		}
		public void InitStepChances()
		{
			ChancesByStep = "100";
			ChancesAmount = 1;
		}

		public DateTime CreationDate { get; set; }
		public DateTime? ParticipationDate { get; set; }


		/// <summary>
		/// Provide user's origin
		/// </summary>
		[StringLength(25)]
		public string Provenance { get; set; }
	}
}
