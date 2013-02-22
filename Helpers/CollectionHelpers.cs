using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Helpers
{
    public static class CollectionHelpers
    {
      public static void Sort<TSource, TValue>(this List<TSource> source,Func<TSource, TValue> selector)
      {
        var comparer = Comparer<TValue>.Default;
        source.Sort((x, y) => comparer.Compare(selector(x), selector(y)));
      }

      public static void SortDescending<TSource, TValue>(this List<TSource> source, Func<TSource, TValue> selector)
      {
          var comparer = Comparer<TValue>.Default;
          source.Sort((x, y) => comparer.Compare(selector(y), selector(x)));
      }

      public static IList<T> Shuffle<T>(this IList<T> list)
      {
          RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
          int n = list.Count;
          while (n > 1)
          {
              byte[] box = new byte[1];
              do provider.GetBytes(box);
              while (!(box[0] < n * (Byte.MaxValue / n)));
              int k = (box[0] % n);
              n--;
              T value = list[k];
              list[k] = list[n];
              list[n] = value;
          }
          return list;
      }

      public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this List<TKey> keys, List<TValue> values)
      {

          Dictionary<TKey, TValue> dictToReturn = new Dictionary<TKey, TValue>();
          List<TValue>.Enumerator valuesEnumerator = values.GetEnumerator();
          foreach (TKey key in keys)
          {
              valuesEnumerator.MoveNext();
              dictToReturn.Add(key, valuesEnumerator.Current);
          }

          return dictToReturn;
      }

        public static V GetValue<T,V>( this Dictionary<T,V> dictionary, T key)
        {
            V value;
            dictionary.TryGetValue(key, out value);
            return value;
        }

      public static IEnumerable<Tuple<T, V>> EnumerablesToTupledEnumerable<T, V>(this IEnumerable<T> firstItems, IEnumerable<V> secondItems) 
          where T : class 
          where V : class
      {
          List<Tuple<T,V> > tupledList = new List<Tuple<T, V>>();
          IEnumerator<V> secondEnumerator = secondItems.GetEnumerator();
          
          foreach (T firstItem in firstItems)
          {
              secondEnumerator.MoveNext();
              tupledList.Add(new Tuple<T, V>(firstItem, secondEnumerator.Current));
              
          }

          return tupledList;
      }
    }
    
}



#region Archived code

//public static IEnumerable<Tuple<T, V>> EnumerablesToTupledEnumerable<T, V>(this IEnumerable<T> firstItems, IEnumerable<V> secondItems) 
//         where T : class 
//         where V : class
//     {

//         if (firstItems.Count() != secondItems.Count())
//         {
//             "Enumerable counts are not equal".ThrowFormattedException();
//         }
//         List<Tuple<T,V> > tupledList = new List<Tuple<T, V>>();
//         List<V> enumerable = secondItems as List<V> ?? secondItems.ToList();
//         IEnumerator<V> secondEnumerator = enumerable.GetEnumerator();

//             firstItems.Each<T>((x, i) => tupledList.Add(new Tuple<T, V>(x, enumerable.ElementAt(i))));


//         return tupledList;
//     }

//     public static void Each<T>(this IEnumerable els, Action<T, int> a)
//     {
//         int i = 0;
//         foreach (T e in els)
//         {
//             a(e, i++);
//         }
//     }
//   }

#endregion