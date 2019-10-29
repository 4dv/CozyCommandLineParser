using System;

namespace CozyCommandLineParser.Checkers
{
    public class CheckFailedException : Exception
    {
        public CheckFailedException(string message) : base(message)
        {
        }
    }
}