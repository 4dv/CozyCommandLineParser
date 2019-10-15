
using System.Collections;
using System.Collections.Generic;

namespace CozyCommandLineParser.Utils
{
    public class MultiPassEnumerator<T> : IEnumerable<T>, IEnumerator<T>
    {
        private IEnumerator<T> enumerator;

        public MultiPassEnumerator(IEnumerable<T> values)
        {
            this.enumerator = values.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Reset();
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        public bool MoveNext()
        {
            IsPassFinished = !enumerator.MoveNext();
            return !IsPassFinished;
        }

        public bool IsPassFinished { get; private set; }

        public bool IsAllFinished => IsPassFinished && valuesForNextPass.Count == 0;

        /// <summary>
        /// reset doesn't restart current enumerator. Instead is starts new pass. If we have some values
        /// saved for the next pass, it first enumerate through them, then continue with unprocessed before items.
        /// If we don't have anything saved for the next pass, we just continue where we were.
        /// </summary>
        public void Reset()
        {
            if (valuesForNextPass.Count > 0)
            {
                while(MoveNext())
                    SaveCurrentToNextPass();
                enumerator = valuesForNextPass.GetEnumerator();
                valuesForNextPass = new List<T>();
            }
        }

        public T GetNext()
        {
            MoveNext();
            return Current;
        }

        /// <summary>
        /// returns Current value if pass is not finished, otherwise - default value of type T (null for classes)
        /// </summary>
        public T Current => IsPassFinished ? default(T) : enumerator.Current;

        object IEnumerator.Current => Current;

        private List<T> valuesForNextPass = new List<T>();

        public void SaveCurrentToNextPass()
        {
            valuesForNextPass.Add(Current);
        }
    }
}