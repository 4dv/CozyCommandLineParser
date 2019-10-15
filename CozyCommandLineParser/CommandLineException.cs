using System;

namespace CozyCommandLineParser
{
    internal class CommandLineException : Exception
    {
        public CommandLineException(string message) : base(message)
        {
        }
    }
}