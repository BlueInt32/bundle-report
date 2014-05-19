using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CsvHelper.TypeConversion;

namespace Collecte.CanalServiceBase
{
	//public class DMYDateConverter : CsvHelper.TypeConversion.DefaultTypeConverter
	//{
	//	private const String dateFormat = @"dd/MM/yyyy HH:mm";

	//	public override bool CanConvertFrom(Type type)
	//	{
	//		bool ret = typeof(String) == type;
	//		return ret;
	//	}

	//	public override bool CanConvertTo(Type type)
	//	{
	//		bool ret = typeof(System.DateTime) == type;
	//		return ret;
	//	}

	//	public override object ConvertFromString(TypeConverterOptions options, string text)
	//	{
	//		DateTime newDate = default(System.DateTime);
	//		try
	//		{
	//			newDate = DateTime.ParseExact(text, dateFormat, CultureInfo.InvariantCulture);
	//		}
	//		catch (Exception ex)
	//		{
	//			System.Diagnostics.Debug.WriteLine(String.Format(@"Error parsing date '{0}': {1}", text, ex.Message));
	//		}

	//		return newDate;
	//	}

	//	public override string ConvertToString(TypeConverterOptions options, object value)
	//	{
	//		DateTime oldDate = (System.DateTime)value;
	//		return oldDate.ToString(dateFormat);
	//	}
	//}
}
