using System;
using System.Text;

namespace CozyCommandLineParser.Utils
{
    public enum NameConventions
    {
        /// <summary>
        /// MyCommand => myCommand
        /// </summary>
        CamelCase,

        /// <summary>
        /// MyCommand => MyCommand
        /// </summary>
        PascalCase,

        /// <summary>
        /// MyCommand => mycommand
        /// </summary>
        LowerCase,

        /// <summary>
        /// MyCommand => my_command
        /// </summary>
        SnakeCase,

        /// <summary>
        /// MyCommand => my-command
        /// </summary>
        KebabCase,
    }


    public class LetterCaseConverter
    {
        private readonly NameConventions convention;

        public LetterCaseConverter(NameConventions convention)
        {
            this.convention = convention;
        }


        /// <summary>
        /// converts strings like SeveralWords to severalWords or several-words depending on convention argument
        /// </summary>
        public string FromGenericCase(string str)
        {
            return FromGenericCase(str, convention);
        }


        /// <summary>
        /// converts strings like SeveralWords to severalWords or several-words depending on convention argument
        /// </summary>
        public static string FromGenericCase(string str, NameConventions convention)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            switch (convention)
            {
                case NameConventions.CamelCase:
                    return Char.ToLower(str[0], ParserOptions.Culture) + str.Substring(1);

                case NameConventions.PascalCase:
                    break;

                case NameConventions.LowerCase:
                    return str.ToLowerInvariant();

                case NameConventions.SnakeCase:
                    return SubCapitals(str, '_');

                case NameConventions.KebabCase:
                    return SubCapitals(str, '-');
            }

            return str;
        }

        /// <summary>
        /// converts strings like several_words to SeveralWords.
        /// NB we don't know how to convert serveralwords back to SeveralWords, so return it as it is
        /// </summary>
        public string ToGenericCase(string str)
        {
            return ToGenericCase(str, convention);
        }

        /// <summary>
        /// converts strings like several_words to SeveralWords.
        /// NB we don't know how to convert serveralwords back to SeveralWords, so return it as it is
        /// </summary>
        public static string ToGenericCase(string str, NameConventions convention)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            switch (convention)
            {
                case NameConventions.CamelCase:
                    return Char.ToUpper(str[0], ParserOptions.Culture) + str.Substring(1);

                case NameConventions.PascalCase:
                case NameConventions.LowerCase: // no information how to convert back lowercase, return it as it is
                    break;

                case NameConventions.SnakeCase:
                    return SeparatorToCapitals(str, '_');

                case NameConventions.KebabCase:
                    return SeparatorToCapitals(str, '-');
            }

            return str;
        }

        private static string SeparatorToCapitals(string str, char sep)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            bool nextToUpper = true;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (ch == sep) nextToUpper = true;
                else
                {
                    sb.Append(nextToUpper ? Char.ToUpper(ch, ParserOptions.Culture) : ch);
                    nextToUpper = false;
                }
            }

            return sb.ToString();
        }

        private static string SubCapitals(string str, char sub)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (Char.IsUpper(ch))
                {
                    if (i > 0) sb.Append(sub);
                    sb.Append(Char.ToLower(ch, ParserOptions.Culture));
                }
                else sb.Append(ch);
            }

            return sb.ToString();
        }
    }
}