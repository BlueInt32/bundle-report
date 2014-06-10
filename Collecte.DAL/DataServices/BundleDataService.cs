using Collecte.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Tools;
using Tools.Orm;
using Tools.EntityFramework;
using System.Data.Entity;

namespace Collecte.DAL
{
	public class BundleDataService : Crud<Bundle, DateTime>
	{
		public OperationResult<Bundle> Create(Bundle inputObject)
		{
			using (CollecteContext context = new CollecteContext())
			{
				context.Bundles.Add(inputObject);
				
				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();
				if (errors.Count() > 0)
					return OperationResult<Bundle>.BadResultWithList("Erreur(s) de validation pour la création, voir ErrorList", errors.FormatValidationErrorList());

				int nbSaved = context.SaveChanges();
				return nbSaved != 1 ? OperationResult<Bundle>.BadResult("Bundle non créé", inputObject) : OperationResult<Bundle>.OkResultInstance(inputObject);
			}
		}
		public OperationResult<Bundle> Retrieve(DateTime date)
		{
			using (CollecteContext context = new CollecteContext())
			{
				Bundle BundleFromDb = context.Bundles.Where(b => b.Date == date).FirstOrDefault();
				if (BundleFromDb == null)
					return OperationResult<Bundle>.BadResult("Bundle introuvable");
				return OperationResult<Bundle>.OkResultInstance(BundleFromDb);
			}
		}
		public OperationResult<Bundle> Update(Bundle editedBundle)
		{
			using (CollecteContext context = new CollecteContext())
			{
				context.Entry(editedBundle).State = EntityState.Modified;

				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();
				if (errors.Count() > 0)
					return OperationResult<Bundle>.BadResultWithList("Erreur(s) de validation pour la modification, voir ErrorList", errors.FormatValidationErrorList());

				context.SaveChanges();
				return OperationResult<Bundle>.OkResultInstance(editedBundle);
			}
		}
		public OperationResult<Bundle> Delete(DateTime date)
		{
			using (CollecteContext context = new CollecteContext())
			{
				Bundle BundleFromDb = context.Bundles.Where(u => u.Date == date).FirstOrDefault();
				if (BundleFromDb == null)
					return OperationResult<Bundle>.BadResult("Bundle introuvable");
				context.Bundles.Remove(BundleFromDb);
				if (context.SaveChanges() != 1)
					return OperationResult<Bundle>.BadResult("Bundle non supprimé", BundleFromDb);
				else
					return OperationResult<Bundle>.OkResult;
			}
		}

		public OperationResult<Bundle> GetBundleByDate(DateTime date)
		{
			if (date.Minute != 0 || date.Second != 0 || date.Hour != 0 || date.Millisecond != 0)
				throw new CollecteException("Les bundles ne traitent que des dates formattées à minuit pile.");

			using (CollecteContext context = new CollecteContext())
			{
				try
				{
					Bundle BundleFromDb = context.Bundles.Include("BundleFiles").Where(b => b.Date == date).FirstOrDefault();
					if (BundleFromDb == null)
						return OperationResult<Bundle>.BadResult("Bundle introuvable");
					return OperationResult<Bundle>.OkResultInstance(BundleFromDb);
				}
				catch
				{
					return OperationResult<Bundle>.BadResult("Probleme de connexion à la base.");
				}
			}
		}

		public OperationResult<List<Bundle>> ListBundles()
		{
			using (CollecteContext context = new CollecteContext())
			{
				context.Database.Connection.Open();
				return OperationResult<List<Bundle>>.OkResultInstance(context.Bundles.Include("BundleFiles").OrderByDescending(b => b.Date).ToList());
			}
		}

		public OperationResult<Bundle> DeleteBundles()
		{
#if DEBUG // this method should not be called in production...
			using (CollecteContext context = new CollecteContext())
			{
				foreach (var item in context.BundleFiles)
				{
					context.BundleFiles.Remove(item);
				}

				foreach (var item in context.Bundles)
				{

					context.Bundles.Remove(item);
				}
				context.SaveChanges();
			}
			return OperationResult<Bundle>.OkResult;
#endif
			return OperationResult<Bundle>.OkResult;
		}
	}
}
