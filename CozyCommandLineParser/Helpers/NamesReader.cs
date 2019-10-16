using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Checkers;
using CozyCommandLineParser.Attributes;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser.Helpers
{
    public class NamesReader
    {
        private readonly ParserOptions options;
        private readonly LetterCaseConverter converter;

        public NamesReader(ParserOptions options)
        {
            this.options = options;
            this.converter = new LetterCaseConverter(options.DefaultNameConvention);
        }

        public IReadOnlyList<string> GetNames(MemberInfo mi)
        {
            var attr = Check.NotNull(mi.GetCustomAttribute<NamedAttribute>());

            return attr.Names ?? new[]
                       {GetDefaultName(mi, attr)};
        }

        private string GetDefaultName(MemberInfo mi, NamedAttribute attr)
        {
            var pfx = attr is OptionAttribute ? options.DefaultOptionLongPrefix : "";
            return pfx + converter.FromGenericCase(mi.Name);
        }

        public string GetFirstName(MemberInfo mi)
        {
            return GetNames(mi).First();
        }
    }
}