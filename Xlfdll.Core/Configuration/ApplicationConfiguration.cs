using System;

namespace Xlfdll.Core
{
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration(IConfigurationProcessor processor)
        {
            this.Processor = processor;
            this.Current = this.Processor.LoadConfiguration();
        }

        public Configuration Current { get; private set; }
        private IConfigurationProcessor Processor { get; }

        public void Save()
        {
            this.Processor.SaveConfiguration(this.Current);
        }

        public void Reload()
        {
            this.Current = this.Processor.LoadConfiguration();
        }

        public Boolean Check()
        {
            return this.Processor.CheckConfiguration();
        }
    }
}