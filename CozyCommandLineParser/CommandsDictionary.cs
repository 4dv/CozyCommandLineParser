using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Checkers;
using CozyCommandLineParser.Attributes;
using CozyCommandLineParser.Helpers;

namespace CozyCommandLineParser
{
    public class CommandsDictionary
    {
        private Dictionary<string, MethodInfo> commandsDic;
        private MethodInfo defaultCommand;

        public CommandsDictionary(IEnumerable<Type> types, NamesReader namesReader)
        {
            var commands = types.SelectMany(t => t.GetMethods()
                .Where(m => m.GetCustomAttribute<CommandAttribute>() != null)).ToList();

            commandsDic = new Dictionary<string, MethodInfo>();

            foreach (MethodInfo methodInfo in commands)
            {
                var attr = methodInfo.GetCustomAttribute<CommandAttribute>();
                Check.NotNull(attr, "attr");

                var names = namesReader.GetNames(methodInfo);

                if (attr.IsDefault)
                {
                    if (defaultCommand != null)
                        CommandLine.Error(
                            $"Only one default command is allowed, {namesReader.GetFirstName(defaultCommand)} " +
                            $"and {names.FirstOrDefault()} both set as default");

                    defaultCommand = methodInfo;
                }

                foreach (string name in names)
                {
                    if (commandsDic.ContainsKey(name))
                        CommandLine.Error($"More than one method registered for the same command: 'name'");

                    commandsDic[name] = methodInfo;
                }
            }
        }

        public string GetDescriptions()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, MethodInfo> cmdPair in commandsDic)
            {
                var mi = cmdPair.Value;
                var attr = mi.GetCustomAttribute<CommandAttribute>();
                Check.NotNull(attr, "attr");

                sb.Append(cmdPair.Key + ": ");
                if (attr.IsDefault) sb.Append("Default. ");
                sb.AppendLine(attr.Description);
            }

            return sb.ToString();
        }

        public MethodInfo GetMatchingMethod(string cmd)
        {
            if (cmd == null) return defaultCommand;

            if (commandsDic.TryGetValue(cmd, out var methodInfo))
                return methodInfo;

            CommandLine.Error($"Command {cmd} is not found");
            return null;
        }
    }
}