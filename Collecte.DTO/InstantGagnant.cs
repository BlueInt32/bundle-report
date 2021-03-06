﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Collecte.DTO
{
	public class InstantGagnant
	{
		public int InstantGagnantId { get; set; }
		public int LotId { get; set; }
		public DateTime StartDateTime { get; set; }


		public bool Won { get; set; }
		public DateTime? WonDate { get; set; }
		public string Label { get; set; }
		public string FrontHtmlId { get; set; }

		public Guid? UserId { get; set; }
		public virtual User User { get; set; }
	}
}
