using Microsoft.Extensions.DependencyInjection;

namespace DcsExportLib
{
    /// <summary>
    /// Implement internal IoC logic
    /// </summary>
    public static class DcsExporterBuilder
    {
        private static readonly ServiceProvider ServiceProvider;

        private static ServiceProvider BuildServices()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<IModuleLookup,ModuleLookup>();
            services.AddTransient<IDcsExporter,DcsExporter>();

            return services.BuildServiceProvider();
        }

        static DcsExporterBuilder()
        {
            ServiceProvider = BuildServices();
        }

        public static IDcsExporter CreateExporter(ExporterConfig config)
        {
            var exporter = ServiceProvider.GetService<IDcsExporter>();

            if (exporter == null)
                throw new InvalidOperationException("Exporter was not resolved");

            exporter.Config = config;
            return exporter;
        }

        public static IModuleLookup CreateModuleLookup()
        {
            var moduleLookup = ServiceProvider.GetService<IModuleLookup>();

            if (moduleLookup == null)
                throw new InvalidOperationException("Module lookup was not resolved.");
            
            return moduleLookup;
        }
    }
}
