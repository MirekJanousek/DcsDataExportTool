using DCSLookupApp;
using DcsClickableExportLib;
using DcsClickableExportLib.Models;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

// TODO MJ: path should be in config
string dcsPath = @"y:\Games\DCS World OpenBeta";

// TEST SECTION

//string i18FilePath = AppContext.BaseDirectory + @"Scripts\i_18n.lua";
//string exportFunctionsFilePath = AppContext.BaseDirectory + @"Scripts\ExportFunctions.lua";

//Script script = new Script();
//((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths =
//    new string[] { "Scripts/?", "Scripts/?.lua" };

//script.DoString(@"
//                LockOn_Options = {}
//                LockOn_Options.script_path = [[y:\Games\DCS World OpenBeta\Mods\aircraft\F-5E\Cockpit\Scripts\]]
//              ");

//script.DoFile(i18FilePath);
//script.DoFile(exportFunctionsFilePath);

//DynValue loadedScript = script.LoadFile(@"y:\Games\DCS World OpenBeta\Mods\aircraft\F-5E\Cockpit\Scripts\clickabledata.lua");

//script.Call(loadedScript);

//Table elements = (Table)script.Globals["elements"];

// TEST SECTION END







ModuleLookup moduleLookup = new ModuleLookup();
var modules = moduleLookup.GetInstalledModules(dcsPath);


ConsoleAppManager consoleAppManager = new ConsoleAppManager();
consoleAppManager.SetConsoleTitle();

DcsModuleInfo selectedModule = consoleAppManager.PromptSelectModule(modules);

consoleAppManager.DisplayExportStartedMessage(selectedModule);

DcsExporter exporter = new DcsExporter(dcsPath);
exporter.ExportClickableData(selectedModule);

Console.ReadLine();