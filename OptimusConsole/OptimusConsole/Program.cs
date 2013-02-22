using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace OptimusConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			int fileNum = FileOperations.GetFileCount(@"D:\Muthafucking Numbers yo", "*.txt") - 1;

			//BigInteger x = ReadNumber(string.Format("D:\\big ass mafuckin number {0}.txt", ++fileNum));

			BigInteger y = ReadNumber(string.Format("D:\\Muthafucking Numbers yo\\big ass mafuckin number 1.txt"));

			int pow = ( int ) Math.Pow( 2, fileNum);

			Helpers.Reporting.WriteStart("Returning to last square: {0}");
			Console.WriteLine("will square {0} times to return to previous end number", fileNum);
			for (int i = 0; i < fileNum; i++)
			{
				Console.WriteLine("squaring for the {0} time to return", i.OrdinateInt());
				y = BigInteger.Pow(y, 2);
			}
			Helpers.Reporting.WriteResultToConsole("Finished returning to last square: Elapsed {0}");
			Console.WriteLine();			
			//BigInteger q = Divide(y, x);
			bool keepGoing = true;
			do
			{
				string sAsString;
				BigInteger s = Square(y, out sAsString);
				WriteNumber(string.Format("D:\\Muthafucking Numbers yo\\big ass mafuckin number {0}.txt", ++fileNum), sAsString);
				y = s;
				string key = ConsoleHelpers.ReadKeyTimeout("Keep going? ", 3);
				//ConsoleKeyInfo key = Console.ReadKey();
				if ( key.Equals( "N"))
					keepGoing = false;
				
				Console.WriteLine();
			} while (keepGoing);

			//BigInteger s = Square(y);

			//WriteNumber(string.Format("D:\\big ass mafuckin number {0}.txt", ++fileNum), s);

			//BigInteger s2 = Square(s);

			//WriteNumber(string.Format("D:\\big ass mafuckin number {0}.txt", ++fileNum), s2);

			Console.WriteLine("***************************************************");

			Console.ReadKey();
		}

		private static void WriteNumber(string path, string x)
		{
			Helpers.Reporting.WriteStart("Start write number to file and length to other file: {0}");
			
			Helpers.FileOperations.AppendToFile( path, x);
			Helpers.FileOperations.AppendToFile( @"D:\Muthafucking Numbers yo\Mapping of Numbers and lengths.txt",
				string.Format( " Path: {0}, Length: {1}", path, x.Length.OrdinateInt()), true);

			Helpers.Reporting.WriteEnd("End write number to file: {0}");
			Helpers.Reporting.WriteResultToConsole("Time to write: {0}");
			Console.WriteLine();
			
		}

		public static BigInteger ReadNumber( string path)
		{
			Helpers.Reporting.WriteStart("Start read number from file: {0}");
			string num = Helpers.FileOperations.ReadAllFromFile( path );

			Helpers.Reporting.WriteEnd("End read number from file: {0}");
			Helpers.Reporting.WriteResultToConsole("Time to read: {0}");
			Console.WriteLine();

			BigInteger x = new BigInteger(0);
			Helpers.Reporting.WriteStart("Start parse number from file: {0}");
			bool parsed = BigInteger.TryParse(num, out x);
			//if (! parsed)
			//	"Unsuccessful parse of a Big Integer from a file [ Path: {0} ]".ThrowFormattedException( path );

			Helpers.Reporting.WriteResultToConsole("End parse number from file: Time Taken = {0}");

			//Console.WriteLine("Results: [ Parsed = {0}, Length = {1} ] ", parsed, num.Length.OrdinateInt());

			Console.WriteLine();

			return x;
		}

		public static BigInteger Divide( BigInteger x, BigInteger y)
		{
						#region Start Divide

			Helpers.Reporting.WriteStart("Start division: {0}");
			BigInteger divisionResult = x / y;

			Helpers.Reporting.WriteEnd("End read division: {0}");
			Helpers.Reporting.WriteResultToConsole("Time to divide: {0}");
			Console.WriteLine("Results: [ Number = {1}, Length = {0} ] ", divisionResult.ToString().Length, divisionResult);

			#endregion

			return divisionResult;
		}

		public static BigInteger Square( BigInteger x, out string s)
		{
						#region Start Divide

			Helpers.Reporting.WriteStart("Start square: {0}");
			BigInteger squareResult = BigInteger.Pow(x, 2);

			Helpers.Reporting.WriteEnd("End square: {0}");
			Helpers.Reporting.WriteResultToConsole("Time to square: {0}");
			Console.WriteLine();

			Helpers.Reporting.WriteStart("Start Converting to string: {0}");
			string numberAsString = squareResult.ToString();
			Reporting.WriteResultToConsole("End converting to string, elapsed = {0}");
			Console.WriteLine();
			//timer.Restart();
			int numberLength = numberAsString.Length;
			//timer.Stop();
			//TimeSpan getLength = timer.Elapsed;
			string ordinalString = Helpers.StringHelpers.OrdinateInt(numberLength);
			Console.Write("Results: [ Length = ");
			ConsoleHelpers.WriteInDifferentColor( ordinalString, ConsoleColor.Green);
			Console.Write(" ] ");
			Console.WriteLine();

			#endregion

			s = numberAsString;
			return squareResult;
		}
	}
}
