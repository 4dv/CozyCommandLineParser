using System;
using System.Collections;
using System.Text;

namespace CozyCommandLineParser
{
    public class SimpleOutputPrinter
    {
        public int MaxDeep { get; set; } = 9;
        public string EnumSeparator { get; set; } = Environment.NewLine;

        public void Print(object obj)
        {
            Console.WriteLine(Dump(obj, 0));
        }
        public string Dump(object obj, int deep = 0)
        {
            if (obj == null) return null;
            if (deep > MaxDeep) return null;
            if (obj is IEnumerable enumerable)
            {
                StringBuilder sb = null;
                foreach (object v in enumerable)
                {
                    var res1 = Dump(v, deep + 1);
                    if (res1 == null) continue;
                    if (sb == null) sb = new StringBuilder(res1);
                    else sb.Append(EnumSeparator).Append(res1);
                }

                return sb?.ToString();
            }
            return obj.ToString();
        }
    }
}