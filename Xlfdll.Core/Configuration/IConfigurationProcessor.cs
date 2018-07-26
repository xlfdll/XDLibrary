using System;

namespace Xlfdll.Core
{
    public interface IConfigurationProcessor
    {
        Configuration LoadConfiguration();
        void SaveConfiguration(Configuration configuration);
        Boolean CheckConfiguration();
    }
}