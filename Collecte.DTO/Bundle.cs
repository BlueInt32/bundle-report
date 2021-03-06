﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Collecte.DTO
{
	public class Bundle
	{
		[Key]
		public int BundleId { get; set; }

		[Required]
		public DateTime Date { get; set; }

		public BundleStatus Status { get; set; }

		public int? NbInscriptions { get; set; }
		public int? NbRetoursCanal { get; set; }
		public int? NbOk { get; set; }
		public int? NbKo { get; set; }
		
		public virtual List<BundleFile> BundleFiles { get; set; }
	}
}
