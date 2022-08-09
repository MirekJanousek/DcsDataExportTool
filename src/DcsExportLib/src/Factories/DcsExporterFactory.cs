using DcsExportLib.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace DcsExportLib.Factories
{
    /// <summary>
    /// Implement internal IoC logic
    /// </summary>
    public class DcsExporterFactory : IDcsExporterFactory
    {
        // TODO: Can this be reworked to not provide the factory the access to all services?
        private readonly IServiceProvider _serviceProvider;
        private readonly IExportSettingsValidator _exportSettingsValidator;
        
        public DcsExporterFactory(IServiceProvider serviceProvider, IExportSettingsValidator exportSettingsValidator)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _exportSettingsValidator = exportSettingsValidator ?? throw new ArgumentNullException(nameof(exportSettingsValidator));
        }

        public IDcsExporter GetExporter(ExportSettings exportSettings)
        {
            if (!_exportSettingsValidator.Validate(exportSettings))
            {
                // TODO: change to own exception like ExportException and list the Errors
                throw new ApplicationException("Export settings validation error!");
            }

            var exporter = _serviceProvider.GetRequiredService<IDcsExporter>();

            if (exporter == null)
                throw new InvalidOperationException("Exporter was not resolved");

            exporter.Settings = exportSettings;
            return exporter;
        }

        public IModuleLookup GetModuleLookup()
        {
            var moduleLookup = _serviceProvider.GetRequiredService<IModuleLookup>();

            if (moduleLookup == null)
                throw new InvalidOperationException("Module lookup was not resolved.");

            return moduleLookup;
        }
    }
}
