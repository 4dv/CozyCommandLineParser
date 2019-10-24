using System;
using System.IO;
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
            commandLine.Execute(new string[] {"help"});
            Assert.AreEqual(commandLine, commandLine.LastCommandInstance);

            commandLine = new CommandLine();
            commandLine.Execute(new string[] {});
            Assert.AreEqual(commandLine, commandLine.LastCommandInstance);

            var answer = commandLine.GetHelp(null);

            var expected = @"Usage: testhost [COMMAND] [OPTIONS...]

Available commands:
  -h|--help|help    Default. Prints help, use `help <COMMAND>` to get a help for specific command
  --version|version Prints program version
  anotherCommand    Another command description
  derivedCommand    Command in derived class
  simpleCommand     Simple command to run
  commandWithArgs   This command has one string argument
  commandWithParams This command can have any number of arguments
  commandWithIntParams This command can have any number of int arguments
";
            Assert.AreEqual(expected, answer);
        }

        [Test]
        public void PrintCommandHelpTest()
        {
            var commandLine = new CommandLine();

            var answer = commandLine.GetHelp("anotherCommand");
            var expected = @"anotherCommand    Another command description
  --anotherInt      Int argument
";
            Assert.AreEqual(expected, answer);

        }
    }
}