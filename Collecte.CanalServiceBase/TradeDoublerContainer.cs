using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Collecte.CanalServiceBase.tradedoublerStructure
{
	[XmlRoot("tradeDoublerPending")]
	public class TradeDoublerContainer
	{
		[XmlElement("organizationId")]
		public string OrganizationId { get; set; }
		[XmlElement("sequenceNumber")]
		public int SequenceNumber { get; set; }
		[XmlElement("numberOfTransactions")]
		public int NumberOfTransactions { get; set; }

		[XmlArray("transactions")]
		[XmlArrayItem("transaction")]
		public List<TradeDoublerTransaction> Transactions { get; set; }

		public void SaveToFile(string filePath)
		{
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");
			XmlSerializer xs = new XmlSerializer(typeof(TradeDoublerContainer));
			using (StreamWriter wr = new StreamWriter(filePath))
			{
				xs.Serialize(wr, this, ns);
			}
		}
	}

	//[XmlRoot("transaction")]
	public class TradeDoublerTransaction
	{
		public string eventId { get; set; }
		public Guid orderNumber { get; set; }
		public string status { get; set; }
	}
}
