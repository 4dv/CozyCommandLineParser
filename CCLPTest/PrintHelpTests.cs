using System;
using System.IO;
using System.Threading;
using CozyCommandLineParser;
using NUnit.Framework;

namespace CCLPTest
{
    // redirect output in the test, can not parallel it with others
    [Parallelizable(ParallelScope.None)]
    public class PrintHelpTests
    {
        [Test]
        public void PrintCommandsListTest()
        {
            using (var console = new ConsoleRedirect() )
            {
                var commandLine = new CommandLine();

                commandLine.Execute(new string[0]);

                var expected = @"Usage: Tests: CCLPTest [COMMAND] [OPTIONS...]

Available commands:
  -h|--help|help    Default. Prints help, use `help <COMMAND>` to get a help for specific command
  --version|version Prints program version
  anotherCommand    Another command
  derivedCommand    Command in derivative
  simpleCommand     Simple command to run
  commandWithArgs   This command has one string argument
  commandWithParams This command can have any number of arguments
  commandWithIntParams This command can have any number of int arguments
";
                console.CheckLastOutput(expected);

                commandLine.Execute(new string[]{"help"});
                console.CheckLastOutput(expected);

            }
        }
    }
}