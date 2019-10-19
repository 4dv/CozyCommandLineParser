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
    public class CommandsDictionary : MembersDictionaryBase
    {
//        private readonly Dictionary<string, MethodInfo> commandsDic;
        private MethodInfo defaultCommand;

        public CommandsDictionary(IEnumerable<Type> types, ParserOptions options) : base(options)
        {
            List<MethodInfo> commands = types.SelectMany(t => t.GetMethods()
                .Where(m => m.GetCustomAttribute<CommandAttribute>() != null)).ToList();

            CreateNamesDict(commands);
        }

        public MethodInfo GetMatchingMethod(string cmd)
        {
            if (cmd == null) return defaultCommand;

            if (membersDict.TryGetValue(cmd, out var mi))
                return Ensure.NotNull(mi as MethodInfo);

            CommandLine.Error($"Command {cmd} is not found");
            return null;
        }

        protected override void ProcessMemberInfo(MemberInfo mi0, NamedAttribute attr0)
        {
            var attr = Ensure.NotNull(attr0 as CommandAttribute);
            var mi = Ensure.NotNull(mi0 as MethodInfo);

            if (attr.IsDefault)
            {
                if (defaultCommand != null)
                    CommandLine.Error(
                        $"Only one default command is allowed, {GetFirstName(defaultCommand)} " +
                        $"and {GetFirstName(mi)} both set as default");

                defaultCommand = mi;
            }

            base.ProcessMemberInfo(mi, attr);
        }

        protected override string GetItemDescription(string name, NamedAttribute attr0)
        {
            var attr = Ensure.NotNull(attr0 as CommandAttribute);

            return $"   {name:12} " + (attr.IsDefault ? "Default. " : "") + attr.Description;
        }

    }
}