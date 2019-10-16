using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{
    public class ParserOptions
    {
        public NameConventions DefaultNameConvention { get; set; } = NameConventions.CamelCase;
        public string DefaultOptionLongPrefix { get; set; } = "--";

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

        public static CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
    }
}