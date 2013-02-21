using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Helpers;

namespace testRead
{
	class Program
	{
		static void Main(string[] args)
		
		{
			List<string> primes = new List<string>();

			for (int i = 0; i < 100; i++)
			{
				Console.WriteLine( i.CardinateInt() );
			}


			for (int j = 1; j <= 50; j++)
			{
				Helpers.Reporting.WriteStart("Start read in: {0}");
				string FileContets = Helpers.FileOperations.ReadAllFromFile(
					string.Format(@"D:\Muthafucking Numbers yo\Primes\Primes\CleanedPrimes\primes{0}.txt", j));
				Regex match = new Regex(@"The.*\(from primes.utm.edu\)");
				FileContets = match.Replace(FileContets, "");

				StringBuilder sb = new StringBuilder();

				string[] parts = FileContets.Split(new char[] {'\n', '\t', '\r', '\f', '\v', '\\'},
				                                   StringSplitOptions.RemoveEmptyEntries);
				Helpers.Reporting.WriteResultToConsole(string.Format("Done with {0} set of primes, elapsed: [ {1} ]", j.CardinateInt()));
				int size = parts.Length;
				for (int i = 0; i < size; i++)
					sb.AppendFormat("{0} ", parts[i]);

				primes = (sb.ToString().Split(new[] {' ',}, StringSplitOptions.RemoveEmptyEntries).ToList());

				Console.WriteLine( (primes.Count * j).OrdinateInt());
				Helpers.FileOperations.WriteListToFile( string.Format(@"D:\Muthafucking Numbers yo\Primes\Primes\CleanedPrimes\primes{0}.txt", j), primes);
			}

			Console.ReadKey();
		}
	}
}
