using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CozyCommandLineParser.Attributes;
using CozyCommandLineParser.Helpers;

namespace CozyCommandLineParser
{
    public class CommandsSearcher
    {
        public static List<Type> FindAllTypes(ParserOptions options, Assembly defaultAssembly)
        {
            var allTypes = new List<Type>();

            var types = options.SearchInTypes;
            if (types != null) allTypes.AddRange(types);

            var assemblies = options.SearchInAssemblies;
            if (options.SearchInTypes == null && options.SearchInAssemblies == null)
            {
                assemblies = new[] {defaultAssembly};
            }

            var nsSet = options.SearchFilterByNamespaces == null ? null : new HashSet<string>( options.SearchFilterByNamespaces);

            if (assemblies != null)
            {
                var typesFromAssemblies = assemblies.SelectMany(a => GetMatchingTypes(a, nsSet)).Where(t =>
                    t.GetMethods().Any(m => m.HasAttribute<CommandAttribute>()));
                allTypes.AddRange(typesFromAssemblies);
            }

            return allTypes;
        }

        private static IEnumerable<Type> GetMatchingTypes(Assembly assembly, HashSet<string> namespaces)
        {
            IEnumerable<Type> types = assembly.GetTypes();
            if (namespaces != null) types = types.Where(t => namespaces.Contains(t.Namespace));
            return types.OrderBy(t => t.Name);
        }
    }
}