using DcsExportLib.DcsObjects;
using DcsExportLib.Enums;
using DcsExportLib.Models;

using NLua;

using System.Text;

namespace DcsExportLib.Executors
{
    internal class CommonExecutor : IExecutor
    {
        public LuaTable ExecuteClickables(Lua lua, DcsModuleInfo moduleInfo)
        {
            lua.State.Encoding = Encoding.UTF8;

            LockOnOptions options = new LockOnOptions(moduleInfo.ScriptFolder);
            lua[DcsVariables.LockOnOptions] = options;
            lua.DoString("package.path = package.path .. ';Scripts/?.lua'");

            lua.DoFile(ProgramPaths.ExportFunctionsFilePath);
            
            var loadRes = lua.LoadFile(moduleInfo.ClickableElementsFolderPath);
            loadRes.Call();

            // TODO replace with own exception telling consumer that module loading went wrong
            return lua[DcsVariables.Elements] as LuaTable ?? throw new InvalidOperationException("Cannot load table of elements!");
        }
    }
}
