using Collecte.DAL;
using Collecte.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace Collecte.Logic
{
	public class InstantGagnantLogic
	{
		InstantGagnantDataService IgDal { get; set; }
		public InstantGagnantLogic()
		{
			IgDal = new InstantGagnantDataService();
		}
		public OperationResult<InstantGagnant> GetCurrentInstantGagnant()
		{
			return IgDal.GetCurrentInstantGagnant();
		}

		public OperationResult<InstantGagnant> AddInstantGagnant(InstantGagnant instance)
		{

			return IgDal.AddInstantGagnant(instance);
		}

		public OperationResult<InstantGagnant> DeleteInstantsGagnant()
		{
			return IgDal.DeleteInstantsGagnant();
		}


		public OperationResult<InstantGagnant> PlayInstantGagnant(User MainUser)
		{
			return IgDal.WinInstantGagnant(MainUser);
		}
	}
}
