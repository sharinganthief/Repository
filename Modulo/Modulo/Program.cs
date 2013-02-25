using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Helpers;

namespace Modulo
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //
            // When 1000 is divided by 90, the remainder is 10.
            //
            //Console.WriteLine(1000 % 90);
            //Console.ReadLine();

            // The files used in this example are created in the topic 
            // How to: Write to a Text File. You can change the path and 
            // file name to substitute text files of your own. 

            // Example #1 
            // Read the file as one string. 
            string startText = System.IO.File.ReadAllText(@"C:\Users\sharbison\Desktop\Misc\Primality\2to1024.txt");

            BigInteger testNumber;

            bool parsed = BigInteger.TryParse(startText, out testNumber);

            // Display the file contents to the console. Variable text is a string.
            //System.Console.WriteLine("Contents of WriteText.txt = {0}", text);

            // Example #2 
            // Read each line of the file into a string array. Each element 
            // of the array is one line of the file. 
            //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Public\TestFolder\WriteLines2.txt");

            // Display the file contents by using a foreach loop.
            //System.Console.WriteLine("Contents of WriteLines2.txt = ");
            //foreach (string line in lines)
            //{
                // Use a tab to indent each line of the file.
                //Console.WriteLine("\t" + line);
            //}

            // Keep the console window open in debug mode.
            
            if (parsed)
            Console.WriteLine(testNumber);
            Console.ReadLine();
            
            //BigInteger numberSquared = testNumber ^ 2

            //StringBuilder writer = new StringBuilder(numberSquared);

            //using (StreamWriter outfile = new StreamWriter(@"C:\Users\sharbison\Desktop\Misc\Primality\2to2048.txt"))
            
            //    File.CreateText(@"C:\Users\sharbison\Desktop\Misc\Primality\2to2048.txt")
            //    outfile.Write(numberSquared.ToString());


            List<string> primes = new List<string>();

            //for (int i = 0; i < 100; i++)
            //{
            //    Console.WriteLine( i.CardinateInt() );
            //}

            // Cleans files into lists of primes, one time use

			for (int j = 1; j <= 50; j++)
			{
				Helpers.Reporting.WriteStart("Start read in: {0}");
				string fileContets = Helpers.FileOperations.ReadAllFromFile(
					string.Format(@"C:\Users\sharbison\Desktop\Misc\Primality\Primes\primes{0}.txt", j));
				Regex match = new Regex(@"The.*\(from primes.utm.edu\)");
				fileContets = match.Replace(fileContets, "");

				StringBuilder sb = new StringBuilder();

				string[] parts = fileContets.Split(new char[] {'\n', '\t', '\r', '\f', '\v', '\\'}, StringSplitOptions.RemoveEmptyEntries);
                //Reporting.WriteResultToConsole(string.Format("Done with {0} set of primes, elapsed: [ {1} ]"));
				int size = parts.Length;
                for (int i = 0; i < size; i++)
                {
                    sb.AppendFormat("{0} ", parts[i]);
                }

                primes = (sb.ToString().Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList());

				Console.WriteLine((primes.Count * j).OrdinateInt());
				Helpers.FileOperations.WriteListToFile( string.Format(@"C:\Users\sharbison\Desktop\Misc\Primality\Primes\CleanedPrimes\primes{0}.txt", j), primes);
			}

			Console.ReadKey();

            /*
             * 982451653 last prime in file, next number to test: 982451657
             * need to add logic to skip numbers ending in 5
             */

            // start get list

            /*
             * public static List<string> getAllFromFileAsList( this string path )
             * {
             *  return ReadAllFromFile ( path ).Split( ... ) .toList();
             * }
             * 
             * BigInteger testNumber = 982451657
             * BigInteger stopper = Math.Ceiling(Math.Pow(testnumber, 0.5));
             * do while (parts[i] < stopper)
             *   {
             *
             *   } 
             */

            //string fileContets = Helpers.FileOperations.ReadAllFromFile(
            //        string.Format(@"C:\Users\sharbison\Desktop\Misc\Primality\Primes\primes{0}.txt", j));

            // // list 
            //string[] parts = fileContets
            //    .Split(new char[] { '\n', '\t', '\r', '\f', '\v', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            //List<string> partsList = fileContets
            //    .Split(new char[] { '\n', '\t', '\r', '\f', '\v', '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList();


            /*
             * List<string> primeList = new List<string>();
             * 
             * for (int k = 0; k < 999999; k++)
             *{
             * 
             *foreach (string divisor in partsList)
             *{
             * try 
             *  {          
             *  BigInteger divisorAsInt = BigInteger.Parse(divisor);
             *  BigInteger x = testNumber % divisorAsInt;
             *  }
             *  catch (System.Exception)
             *  {
             *    Console.Writeline("{0} is an invalid divisor.", divisor);
             *    throw;
             *  }
             *  catch (x = 0)
             *  {
             *    Console.Writeline("{0} not prime.", testNumber);
             *    testNumber = testNumber + 2;
             *    i = 0 (trying to reset the divisor in partList to index 0);
             *    j = 0;
             *    throw;
             *  }
             *  finally
             *  {
             *  Console.Writeline("{0} not a divisor.", divisor);
             *  }
             * 
             *} 
             * Console.Writeline("{0} is prime.", testNumber);
             * primeList.Add(testnumber);
             *}
             */

            //Helpers.FileOperations.WriteListToFile( string.Format(@"C:\Users\sharbison\Desktop\Misc\Primality\Primes\CleanedPrimes\primes51.txt"), primeList);
            //logic to continue or leave ..
            //if x = 0 then , etc, restart loop
            //if no x = 0 value returned, testnumber prime, add to file at end then (parts[k] = testNumber, k++)
            //testNumber = testNumber + 2, i = 0, j = 0, etc, restart loop
              
             

            // end get list
        }
    }
}