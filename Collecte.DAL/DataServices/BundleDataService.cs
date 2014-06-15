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
		public StdResult<Bundle> Create(Bundle inputObject)
		{
			using (CollectContext context = new CollectContext())
			{
				context.Bundles.Add(inputObject);
				
				int nbSaved = context.SaveChanges();
				return nbSaved != 1 
					? StdResult<Bundle>.BadResult("Bundle non créé", inputObject) 
					: StdResult<Bundle>.OkResultInstance(inputObject);
			}
		}
		public StdResult<Bundle> Get(DateTime date)
		{
			using (CollectContext context = new CollectContext())
			{
				Bundle BundleFromDb = context.Bundles.Where(b => b.Date == date).FirstOrDefault();
				if (BundleFromDb == null)
					return StdResult<Bundle>.BadResult("Bundle introuvable");
				return StdResult<Bundle>.OkResultInstance(BundleFromDb);
			}
		}
		public StdResult<Bundle> Update(Bundle editedBundle)
		{
			using (CollectContext context = new CollectContext())
			{
				context.Entry(editedBundle).State = EntityState.Modified;

				context.SaveChanges();
				return StdResult<Bundle>.OkResultInstance(editedBundle);
			}
		}
		public StdResult<Bundle> Delete(DateTime date)
		{
			using (CollectContext context = new CollectContext())
			{
				Bundle BundleFromDb = context.Bundles.Where(u => u.Date == date).FirstOrDefault();
				if (BundleFromDb == null)
					return StdResult<Bundle>.BadResult("Bundle introuvable");
				context.Bundles.Remove(BundleFromDb);
				if (context.SaveChanges() != 1)
					return StdResult<Bundle>.BadResult("Bundle non supprimé", BundleFromDb);
				else
					return StdResult<Bundle>.OkResult;
			}
		}

		public StdResult<List<Bundle>> ListBundles()
		{
			using (CollectContext context = new CollectContext())
			{
				var resultList = context.Bundles
					.Include("BundleFiles")
					.OrderByDescending(b => b.Date)
					.ToList();
				return StdResult<List<Bundle>>.OkResultInstance(resultList);
			}
		}

		public StdResult<Bundle> DeleteBundles()
		{
#if DEBUG // this method should not be called in production...
			using (CollectContext context = new CollectContext())
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
			return StdResult<Bundle>.OkResult;
#endif
			return StdResult<Bundle>.OkResult;
		}
	}
}
