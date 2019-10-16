using System;

namespace CozyCommandLineParser
{
    public class CommandLineException : Exception
    {
        public CommandLineException(string message) : base(message)
        {
        }
    }
}