using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Collecte.DTO
{
	public class ConnexionType
	{
		[Key]
		public int Id { get; set; }
		public string UrlToken { get; set; }
	}
}
