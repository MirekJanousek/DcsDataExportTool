using DcsExportLib.Executors;
using DcsExportLib.Models;

namespace DcsExportLib.Factories;

internal interface IExecutorFactory
{
    IExecutor GetExecutor(DcsModuleInfo moduleInfo);
}