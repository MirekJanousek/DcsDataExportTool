using System.Diagnostics;
using DCSExporterApp;
using DCSExporterApp.Factories;

using DcsExportLib;
using DcsExportLib.Factories;
using DcsExportLib.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DCSExporterApp
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;

        public static void Main(string[] args)
        {
            RegisterUnhandledExceptionHandling();
            RegisterServices();

            var settingsFactory = _serviceProvider.GetRequiredService<ISettingsFactory>();
            var exportSettings = settingsFactory.GetSettings<ExportSettings>();
            
            IModuleLookup moduleLookup = DcsExport.Factory.GetModuleLookup();
            var modules = moduleLookup.GetInstalledModules(exportSettings.DcsFolderPath);

            ConsoleAppManager consoleAppManager = new ConsoleAppManager();
            consoleAppManager.SetConsoleTitle();

            DcsModuleInfo selectedModule = consoleAppManager.PromptSelectModule(modules);

            consoleAppManager.DisplayExportStartedMessage(selectedModule);
            
            IDcsExporter exporter = DcsExport.Factory.GetExporter(exportSettings);
            exporter.ExportClickableData(selectedModule);

            Console.ReadLine();
        }

        private static void RegisterServices()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(GetConfiguration);
            services.AddTransient<ISettingsFactory, SettingsFactory>();

            _serviceProvider = services.BuildServiceProvider();
        }
        
        private static IConfiguration GetConfiguration(IServiceProvider arg)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json").Build();
        }

        private static void RegisterUnhandledExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
            {
                Console.Clear();
                Console.WriteLine("Unhandled exception has occurred. See details below:");

                if (e.ExceptionObject is Exception)
                {

                    Console.WriteLine($"Message: {((Exception)e.ExceptionObject).Message}");
                    Console.WriteLine($"Stack: {((Exception)e.ExceptionObject).StackTrace}");
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }


    }
}