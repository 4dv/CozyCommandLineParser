﻿using CozyCommandLineParser;
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
            commandLine.Execute(new string[] { });
            Assert.AreEqual(commandLine, commandLine.LastCommandInstance);

            var answer = commandLine.GetHelp(null);

            var expected = @"Usage: testhost [COMMAND] [OPTIONS...]

Available commands:
  anotherCommand    Another command description
  derivedCommand    Command in derived class
  simpleCommand     Simple command to run
  commandWithArgs   This command has two arguments
  commandWithParams This command can have any number of arguments
  commandWithIntParams This command can have any number of int arguments
  -h|--help|help    Default. Prints help, use `help <COMMAND>` to get a help for specific command
  --version|version Prints program version
";
            Assert.AreEqual(expected, answer);
        }

        [Test]
        public void PrintCommandHelpTest()
        {
            var commandLine = new CommandLine();

            var answer = commandLine.GetHelp("anotherCommand");
            var expected = @"anotherCommand [val]
  val Int32, Default=0
  --anotherInt      Int argument
";
            Assert.AreEqual(expected, answer);
        }

        [Test]
        public void PrintCommandWithArgHelpTest()
        {
            var commandLine = new CommandLine();

            var answer = commandLine.GetHelp("commandWithArgs");
            var expected = @"commandWithArgs [arg] [arg2]
  arg String, Default=abc first argument for the command
  arg2 Int32, Default=42 second argument, it is int
  --stringOption|-s some option with string value
  --intOption       Int option will use default name --intOption
  -b                
  --someDateTime    
  --someDouble      
";
            Assert.AreEqual(expected, answer);
        }
    }
}