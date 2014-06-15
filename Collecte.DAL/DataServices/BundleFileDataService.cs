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
		public Tools.StdResult<BundleFile> Create(BundleFile inputObject, DateTime bundleDate)
		{
			using (CollectContext context = new CollectContext())
			{
				context.BundleFiles.Add(inputObject);
				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();
				if (errors.Count() > 0)
					return StdResult<BundleFile>.BadResultWithList("Erreur(s) de validation pour la création, voir ErrorList", errors.FormatValidationErrorList());

				// get the right bundle
				if (bundleDate.Minute != 0 || bundleDate.Second != 0 || bundleDate.Hour != 0 || bundleDate.Millisecond != 0)
					throw new CollecteException("Les bundles ne traitent que des dates formattées à minuit pile.");

				Bundle BundleFromDb = context.Bundles.Include("BundleFiles").Where(b => b.Date == bundleDate).FirstOrDefault();
				if (BundleFromDb == null)
					throw new CollecteException("Bundle introuvable à cette date.");

				BundleFromDb.BundleFiles.Add(inputObject);

				int nbSaved = context.SaveChanges();
				return nbSaved < 1 ? StdResult<BundleFile>.BadResult("BundleFile non créé", inputObject) : StdResult<BundleFile>.OkResultInstance(inputObject);
			}
		}
		public Tools.StdResult<BundleFile> Create(BundleFile inputObject)
		{
			throw new NotImplementedException();
		}

		public Tools.StdResult<BundleFile> Get(int id)
		{
			using (CollectContext context = new CollectContext())
			{
				BundleFile UserFromDb = context.BundleFiles.Where(u => u.BundleFileId == id).FirstOrDefault();
				if (UserFromDb == null)
					return StdResult<BundleFile>.BadResult("BundleFile introuvable");
				return StdResult<BundleFile>.OkResultInstance(UserFromDb);
			}
		}

		public Tools.StdResult<BundleFile> Update(BundleFile inputObject)
		{
			using (CollectContext context = new CollectContext())
			{
				context.BundleFiles.Attach(inputObject);
				var entry = context.Entry(inputObject);
				//entry.State = EntityState.Modified;

				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();
				if (errors.Count() > 0)
					return StdResult<BundleFile>.BadResultWithList("Erreur(s) de validation pour la création, voir ErrorList", errors.FormatValidationErrorList());

				context.SaveChanges();
				return StdResult<BundleFile>.OkResultInstance(inputObject);
			}
		}

		public Tools.StdResult<BundleFile> Delete(int id)
		{
			using (CollectContext context = new CollectContext())
			{
				BundleFile UserFromDb = context.BundleFiles.Where(u => u.BundleFileId == id).FirstOrDefault();
				if (UserFromDb == null)
					return StdResult<BundleFile>.BadResult("BundleFile introuvable");
				context.BundleFiles.Remove(UserFromDb);
				if (context.SaveChanges() != 1)
					return StdResult<BundleFile>.BadResult("BundleFile non supprimé", UserFromDb);
				else
					return StdResult<BundleFile>.OkResult;
			}
		}
	}
}
