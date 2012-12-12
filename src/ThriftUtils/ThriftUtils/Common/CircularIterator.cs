/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System.Collections;
using System.Collections.Generic;

namespace ThriftUtils.Common
{
    internal class CircularIterator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;

        public CircularIterator(IEnumerable<T> array)
        {
            enumerator = CreateEnumerator(array).GetEnumerator();
        }

        private static IEnumerable<T> CreateEnumerator(IEnumerable<T> enumerable)
        {
            var list = new List<T>(enumerable);
            for (;;)
            {
                foreach (var item in list)
                {
                    yield return item;
                }
            }
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            enumerator.Dispose();
        }

        #endregion

        #region Implementation of IEnumerator

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        public T Current
        {
            get { return enumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        #endregion
    }
}


