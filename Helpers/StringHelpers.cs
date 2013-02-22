using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class StringHelpers
    {
		public static string OrdinateInt( this int intToOrdinate, bool addSuffix = true)
		{
			return string.Format("{0:#,0}{1}", intToOrdinate, addSuffix ? intToOrdinate.GetSuffix() : string.Empty );
		}

		public static string GetSuffix( this int x)
		{
			string suff;
			int ones = x % 10;
			decimal decToFloor = (Convert.ToDecimal(x/10));
			int tens = ( int ) Math.Floor(decToFloor) % 10;
			if (tens == 1) suff = "th";
			else 
			switch (ones) 
			{
				case 1 : suff = "st"; break;
				case 2 : suff = "nd"; break;
				case 3 : suff = "rd"; break;
				default : suff = "th"; break;
			}
			return suff;
		}

        public static string ToStringSafe(this string stringToCheck)
        {
            return string.IsNullOrEmpty(stringToCheck) ? string.Empty : stringToCheck;
        }

        public static string ToCommaSeparatedString(this List<string> list)
        {
            return list.ToDeliminatedString(", ");
        }

        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? string.Format("{0:0} days, ", span.Days) : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} hours, ", span.Hours) : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} minutes, ", span.Minutes) : string.Empty,
                span.Duration().Seconds > 0 ? string.Format("{0:0} seconds", span.Seconds) : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

            return formatted;
        }

        public static string ToDeliminatedString(this List<string> list, string delimeter)
        {
            //StringBuilder sb = new StringBuilder();
            //string[] listAsArray = list.ToArray();
            //for (int i = 0; i < listAsArray.Count(); i++)
            //{
            //    if (i == 0)
            //        sb.Append(string.Format("{0}",
            //                                listAsArray[i] ?? string.Empty));
            //    else
            //        sb.Append(string.Format("{0}{1}", delimeter,
            //                                listAsArray[i] ?? string.Empty));
            //}

            //return sb.ToString();

            return list.Aggregate((i, j) => i + delimeter + j);
        }

        public static List<string> CommaSeparatedStringToList(this string stringToSeparate)
        {
            return stringToSeparate.DeliminatedStringToList(',');
        }

        public static List<string> DeliminatedStringToList(this string stringToSeparate, char deliminator)
        {
            return stringToSeparate.Split(new[] { deliminator }).ToArray().ToList();
        }

        public static string Concat(string separator, params string[] strings)
        {

            string result = "";

            for (int i = 0; i < strings.Length; i++)
            {

                if (i > 0)

                    result += separator;

                result += strings[i];

            }

            return result;

        }

		//public static void ShowMessage(this string message, params object[] args)
		//{
		//	//MessageBox.Show(string.Format(message, args));
		//}

        public static IEnumerable<string> SmartSplit(this string input, int maxLength)
        {
            int i = 0;
            while (i + maxLength < input.Length)
            {
                int index = input.LastIndexOf(' ', i + maxLength);
                yield return input.Substring(i, index - i);

                i = index + 1;
            }

            yield return input.Substring(i);
        }

        public static Tuple<string, string> SplitNameToFirstAndRemainingAsLast(this string fullName)
        {
            List<string> nameParts = fullName.Trim().Split(' ').ToList();
            string first = nameParts.FirstOrDefault();
            nameParts.Remove(first);
            string last = string.Empty;
            if (nameParts.Count >= 1)
            {
                last = nameParts.ToDeliminatedString(" ");
            }


            return new Tuple<string, string>(first, last);
        }

        public static Tuple<string, string> GetTupleFromName(this string name)
        {
            return !string.IsNullOrWhiteSpace(name)
                       ? name.SplitNameToFirstAndRemainingAsLast()
                       : new Tuple<string, string>(null, null);
        }
    }
}
