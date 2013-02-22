using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public class Linqify<T> : IEnumerable<T>
    {
        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static string SQLToLINQ(string sql)
        {
            //parse clauses
            //dynamic linq that ho
            return string.Empty;
        }

    }
}
