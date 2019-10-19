using System.Collections;
using System.Collections.Generic;

namespace CozyCommandLineParser.Utils
{
    public class MultiPassEnumerator<T> : IEnumerable<T>, IEnumerator<T>
    {
        private const int INITIAL_POS = -1;

        private int currentPos = INITIAL_POS;
        private bool saveCurrentToNextPass;
        private List<T> values;
        private List<T> valuesForNextPass = new List<T>();

        public MultiPassEnumerator(IEnumerable<T> values)
        {
            this.values = new List<T>(values);
        }

        public bool SaveCurrentToNextPassDefault { get; set; }

        public bool IsPassFinished => currentPos >= values.Count;

        public bool IsAllFinished => IsPassFinished && valuesForNextPass.Count == 0;

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
        }

        public bool MoveNext()
        {
            if (saveCurrentToNextPass && currentPos >= 0)
                valuesForNextPass.Add(Current);

            currentPos++;
            saveCurrentToNextPass = SaveCurrentToNextPassDefault;
            Current = IsPassFinished ? default(T) : values[currentPos];
            return !IsPassFinished;
        }

        /// <summary>
        /// reset doesn't restart current enumerator. Instead is starts new pass. If we have some values
        /// saved for the next pass, it first enumerate through them, then continue with unprocessed before items.
        /// If we don't have anything saved for the next pass, we just continue where we were.
        /// </summary>
        public void Reset()
        {
            MoveNext(); // to save current, if it should be saved
            currentPos--; // to position before next element

            if (valuesForNextPass.Count == 0) return;

            bool orig = SaveCurrentToNextPassDefault;
            SaveCurrentToNextPassDefault = true;
            while (MoveNext()) ;
            SaveCurrentToNextPassDefault = orig;

            values = valuesForNextPass;
            valuesForNextPass = new List<T>();
            currentPos = INITIAL_POS;
            Current = default(T);
        }

        /// <summary>
        /// returns Current value if pass is not finished, otherwise - default value of type T (null for classes)
        /// </summary>
        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public T GetNext()
        {
            MoveNext();
            return Current;
        }

        public void SaveCurrentToNextPass()
        {
            saveCurrentToNextPass = true;
        }
    }
}