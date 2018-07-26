using System;
using System.Collections.Generic;

namespace Xlfdll.Core
{
    public class ConfigurationSection : Dictionary<String, String>
    {
        public ConfigurationSection(String name)
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("The name of the configuration section is null or empty.");
            }

            this.Name = name;
        }

        public String Name { get; }
    }
}