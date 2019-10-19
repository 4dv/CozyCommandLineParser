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
            foreach (var cmdPair in membersDict)
            {
                var mi = cmdPair.Value;
                var attr = Ensure.NotNull(mi.GetCustomAttribute<NamedAttribute>());

                sb.AppendLine(GetItemDescription(cmdPair.Key, attr));
            }

            return sb.ToString();
        }

        protected virtual string GetItemDescription(string name, NamedAttribute attr)
        {
            return $"   {name:12} {attr.Description}";
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
                    if (membersDict.ContainsKey(name))
                        CommandLine.Error($"More than one member are registered with the same name: '{name}'");

                    membersDict[name] = mi;
                }
            }
        }

        protected virtual void ProcessMemberInfo(MemberInfo mi, NamedAttribute attr)
        {

        }
    }
}