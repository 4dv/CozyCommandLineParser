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
            allMembers = new HashSet<MemberInfo>();

            Assembly libAssembly = typeof(MembersDictionaryBase).Assembly;

            foreach (MemberInfo mi in members)
            {
                var attr = mi.GetCustomAttribute<NamedAttribute>();
                Ensure.NotNull(attr, "attr");

                IReadOnlyList<string> names = GetNames(mi);

                ProcessMemberInfo(mi, attr);

                foreach (string name in names)
                {
                    if (membersDict.TryGetValue(name, out var existingMember))
                    {
                        // todo DimaCh test overwrite works as expected
                        // overwrite if the existing command is from our assembly
                        if (existingMember.DeclaringType?.Assembly == libAssembly)
                            allMembers.Remove(existingMember);
                        else if (mi.DeclaringType?.Assembly == libAssembly)
                            continue; // new type is from our assembly, don't do anything
                        CommandLine.Error($"More than one member is registered with the same name: '{name}'");
                    }

                    membersDict[name] = mi;
                    allMembers.Add(mi);
                }
            }
        }

        protected virtual void ProcessMemberInfo(MemberInfo mi, NamedAttribute attr)
        {
        }
    }
}