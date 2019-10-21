using System;
using System.IO;
using NUnit.Framework;

namespace CCLPTest
{
    public class ConsoleRedirect : IDisposable
    {
        private TextWriter oldOut;
        private StringWriter writer;

        public ConsoleRedirect()
        {
            oldOut = Console.Out;
            writer = new StringWriter();
            Console.SetOut(writer);
        }

        public void CheckLastOutput(string expected)
        {
            Assert.AreEqual(expected, writer.ToString());
            writer.GetStringBuilder().Clear();
        }

        public void Dispose()
        {
            Console.SetOut(oldOut);
            writer.Dispose();
        }
    }
}