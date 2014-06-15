using Collecte.DTO;
using System;
using System.Collections.Generic;
using System.Linq;	
using System.Text;
using Tools;

namespace Collecte.DAL
{
	public class InstantGagnantDataService
	{
		public StdResult<InstantGagnant> GetCurrentInstantGagnant()
		{
			using (CollectContext context = new CollectContext())
			{
				DateTime now = DateTime.Now;
				var query = from ig in context.InstantsGagnants
							where now > ig.StartDateTime
							&& !ig.Won
							orderby ig.StartDateTime
							select ig;
				if (query.Any<InstantGagnant>())
				{
					InstantGagnant ig = query.First<InstantGagnant>();
					return StdResult<InstantGagnant>.OkResultInstance(ig);
				}
				else
					return StdResult<InstantGagnant>.BadResult("Pas d'instant gagnant disponible");

			}
		}

		public StdResult<InstantGagnant> AddInstantGagnant(InstantGagnant instance)
		{
			using (CollectContext context = new CollectContext())
			{
				context.InstantsGagnants.Add(instance);
				context.SaveChanges();

				return StdResult<InstantGagnant>.OkResultInstance(instance);
			}
		}

		public StdResult<InstantGagnant> DeleteInstantsGagnant()
		{
			using (CollectContext context = new CollectContext())
			{
				foreach (User user in context.Users)
				{
					user.InstantsGagnantWon = null;
				}

				foreach (InstantGagnant ig in context.InstantsGagnants)
				{
					context.InstantsGagnants.Remove(ig);
				}
				context.SaveChanges();

				return StdResult<InstantGagnant>.OkResult;
			}
		}

		public StdResult<InstantGagnant> WinInstantGagnant(User MainUser)
		{
			using (CollectContext context = new CollectContext())
			{
				DateTime now = DateTime.Now;
				var query = from ig in context.InstantsGagnants
							where now > ig.StartDateTime
							&& !ig.Won
							orderby ig.StartDateTime
							select ig;
				if (query.Any<InstantGagnant>())
				{
					InstantGagnant ig = query.First<InstantGagnant>();

					ig.UserId = MainUser.Id;
					ig.Won = true;
					ig.WonDate = DateTime.Now;
					MainUser.InstantsGagnantWon.Add(ig);
					context.SaveChanges();
					return StdResult<InstantGagnant>.OkResultInstance(ig);
				}
				else
					return StdResult<InstantGagnant>.BadResult("Pas d'instant gagnant disponible");
			}
		}
	}
}
