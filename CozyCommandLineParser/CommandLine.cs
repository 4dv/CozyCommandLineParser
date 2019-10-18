using System;
using System.Collections.Generic;
using System.Reflection;
using Checkers;
using CozyCommandLineParser.Helpers;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{
    public class CommandLine
    {
        private readonly ParserOptions options;

        // CommandName => CommandMethod
        private readonly CommandsDictionary commandsDic;

        /// <summary>
        /// if types is not null, commands will be taken from those types
        /// if assemblies is not null, commands will be searched in the specified assemblies
        /// if types and assemblies are both not null, commands will be taken from specified types and will be searched in
        /// assemblies
        /// if types and assemblies are both null, commands will be searched in the CallingAssembly
        /// </summary>
        public CommandLine(ParserOptions options = null)
        {
            this.options = options = options ?? new ParserOptions();

            List<Type> types = CommandsSearcher.FindAllTypes(options, Assembly.GetCallingAssembly());
            commandsDic = new CommandsDictionary(types, options);
        }

        public string HelpHeader { get; set; }

        public object LastCommandInstance { get; private set; }


        public void PrintHelp()
        {
            Console.WriteLine(HelpHeader);
            Console.WriteLine(commandsDic.GetDescriptions());
        }


        public object Execute(string[] args)
        {
            var argsEnumerator = new MultiPassEnumerator<string>(args);
            MethodInfo methodInfo = commandsDic.GetMatchingMethod(argsEnumerator.GetNext());
            if (methodInfo == null)
            {
                PrintHelp();
                return null;
            }

            Type type = Ensure.NotNull(methodInfo.DeclaringType);

            object instance = Activator.CreateInstance(type);
            var optionsDictionary = new OptionsDictionary(type, options);
            optionsDictionary.FillProperties(instance, argsEnumerator);
            object[] parameters = optionsDictionary.CreateParameters(methodInfo, argsEnumerator);

            LastCommandInstance = instance;

            return methodInfo.Invoke(instance, parameters);
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