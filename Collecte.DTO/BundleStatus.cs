using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collecte.DTO
{
	public enum BundleStatus
	{
		NoFileCreated,
		CsvInCreated,
		CsvInSentToCanal,
		CsvOutReceived,
		CsvOutParsed,
		XmlCreated,
		XmlSentToTrade
	}
}
