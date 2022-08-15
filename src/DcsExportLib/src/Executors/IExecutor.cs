using DcsExportLib.Models;
using NLua;

namespace DcsExportLib.Executors;

public interface IExecutor
{
    LuaTable ExecuteClickables(Lua lua, DcsModuleInfo moduleInfo);
}