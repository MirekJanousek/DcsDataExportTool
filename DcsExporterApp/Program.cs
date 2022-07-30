using DCSLookupApp;
using DcsClickableExportLib;
using DcsClickableExportLib.Models;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

// TODO MJ: path should be in config
string dcsPath = @"y:\Games\DCS World OpenBeta";

ModuleLookup moduleLookup = new ModuleLookup();
var modules = moduleLookup.GetInstalledModules(dcsPath);


ConsoleAppManager consoleAppManager = new ConsoleAppManager();
consoleAppManager.SetConsoleTitle();

DcsModuleInfo selectedModule = consoleAppManager.PromptSelectModule(modules);

consoleAppManager.DisplayExportStartedMessage(selectedModule);

DcsExporter exporter = new DcsExporter(dcsPath);
exporter.ExportClickableData(selectedModule);

Console.ReadLine();