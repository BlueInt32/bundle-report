using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collecte.DAL
{
	public class TradeDoublerProvider
	{
		public static int GetTradeDoublerSequenceNumber()
		{
			using (DataContext context = new DataContext())
			{
				var sequenceNumber = (from seq in context.TradeDoublerIndex
									 where seq.Id == 1
									 select seq).FirstOrDefault();
				return sequenceNumber != null ? sequenceNumber.Value : 0;
			}
		}
		public static void SetTradeDoublerSequenceNumber(int value)
		{
			using (DataContext context = new DataContext())
			{
				var sequenceNumber = (from seq in context.TradeDoublerIndex
									  where seq.Id == 1
									  select seq).FirstOrDefault();
				if (sequenceNumber != null)
				{
					sequenceNumber.Value = value;


					context.SaveChanges();
				}
			}
		}

	}
}
