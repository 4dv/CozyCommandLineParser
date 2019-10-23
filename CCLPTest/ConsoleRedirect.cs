using System;
using System.IO;
using NUnit.Framework;

namespace CCLPTest
{
    public class ConsoleRedirect : IDisposable
    {
        private TextWriter oldOut;
        public TextWriter OriginalOutput => oldOut;
        private StringWriter writer;

        public ConsoleRedirect()
        {
            oldOut = Console.Out;
            writer = new StringWriter();
            Console.SetOut(writer);
        }

        public string Output => writer.ToString();

        public void CheckLastOutput(string expected)
        {
            string actual = writer.ToString();
            ClearOutput();
            Assert.AreEqual(expected, actual);
        }

        public void ClearOutput()
        {
            writer.GetStringBuilder().Clear();
        }

        public void Dispose()
        {
            Console.SetOut(oldOut);
            writer.Dispose();
        }
    }
}