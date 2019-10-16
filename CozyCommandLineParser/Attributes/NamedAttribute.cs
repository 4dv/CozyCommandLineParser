using System;
using System.Collections.Generic;
using System.Reflection;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser.Attributes
{
    public class NamedAttribute : Attribute
    {
        public string Description { get; set; }

        protected string[] names;

        /// <summary>
        /// return explicitly specified names or null if names were not set
        /// </summary>
        public IReadOnlyList<string> Names => names;

        public NamedAttribute(string name = null, string description = null)
        {
            this.names = name?.Split('|');
            this.Description = description;
        }
    }
}