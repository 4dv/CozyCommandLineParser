using System;
using System.Runtime.CompilerServices;
using CozyCommandLineParser;
using CozyCommandLineParser.Attributes;

namespace SampleProject
{
    class Program
    {
        [Option(Description = "Verbose mode")]
        public bool Verbose { get; set; }

        [Command]
        public void SayHi(string name)
        {
            LogExecutedIfVerbose();
            Console.WriteLine("Hi " + name);
        }

        private void LogExecutedIfVerbose([CallerMemberName] string callerName = "")
        {
            if (Verbose) Console.WriteLine(callerName + " was called");
        }

        [Command]
        public void SayBuy(string name)
        {
            LogExecutedIfVerbose();
            Console.WriteLine("Buy " + name);
        }

        static void Main(string[] args)
        {
            new CommandLine(
                    new ParserOptions()
                        {OutputPrinter = msg => new SimpleOutputPrinter() {EnumSeparator = " "}.Print(msg)})
                .Execute(args);
        }
    }
}