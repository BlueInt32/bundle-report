#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace Tools.String
{
	public enum RegexTemplate
	{
		NomPrenomPseudoOuSimilaire, 
		Mail,
		Telephone,
		Url,
		UrlWord,
		TitragePhrase,
		YoutubeUrl
	}
	public class RegexValidators
	{
		public static Dictionary<RegexTemplate, string> RegexDictionary
		{
			get
			{
				return new Dictionary<RegexTemplate, string>
				{
					{RegexTemplate.NomPrenomPseudoOuSimilaire, @"^[a-zA-Z0-9áàâäãéèêëíìîïĩóòôöõúüîôûúùýñçÿ\s.\-_' \+\&]+$"},
					{RegexTemplate.TitragePhrase, @"^[a-zA-Z0-9àäâéèêëïîöôüûçÇÉÈ\s\.!?\-_', \+\&\(\)]+$"},
					{RegexTemplate.Mail, @"^([\w\-\._]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$"},
					{RegexTemplate.Telephone, "^(01|02|03|04|05|06|07|09)[0-9]{8}$"},
					{RegexTemplate.Url, @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)"},
					{RegexTemplate.UrlWord, "[a-z0-9_-]+"},
					{RegexTemplate.YoutubeUrl, @"http://youtu\.be/[a-zA-Z0-9]+"}
				};
			}
		}

			/// <summary>
		/// Liste des OperationResults accumulés par l'instance courante de Validator
		/// </summary>
		private List<StdResult<NoType>> Results { get; set; }

		public RegexValidators()
		{
			Results = new List<StdResult<NoType>>();
		}

		/// <summary>
		/// Retourne un OperationResult résumant tous les strings validés par l'instance courante de Validator.
		/// </summary>
		public StdResult<NoType> InstanceResult
		{
			get
			{
				bool resultBool = true;
				StringBuilder resultString = new StringBuilder();
				foreach (StdResult<NoType> operationResult in Results)
				{
					resultBool = resultBool & operationResult.Result;
					resultString.AppendLine(operationResult.Message);
				}

				return new StdResult<NoType> { Result = resultBool, Message = resultString.ToString() };
			}
		}

		private StdResult<NoType> TooLongResult(string identificateur, int sizeMax)
		{
			return StdResult<NoType>.BadResult(string.Format("{0} trop long (max {1})", identificateur, sizeMax));
		}

		public StdResult<NoType> GenericValidation(string input, RegexTemplate regexTemplate, string uiIdentification)
		{
			Regex regGeneric = new Regex(RegexDictionary[regexTemplate]);
			StdResult<NoType> res = regGeneric.IsMatch(input)
									? StdResult<NoType>.OkResult
									: StdResult<NoType>.BadResult(string.Format("{1} n'est pas valide.", input, uiIdentification));
			Results.Add(res);
			return res;
		}

		/// <summary>
		/// Validates the mail.
		/// </summary>
		/// <param name="email">Email à valider</param>
		/// <param name="identification">[Optionnel] Permet d'identifier le champ à valider pour une liste de résultats.</param>
		/// <returns></returns>
		public StdResult<NoType> ValidateMail(string email, string identification = "Email")
		{
			Regex regEmail = new Regex(RegexDictionary[RegexTemplate.Mail]);
			StdResult<NoType> res = regEmail.IsMatch(email)
									? StdResult<NoType>.OkResult
									: StdResult<NoType>.BadResultFormat("Email '{0}' mal formé.", email);
			Results.Add(res);
			return res;
		}

		/// <summary>
		/// Validates the nom prenom pseudo.
		/// </summary>
		/// <param name="nomPrenom">Nom, prenom, pseudo...</param>
		/// <param name="identification">[Optionnel] Permet d'identifier le champ à valider pour une liste de résultats (Propriété 'Results')</param>
		/// <returns></returns>
		public StdResult<NoType> ValidateNomPrenomPseudo(string nomPrenom, string identification = "Nom")
		{
			Regex regNomPrenom = new Regex(RegexDictionary[RegexTemplate.NomPrenomPseudoOuSimilaire]);

			StdResult<NoType> res = regNomPrenom.IsMatch(nomPrenom)
									? StdResult<NoType>.OkResult
									: StdResult<NoType>.BadResult("ValidateNomPrenomPseudo");

			Results.Add(res);
			return res;
		}

		/// <summary>
		/// Validates the telephone.
		/// </summary>
		/// <param name="input">Numéro de téléphone à Valider.</param>
		/// <param name="identification">[Optionnel] Permet d'identifier le champ à valider pour une liste de résultats (Propriété 'Results').</param>
		/// <returns></returns>
		public StdResult<NoType> ValidateTelephone(string input, string identification = "Téléphone")
		{
			string trimInput = input.Trim();
			Regex r = new Regex("Telephone");

			StdResult<NoType> res = r.IsMatch(trimInput)
									? StdResult<NoType>.OkResult
									: StdResult<NoType>.BadResult("ValidateTelephone");

			Results.Add(res);
			return res;
		}

		public StdResult<NoType> ValidatePhraseLibelle(string input, string identification)
		{
			string trimInput = input.Trim();
			Regex r = new Regex(RegexDictionary[RegexTemplate.TitragePhrase]);

			StdResult<NoType> res = r.IsMatch(trimInput)
									? StdResult<NoType>.OkResult
									: StdResult<NoType>.BadResult(string.Format("Le format {0} n'est pas valide", identification));

			Results.Add(res);
			return res;
		}

		/// <summary>
		/// Validates the date.
		/// </summary>
		/// <param name="input">Valide une date (sous forme d'un string d'entrée)</param>
		/// <param name="identification">[Optionnel] Permet d'identifier le champ à valider pour une liste de résultats (Propriété 'Results').</param>
		/// <returns></returns>
		public StdResult<NoType> ValidateDate(string input, string identification = "Date")
		{
			DateTime scannedDateTime;
			StdResult<NoType> res = DateTime.TryParse(input, out scannedDateTime)
									? StdResult<NoType>.OkResult
									: StdResult<NoType>.BadResult("ValidateDate");

			Results.Add(res);
			return res;
		}

		public StdResult<NoType> ValidateSizeMax(string input, int sizeMax, string identification = "SizeMax")
		{
			StdResult<NoType> res = input.Length > sizeMax ? TooLongResult(identification, sizeMax) : StdResult<NoType>.OkResult;
			Results.Add(res);
			return res;
		}
	}
}