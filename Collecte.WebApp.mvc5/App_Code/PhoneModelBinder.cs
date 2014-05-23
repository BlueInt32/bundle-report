using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Collecte.WebApp.App_Code
{
	public class PhoneModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			PhoneModel model = new PhoneModel();
			string postPhone = bindingContext.ValueProvider.GetValue("numtel") != null ? bindingContext.ValueProvider.GetValue("numtel").AttemptedValue : "";
			
			
			Regex validPhone = new Regex(@"^(0[1-79]([0-9]{2}){4})+$");
			if (string.IsNullOrWhiteSpace(postPhone))
				return new PhoneModel { IsValid = false, Message = "Le numéro de téléphone est obligatoire." };
			if (!validPhone.IsMatch(postPhone))
				return new PhoneModel { IsValid = false, Message = "Numéro de téléphone non valide." };
			model.Phone = postPhone;

			model.IsValid = true;
			model.Message = "ok";
			return model;
		}
	}

	public class PhoneModel
	{
		public string Phone { get; set; }

		public bool IsValid { get; set; }
		public string Message { get; set; }

	}
}