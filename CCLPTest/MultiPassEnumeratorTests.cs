using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CozyCommandLineParser.Utils;
using NUnit.Framework;

namespace CCLPTest
{
    [TestFixture]
    public class MultiPassEnumeratorTests
    {
        [Test]
        public void MultiPassTest()
        {
            var mp = new MultiPassEnumerator<int>(new[] {1, 2, 3, 4});

            Assert.AreEqual(1, mp.GetNext());

            List<int> result = new List<int>();

            foreach (var value in mp)
            {
                if (value % 2 == 0) mp.SaveCurrentToNextPass();
                result.Add(value);
            }

            Assert.AreEqual(new[]{2, 3, 4}, result);
            Assert.AreEqual(0, mp.GetNext());
            Assert.AreEqual(0, mp.Current);

            result.Clear();
            foreach (var value in mp)
            {
                result.Add(value);
            }

            Assert.AreEqual(new[]{2, 4}, result);

            result.Clear();
            foreach (var value in mp)
            {
                result.Add(value);
            }

            Assert.AreEqual(0, result.Count);
        }
    }
}