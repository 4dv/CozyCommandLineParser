using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{
    public class ParserOptions
    {
        public ParserOptions(Assembly asm = null)
        {
            var fname = System.AppDomain.CurrentDomain.FriendlyName;
            var sb = new StringBuilder($"Usage: {fname} [COMMAND] [OPTIONS...]");
            sb.AppendLine().AppendLine().AppendLine("Available commands:");

            HelpHeader = sb.ToString();

            /*asm = asm ?? Assembly.GetCallingAssembly();
            var description = asm.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
            var title = asm.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
            var version = asm.GetCustomAttribute<AssemblyVersionAttribute>()?.Version;
            HelpHeader = $"{title} {version}";
            if(!string.IsNullOrWhiteSpace(HelpHeader) && !string.IsNullOrWhiteSpace(description))
                HelpHeader += Environment.NewLine + $"{description}";*/
        }
        public NameConventions DefaultNameConvention { get; set; } = NameConventions.CamelCase;

        public string DefaultOptionLongPrefix { get; set; } = "--";

        public string HelpHeader { get; set; }


        /// <summary>
        /// all arguments after this string will be treated as positional arguments
        /// </summary>
        public string EndOfNamedOptions { get; set; } = "--";

        /// <summary>
        /// if both SearchInAssemblies and SearchInTypes are null, will search in CallingAssembly
        /// </summary>
        public IReadOnlyList<Assembly> SearchInAssemblies { get; set; } = null;

        /// <summary>
        /// if both SearchInTypes and SearchInAssemblies specified, will check all types from SearchInAssemblies and add
        /// explicitly specified types from SearchInTypes
        /// </summary>
        public IReadOnlyList<Type> SearchInTypes { get; set; } = null;

        /// <summary>
        /// If not null, will filter types found in assemblies and leave only from namespaces specified in SearchFilterByNamespaces
        /// </summary>
        public IReadOnlyList<string> SearchFilterByNamespaces { get; set; } = null;

        /// <summary>
        /// culture used to parse and format numbers, datetime etc
        /// </summary>
        public static CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
    }
}