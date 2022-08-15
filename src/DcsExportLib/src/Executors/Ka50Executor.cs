using System.Text;
using DcsExportLib.DcsObjects;
using DcsExportLib.Enums;
using DcsExportLib.Models;

using NLua;

namespace DcsExportLib.Executors
{
    public class Ka50Executor : IExecutor
    {
        public LuaTable ExecuteClickables(Lua lua, DcsModuleInfo moduleInfo)
        {
            lua.State.Encoding = Encoding.UTF8;

            LockOnOptions options = new LockOnOptions(moduleInfo.ScriptFolder);
            lua[DcsVariables.LockOnOptions] = options;
            lua.DoString("package.path = package.path .. ';Scripts/?.lua'");

            lua.DoFile(ProgramPaths.ExportFunctionsFilePath);
            
            string localizeFunctionStr =
                @"function LOCALIZE(str)
                        return str
                    end";

            string content = File.ReadAllText(moduleInfo.ClickableElementsFolderPath).Replace(@"\%", "%", StringComparison.InvariantCulture);
            content = content.Replace("dofile(LockOn_Options.script_path..\"Hint_localizer.lua\")", string.Empty);
            content = localizeFunctionStr + "\r\n\r\n" + content;
            lua.DoString(content);

            if (lua[DcsVariables.Elements] is not LuaTable elementsTable)
            {
                lua.NewTable(DcsVariables.Elements);
            }
                
            return lua[DcsVariables.Elements] as LuaTable ?? throw new InvalidOperationException("Cannot load table of elements!");
        }
    }
}
