using System;
using System.Runtime.CompilerServices;
using CozyCommandLineParser.Attributes;
using NUnit.Framework;

namespace CCLPTest
{
    public class TestCommands
    {
        private int lastCalledCount;

        private string lastExecutedCommand;

        private object[] lastFunctionArgs;

        [Option("--stringOption|-s", "some option with string value")]
        public string StringOption { get; set; }

        [Option(Description = "Int option will use default name --intOption")]
        public int IntOption { get; set; }

        [Option("-b")] public bool BoolOption { get; set; }

        [Option] public DateTime SomeDateTime { get; set; }

        [Option] public double SomeDouble { get; set; }

        [Command(Description = "Simple command to run")]
        public int SimpleCommand()
        {
            CommandExecuted(new object[] { });
            return 10;
        }

        public void CheckLastExecutedCommand(string expectedCommand, object[] expectedOptions = null)
        {
            Assert.AreEqual(expectedCommand, lastExecutedCommand);
            Assert.AreEqual(1, lastCalledCount);
            Assert.AreEqual(expectedOptions, lastFunctionArgs);
        }

        // if command arguments doesn't have any attributes, they work as positional arguments
        [Command("commandWithArgs", "This command has one string argument")]
        public void SomeCommandWithArgs(string arg = "abc", int arg2 = 42)
        {
            CommandExecuted(new object[] {arg, arg2});
        }

        // if command arguments doesn't have any attributes, they work as positional arguments
        [Command("commandWithParams", "This command can have any number of arguments")]
        public void SomeCommandWithParams(int num = 11, params string[] otherArgs)
        {
            CommandExecuted(new object[] {num, otherArgs});
        }

        // if command arguments doesn't have any attributes, they work as positional arguments
        [Command(Description = "This command can have any number of int arguments")]
        public void CommandWithIntParams(int num = 11, params int[] otherArgs)
        {
            CommandExecuted(new object[] {num, otherArgs});
        }


        private void CommandExecuted(object[] args, [CallerMemberName] string callerName = "")
        {
            lastExecutedCommand = callerName;
            lastCalledCount++;
            lastFunctionArgs = args;
        }
    }
}