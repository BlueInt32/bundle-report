using Collecte.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.Orm;
using Tools.EntityFramework;
using Tools;
using System.Data.Entity.Validation;

namespace Collecte.DAL
{
	public class BundleFileDataService : Crud<BundleFile, int>
	{
		public Tools.OperationResult<BundleFile> Create(BundleFile inputObject, DateTime bundleDate)
		{
			using (CollecteContext context = new CollecteContext())
			{
				context.BundleFiles.Add(inputObject);
				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();
				if (errors.Count() > 0)
					return OperationResult<BundleFile>.BadResultWithList("Erreur(s) de validation pour la création, voir ErrorList", errors.FormatValidationErrorList());

				// get the right bundle
				if (bundleDate.Minute != 0 || bundleDate.Second != 0 || bundleDate.Hour != 0 || bundleDate.Millisecond != 0)
					throw new CollecteException("Les bundles ne traitent que des dates formattées à minuit pile.");

				Bundle BundleFromDb = context.Bundles.Include("BundleFiles").Where(b => b.Date == bundleDate).FirstOrDefault();
				if (BundleFromDb == null)
					throw new CollecteException("Bundle introuvable à cette date.");

				BundleFromDb.BundleFiles.Add(inputObject);

				int nbSaved = context.SaveChanges();
				return nbSaved < 1 ? OperationResult<BundleFile>.BadResult("BundleFile non créé", inputObject) : OperationResult<BundleFile>.OkResultInstance(inputObject);
			}
		}
		public Tools.OperationResult<BundleFile> Create(BundleFile inputObject)
		{
			throw new NotImplementedException();
		}

		public Tools.OperationResult<BundleFile> Retrieve(int id)
		{
			using (CollecteContext context = new CollecteContext())
			{
				BundleFile UserFromDb = context.BundleFiles.Where(u => u.BundleFileId == id).FirstOrDefault();
				if (UserFromDb == null)
					return OperationResult<BundleFile>.BadResult("BundleFile introuvable");
				return OperationResult<BundleFile>.OkResultInstance(UserFromDb);
			}
		}

		public Tools.OperationResult<BundleFile> Update(BundleFile inputObject)
		{
			using (CollecteContext context = new CollecteContext())
			{
				context.BundleFiles.Attach(inputObject);
				var entry = context.Entry(inputObject);
				//entry.State = EntityState.Modified;

				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();
				if (errors.Count() > 0)
					return OperationResult<BundleFile>.BadResultWithList("Erreur(s) de validation pour la création, voir ErrorList", errors.FormatValidationErrorList());

				context.SaveChanges();
				return OperationResult<BundleFile>.OkResultInstance(inputObject);
			}
		}

		public Tools.OperationResult<BundleFile> Delete(int id)
		{
			using (CollecteContext context = new CollecteContext())
			{
				BundleFile UserFromDb = context.BundleFiles.Where(u => u.BundleFileId == id).FirstOrDefault();
				if (UserFromDb == null)
					return OperationResult<BundleFile>.BadResult("BundleFile introuvable");
				context.BundleFiles.Remove(UserFromDb);
				if (context.SaveChanges() != 1)
					return OperationResult<BundleFile>.BadResult("BundleFile non supprimé", UserFromDb);
				else
					return OperationResult<BundleFile>.OkResult;
			}
		}
	}
}
