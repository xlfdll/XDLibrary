namespace Xlfdll.Configuration
{
    public abstract class ApplicationSettings
    {
        public ApplicationSettings(ApplicationConfiguration appConfiguration)
        {
            this.Provider = appConfiguration;
        }

        public ApplicationConfiguration Provider { get; }
    }
}