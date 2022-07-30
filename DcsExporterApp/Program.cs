using DCSExporterApp;
using DcsExportLib;
using DcsExportLib.Models;

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