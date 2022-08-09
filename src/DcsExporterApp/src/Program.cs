using DCSExporterApp.Factories;
using DCSExporterApp.FileExports;
using DcsExportLib;
using DcsExportLib.Factories;
using DcsExportLib.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DCSExporterApp
{
    public class Program
    {
        private static readonly ConsoleAppManager ConsoleAppManager = new();
        private static AppSettings _appSettings = new();
        private static IServiceProvider _serviceProvider = null!;

        public static void Main(string[] args)
        {
            RegisterUnhandledExceptionHandling();
            RegisterServices();

            var settingsFactory = _serviceProvider.GetRequiredService<ISettingsFactory>();
            var exportSettings = settingsFactory.GetSettings<ExportSettings>();
            _appSettings = settingsFactory.GetSettings<AppSettings>();

            ValidateSettings();
            
            IModuleLookup moduleLookup = DcsExport.Factory.GetModuleLookup();
            var modules = moduleLookup.GetInstalledModules(exportSettings.DcsFolderPath);
            
            ConsoleAppManager.SetConsoleTitle();

            DcsModuleInfo selectedModule = ConsoleAppManager.PromptSelectModule(modules);

            ConsoleAppManager.DisplayExportStartedMessage(selectedModule);
            
            IDcsExporter exporter = DcsExport.Factory.GetExporter(exportSettings);
            DcsModule? exportedModule = exporter.Export(selectedModule);

            if (exportedModule == null)
            {
                ConsoleAppManager.ShowNoExportedModuleMessage();
            }
            else
            {
                SaveToFile(exportedModule);
                ConsoleAppManager.NotifyExportDone();
            }

            ConsoleAppManager.PromptExitConfirmation();
        }

        private static void ValidateSettings()
        {
            try
            {
                List<string> errors = new List<string>();

                // check the export file directory setting
                errors.AddRange(ValidatePathSettings(_appSettings.ExportDirectoryPath, nameof(_appSettings.ExportDirectoryPath)));

                if (errors.Count > 0)
                {
                    ConsoleAppManager.NotifyWrongSettings(errors);
                    ConsoleAppManager.PromptExitConfirmation();
                    Environment.Exit(0);
                }
            }
            catch
            {
                ConsoleAppManager.NotifyException("Error while validating the application settings");
                ConsoleAppManager.PromptExitConfirmation();
                Environment.Exit(0);
            }
        }

        private static ICollection<string> ValidatePathSettings(string path, string settingName)
        {
            List<string> errors = new List<string>();

            // check the export file directory path exists
            if (string.IsNullOrWhiteSpace(_appSettings.ExportDirectoryPath))
            {
                errors.Add($"{nameof(_appSettings.ExportDirectoryPath)} is empty");
            }
            else
            {
                // check the path is valid
                DirectoryInfo dirInfo = new DirectoryInfo(_appSettings.ExportDirectoryPath);

                try
                {
                    if (!dirInfo.Exists)
                    {
                        dirInfo.Create();
                    }
                }
                catch
                {
                    errors.Add($"ExportDirectoryPath path is invalid: {_appSettings.ExportDirectoryPath}");
                }
            }

            return errors;
        }

        private static void SaveToFile(DcsModule module)
        {
            try
            {
                var fileExport = _serviceProvider.GetRequiredService<ICsvFileExport>();
                fileExport.Export(_appSettings.ExportDirectoryPath, module);
            }
            catch (Exception? ex)
            {
                ConsoleAppManager.NotifyException("Error while exporting data into file.", ex);
                ConsoleAppManager.PromptExitConfirmation();
                Environment.Exit(0);
            }
        }

        private static void RegisterServices()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(GetConfiguration);
            services.AddTransient<ISettingsFactory, SettingsFactory>();
            services.AddTransient<ICsvFileExport, CsvFileExport>();

            _serviceProvider = services.BuildServiceProvider();
        }
        
        private static IConfiguration GetConfiguration(IServiceProvider arg)
        {
            try
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                    .AddJsonFile("appsettings.json").Build();
            }
            catch
            {
                ConsoleAppManager.NotifyException("Error while getting the application settings");
                ConsoleAppManager.PromptExitConfirmation();
                Environment.Exit(0);
                return null;
            }
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