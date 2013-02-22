using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class IntHelpers
    {
        public static int ToIntSafe<T>(this T tParam)
        {
            return Int32.Parse(tParam.ToString());
        }
        /// <summary>
        /// Takes in a list of int and returns a list of ranged tuples from the min to the max by a range of 10
        /// </summary>
        /// <param name="numbersToSplit"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, int>> GetContiguousRanges(this List<int> numbersToSplit)
        {
            return GetContiguousRanges(numbersToSplit, 15);
        }

        /// <summary>
        /// Takes in a list of int and returns a list of ranged tuples from the min to the max by a specified range
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="range"> </param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, int>> GetContiguousRanges(this List<int> numbersToSplit, int range)
        {
            List<Tuple<int, int>> results = new List<Tuple<int, int>>();

            numbersToSplit.Sort();
            //int totalMax = numbersToSplit.Max();
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (numbersToSplit.Any())
            {
                //Get the min and max
                int min = numbersToSplit.Min();
                int max = min + range;

                //Remove occurences of the minimum - maximum
                //if (numbersToSplit.Any( => x == max))
                    numbersToSplit.RemoveAll(num => num <= max);

                results.Add(new Tuple<int, int>(min, max));
            }

            watch.Stop();
            long elapsed = watch.ElapsedMilliseconds / 1000;
            if (elapsed >= 3)
                "Length list took [ {0} ] seconds - consider revising the code.... douchefag".ThrowFormattedException(new object[] { });


            return results;


        }

		public static int GetRandom( int lower, int higher)
		{
			Random r = new Random(Guid.NewGuid().GetHashCode());
			return r.Next(lower, higher);
		}
		
		public static double GetRandom()
		{
			Random r = new Random(Guid.NewGuid().GetHashCode());
			return r.NextDouble()*100;
		}
    }
}


#region Archived code 

            ////results = new List<Tuple<int, int>>();
            //watch.Restart();
//return results;
//return nums.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / 3).Select(x => x.Select(v => v.Value).ToList()).ToList();
////Below is far bette ron performance but has less accurate buckets

//while (min <= totalMax)
//{
//    //Get the min and max
//    int max = min + range;
//    results.Add(new Tuple<int, int>(min, max));
//    min = max;
//}
//if (numbersToSplit != null)
//{

//    min = 0;
//    int max = 0;
//    int increment = range;
//    while (numbersToSplit.Any())
//    {
//        //Get the min and max
//        min = numbersToSplit.Min();
//        max = min + range;

//        //Remove occurences of the minimum - maximum
//        numbersToSplit.RemoveAll(num => num <= max);

//        results.Add(new Tuple<int, int>(min, max));
//    }
//}

            //watch.Stop();
            //elapsed = watch.ElapsedMilliseconds / 1000;



//Worse performance but it has more accurate buckets - what i want :)
//if (numbersToSplit != null)
//{

//int min = 0;
//int max = 0;
//int increment = range;
//while (numbersToSplit.Any())
//{
//    //Get the min and max
//    min = numbersToSplit.Min();
//    max = min + range;

//    //Remove occurences of the minimum - maximum
//    numbersToSplit.RemoveAll(num => num <= max);
//    //numbersToSplit.ToList().RemoveAll();

//    results.Add(new Tuple<int, int>(min, max));
//}
//}

#endregion