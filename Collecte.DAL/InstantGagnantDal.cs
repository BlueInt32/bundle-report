using Collecte.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace Collecte.DAL
{
	public class InstantGagnantDal
	{
		public OperationResult<InstantGagnant> GetCurrentInstantGagnant()
		{
			using (DataContext context = new DataContext())
			{
				DateTime now = DateTime.Now;
				var query = from ig in context.InstantsGagnants
							where now > ig.Start
							&& !ig.Won
							orderby ig.Start
							select ig;
				if (query.Any<InstantGagnant>())
				{
					InstantGagnant ig = query.First<InstantGagnant>();
					return OperationResult<InstantGagnant>.OkResultInstance(ig);
				}
				else
					return OperationResult<InstantGagnant>.BadResult("Pas d'instant gagnant disponible");

			}
		}

		public OperationResult<InstantGagnant> AddInstantGagnant(InstantGagnant instance)
		{
			using (DataContext context = new DataContext())
			{
				context.InstantsGagnants.Add(instance);
				context.SaveChanges();

				return OperationResult<InstantGagnant>.OkResultInstance(instance);
			}
		}

		public OperationResult<InstantGagnant> DeleteInstantsGagnant()
		{
			using (DataContext context = new DataContext())
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

				return OperationResult<InstantGagnant>.OkResult;
			}
		}

		public OperationResult<InstantGagnant> WinInstantGagnant(User MainUser)
		{
			using (DataContext context = new DataContext())
			{
				DateTime now = DateTime.Now;
				var query = from ig in context.InstantsGagnants
							where now > ig.Start
							&& !ig.Won
							orderby ig.Start
							select ig;
				if (query.Any<InstantGagnant>())
				{
					InstantGagnant ig = query.First<InstantGagnant>();

					ig.UserId = MainUser.Id;
					ig.Won = true;
					ig.WonDate = DateTime.Now;
					MainUser.InstantsGagnantWon.Add(ig);
					context.SaveChanges();
					return OperationResult<InstantGagnant>.OkResultInstance(ig);
				}
				else
					return OperationResult<InstantGagnant>.BadResult("Pas d'instant gagnant disponible");
			}
		}
	}
}
