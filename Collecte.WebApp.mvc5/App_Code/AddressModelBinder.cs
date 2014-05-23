using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Collecte.WebApp.App_Code
{
	public class AddressModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			string postNumvoie = bindingContext.ValueProvider.GetValue("numvoie") != null ? bindingContext.ValueProvider.GetValue("numvoie").AttemptedValue:"";
			string postTypevoie = bindingContext.ValueProvider.GetValue("typevoie") != null ? bindingContext.ValueProvider.GetValue("typevoie").AttemptedValue:"";
			string postNomvoie = bindingContext.ValueProvider.GetValue("nomvoie") != null ? bindingContext.ValueProvider.GetValue("nomvoie").AttemptedValue:"";
			string postVille = bindingContext.ValueProvider.GetValue("ville") != null ? bindingContext.ValueProvider.GetValue("ville").AttemptedValue:"";
			string postCp = bindingContext.ValueProvider.GetValue("cp") != null ? bindingContext.ValueProvider.GetValue("cp").AttemptedValue : "";


			AddressModel model = new AddressModel { IsValid = true, Message = "", HtmlId = "" };
			List<string> badIds = new List<string>();
			Regex validWords = new Regex(@"^(\w|[0-9]|\-| )+$");
			if (string.IsNullOrWhiteSpace(postNumvoie))
			{
				model.IsValid = false; model.Message = "Le numéro de voie est obligatoire."; badIds.Add("numvoie");
			}
			else if (!validWords.IsMatch(postNumvoie))
			{
				model.IsValid = false; model.Message = "Numéro de voie non valide."; badIds.Add("numvoie");
			}
			model.NumVoie = postNumvoie;

			if (string.IsNullOrWhiteSpace(postTypevoie))
			{
				model.IsValid = false; model.Message = "Le type de voie est obligatoire."; badIds.Add("typevoie");
			}
			else if (!validWords.IsMatch(postTypevoie))
			{
				model.IsValid = false; model.Message = "Type de voie non valide."; badIds.Add("typevoie");
			}
			model.TypeVoie = postTypevoie;

			if (string.IsNullOrWhiteSpace(postNomvoie))
			{
				model.IsValid = false; model.Message = "Le nom de voie est obligatoire."; badIds.Add("nomvoie");
			}
			else if (!validWords.IsMatch(postNomvoie))
			{
				model.IsValid = false; model.Message = "Nom de voie non valide."; badIds.Add("nomvoie");
			}
			model.NomVoie = postNomvoie;

			if (string.IsNullOrWhiteSpace(postVille))
			{
				model.IsValid = false; model.Message = "Le nom de votre ville est obligatoire."; badIds.Add("ville");
			}
			else if (!validWords.IsMatch(postVille))
			{

				model.IsValid = false; model.Message = "Nom de ville non valide."; badIds.Add("ville");
			}
			model.City = postVille;


			int zipcode;
			if (string.IsNullOrWhiteSpace(postCp))
			{
				model.IsValid = false; model.Message = "Le code postal est obligatoire."; badIds.Add("cp");
			}
			else if (postCp.Length != 5 || !int.TryParse(postCp, out zipcode))
			{
				model.IsValid = false; model.Message = "Code postal non valide."; badIds.Add("cp");
			}

			model.HtmlId = string.Join(",", badIds);
			return model;
		}
	}

	public class AddressModel
	{
		public string NomVoie { get; set; }
		public string TypeVoie { get; set; }
		public string NumVoie { get; set; }
		public string City { get; set; }
		public int ZipCode { get; set; }

		public bool IsValid { get; set; }
		public string Message { get; set; }
		public string HtmlId { get; set; }
	}
}