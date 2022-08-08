using Microsoft.Extensions.Configuration;

namespace DCSExporterApp.Factories
{
    internal class SettingsFactory : ISettingsFactory
    {
        private readonly IConfiguration _configuration;

        public SettingsFactory(IConfiguration configuration)
        {
            _configuration= configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public T GetSettings<T>() where T : new()
        {
            T settingsObject = new T();
            _configuration.GetSection(typeof(T).Name).Bind(settingsObject);
            return settingsObject;
        } 
    }
}
