#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Tools
{
	public class OmnitureHelper
	{
		public Dictionary<string, string> Tags { get; set; }

		public OmnitureHelper Init()
		{
			Tags = new Dictionary<string, string>
			       	{
			       		{"Intro", "EC - ZA - Event - Grande Incruste - Intro"},
			       		{"Home", "EC - ZA - Event - Grande Incruste - Home"},
			       		{"Casting sélection scène", "EC - ZA - Event - Grande Incruste - Casting selection scene"},
			       		{"Casting", "EC - ZA - Event - Grande Incruste - Casting selection scene"},
			       		{"Casting détection webcam", "EC - ZA - Event - Grande Incruste - Casting detection webcam"},
			       		{"Casting Mafiosa", "EC - ZA - Event - Grande Incruste - Casting mafiosa"},
			       		{"Casting Maison close", "EC - ZA - Event - Grande Incruste - Casting maison close"},
			       		{"Casting Braquo", "EC - ZA - Event - Grande Incruste - Casting braquo"},
			       		{"Casting formulaire inscription", "EC - ZA - Event - Grande Incruste - Casting Formulaire inscription"},
			       		{"CastingFini", "EC - ZA - Event - Grande Incruste - Casting fini"},
			       		{"Realisations", "EC - ZA - Event - Grande Incruste - Les realisations"},
			       		{"VoirRealisation", "EC - ZA - Event - Grande Incruste - Video lecture- ID Video"},
			       		{"MesRealisations", "EC - ZA - Event - Grande Incruste - Mes realisations"},
			       		{"Jury", "EC - ZA - Event - Grande Incruste - Jury"},
			       		{"Tournage", "EC - ZA - Event - Grande Incruste - Journee tournage"},
			       	};

			return this;
		}

		public string OutputJS
		{
			get
			{
				StringBuilder output = new StringBuilder();
				output.Append("[");

				foreach (KeyValuePair<string, string> keyValuePair in
					Tags.Where(keyValuePair => keyValuePair.Key.StartsWith("Casting")))
				{
					output.AppendFormat("'{0}',", keyValuePair.Value);
				}

				output.Remove(output.Length - 1, 1);
				output.Append("]");
				return output.ToString();
			}
		}
	}
}