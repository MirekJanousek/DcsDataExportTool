using DcsExportLib.Builders;
using DcsExportLib.Exporters;
using DcsExportLib.Factories;
using DcsExportLib.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace DcsExportLib
{
    public class DcsExport
    {
        private static IDcsExporterFactory? _factoryInstance = null;
        private static IServiceProvider? _serviceProvider = null;

        private DcsExport()
        {
        }
        

        public static IDcsExporterFactory Factory
        {
            get
            {
                if (_serviceProvider == null)
                {
                    BuildServiceCollection();
                }

                if (_factoryInstance == null)
                {
                    if (_serviceProvider == null)
                        throw new InvalidOperationException("Service collection is not built!");

                    _factoryInstance = _serviceProvider.GetRequiredService<IDcsExporterFactory>();
                }
                return _factoryInstance;
            }
        }
        

        private static void BuildServiceCollection()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddTransient<IModuleLookup, ModuleLookup>();
            services.AddTransient<IDcsExporter, DcsExporter>();
            services.AddTransient<IDcsExporterFactory, DcsExporterFactory>();
            services.AddTransient<IExportSettingsValidator, ExportSettingsValidator>();
            services.AddTransient<IClickableDataLoader, CommonClickableDataLoader>();
            services.AddTransient<ILoaderFactory, LoaderFactory>();
            services.AddTransient<IDcsModuleInfoBuilder, DcsModuleInfoBuilder>();
            
            
            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
