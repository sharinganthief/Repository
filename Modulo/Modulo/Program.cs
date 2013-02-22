using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
            string startText = System.IO.File.ReadAllText(@"C:\Users\sharbison\Desktop\Misc\Primality\2to1000.txt");

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
            

        }

        //public static bool TryParse(string startText)
        //{
        //    BigInteger testNumber;
        //    return TryParse(startText, out testNumber);
        //}

        //public static bool TryParse(string startText, out BigInteger testNumber)
        //{
        //    return TryParse(startText, out testNumber);
        //    //testNumber = new BigInteger();
        //    //return false;
        //}
    }
}