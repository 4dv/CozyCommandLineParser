using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CozyCommandLineParser.Attributes;
using CozyCommandLineParser.Utils;
using Ensure = CozyCommandLineParser.Checkers.Ensure;

namespace CozyCommandLineParser
{
    public class CommandLine
    {
        protected readonly ParserOptions Options;

        protected readonly CommandsDictionary Commands;

        /// <summary>
        /// if types is not null, commands will be taken from those types
        /// if assemblies is not null, commands will be searched in the specified assemblies
        /// if types and assemblies are both not null, commands will be taken from specified types and will be searched in
        /// assemblies
        /// if types and assemblies are both null, commands will be searched in the CallingAssembly
        /// </summary>
        public CommandLine(ParserOptions options = null)
        {
            this.Options = options = options ?? new ParserOptions(Assembly.GetCallingAssembly());

            List<Type> types = CommandsSearcher.FindAllTypes(options, this.GetType());
            Commands = new CommandsDictionary(types, options);
        }

        public object LastCommandInstance { get; private set; }


        [Command("-h|--help|help", "Prints help, use `help <COMMAND>` to get a help for specific command",
            IsDefault = true)]
        public void PrintHelp(string command = null)
        {
            Console.WriteLine(GetHelp(command));
        }

        public string GetHelp(string command)
        {
            var sb = new StringBuilder();
            if (command == null)
            {
                sb.Append(Options.HelpHeader);
                sb.Append(Commands.GetAllDescriptions());
            }
            else
            {
                var methodInfo = Commands.GetMatchingMethod(command);
                sb.AppendLine(Commands.GetFullDescription(methodInfo).Trim());

                Type type = Ensure.NotNull(methodInfo.DeclaringType);

                var optionsDictionary = new OptionsDictionary(type, Options);
                sb.Append(optionsDictionary.GetAllDescriptions());
            }

            return sb.ToString();
        }

        [Command("--version|version", "Prints program version")]
        public void PrintVersion()
        {
            Console.Write(Options.VersionInfo);
        }


        public object Execute(string[] args)
        {
            var argsEnumerator = new MultiPassEnumerator<string>(args);
            MethodInfo methodInfo = Commands.GetMatchingMethod(argsEnumerator.GetNext());
            if (methodInfo == null)
            {
                PrintHelp();
                return null;
            }

            Type type = Ensure.NotNull(methodInfo.DeclaringType);

            // don't create instance for our default commands, we already have it
            object instance = type == this.GetType() ? this : Activator.CreateInstance(type);

            var optionsDictionary = new OptionsDictionary(type, Options);
            optionsDictionary.FillProperties(instance, argsEnumerator);
            object[] parameters = optionsDictionary.CreateParameters(methodInfo, argsEnumerator);

            LastCommandInstance = instance;

            // todo DimaCh add meaningful message if we don't have enough arguments for the command
            var res = methodInfo.Invoke(instance, parameters);
            if(res?.GetType() != typeof(void))
                Options?.OutputPrinter.Invoke(res);
            return res;
        }


        public static void Error(string message)
        {
            throw new CommandLineException(message);
        }

        public static void Warn(string message)
        {
            Console.WriteLine("Warning: " + message);
        }
    }
}