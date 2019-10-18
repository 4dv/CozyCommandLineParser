using System.Collections.Generic;
using CozyCommandLineParser.Utils;
using NUnit.Framework;

namespace CCLPTest
{
    public class MultiPassEnumeratorTests
    {
        [Test]
        public void MultiPassTest()
        {
            var mp = new MultiPassEnumerator<int>(new[] {1, 2, 3, 4});

            var result = new List<int>();

            foreach (int value in mp)
            {
                if (value % 2 == 0) mp.SaveCurrentToNextPass();
                result.Add(value);
            }

            Assert.AreEqual(new[] {1, 2, 3, 4}, result);
            Assert.AreEqual(0, mp.GetNext());
            Assert.AreEqual(0, mp.Current);

            result.Clear();

            mp.Reset();
            Assert.AreEqual(2, mp.GetNext());
            mp.SaveCurrentToNextPass();

            foreach (int value in mp) result.Add(value);

            Assert.AreEqual(new[] {2, 4}, result);

            result.Clear();
            foreach (int value in mp) result.Add(value);

            Assert.AreEqual(0, result.Count);
        }
    }
}