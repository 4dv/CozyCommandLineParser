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
        public static List<Type> FindAllTypes(ParserOptions options,
            params Type[] additionalTypes)
        {
            var allTypes = new List<Type>();

            IReadOnlyList<Type> types = options.SearchInTypes;
            if (types != null) allTypes.AddRange(types);

            IReadOnlyList<Assembly> assemblies = options.SearchInAssemblies;
            if (options.SearchInTypes == null && options.SearchInAssemblies == null)
                assemblies = new[] {options.CallingAssembly};

            HashSet<string> nsSet = options.SearchFilterByNamespaces == null
                ? null
                : new HashSet<string>(options.SearchFilterByNamespaces);

            if (assemblies != null)
            {
                IEnumerable<Type> typesFromAssemblies = assemblies.SelectMany(a => GetMatchingTypes(a, nsSet)).Where(
                    t =>
                        t.GetMethods().Any(m => m.HasAttribute<CommandAttribute>()));
                allTypes.AddRange(typesFromAssemblies);
            }

            allTypes.AddRange(additionalTypes);

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