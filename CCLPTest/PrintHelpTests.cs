using System.Threading;
using CozyCommandLineParser;
using NUnit.Framework;

namespace CCLPTest
{
    public class PrintHelpTests
    {
       [Test]
        public void PrintCommandsListTest()
        {
            var commandLine = new CommandLine();
            commandLine.Execute(new string[0]);
        }
    }
}