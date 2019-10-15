
using System.Collections.Generic;

namespace CozyCommandLineParser.Utils
{
    public class FriendlyEnumerator<T> where T:class
    {
        private readonly IEnumerator<T> enumerator;
        public bool IsFinished { get; private set; }

        public FriendlyEnumerator(IEnumerable<T> values)
        {
            enumerator = values.GetEnumerator();
        }

        /// <summary>
        /// return next element or null if we come to the end. GetNext should be called at least one time, before we can use Current
        /// </summary>
        /// <returns></returns>
        public T GetNext()
        {
            IsFinished = !enumerator.MoveNext();
            return Current;
        }

        /// <summary>
        /// return current element or null if we come to the end. GetNext should be called at least one time, before we can use Current
        /// </summary>
        public T Current => IsFinished ? null : enumerator.Current;
    }
}