using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CozyCommandLineParser.Attributes;
using Ensure = CozyCommandLineParser.Checkers.Ensure;

namespace CozyCommandLineParser
{
    public class CommandsDictionary : MembersDictionaryBase
    {
        private MethodInfo defaultCommand;

        public CommandsDictionary(IEnumerable<Type> types, ParserOptions options) : base(options)
        {
            List<MethodInfo> commands = types.SelectMany(t => t
                .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttribute<CommandAttribute>() != null)).ToList();

            CreateNamesDict(commands);
        }

        public MethodInfo GetMatchingMethod(string cmd)
        {
            if (cmd == null) return defaultCommand;

            if (membersDict.TryGetValue(cmd, out var mi))
                return Ensure.NotNull(mi as MethodInfo);

            CommandLine.Error($"Command '{cmd}' is not found");
            return null;
        }

        protected override void ProcessMemberInfo(MemberInfo mi0, NamedAttribute attr0)
        {
            var attr = Ensure.NotNull(attr0 as CommandAttribute);
            var mi = Ensure.NotNull(mi0 as MethodInfo);

            if (attr.IsDefault)
            {
                defaultCommand = OverwriteDefaultIfExists(mi, defaultCommand,
                    () => $"Only one default command is allowed, {GetFirstName(defaultCommand)} " +
                    $"and {GetFirstName(mi)} both set as default") as MethodInfo;
            }

            base.ProcessMemberInfo(mi, attr);
        }

        protected override string GetItemDescription(string name, NamedAttribute attr0)
        {
            var attr = Ensure.NotNull(attr0 as CommandAttribute);
// todo make formatting more flexible
            return $"  {name,-17} " + (attr.IsDefault ? "Default. " : "") + attr.Description;
        }
    }
}