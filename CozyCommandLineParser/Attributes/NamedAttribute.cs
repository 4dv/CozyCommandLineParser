using System;
using System.Collections.Generic;

namespace CozyCommandLineParser.Attributes
{
    public class NamedAttribute : Attribute
    {
        protected string[] names;

        public NamedAttribute(string name = null, string description = null)
        {
            names = name?.Split('|');
            Description = description;
        }

        public string Description { get; set; }

        /// <summary>
        /// return explicitly specified names or null if names were not set
        /// </summary>
        public IReadOnlyList<string> Names => names;
    }
}