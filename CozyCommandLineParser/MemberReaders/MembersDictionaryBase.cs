using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Checkers;
using CozyCommandLineParser.Attributes;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{
    public class MembersDictionaryBase
    {
        protected readonly ParserOptions options;
        private LetterCaseConverter converter;
        protected Dictionary<string, MemberInfo> membersDict;
        private HashSet<MemberInfo> allMembers;

        public MembersDictionaryBase(ParserOptions options)
        {
            this.options = options;
            converter = new LetterCaseConverter(options.DefaultNameConvention);
        }

        public IReadOnlyList<string> GetNames(MemberInfo mi)
        {
            NamedAttribute attr = Ensure.NotNull(mi.GetCustomAttribute<NamedAttribute>());

            return attr.Names ?? new[]
                       {GetDefaultName(mi, attr)};
        }

        public string GetDescriptions()
        {
            var sb = new StringBuilder();
            foreach (var mi in allMembers)
            {
                NamedAttribute attr = Ensure.NotNull(mi.GetCustomAttribute<NamedAttribute>());

                var names = GetNames(mi).ToList();
                var namesStr = string.Join("|", names);

                sb.AppendLine(GetItemDescription(namesStr, attr));
            }

            return sb.ToString();
        }

        protected virtual string GetItemDescription(string name, NamedAttribute attr)
        {
            return $"  {name,-17} {attr.Description}";
        }

        protected virtual string GetDefaultName(MemberInfo mi, NamedAttribute attr)
        {
//            string pfx = attr is OptionAttribute ? options.DefaultOptionLongPrefix : "";
//            return pfx + converter.FromGenericCase(mi.Name);
            return converter.FromGenericCase(mi.Name);
        }

        public string GetFirstName(MemberInfo mi)
        {
            return GetNames(mi).First();
        }

        public void CreateNamesDict(IEnumerable<MemberInfo> members)
        {
            membersDict = new Dictionary<string, MemberInfo>();

            foreach (MemberInfo mi in members)
            {
                var attr = mi.GetCustomAttribute<NamedAttribute>();
                Ensure.NotNull(attr, "attr");

                IReadOnlyList<string> names = GetNames(mi);

                ProcessMemberInfo(mi, attr);

                foreach (string name in names)
                {
                    membersDict.TryGetValue(name, out var existingMember);

                    membersDict[name] = OverwriteDefaultIfExists(mi, existingMember,
                        () => $"More than one member is registered with the same name: '{name}'");
                }
            }

            allMembers = new HashSet<MemberInfo>(membersDict.Values);
        }

        protected MemberInfo OverwriteDefaultIfExists(MemberInfo mi, MemberInfo oldValue, Func<string> onClashError)
        {
            if (oldValue == null)
                return mi;
            Assembly libAssembly = typeof(MembersDictionaryBase).Assembly;

            if (oldValue.DeclaringType?.Assembly == libAssembly)
                return mi;

            if (mi.DeclaringType?.Assembly == libAssembly)
                return oldValue;

            CommandLine.Error(onClashError.Invoke());
            return null;
        }

        protected virtual void ProcessMemberInfo(MemberInfo mi, NamedAttribute attr)
        {
        }
    }
}