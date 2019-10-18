using System;
using System.Linq;
using System.Reflection;
using Checkers;
using CozyCommandLineParser.Attributes;
using CozyCommandLineParser.Helpers;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{
    public class OptionsDictionary : MembersDict
    {
        public OptionsDictionary(Type type, ParserOptions options) : base(options)
        {
            var props = type.GetProperties().Where(pi => pi.HasAttribute<OptionAttribute>());
            CreateNamesDict(props);
        }

        public void FillProperties(object instance, MultiPassEnumerator<string> argsEnumerator)
        {
            var nameValSplit = new char[] {'='};
            foreach (string arg in argsEnumerator)
            {
                if (arg == options.EndOfNamedOptions)
                    break;

                var ar = arg.Split(nameValSplit, 2);
                var name = ar[0];
                var valueStr = ar.Length > 1 ? ar[1] : string.Empty;


                if (membersDict.TryGetValue(name, out var mi))
                {
                    var pi = Ensure.NotNull(mi as PropertyInfo);
                    object value;
                    if (pi.PropertyType == typeof(bool) && ar.Length == 1)
                        value = true;
                    else value = ConvertToType(valueStr, pi.PropertyType);
                    pi.SetValue(instance, value);
                }
                else
                {
                    if(name.StartsWith(options.DefaultOptionLongPrefix))
                        CommandLine.Error($"Option with name {name} is not found, if this is not an option name, put it after {options.EndOfNamedOptions} separator");
                    argsEnumerator.SaveCurrentToNextPass(); // will be processed as positional arguments
                }
            }
        }

        protected override string GetDefaultName(MemberInfo mi, NamedAttribute attr)
        {
            return options.DefaultOptionLongPrefix + base.GetDefaultName(mi, attr);
        }

        public object[] CreateParameters(MethodInfo methodInfo, MultiPassEnumerator<string> argsEnumerator)
        {
            argsEnumerator.Reset();
            object[] res = methodInfo.GetParameters()
                .Select(pi =>
                    argsEnumerator.MoveNext() ? ConvertToType(argsEnumerator.Current, pi.ParameterType) : Type.Missing)
                .ToArray();

            if (argsEnumerator.MoveNext() || !argsEnumerator.IsAllFinished)
            {
                argsEnumerator.SaveCurrentToNextPass();
                CommandLine.Error("Get more arguments, than expected by function, excessive arguments: [" +
                                  string.Join(",", argsEnumerator) + ']');
            }

            return res;
        }

        private object ConvertToType(string str, Type type)
        {
            return Convert.ChangeType(str, type, ParserOptions.Culture);
        }
    }
}