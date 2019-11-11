using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public string GetFullDescription(MemberInfo mi)
        {
            NamedAttribute attr = Ensure.NotNull(mi.GetCustomAttribute<NamedAttribute>());

            var names = GetNames(mi).ToList();
            var namesStr = string.Join("|", names);

            var sb = new StringBuilder();
            sb.Append(namesStr);


            var mb = (MethodBase) mi;
            foreach (ParameterInfo pi in mb.GetParameters())
            {
                sb.Append(" ");
                var name = pi.Name;

                if (pi.HasDefaultValue)
                    name = $"[{name}]";
                sb.Append(name);
            }

            sb.AppendLine();

            foreach (ParameterInfo pi in mb.GetParameters())
            {
                sb.Append($"  {pi.Name} {pi.ParameterType.Name}, ");

                if (pi.HasDefaultValue)
                    sb.Append("Default=").Append(pi.DefaultValue);

                attr = pi.GetCustomAttribute<NamedAttribute>();
                if (attr != null)
                {
                    sb.Append(' ').Append(attr.Description);
                }

                sb.AppendLine();
            }
            sb.AppendLine();
            return sb.ToString();
        }

        protected override string GetItemDescription(string name, NamedAttribute attr0)
        {
            var attr = Ensure.NotNull(attr0 as CommandAttribute);
// todo make formatting more flexible
            return $"  {name,-17} " + (attr.IsDefault ? "Default. " : "") + attr.Description;
        }
    }
}