using System;
using CozyCommandLineParser;
using CozyCommandLineParser.Attributes;

namespace SampleProject
{
    class Program
    {
        [Command]
        public void SayHi(string name)
        {
            Console.WriteLine("Hi " + name);
        }

        [Command]
        public void SayBuy(string name)
        {
            Console.WriteLine("Buy " + name);
        }

        static void Main(string[] args)
        {
            new CommandLine().Execute(args);
        }
    }
}