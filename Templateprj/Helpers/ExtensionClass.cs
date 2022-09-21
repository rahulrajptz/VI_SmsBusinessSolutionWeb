using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Templateprj.Helpers
{
	public static class ExtensionClass
	{
		public static bool IsValidHttpUrl(this string source)
		{
			Uri uriResult;
			return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
		}

		public static int? ToNullableInt32(this string s)
		{
			int i;
			if (int.TryParse(s, out i)) return i;
			return null;
		}

		public static long? ToNullableInt64(this string s)
		{
			long i;
			if (long.TryParse(s, out i)) return i;
			return null;
		}

		public static string ToNullableString(this string s, string replaceStr = null)
		{
			if (string.IsNullOrWhiteSpace(s) || s == "null")
				return replaceStr;
			return s;
		}

		public static string ToMaskedString(this string str)
		{
			var index = str.IndexOf('@');
			var length = str.Length;
			if (index == -1)
			{
				if (length > 5)
					return str.Replace(str.Substring(3, 3), "***");
				
				return length > 2 ? str.Replace(str.Substring(0, 1), "*") : str;
			}
			//Email
			try
			{
				if (index > 4 & length > 10)
					return str.Replace(str.Substring(index - 3, 3), "***").Replace(str.Substring(index + 1, 3), "***");
				if (index > 1 & length > 5)
					return str.Replace(str.Substring(index - 1, 1), "*").Replace(str.Substring(index + 1, 3), "***");
				if (index > 4 && length > 5)
					return str.Replace(str.Substring(index - 3, 3), "***").Replace(str.Substring(index + 1, 1), "*");
			}
			catch
			{
				if (length > 5)
					return str.Replace(str.Substring(3, 3), "***");
				if (length > 2)
					return str.Replace(str.Substring(0, 1), "*");
			}
			return str;
		}

		public static string RemoveSpecialChars(this string str)
		{
			if (string.IsNullOrWhiteSpace(str))
				return "";

			str = str.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("&", "&amp;");
			str = str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "&#xA;").Replace("\t", "&#x9;");
			//	str = Regex.Replace(str, @"[!""#$%&'*+,\-./;<=>?@[\\\]^_`{|}~]", "").Trim(); ;
			return str;
		}

		public static string ToLowerOrDefault(this string input, string defaultValue = "")
		{
			return (input != null ? input.ToLower() : defaultValue);
		}

		public static string ToTitleCase(this string input)
		{
			if (string.IsNullOrWhiteSpace(input))
				return null;

			var textInfo = new CultureInfo("en-US", false).TextInfo;
			return textInfo.ToTitleCase(input);
		}

		public static string[] GetColumnNames(this DataTable dataTable)
		{
			return (from DataColumn column in dataTable.Columns select column.ColumnName).ToArray();
		}

		private static readonly Random random = new Random();
		public static string RandomString(int length)
		{
			const string chars = "ABDEFGHLMNOPQRSTUVWXYZ123456789abdefghinpqrt";
			return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
		}

		#region XML
		public static string SerializeXml<T>(this T source) where T : class, new()
		{
			//	source.CheckNull("Object to be serialized.");

			var serializer = new XmlSerializer(typeof(T));
			using (var writer = new StringWriter())
			{
				serializer.Serialize(writer, source);
				return writer.ToString();
			}
		}
		/// <summary>
		/// Extension method to string which attempts to deserialize XML with the same name as the source string.
		/// </summary>
		/// <typeparam name="T">The type which to be deserialized to.</typeparam>
		/// <param name="xml">The source string</param>
		/// <returns>The deserialized object, or null if unsuccessful.</returns>
		public static T DeserializeXml<T>(this string xml) where T : class, new()
		{
			//	XML.CheckNull("XML-string");

			var serializer = new XmlSerializer(typeof(T));
			using (var reader = new StringReader(xml))
			{
				try { return (T)serializer.Deserialize(reader); }
				catch { return null; } // Could not be deserialized to this type.
			}
		}
		#endregion
	}

	public static class MySelectList
	{
		public static SelectList ToSelectList(this DataTable table, List<SelectListItem> custmList = null, string valueField = "VALUE", string textField = "TEXT")
		{
			var list = table.ToSelectItemList(custmList, valueField, textField);
			return new SelectList(list, valueField, textField);
		}

		public static SelectList ToSelectList(this DataTable table, object selectedValue, List<SelectListItem> custmList = null, string valueField = "VALUE", string textField = "TEXT")
		{
			var list = table.ToSelectItemList(custmList, valueField, textField);
			return new SelectList(list, valueField, textField, selectedValue);
		}

		public static List<SelectListItem> ToSelectItemList(this DataTable table, List<SelectListItem> custmList = null, string valueField = "VALUE", string textField = "TEXT")
		{
			var list = new List<SelectListItem>();
			if (custmList != null)
				list = custmList.ToList();

			if (table == null) return list;

			list.AddRange(from DataRow row in table.Rows
				select new SelectListItem()
				{
					Text = row[textField].ToString(), Value = row[valueField].ToString()
				});

			return list;
		}

		public static SelectList GetEmptyList()
		{
			var list = new List<SelectListItem>();
			return new SelectList(list, "Value", "Text");
		}

		public static SelectList GetDefaultList(string defaultText)
		{
			var list = new List<SelectListItem> {new SelectListItem {Text = defaultText}};
			return new SelectList(list, "Value", "Text");
		}

        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }
    }
	
}