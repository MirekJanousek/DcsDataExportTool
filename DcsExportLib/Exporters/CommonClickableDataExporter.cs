using DcsClickableExportLib.Models;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace DcsClickableExportLib.Exporters
{
    internal class CommonClickableDataExporter : IClickableDataExporter
    {
        public void ExportData(DcsModuleInfo moduleInfo)
        {
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table,
               typeof(ConvertedClass)
               , v => { return new ConvertedClass { Id = 1 }; }
           );

            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table,
                typeof(IList<ClickableElement>)
                , v =>
                {
                    foreach (TablePair pair in v.Table.Pairs)
                    {
                        ConvertedClass t = pair.Value.Table.Get("class").ToObject<ConvertedClass>();
                    }

                    return new List<ClickableElement>();
                }
            );

            Script clickableScript = new Script();
            ((ScriptLoaderBase)clickableScript.Options.ScriptLoader).ModulePaths =
                new string[] { "Scripts/?", "Scripts/?.lua" };

            clickableScript.DoString(@"
                                            LockOn_Options = {}
                                            LockOn_Options.script_path = [[y:\Games\DCS World OpenBeta\Mods\aircraft\F-5E\Cockpit\Scripts\]]
                                          ");
            clickableScript.DoFile(ProgramPaths.I18FilePath);
            clickableScript.DoFile(ProgramPaths.ExportFunctionsFilePath);


            //clickableScript.DoString(@"LockOn_Options.script_path = LockOn_Options.script_path..[[y:\Games\DCS World OpenBeta\Mods\aircraft\F-5E\Cockpit\Scripts\]]");
            clickableScript.Registry["LockOn_Options.script_path"] =
                @"y:\Games\DCS World OpenBeta\Mods\aircraft\F-5E\Cockpit\Scripts\";
            clickableScript.Globals["LockOn_Options.script_path"] =
                @"y:\Games\DCS World OpenBeta\Mods\aircraft\F-5E\Cockpit\Scripts\";

            string filePath = @"y:\Games\DCS World OpenBeta\Mods\aircraft\F-5E\Cockpit\Scripts\clickabledata.lua";
            DynValue returnVal = clickableScript.DoFile(filePath);

            Table elements = (Table)clickableScript.Globals["elements"];

            IList<ClickableElement> clicks =
                clickableScript.Globals.Get("elements").ToObject<IList<ClickableElement>>();
        }
    }
}
