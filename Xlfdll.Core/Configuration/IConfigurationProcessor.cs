using System;

namespace Xlfdll.Configuration
{
    public interface IConfigurationProcessor
    {
        Configuration LoadConfiguration();
        void SaveConfiguration(Configuration configuration);
        Boolean CheckConfiguration();
    }
}