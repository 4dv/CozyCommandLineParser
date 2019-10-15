using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Checkers;
using CozyCommandLineParser.Attributes;
using CozyCommandLineParser.Helpers;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{
    public class CommandLine
    {
        public static string OPTION_LONG_BEGINNING = "--";

        public static ParserOptions Options { get; } = new ParserOptions();

        // CommandName => CommandMethod
        private CommandsDictionary commandsDic;

//        private MethodInfo defaultCommand;
        private FriendlyEnumerator<string> argsEnumerator;

        public List<Type> SearchInTypes { get; } = new List<Type>();
        public List<Assembly> SearchInAssemblies { get; } = new List<Assembly>();


        public bool IgnoreCase { get; set; } = true;
        public string HelpHeader { get; set; }

        /// <summary>
        /// if types is not null, commands will be taken from those types
        /// if assemblies is not null, commands will be searched in the specified assemblies
        /// if types and assemblies are both not null, commands will be taken from specified types and will be searched in assemblies
        /// if types and assemblies are both null, commands will be searched in the CallingAssembly
        /// </summary>
        public CommandLine(IList<Assembly> assemblies = null, IList<Type> types = null)
        {
            if (types == null && assemblies == null)
            {
                assemblies = new[] {Assembly.GetCallingAssembly()};
            }

            FindAllCommands(assemblies, types);
        }

        public object Execute(string[] args)
        {
            argsEnumerator = new FriendlyEnumerator<string>(args);
            FindMatchingCommandAndRun();
            return null;
        }

        private void FindAllCommands(IList<Assembly> assemblies = null, IList<Type> types = null)
        {
            var allTypes = new List<Type>();
            if (types != null) allTypes.AddRange(types);

            if (assemblies != null)
            {
                var typesFromAssemblies = assemblies.SelectMany(a => a.GetTypes()).Where(t =>
                    t.GetMethods().Any(m => m.HasAttribute<CommandAttribute>()));
                allTypes.AddRange(typesFromAssemblies);
            }


            commandsDic = new CommandsDictionary(allTypes);
        }

        [Command("Help", "Print help")]
        public void PrintHelp()
        {
            Console.WriteLine(HelpHeader);
            Console.WriteLine(commandsDic.GetDescriptions());
        }


        private void FindMatchingCommandAndRun()
        {
            var methodInfo = commandsDic.GetMatchingMethod(argsEnumerator.GetNext());
            if (methodInfo == null)
            {
                PrintHelp();
                return;
            }

            var type = Check.NotNull(methodInfo.DeclaringType);

            var instance = Activator.CreateInstance(type);
            var options = new OptionsDictionary(type);
            options.FillProperties(instance);
            var parameters = options.CreateParameters(methodInfo);

            methodInfo.Invoke(instance, parameters);
        }


        public static void Error(string message)
        {
            throw new CommandLineException(message);
        }
    }
}