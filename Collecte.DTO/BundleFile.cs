using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Collecte.DTO
{
	public class BundleFile
	{
		public int BundleFileId { get; set; }

		public BundleFileType Type { get; set; }

		[StringLength(255)]
		public string FileName { get; set; }

		public DateTime CreationDate { get; set; }

		public int BundleId { get; set; }
		public virtual Bundle Bundle { get; set; }
	}
}
