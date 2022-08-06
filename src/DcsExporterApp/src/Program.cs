using DCSExporterApp;

using DcsExportLib;
using DcsExportLib.Models;

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

// TODO MJ: path should be in config
string dcsPath = @"y:\Games\DCS World OpenBeta";

IModuleLookup moduleLookup = DcsExporterBuilder.CreateModuleLookup();
var modules = moduleLookup.GetInstalledModules(dcsPath);

ConsoleAppManager consoleAppManager = new ConsoleAppManager();
consoleAppManager.SetConsoleTitle();

DcsModuleInfo selectedModule = consoleAppManager.PromptSelectModule(modules);

consoleAppManager.DisplayExportStartedMessage(selectedModule);

// TODO MJ: get from app configuration
ExporterConfig config = new ExporterConfig { DcsPath = dcsPath };

IDcsExporter exporter = DcsExporterBuilder.CreateExporter(config);
exporter.ExportClickableData(selectedModule);

Console.ReadLine();