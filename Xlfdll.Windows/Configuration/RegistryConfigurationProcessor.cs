using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Win32;

using XDCore = Xlfdll.Core;

namespace Xlfdll.Windows.Configuration
{
    public class RegistryConfigurationProcessor : XDCore.IConfigurationProcessor
    {
        public RegistryConfigurationProcessor(String root, RegistryConfigurationScope scope)
        {
            this.RootPath = Path.Combine(@"Software", root);
            this.Scope = scope;
        }

        public String RootPath { get; }
        public RegistryConfigurationScope Scope { get; }

        public XDCore.Configuration LoadConfiguration()
        {
            XDCore.Configuration configuration = new XDCore.Configuration();

            if (this.CheckConfiguration())
            {
                RegistryKey configRegistryKey = RegistryConfigurationProcessor.GetRegistryKey(this.RootPath, this.Scope);

                foreach (String sectionName in configRegistryKey.GetSubKeyNames())
                {
                    configuration.AddSection(sectionName);

                    RegistryKey sectionRegistryKey = RegistryConfigurationProcessor.GetRegistryKey(Path.Combine(this.RootPath, sectionName), this.Scope);

                    foreach (String key in sectionRegistryKey.GetValueNames())
                    {
                        configuration[sectionName].Add(key, sectionRegistryKey.GetValue(key).ToString());
                    }

                    sectionRegistryKey.Close();
                }

                configRegistryKey.Close();
            }

            return configuration;
        }

        public void SaveConfiguration(XDCore.Configuration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("The configuration object is null.");
            }

            // Check and remove old key to clean up
            RegistryKey rootRegistryKey = RegistryConfigurationProcessor.GetRegistryKey(String.Empty, this.Scope);
            RegistryKey configRegistryKey = rootRegistryKey.OpenSubKey(this.RootPath); // Read-only

            if (configRegistryKey != null)
            {
                configRegistryKey.Close();

                rootRegistryKey.DeleteSubKeyTree(this.RootPath);
            }

            configRegistryKey = RegistryConfigurationProcessor.GetRegistryKey(this.RootPath, this.Scope);

            foreach (XDCore.ConfigurationSection section in configuration.Sections)
            {
                RegistryKey sectionRegistryKey = RegistryConfigurationProcessor.GetRegistryKey(Path.Combine(this.RootPath, section.Name), this.Scope);

                foreach (KeyValuePair<String, String> pair in section)
                {
                    sectionRegistryKey.SetValue(pair.Key, pair.Value);
                }

                sectionRegistryKey.Close();
            }

            configRegistryKey.Close();
        }

        public Boolean CheckConfiguration()
        {
            RegistryKey rootRegistryKey = RegistryConfigurationProcessor.GetRegistryKey(String.Empty, this.Scope);
            RegistryKey configRegistryKey = rootRegistryKey.OpenSubKey(this.RootPath);

            Boolean result = (configRegistryKey != null);

            configRegistryKey?.Close();

            return result;
        }

        private static RegistryKey GetRegistryKey(String name, RegistryConfigurationScope scope)
        {
            switch (scope)
            {
                case RegistryConfigurationScope.Machine:
                    {
                        return !String.IsNullOrEmpty(name) ? Registry.LocalMachine.CreateSubKey(name) : Registry.LocalMachine;
                    }
                case RegistryConfigurationScope.User:
                    {
                        return !String.IsNullOrEmpty(name) ? Registry.CurrentUser.CreateSubKey(name) : Registry.CurrentUser;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }

    public enum RegistryConfigurationScope { Machine, User }
}